using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDemo
{
    public partial class Clientside : System.Web.UI.Page
    {
        static Aes aes = Aes.Create();

   
        static readonly byte[] Key = Encoding.UTF8.GetBytes("Your16CharKey123"); 
        static readonly byte[] IV = Encoding.UTF8.GetBytes("Your16CharIV_456");  

        protected void Page_Load(object sender, EventArgs e)
        {
           
            aes.Key = Key;
            aes.IV = IV;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text;

            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 12345))
                {
                    NetworkStream stream = client.GetStream();

                    
                    string encryptedMessage = Encrypt(message);
                    byte[] messageBytes = Encoding.UTF8.GetBytes(encryptedMessage);
                    stream.Write(messageBytes, 0, messageBytes.Length);

                    StringBuilder responseBuilder = new StringBuilder();

               
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while (true)
                    {
                        
                        bytesRead = stream.Read(buffer, 0, buffer.Length);

                        if (bytesRead == 0)
                        {
                            
                            break;
                        }

                        string encryptedResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                      
                        string decryptedResponse = Decrypt(encryptedResponse);

                    
                        responseBuilder.AppendLine(decryptedResponse);
                    }

                    
                    lblResponse.Text = "Responses from server:<br/>" + responseBuilder.ToString().Replace("\n", "<br/>");
                }
            }
            catch (Exception ex)
            {
                lblResponse.Text = "Error: " + ex.Message;
            }
        }

        
        private string Encrypt(string plainText)
        {
            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptedBytes) + ":" + Convert.ToBase64String(aes.IV);
            }
        }

        
        private string Decrypt(string encryptedText)
        {
            string[] parts = encryptedText.Split(':');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid encrypted text format. Expected format: '<ciphertext>:<IV>'");
            }

            byte[] encryptedBytes = Convert.FromBase64String(parts[0]);
            byte[] iv = Convert.FromBase64String(parts[1]);

         
            aes.IV = iv;

            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}

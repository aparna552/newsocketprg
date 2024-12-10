<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clientside.aspx.cs" Inherits="ClientDemo.Clientside" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
    <label for="message">Enter Message (e.g., SetA-Two):</label>
    <asp:TextBox ID="txtMessage" runat="server" Width="300"></asp:TextBox>
    <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
</div>
<div>
    <asp:Label ID="lblResponse" runat="server" Text="" ForeColor="Blue"></asp:Label>
</div>
        </div>
    </form>
</body>
</html>

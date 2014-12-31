<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AllTest.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    UserName:<asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
        <br />
    Password:<asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br />
    Password Again:<asp:TextBox ID="txtPassword2" runat="server" TextMode="Password"></asp:TextBox>
        <br />
    NickName:<asp:TextBox ID="txtNickName" runat="server"></asp:TextBox>
        <br />
    Mobile:<asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>    
        <br />
    Mail:<asp:TextBox ID="txtMail" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="CreateSubmit" 
            onclick="btnSubmit_Click" />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Modify" />
        <asp:Button ID="Button2" runat="server" Text="Delete" />
    </div>
    </form>
</body>
</html>
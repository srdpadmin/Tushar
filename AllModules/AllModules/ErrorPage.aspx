<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="AllModules.ErrorPage" EnableTheming="true" Theme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
      <link  href="App_Themes/Default/metro.css" rel="stylesheet" type="text/css" />
</head>
<body class="fColor">
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="REFRESH" runat="server" Text="REFRESH" />
    <asp:Button ID="DeleteAllErrors" runat="server" onclick="DeleteAllErrors_Click"  Text="Delete Errors" />
   
              
    <asp:GridView ID="errorGrid" runat="server" AutoGenerateColumns="true"  CssClass="TableClass" 
        RowStyle-BackColor="Beige" RowStyle-HorizontalAlign="Left" 
        AlternatingRowStyle-BackColor="AliceBlue"
        HeaderStyle-BackColor="AppWorkspace"></asp:GridView>
    </div>
    </form>
</body>
</html>

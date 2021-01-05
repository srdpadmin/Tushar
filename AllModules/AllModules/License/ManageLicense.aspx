<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageLicense.aspx.cs" Theme="Default"
Inherits="License.ManageLicense" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body class="fColor">
    <form id="form1" runat="server">
    <div>
     <table ID="tb1" CssClass="TableClass">
     <tr>
     <td> Machine Key</td>
     <td><asp:Label ID="MachineKey" runat="server" /> </td>
     </tr>
     <tr>
     <td> License Key</td>
     <td> <asp:TextBox ID="LicenseKey" runat="server" Width="100%" /></td>
     </tr>
     <tr>
     <td>License Status </td>
     <td><asp:Label ID="lblLicenseStatus"  runat="server"></asp:Label> </td>
     </tr>
     <tr>
     <td><asp:Button ID="Submit" Text="Submit" runat="server" OnClick="Submit_Click" /></td>
     <td><asp:Button ID="Login" Text="Back To Login" runat="server" OnClick="Login_Click" />  </td>
     </tr>
        </table>
    
    </div>
    </form>
</body>
</html>

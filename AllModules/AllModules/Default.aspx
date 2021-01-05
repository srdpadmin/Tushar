<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AllModules.Default" 
Theme="Default" MasterPageFile="~/Site.Master" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="headMaster" runat="server">
<style type="text/css">

#bg   {
  vertical-align:text-top;  top:-50%; min-width: 100%; min-height: 120%; background-image:url('Images/Illustration.png'); background-repeat:no-repeat; z-index:-10; 
}
</style>
</asp:Content>

 <asp:Content ContentPlaceHolderID="mainMaster" runat="server">
 <%--<div >
   <img src="Images/illustration.png"  />
</div>--%>
 
    <div id="bg"  >
    <div style="padding-top:10px;padding-left:10px; color:White;">
    Use Navigation menu above to access modules.
    </div>
    </div>
    </asp:Content>
    <%--<table class="TableClass fColor" cellspacing="0" style="width:300px;"  >
    <tr class="theadColumnWithBackground">
        <td></td>         
    </tr>
    <tr>
    <td> <asp:LinkButton ID="qo" runat="server" Text="Click for Demo"  OnClick="QO_Click"></asp:LinkButton></td>
               
    </tr>
    <tr class="theadColumnWithBackground">
        <td>Salary Management System</td>          
    </tr>
    <tr>
     <td> <asp:LinkButton ID="sm" runat="server" Text="Click for Demo"  OnClick="SM_Click"></asp:LinkButton></td> 
    </tr>
     <tr  class="theadColumnWithBackground">
      <td>Navigation</td>       
    </tr>
    <tr>
     <td><a href="http://www.ssols.in">Click to go back to ssols.in</a></td> 
      </tr>
    <tr>             
    <td><asp:HyperLink ID="logout" runat="server" NavigateUrl="~/Authorization/Logout.aspx">Logout</asp:HyperLink> </td>
    </tr>
    </table>--%>
    
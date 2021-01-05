<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnauthorizedPage.aspx.cs" 
Inherits="Authorization.UnauthorizedPage" MasterPageFile="~/Authorization/UnAuthorized.Master" Theme="Default" %> 

<asp:Content ID="sm" runat="server" ContentPlaceHolderID="main" >
<br />
You have attempted to access a page that you are not authorized to view.
<%--
<asp:HyperLink ID="returnPage" runat="server" NavigateUrl="~/Default.aspx">here </asp:HyperLink> to go back.
--%>
</asp:Content>
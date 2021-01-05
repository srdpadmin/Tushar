<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="Authorization.Logout"
    MasterPageFile="~/Authorization/UnAuthorized.Master" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="MenuBar" runat="server" />--%>
<asp:Content ID="se" runat="server" ContentPlaceHolderID="main" >
You have been logged out. To log in please return to <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Default.aspx">Login Page</asp:HyperLink>
</asp:Content>
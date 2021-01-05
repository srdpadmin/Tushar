<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministrationPage.aspx.cs" 
Inherits="Authorization.AdministrationPage" MasterPageFile="~/Authorization/Authorized.Master" EnableTheming="true"  StylesheetTheme="Default" %>

<%@ Register TagName="CreateNew" TagPrefix="Users" Src="~/Authorization/Controls/CreateUser.ascx" %>
<%--<%@ Register TagName="Manage" TagPrefix="MUsers" Src="~/Authorization/Controls/ManageUsers.ascx" %>
<%@ Register TagName="Manage" TagPrefix="Roles" Src="~/Authorization/Controls/ManageRoles.ascx" %>
--%>
<asp:Content id="sm" runat="server" ContentPlaceHolderID="main" >    
<Users:CreateNew runat="server" ID="createuser" Visible="false"  />
<%--<Roles:Manage runat="server" ID="manageroles" Visible="false"  />
<MUsers:Manage runat="server" ID="manageuser" Visible="false"  />--%>
</asp:Content>


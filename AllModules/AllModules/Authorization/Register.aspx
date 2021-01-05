<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Authorization.Register"
MasterPageFile="~/Authorization/UnAuthorized.Master" EnableTheming="true"  StylesheetTheme="Default"  %>
<%@ Register TagName="CreateNew" TagPrefix="Users" Src="~/Authorization/Controls/CreateUser.ascx" %>

<asp:Content id="sm" runat="server" ContentPlaceHolderID="main" >    
<Users:CreateNew runat="server" ID="createuser"   />
</asp:Content>

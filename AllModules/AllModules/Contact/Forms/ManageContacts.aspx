<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageContacts.aspx.cs" Inherits="Contact.Forms.ManageContacts" 
 Theme="Default" MasterPageFile="~/Contact/ContactMaster.Master" %>
<%@ Register Src="~/Contact/Controls/Contacts.ascx" TagName="Con" TagPrefix="tact" %>
<asp:Content ID="header" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="mains" runat="server" ContentPlaceHolderID="main">
<tact:Con ID="contacts" runat="server" />
</asp:Content>
 
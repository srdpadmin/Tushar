<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageTermsConditions.aspx.cs" 
Inherits="TermsConditions.Forms.ManageTermsConditions" Theme="Default" MasterPageFile="~/TermsConditions/TermCondition.Master" %>
<%@ Register Src="~/TermsConditions/Controls/MasterTermCondition.ascx" TagName="cc1" TagPrefix="mtc" %>
 
 <asp:Content id="he" runat="server" ContentPlaceHolderID ="head">
 <style type="text/css">
    .OverRideWidth
    {
        width:50%;
    }
 </style>
 </asp:Content>
 <asp:Content ID="myc" runat="server" ContentPlaceHolderID="main">
  <mtc:cc1 id="myTC" runat="server" />
 </asp:Content>
   
   

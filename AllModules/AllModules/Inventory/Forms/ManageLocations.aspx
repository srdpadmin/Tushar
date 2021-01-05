<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageLocations.aspx.cs" Inherits="AllModules.Inventory.Forms.ManageLocations"
Theme="Default" MasterPageFile="~/Inventory/Inventory.Master" %>

<%@ Register Src="~/Inventory/Controls/LocationControl.ascx" TagName="cc1" TagPrefix="mtc" %>
 
 <asp:Content id="he" runat="server" ContentPlaceHolderID ="head">
 <style type="text/css">
    .OverRideWidth
    {
        width:50%;
    }
 </style>
 </asp:Content>
 <asp:Content ID="myc" runat="server" ContentPlaceHolderID="main">
    <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:100px;"><a href="StockLedger.aspx" title="Stock"><b>I&nbsp;-&nbsp;Control</b></a> </li>
    <li>Manage Locations</li>
    </ul>
  <mtc:cc1 id="myTC" runat="server" />
 </asp:Content>
   

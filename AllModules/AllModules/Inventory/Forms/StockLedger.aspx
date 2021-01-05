﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockLedger.aspx.cs" 
Theme="Default" MasterPageFile ="~/Inventory/Inventory.Master" Inherits="Inventory.Forms.StockLedger" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="headr" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function ShowHide(id) {
        var el = document.getElementById('AdvSearch');
        var expcol = document.getElementById('imgA');
        if (el.style.display == 'none') {
            el.style.display = "";
            /*expcol.src = "Images/imgUblue.png" */
            expcol.src = "../../Images/up.png"
        }
        else {
            el.style.display = 'none';
            /* expcol.src = "Images/imgDblue.png" */
            expcol.src = "../../Images/down.png"
        }
    } 
</script>
<style type="text/css">
.popupHover
{
    background-color:#FAF8CC;
}
.LeftSide
{
  float:left;
clear:right;
height:0; 
overflow:hidden
}
</style>
</asp:Content>
<asp:Content ID="Inventory" ContentPlaceHolderID="main" runat="server">  
<ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:100px;"><a href="StockLedger.aspx" title="Inventory"><b>I&nbsp;-&nbsp;Control</b></a> </li>
    <li>Search Stock</li>
    </ul>
<asp:Panel ID="SearchTable" runat="server" >
 <div style="border:solid 1px #498AF3; width:650px; " >     
    <asp:TextBox ID="txtSearch" runat="server"  BorderStyle="None" BorderWidth="0px"  Height="18px" Width="560px"   ></asp:TextBox>        
    <asp:ImageButton ID="search" runat="server" CssClass="vAlign" ImageUrl="~/Images/imgSBlueBig.png" ValidationGroup="Val"  OnClick="SearchBtnClick" ToolTip="Save Search" Height="25px" Width="25px"     />    
    <img src="../../Images/down.png" class="vAlign" id="imgA"  onclick="javascript:ShowHide(this);return false;" style="height:25px;width:25px;border-style: none;" />
    <asp:ImageButton ID="reset" runat="server" CssClass="vAlign" ImageUrl="~/Images/refresh.png" ToolTip="Clear Search" OnClick="ClearBtnClick" Height="25px" Width="25px"   />
    <ajax:TextBoxWatermarkExtender ID="ajaxWaterMark" runat="server" TargetControlID="txtSearch"
     WatermarkText="Search..." ></ajax:TextBoxWatermarkExtender>    
 </div>
<div id="AdvSearch" style="border:solid 1px #AACCEE; width:550px;display:none; margin-top:5px;">
<div style="border:solid 1px #498AF3; height:20px; background-color:#498AF3; color:White;">
<div style="margin-left:5px;margin-top:2px;">
<asp:Label ID="lblSearchControl"  runat="server" Text=" Advanced Search"  SkinID="" />
</div>
</div>

<div>
<table >
     <tr>
        <td>
            Stock #  </td>
        <td>            
            <asp:TextBox ID="txtIDSearch" runat="server" Width="150px" ></asp:TextBox>
        </td>
        <td>
            Created By  
        </td>
        <td>
            <asp:TextBox ID="txtCreatedBySearch" Width="135px" runat="server"></asp:TextBox>            
        </td> 
    </tr>
    <tr>
        <td>
            Contact/Company Name
        </td>
        <td>
            <asp:TextBox ID="txtCompanySearch" Width="150px" runat="server"></asp:TextBox>
        </td>
        <td>
         <span>From Date</span>
         <span><asp:TextBox ID="txtFromSearch" Width="70px"  runat="server"></asp:TextBox>
         <asp:ImageButton ID="imgPop1" runat="server" ImageUrl="~/Images/Calendar.png" />
         <ajax:CalendarExtender ID="ceTo" runat="server"   TargetControlID="txtFromSearch"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgPop1"  />
        </span>
        </td>
         <td> 
         <span>To Date</span>
         <span><asp:TextBox ID="txtToSearch" Width="70px"  runat="server"  ></asp:TextBox>
         <asp:ImageButton ID="imgPop2" runat="server" ImageUrl="~/Images/Calendar.png" />
         <ajax:CalendarExtender ID="ceFrom" runat="server"   TargetControlID="txtToSearch"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgPop2"  />
         </span>
         </td>     
        </tr>
        <tr>
        <td colspan="5">  
         <asp:CompareValidator ID="cmpVal1" ControlToCompare="txtToSearch"  ValidationGroup="Val"   EnableClientScript="true" 
         ControlToValidate="txtFromSearch" Type="Date" Operator="LessThanEqual"   Display="Dynamic" 
         ErrorMessage="To Date cannot be before than From Date" runat="server"></asp:CompareValidator>
        </td>
        </tr>
</table>
 </div>
 </div>
 </asp:Panel>   
 <asp:Panel ID="AddDeleteButtonPanel" runat="server">
        <table>
        <tr>  
           
        <td>
         <asp:ImageButton runat="server" ID="DeleteButton" AlternateText="Delete Stock Entry" ToolTip="Delete Stock Entry"
         OnClick="DeleteButton_Click" ImageUrl="~/Images/iBin.gif" OnClientClick="return confirm('Are you sure you want to delete this Stock Entry! Do you want to continue ?')" />         
        </td>         
       <%-- <td>
        <asp:ImageButton runat="server" ID="AddNewButton" AlternateText="Add New Stock Entry" ToolTip="Add New Stock Entry"
         OnClientClick="return false;" ImageUrl="~/Images/iAdd.PNG" />          
        </td>   --%>    
        </tr>
        </table>
        </asp:Panel>
   <div>
    <br />
    <asp:GridView ID="InventoryGridView" runat="server" AutoGenerateEditButton="false"
    AutoGenerateDeleteButton="false" AutoGenerateColumns="false" SkinID="metro" OnRowCommand="InventoryGridView_RowCommand"
    ShowFooter="true"   DataKeyNames="ID" DataSourceID="ODODS" OnRowCreated="InventoryGridView_RowCreated"  >  
    
    <Columns>   
         
    <asp:TemplateField ItemStyle-Width="10px"> 
    <HeaderTemplate>
    <%--<asp:ImageButton runat="server" ID="Copy" ImageUrl="~/Images/imgCopy.png" ToolTip="Create a copy from existing Bill" />  --%>
    </HeaderTemplate>    
     <ItemTemplate>   
     <asp:CheckBox ID="cbSelect" runat="server"  />    
     </ItemTemplate> 
    </asp:TemplateField>  
    
     <asp:TemplateField>    
     <HeaderTemplate>
     <table style="width:100%;float:left;">
     <tr>    
     <td style="border:0px;width:25px;"></td>   
     <td style="border:0px;width:25px;"></td>       
     <td style="border:0px;width:25px;">ID #</td>
     <td style="border:0px;width:200px;">Customer</td>
     <td style="border:0px;width:50px;">Item Total</td>
     <td style="border:0px;width:50px;"> Date</td>
     <td style="border:0px;width:40px;">Type </td>
     <td style="border:0px;width:50px;">Created By</td>    
     <td style="border:0px;width:50px;">Amended By</td>
     </tr>
     </table>
     </HeaderTemplate> 
     <ItemTemplate >   
     <asp:Panel ID="pnlOrder" runat="server">     
     <table class="TableClass" style="border:0px;width:100%;float:left;">     
     <tr style="border:0px;">
     <td style="border:0px;width:25px;"><asp:Image ID="imgCollapsible" ImageUrl="~/Images/iADD.bmp"  runat="server" /></td> <%--Style="margin-right: 5px;"--%>
     <td style="border:0px;width:25px;"><asp:LinkButton ID="SelectButton" runat="server" CommandName="View" Text="View"  CommandArgument='<%# Eval("ID") %>' /> </td>     
     <td style="border:0px;width:25px;"><%# Eval("ID") %>  </td>
     <td style="border:0px;width:200px;"><%# Eval("Company") %></td>
     <td style="border:0px;width:50px;"><%# Eval("SubTotal") %> </td>
     <td style="border:0px;width:50px;"><%# Eval("TransactionDate", "{0:dd/MM/yyyy}")%> </td>
     <td style="border:0px;width:40px;margin-left:30px;  text-align:left;" class='<%# (int)Eval("StockType") == 0 ? "greenClass" : "blueClass" %>'> <%# Eval("StockTypeName")%> </td>
     <td style="border:0px;width:50px; text-align:justify;"><%# Eval("CreatedByName") %> </td>      
     <td style="border:0px;width:50px;text-align:justify;"><%# Eval("AmendedByName") %></td>
     </tr>
     </table> 
     </asp:Panel>
        <br />
        <br />
     
     <asp:Panel ID="pnlInventoryItem" runat="server" CssClass="LeftSide"  Width="100%"     >
    
     <asp:GridView ID="ItemsGridView" runat="server" DataSourceID="OIDODS" 
     AutoGenerateColumns="false"  SkinID="metro" OnRowCommand="ItemGridView_RowCommand" >
     <Columns > 
      <asp:TemplateField HeaderText="Item Code">
        <ItemTemplate>
        <%# Eval("Code") %>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
        <%# Eval("Description") %>
        </ItemTemplate>
       </asp:TemplateField>
       
       <asp:TemplateField HeaderText="Ordered ">
        <ItemTemplate>
        <%# Eval("OrderedQuantity")%>
        </ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField HeaderText="Received ">
        <ItemTemplate>
        <%# Eval("ReceivedQuantity")%>
        </ItemTemplate>
       </asp:TemplateField>
        <asp:TemplateField HeaderText="Quantity ">
        <ItemTemplate>
        <%# Eval("Quantity")%>
        </ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField HeaderText="Rate">
        <ItemTemplate>
        <%# Eval("Rate")%>
        </ItemTemplate>
       </asp:TemplateField>
       
      <asp:TemplateField HeaderText="SubTotal">
        <ItemTemplate>
        <%# Eval("SubTotal")%>
        </ItemTemplate>
       </asp:TemplateField>
       
       <asp:TemplateField HeaderText="Discount %">
        <ItemTemplate>
        <%# Eval("Discount")%>
        </ItemTemplate>
       </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Disc">
        <ItemTemplate>
        <%# Eval("DiscountAmount")%>
        </ItemTemplate>
       </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Tax %">
        <ItemTemplate>
        <%# Eval("Tax")%>
        </ItemTemplate>
       </asp:TemplateField>
         
        <asp:TemplateField HeaderText="Tax">
        <ItemTemplate>
        <%# Eval("TaxAmount")%>
        </ItemTemplate>
       </asp:TemplateField>
       
       <asp:TemplateField HeaderText="Total">
        <ItemTemplate>
        <%# Eval("Total") %>
        </ItemTemplate>
       </asp:TemplateField>
     </Columns>
     </asp:GridView>
      
    </asp:Panel> 
     
  
      <asp:ObjectDataSource ID="OIDODS" runat="server" 
    TypeName="Inventory.BusLogic.StockItem" ConflictDetection="CompareAllValues"
    SelectMethod="GetStockItems" DataObjectTypeName="Inventory.Data.StockItemData" >
    <SelectParameters>   
    <asp:Parameter Name="StockID" Type="String" DefaultValue="" />
    <asp:Parameter Name="StockType" Type="String" DefaultValue="" />
    </SelectParameters>    
    </asp:ObjectDataSource>         
     <ajax:CollapsiblePanelExtender ID="cpe" runat="Server"  
     TargetControlID="pnlInventoryItem" CollapsedSize="0" Collapsed="True"  
     ExpandControlID="imgCollapsible"     CollapseControlID="imgCollapsible"
     AutoCollapse="False" AutoExpand="False" ScrollContents="false" 
     ImageControlID="imgCollapsible" ExpandedImage="~/Images/iSUB.bmp" 
     CollapsedImage="~/Images/iAdd.bmp" ExpandDirection="Vertical" />
      
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    
</asp:GridView>
    </div>
     <asp:ObjectDataSource ID="ODODS" runat="server" 
    TypeName="Inventory.BusLogic.Stock" ConflictDetection="CompareAllValues"
    SelectMethod="GetStockEntries" DataObjectTypeName="Inventory.Data.StockData" >
    <SelectParameters>   
    
    <asp:ControlParameter ControlID="txtSearch"        Name="searchText"   Type="String" ConvertEmptyStringToNull="true" />    
    <asp:ControlParameter ControlID="txtIDSearch"           Name="Id"           Type="String" ConvertEmptyStringToNull="true" />    
    <asp:ControlParameter ControlID="txtCreatedBySearch"      Name="CreatedBy"    Type="String" ConvertEmptyStringToNull="true" />    
    <asp:ControlParameter ControlID="txtCompanySearch"       Name="Company"      Type="String" ConvertEmptyStringToNull="true" /> 
    <asp:ControlParameter ControlID="txtFromSearch"          Name="FromDate"     Type="String" ConvertEmptyStringToNull="true" /> 
    <asp:ControlParameter ControlID="txtToSearch"            Name="ToDate"       Type="String"  ConvertEmptyStringToNull="true" />  
    
    </SelectParameters>    
    </asp:ObjectDataSource>
   </asp:Content>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Contacts.ascx.cs" Inherits="Contact.Controls.Contacts" %>
<div>
<ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>   
    <li>Create / Manage Contacts </li>
    </ul>
<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
<ContentTemplate> 
<asp:Panel ID="ItemsBtnPanel" runat="server">
<table cellpadding="2" border="0" cellspacing="0"  style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAdd" Height="20px" runat="server" ImageUrl="~/Quotation/Images/iADD.bmp"
                    AlternateText="Add" OnClick="imgAddItem_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEdit" Height="22px" runat="server" ImageUrl="~/Images/iChange.png"
                    AlternateText="Edit" OnClick="imgEditItem_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDelete" Height="22px" runat="server" ImageUrl="~/Quotation/Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDeleteItem_Click" CausesValidation="false" ToolTip="Delete"
                    ></asp:ImageButton>
            </td>             
             <td style="width:350px;" align="left">
                <asp:TextBox ID="txtSearch" runat="server" Width="250px" />
                <asp:Button id="btnSearch" runat="server" Text="Search" />
            </td>
                             
        </tr>
    </table> 

</asp:Panel> 
 
<asp:Panel ID="InsertUpdateCancelBtnPanel" runat="server" >
    <table style="width:20%;">
    <tr>            
            
             <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" />  
             
            <td>
                <asp:Button ID="btnAmend" runat="server" Text="Edit" OnClick="btnAmend_Click" />
            </td>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"   ValidationGroup="ALL" />
            </td>             
            <td>
                <asp:Button ID="btnInsert" runat="server" Text="Create" OnClick="btnInsert_Click" ValidationGroup="ALL"  /> <%--OnClick="InsertPurchaseOrder_Click" CausesValidation="true" OnClientClick="javascript:DoSmth();" --%>
            </td>                                          
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"   /> <%--OnClick="CancelUpdateItem_Click"--%>
            </td>                    
        </tr>       
   </table>
 
<asp:HiddenField ID="ID" runat="server" /><br />
<asp:Label ID="Status" runat="server"></asp:Label>
<table>
<tr><td>Contact Type</td><td>
<asp:DropDownList ID="ddlContactType" runat="server" Enabled="false">
<asp:ListItem Text="Customer" Value="1"></asp:ListItem>
<asp:ListItem Text="Vendor" Value="2"></asp:ListItem>
<asp:ListItem Text="Customer & Vendor" Value="3" Selected="True"></asp:ListItem>
</asp:DropDownList> </td></tr>
<tr><td>Company Name</td><td><asp:TextBox ID="CompanyName" runat="server" Enabled="false"></asp:TextBox></td>
<td><asp:RequiredFieldValidator ID="rfv" runat="server" ErrorMessage="Company Name Required" ControlToValidate="CompanyName" ValidationGroup="TEST"></asp:RequiredFieldValidator></td></tr>
  
<tr><td>First Name</td><td><asp:TextBox ID="FirstName" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Middle Name</td><td><asp:TextBox ID="MiddleName" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Last Name</td><td><asp:TextBox ID="LastName" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Address Line 1 </td><td><asp:TextBox ID="Address1" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Address Line 2</td><td><asp:TextBox ID="Address2" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Address Line 3</td><td><asp:TextBox ID="Address3" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>City</td><td><asp:TextBox ID="City" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>State</td><td><asp:TextBox ID="State" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>PinCode</td><td><asp:TextBox ID="PinCode" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Mobile</td><td><asp:TextBox ID="HomePhone" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Work Phone</td><td><asp:TextBox ID="WorkPhone" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Fax</td><td><asp:TextBox ID="Fax" runat="server" Enabled="false"></asp:TextBox></td></tr>
<tr><td>Email</td><td><asp:TextBox ID="Email" runat="server" Enabled="false"></asp:TextBox></td></tr>

</table>
</asp:Panel>

 <asp:Panel ID="ContactListPanel" runat="server" >
 <asp:ObjectDataSource ID="ContactSearchODS" runat="server" 
                        TypeName="Contact.BusLogic.Contacts" 
                        SelectMethod="GetContacts" >
 <SelectParameters>
 
    <asp:ControlParameter ControlID="txtSearch" ConvertEmptyStringToNull="true" Name="SearchString" Type="String" />
   <%-- <asp:ControlParameter ControlID="VendorName" ConvertEmptyStringToNull="true" Name="VendorName" Type="String" />
    <asp:ControlParameter ControlID="VendorCity" ConvertEmptyStringToNull="true" Name="VendorCity" Type="String" />
     OnRowCommand="SearchViewItemSelected" --%>
 </SelectParameters>
 </asp:ObjectDataSource>
 
 <asp:GridView ID="ContactGridView" runat="server"  SkinID="metroPageSizeControl"
          DataSourceID="ContactSearchODS" DataKeyNames="ID" 
          
          >
 <Columns >
 
     <asp:TemplateField HeaderText="Select"   >
     <ItemTemplate>
      <asp:RadioButton ID="rbtnSelect" runat="server"  AutoPostBack="true"   OnCheckedChanged="SelectButton_Click" />    
     </ItemTemplate> 
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Company" SortExpression="Company" >
     <ItemTemplate>
     <asp:Label ID="Field1" runat="server" Text='<%# Eval("Company") %>' Width="250px" />
     </ItemTemplate> 
     </asp:TemplateField>
     
     
     <asp:TemplateField HeaderText="City" SortExpression="City"  >
     <ItemTemplate>
    <asp:Label ID="Field2" runat="server" Text='<%# Eval("City") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Mobile" SortExpression="Phone1" >
     <ItemTemplate>
    <asp:Label ID="Field3" runat="server" Text='<%# Eval("Phone1") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>
     <asp:TemplateField HeaderText="Work Phone" SortExpression="Phone2" >
     <ItemTemplate>
    <asp:Label ID="Field4" runat="server" Text='<%# Eval("Phone2") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>    
      <asp:TemplateField HeaderText="Fax" SortExpression="Fax" >
     <ItemTemplate>
    <asp:Label ID="Field6" runat="server" Text='<%# Eval("Fax") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>
       <asp:TemplateField HeaderText="Email" SortExpression="Email" >
     <ItemTemplate>
    <asp:Label ID="Field5" runat="server" Text='<%# Eval("Email") %>' Width="200px" />
     </ItemTemplate> 
     </asp:TemplateField>
 </Columns>   
 <PagerStyle CssClass="dlPagerBand" />
 </asp:GridView>
 </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</div>

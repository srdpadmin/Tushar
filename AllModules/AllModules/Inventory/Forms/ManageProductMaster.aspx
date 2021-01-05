<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProductMaster.aspx.cs" Theme="Default"
MasterPageFile ="~/Billing/Billing.Master" Inherits="Billing.Forms.ManageProductMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>
<asp:Content ID="main" runat="server" ContentPlaceHolderID="main">
<script type="text/javascript">
            function deleteItem() {
                if (confirm('Do you want to delete this Item?'))
                    return true;
                else
                    return false;
            }
            function deleteACItem() {
                if (confirm(''))
                    return true;
                else
                    return false;
            }
            function IsOneDecimalPoint(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
                var parts = evt.srcElement.value.split('.');
                if (parts.length > 1 && charCode == 46)
                    return false;
                return true;
            }
        </script>
    <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:100px;"><a href="StockLedger.aspx" title="Inventory"><b>I&nbsp;-&nbsp;Control</b></a> </li>
    <li>Manage Products</li>
    </ul>
    <div>
    <asp:Label ID="ErrorLabel" runat="server" BackColor="Yellow" ForeColor="Red" Visible="false" ></asp:Label>
    <br />
    
    <%--<div style="float:left;width:20%;">--%>
    
    <table cellpadding="2" border="0" cellspacing="0" width="1220px" style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAdd" Height="20px" runat="server" ImageUrl="~/Images/iADD.bmp"
                    AlternateText="Add" OnClick="Add_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEdit" Height="22px" runat="server" ImageUrl="~/Images/iEdit.gif"
                    AlternateText="Edit" OnClick="Edit_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDelete" Height="22px" runat="server" ImageUrl="~/Images/iDel.png"
                    AlternateText="Delete" OnClick="Delete_Click" CausesValidation="false" ToolTip="Delete"
                    OnClientClick="return deleteItem();"></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSave" Height="22px" runat="server" ImageUrl="~/Images/iSave.gif"
                    AlternateText="Update" OnClick="Update_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancel" Height="22px" runat="server" ImageUrl="~/Images/iCan.gif"
                    AlternateText="Cancel" OnClick="Cancel_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
             <td style="width: 5px;" align="left">
            </td>
            <td style="width: 1085px;">
                <asp:HiddenField ID="hdnEditMode" Value="" runat="server" />
            </td>                      
        </tr>
    </table>
    
   <%-- </div>
    <div style="float:right;width:80%;">--%>
    <asp:GridView ID="ProdMasterGrid" runat="server"  Width="100%" OnPageIndexChanging="ProdMasterGrid_PageIndexChanging"
    AutoGenerateColumns="false" SkinID="metro" DataKeyNames="ID"  >
    
    <Columns>
    <asp:TemplateField HeaderText="Select">
    
    <ItemTemplate><asp:RadioButton  ID="cbSelect" runat="server" ToolTip='<%# Eval("ID") %>'  /></ItemTemplate>
    <EditItemTemplate><asp:RadioButton  ID="cbSelect" runat="server" ToolTip='<%# Eval("ID") %>' Enabled="false" />
    <asp:HiddenField ID="hdnCreatedBy" runat="server" Value='<%# Eval("CreatedBy") %>' />
    </EditItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Code">
    <InsertItemTemplate><asp:TextBox ID="Code" runat="server" Width="100px" Text='<%# Bind("Code") %>'></asp:TextBox></InsertItemTemplate>    
    <ItemTemplate><asp:Label ID="Code" runat="server" Text='<%# Eval("Code") %>'></asp:Label></ItemTemplate>
    <EditItemTemplate><asp:TextBox ID="Code" runat="server" Width="70px" Text='<%# Bind("Code") %>'></asp:TextBox></EditItemTemplate>
    </asp:TemplateField>   

     <asp:TemplateField HeaderText="Description">
     <InsertItemTemplate><asp:TextBox ID="Description" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox></InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Description" runat="server" Text='<%# Eval("Description") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate><asp:TextBox ID="Description" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox></EditItemTemplate>
    </asp:TemplateField>

    <%--<asp:TemplateField HeaderText="Balance">
    <InsertItemTemplate><asp:TextBox ID="Balance" runat="server" Width="50px" Text='<%# Bind("Balance","{0:N2}") %>'></asp:TextBox></InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Balance" runat="server" Text='<%# Eval("Balance","{0:N2}") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate><asp:TextBox ID="Balance" runat="server" Width="50px" Text='<%# Bind("Balance","{0:N2}") %>'></asp:TextBox></EditItemTemplate>
    </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Type">
         <InsertItemTemplate><asp:TextBox ID="Type" runat="server" Text='<%# Bind("Type") %>'></asp:TextBox></InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Type" runat="server" Text='<%# Eval("Type") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate><asp:TextBox ID="Type" runat="server" Width="50px"  Text='<%# Bind("Type") %>'></asp:TextBox></EditItemTemplate>
    </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Unit">
        <InsertItemTemplate><asp:TextBox ID="Unit" runat="server" Text='<%# Bind("Unit") %>'></asp:TextBox></InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Unit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate><asp:TextBox ID="Unit" runat="server" Width="50px" Text='<%# Bind("Unit") %>'></asp:TextBox></EditItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Rate">
    <InsertItemTemplate>
    <asp:TextBox ID="Rate" runat="server" Text='<%# Bind("Rate","{0:N2}")%>'  onkeypress="return IsOneDecimalPoint(event);" />
    <asp:FilteredTextBoxExtender ID="quantityValidator1" runat="server" Enabled="True"
    FilterType="Custom" ValidChars="01234567890." TargetControlID="Rate" />
    </InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Rate" runat="server" Text='<%# Eval("Rate","{0:N2}") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate>
    <asp:TextBox ID="Rate" runat="server" Width="50px" Text='<%# Bind("Rate","{0:N2}") %>' onkeypress="return IsOneDecimalPoint(event);" />
    <asp:FilteredTextBoxExtender ID="quantityValidator1" runat="server" Enabled="True"
    FilterType="Custom" ValidChars="01234567890." TargetControlID="Rate" />
    </EditItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Tax">
    <InsertItemTemplate><asp:TextBox ID="Tax" runat="server" Text='<%# Bind("Tax","{0:N2}")%>' onkeypress="return IsOneDecimalPoint(event);" />    
    <asp:FilteredTextBoxExtender ID="quantityValidator2" runat="server" Enabled="True"
    FilterType="Custom" ValidChars="01234567890." TargetControlID="Tax" /></InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Tax" runat="server" Text='<%# Eval("Tax","{0:N2}") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate><asp:TextBox ID="Tax" runat="server" Width="50px" Text='<%# Bind("Tax","{0:N2}") %>' 
    onkeypress="return IsOneDecimalPoint(event);" />    
    <asp:FilteredTextBoxExtender ID="quantityValidator2" runat="server" Enabled="True"
    FilterType="Custom" ValidChars="01234567890." TargetControlID="Tax" /></EditItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Discount">
    <InsertItemTemplate><asp:TextBox ID="Discount" runat="server" Text='<%# Bind("Discount","{0:N2}")%>' onkeypress="return IsOneDecimalPoint(event);" />    
    <asp:FilteredTextBoxExtender ID="quantityValidator3" runat="server" Enabled="True"
    FilterType="Custom" ValidChars="01234567890." TargetControlID="Discount" /></InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Discount" runat="server" Text='<%# Eval("Discount","{0:N2}") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate><asp:TextBox ID="Discount" runat="server" Width="50px" Text='<%# Bind("Discount","{0:N2}") %>'
    onkeypress="return IsOneDecimalPoint(event);" />    
    <asp:FilteredTextBoxExtender ID="quantityValidator3" runat="server" Enabled="True"
    FilterType="Custom" ValidChars="01234567890." TargetControlID="Discount" />
    </EditItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Balance">
    <InsertItemTemplate><asp:Label ID="Balance" runat="server" Text='<%# Eval("BalanceValue") %>'></asp:Label></InsertItemTemplate>
    <ItemTemplate><asp:Label ID="Balance" runat="server" Text='<%# Eval("BalanceValue") %>'></asp:Label></ItemTemplate>    
    <EditItemTemplate><asp:Label ID="Balance" runat="server" Text='<%# Eval("BalanceValue") %>'></asp:Label></EditItemTemplate>
    </asp:TemplateField>
   <asp:TemplateField HeaderText="Created By">
    <ItemTemplate><asp:Label ID="createdBy" runat="server" Text='<%# Eval("CreatedByName") %>'></asp:Label></ItemTemplate>
    <EditItemTemplate><asp:Label ID="createdBy" runat="server" Text='<%# Eval("CreatedByName") %>' ReadOnly="True"></asp:Label></EditItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Created On">
    <ItemTemplate><asp:Label ID="createdOn" runat="server" Text='<%# Eval("CreatedOn","{0:dd/MM/yyyy}") %>'></asp:Label></ItemTemplate>
    <EditItemTemplate><asp:Label ID="createdOn" runat="server" Text='<%# Bind("CreatedOn","{0:dd/MM/yyyy}") %>' ReadOnly="True"></asp:Label></EditItemTemplate>
    </asp:TemplateField>    
    </Columns>    
    </asp:GridView>
    <%--</div>--%>
    </div>
    </asp:Content>
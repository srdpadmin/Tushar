<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" 
Inherits="AllModules.Payroll.Forms.Configuration" Theme="Default" MasterPageFile="~/Payroll/Payroll.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>


<asp:Content ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function IsOneDecimalPoint(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
        var parts = evt.srcElement.value.split('.');
        if (parts.length > 1 && charCode == 46)
            return false;
        return true;
    }
</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="server">
    <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:60px;"><a href="SearchEmployees.aspx" title="Bill"><b>I&nbsp;-&nbsp;Pay</b></a> </li>
    <li>Manage Configuration</li>
    </ul>
        <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
        <table class="TableClass" style="width:50% !Important;">
        <tr class="theadColumnWithBackground">
        <th colspan="2" class="theadColumnWithBackground"> Configuration</th>
 
        </tr>
        <tr>
        <td>Professional tax</td>
        <td><asp:TextBox ID="txtProfTax" Text='<%# Bind("ProfessionalTax","{0:N2}") %>' runat="server"   onkeypress="return IsOneDecimalPoint(event);" ></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="quantityValidator" runat="server" Enabled="True"
        FilterType="Custom" ValidChars="01234567890." TargetControlID="txtProfTax" />  
            </td>
        </tr>
        <tr>
        <td>ESIC tax</td>
        <td><asp:TextBox ID="txtESIC" runat="server" Text='<%# Bind("ESIC","{0:N2}") %>'   onkeypress="return IsOneDecimalPoint(event);" ></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
        FilterType="Custom" ValidChars="01234567890." TargetControlID="txtESIC" />  </td>
        </tr>
        <tr>
        <td>Providend fund</td>
        <td><asp:TextBox ID="txtPF" runat="server" Text='<%# Bind("ProvidentFund","{0:N2}") %>'  onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
        FilterType="Custom" ValidChars="01234567890." TargetControlID="txtPF" />  </td>
        </tr>
        <tr>
        <td>Travel Allowance</td>
        <td><asp:TextBox ID="txtTA" runat="server" Text='<%# Bind("TravelAllowance","{0:N2}") %>'   onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
        FilterType="Custom" ValidChars="01234567890." TargetControlID="txtTA" />  </td>
        </tr>
        <tr>
        <td>Dearness Allowance</td>
        <td><asp:TextBox ID="txtDA" runat="server" Text='<%# Bind("DearnessAllowance","{0:N2}") %>'    onkeypress="return IsOneDecimalPoint(event);"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
        FilterType="Custom" ValidChars="01234567890." TargetControlID="txtDA" />  </td>
        </tr>
        <tr>
        <td>Modified On: <asp:Label ID="lblModifiedOn" runat="server" Text='<%# Bind("Modified","{dd/MM/yyyy}") %>'></asp:Label> </td>
        <td><asp:Button ID="btnSubmit" runat="server" Text="Update" onclick="btnSubmit_Click" /> </td>
        </tr>
        </table>
</asp:Content>


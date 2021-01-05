<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeLookupDDL.ascx.cs" Inherits="AllModules.Payroll.Controls.EmployeeLookupDDL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<table  style="border:solid 1px #498AF3; width:auto;">
<tr  >
     
    <td >
       <asp:Label ID="test" runat="server" Text="Employee Name"   />
    </td>            
    <td>
    <asp:DropDownList ID="emplddlList" runat="server" AutoPostBack="true"  onselectedindexchanged="emplddlList_SelectedIndexChanged" ></asp:DropDownList>
    
    </td>
    
    <td>
        <asp:Label ID="Label1" runat="server" Text="Employee ID" />
    </td>
    <td>
      <asp:TextBox ID="empID" runat="server" Width="50px" Enabled="false"  />
    </td>
    <td>
    <asp:Button ID="Reset" runat="server" Text="Clear" onclick="Reset_Click" /></td>
</tr>
</table>
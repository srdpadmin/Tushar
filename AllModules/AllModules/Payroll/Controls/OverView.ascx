<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverView.ascx.cs" Inherits="Payroll.Controls.OverView" %>
<asp:Panel ID="SalaryReportPanel" runat="server">

<table style="width: 50%;">
    <tr>
    
        <td>
            Year
        </td>
        <td>
            <asp:DropDownList ID="YearList" runat="server" AutoPostBack="true"  onselectedindexchanged="YearList_SelectedIndexChanged" >  </asp:DropDownList>
               
           
        </td>
        <td>
            Month
        </td>
        <td>
            <asp:DropDownList ID="MonthList" runat="server" AutoPostBack="true"    onselectedindexchanged="MonthList_SelectedIndexChanged"   > </asp:DropDownList>
             
           
        </td>
        <td>
             <asp:ImageButton ID="Generate" runat="server" Text="Back" onclick="Generate_Click" ToolTip="Prepare Report"    ImageUrl="~/Images/clipboard.jpg" Height="30px" Width="30px" />
        </td>
          <td>
            <asp:Button ID="PrintReport" runat="server" Text="Current Report" Enabled="False" />                
        </td>
        
        <td>
            <asp:Button ID="PrintAll" runat="server" Text="All Salaries" Enabled="False" />                
        </td>
       <%--<td>
            <asp:Button ID="PrintChart" runat="server" Text="Leaves Chart" Enabled="False" />                
        </td>--%>
    </tr>
    <tr>
        <td colspan="6">
            <asp:Label ID="ResultLbl" runat="server" ></asp:Label>
        </td>
       <%-- <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>--%>
    </tr>
     
</table>
 
<asp:GridView ID="CommonGrid" runat="server" AutoGenerateColumns="False" 
    ShowFooter="true" onrowdatabound="CommonGrid_RowDataBound"  AllowPaging="false"
     SkinID="metroNoPaging"   FooterStyle-Font-Bold="true"
   >
    <Columns>   
    <asp:BoundField DataField="EmpID"  HeaderText="ID" />
    <asp:BoundField DataField="EmployeeName" HeaderText="Name" />
    <asp:BoundField DataField="NetSalary" HeaderText="NetSalary" DataFormatString="{0:n2}" />
    <asp:BoundField DataField="OverTime" HeaderText="OverTime" DataFormatString="{0:n2}" />
    <asp:BoundField DataField="TotalSalary" HeaderText="TotalSalary" DataFormatString="{0:n2}" />
    <asp:BoundField DataField="AdvDeduction" HeaderText="AdvDeduction" DataFormatString="{0:n2}" />
    <asp:BoundField DataField="NewAdvance" HeaderText="NewAdvance" DataFormatString="{0:n2}" />
    <asp:BoundField DataField="UnPaidLeaves" HeaderText="UnPaidLeaves" DataFormatString="{0:n2}" />
    <%--<asp:BoundField DataField="UnPaidAmount" HeaderText="UnPaidAmount" DataFormatString="{0:n2}" HtmlEncode="False"  />--%>
    <asp:BoundField DataField="PaidLeaves" HeaderText="PaidLeaves" DataFormatString="{0:n2}" />
    <asp:BoundField DataField="BalanceLeaves" HeaderText="BalanceLeaves" DataFormatString="{0:n2}" />    
    <asp:BoundField DataField="NetPayableSalary" HeaderText="NetPayableSalary" DataFormatString="{0:n2}" />
    </Columns>
</asp:GridView>
 
</asp:Panel>
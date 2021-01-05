<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" 
Inherits="Payroll.Forms.Reports" Theme="Default" MasterPageFile="~/Payroll/Payroll.Master" %>
<%@ Register Src="~/Payroll/Controls/OverView.ascx" TagName="oView" TagPrefix="dash" %>


<asp:Content ID="my" runat="server" ContentPlaceHolderID="main">
<%--<table style="width: 50%;">
<tr>
<asp:RadioButtonList ID="rdoBtnList" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
<asp:ListItem Text="Salary Report" Value="salary" Selected="True"></asp:ListItem>
<asp:ListItem Text="Leaves Report" Value="leaves" ></asp:ListItem>
<asp:ListItem Text="Bonus Report" Value="bonus" ></asp:ListItem>
</asp:RadioButtonList>

</tr>
</table>--%>
<ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:60px;"><a href="SearchEmployees.aspx" title="Payroll"><b>I&nbsp;-&nbsp;Pay</b></a> </li>
    <li> Salary Reports</li>
    </ul>
<dash:oView id="salaryReport" runat="server" />


</asp:Content>


 

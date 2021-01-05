<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalarySlip.aspx.cs" Inherits="Payroll.Reports.SalarySlip" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <rsweb:ReportViewer ID="Sslip" runat="server" Width="100%" Height="500px">        
        <LocalReport >  
        <DataSources>            
            <rsweb:ReportDataSource DataSourceId="ODS1" Name="SalaryDataSet_EmployeeDetails" />   
            <rsweb:ReportDataSource DataSourceId="ODS2" Name="SalaryDataSet_Salary"  />         
        </DataSources>    
        </LocalReport> 
        </rsweb:ReportViewer>
     </div>    
    <asp:ObjectDataSource ID="ODS1" runat="server" 
        SelectMethod="GetEmployeeDetails" 
        TypeName="Payroll.BusLogic.SalaryReports">         
    </asp:ObjectDataSource>
    
    <asp:ObjectDataSource ID="ODS2" runat="server" 
        SelectMethod="GetSalaryDetails" 
        TypeName="Payroll.BusLogic.SalaryReports">         
    </asp:ObjectDataSource>
      <%--ReportPath="Payroll\Reports\SalarySlip.rdlc"--%>
    </form>
</body>
</html>

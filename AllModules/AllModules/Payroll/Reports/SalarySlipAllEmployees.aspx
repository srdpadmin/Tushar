<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalarySlipAllEmployees.aspx.cs" Inherits="Payroll.Reports.SalarySlipAllEmployees" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField ID="DM" runat="server" />
    <rsweb:ReportViewer ID="AllSalaries" runat="server" Width="100%" Height="500px" >        
       <LocalReport >  
      <DataSources>            
             <rsweb:ReportDataSource DataSourceId="ODS1" Name="SalaryDataSet_EmployeeDetails" />   
             <rsweb:ReportDataSource DataSourceId="ODS2" Name="SalaryDataSet_SalaryInformation"  /> 
        </DataSources>    
        </LocalReport>
        </rsweb:ReportViewer>
     </div>    
      <asp:ObjectDataSource ID="ODS1" runat="server" 
        SelectMethod="GetAllEmployeeDetails"
        TypeName="Payroll.BusLogic.SalaryReports">         
        <SelectParameters>
        <asp:ControlParameter ControlID="DM" Name="DMID" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
    <asp:ObjectDataSource ID="ODS2" runat="server" 
        SelectMethod="GetSalaryDetails" 
        TypeName="Payroll.BusLogic.SalaryReports">         
    </asp:ObjectDataSource> 
     <%--ReportPath="Payroll\Reports\SalarySlipAllEmployees.rdlc"--%>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeavesReport.aspx.cs" Inherits="AllModules.Payroll.Reports.LeavesReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <rsweb:ReportViewer ID="RRV" runat="server" Width="100%" Height="500px">
     <%--<LocalReport  ReportPath="Payroll\Reports\OverViewReport.rdlc"></LocalReport>  --%>
    </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>

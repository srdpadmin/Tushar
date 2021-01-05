<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageLeaves.aspx.cs" Inherits="Payroll.Forms.ManageLeaves" %>
<%@ Register  Src="~/Payroll/Controls/LeaveTransactions.ascx" TagName="LT" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>

    <form id="form1" runat="server">
    <uc:LT id="st" runat="server"></uc:LT>
    <div>
    
   <%-- <LT:leaves runat="server" id="lc" ></LT:leaves>--%>
    </div>
    </form>
</body>
</html>

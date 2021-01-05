<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Payroll.Forms.Test" Theme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>

    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager id="Sm"  runat="server"></cc1:ToolkitScriptManager>
    <div>
    <asp:TextBox ID="txtJoinDate"     runat="server"  />
    <cc1:CalendarExtender ID="ceJD" runat="server" TargetControlID="txtJoinDate"  Format="dd/MM/yyyy"  CssClass="MyCalendar"  />                                                               
                                
    </div>
    </form>
</body>
</html>

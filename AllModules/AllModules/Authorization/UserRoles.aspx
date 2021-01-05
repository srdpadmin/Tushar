<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRoles.aspx.cs" 
Inherits="Authorization.UserRoles" Theme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/MultiCheckCombo.ascx" TagName="MCheck" TagPrefix="mc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
   
    </style>
</head>
<body>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <div>
    <asp:TextBox runat="server" ID="smText" Width="100px"></asp:TextBox>
    <asp:DropDownCheckBoxes ID="checkBoxes1" runat="server"  CssClass="fColor"  
                OnSelectedIndexChanged="checkBoxes_SelcetedIndexChanged"
               OnDataBound="checkBoxes_DataBound"
                AddJQueryReference="True"  UseButtons="False"  AutoPostBack="true"
                UseSelectAllNode="True">
                <Style2 SelectBoxWidth="150"  />
                <Texts SelectBoxCaption="Select Roles" />
                
            </asp:DropDownCheckBoxes>
    <asp:Button ID="testbtn" runat="server" OnClick="testBtn_OnClick"/>
    </div> 
    </form>
</body>
</html>

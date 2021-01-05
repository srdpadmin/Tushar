<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Test._Default"
    Theme="Default" enableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%--<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajx" %>--%>
<head runat="server">
    <title>Inline Forms in Grid View by Jay@Thakar.info</title>
    <link href="../App_Themes/Default/breadcrumbs2.css" />
    <script language="javascript" type="text/javascript">
    
  function textOnFocus(textBox)
  {
    textBox.className='loginHighlight';
  }
  function textOnBlur(textBox)
  {
    textBox.className='loginNormal';
  }  
    </script>

    <style type="text/css">
        .loginHighlight
        {
            border: 2px solid grey;
            background: #FFFFFF url(    "./gr.png" ) repeat-x 0 1px;
            color: #666666;
            width: 80%;
        }
        .loginNormal
        {
            border: 1px solid #cdcdcd;
            background: #FFFFFF url(      "./gr.png" ) repeat-x 0 1px;
            color: #666666;
            width: 80%;
            opacity: 0.6;
            filter: alpha(opacity=60);
        }
        .frmLbl
        {
            font-family: serif;
        }
        .btnNormal
        {
            border: 1px solid #cdcdcd;
            background: #FFFFFF url(      "./grG.png" ) repeat-x 0 1px;
            color: black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   <ul id="breadcrumb">
    <li><a href="#" title="Navigation"><img src="../Images/breadcrumbs_home.png" alt="Home" class="home" /></a></li>
    <li><a href="#" title="Sample page 1">Sample page 1</a></li>
    <li><a href="#" title="Sample page 2">Sample page 2</a></li>
    <li><a href="#" title="Sample page 3">Sample page 3</a></li>
    <li>Current page</li>
    </ul>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <center><div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="Both" Width="60%" OnRowEditing="GridView1_RowEditing"
            AllowPaging="true" PageSize="3" OnPageIndexChanging="GridView1_PageIndexChanging"
            OnRowDataBound="GridView1_RowDataBound" DataKeyNames="UserID">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width:15%;" align="left">
                                    First Name
                                </td>
                                <td style="width:15%;" align="left">
                                    Last Name
                                </td >
                                <td style="width:35%;" align="left">
                                    Web
                                </td>
                                <td style="width:35%;" align="left">
                                    Email
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td style="width:15%;" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "FirstName")%>
                                </td>
                                <td style="width:15%;" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "LastName")%>
                                </td>
                                <td style="width:35%;" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "Web")%>
                                </td>
                                <td style="width:35%;" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "Email")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <center>
                            <ajx:RoundedCornersExtender ID="rce" runat="server" TargetControlID="pnlHead" Radius="6"
                                Corners="Top" BorderColor="black" />
                            <asp:Panel ID="pnlHead" runat="server" Font-Bold="true" ForeColor="white" BackColor="#507CD1"
                                Width="50%">
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                Updating
                                                <%#DataBinder.Eval(Container.DataItem, "FirstName")%>,
                                                <%#DataBinder.Eval(Container.DataItem, "LastName")%>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </asp:Panel>
                            <ajx:RoundedCornersExtender ID="rceDetail" runat="server" TargetControlID="pnlDetail"
                                Radius="6" Corners="Bottom" BorderColor="black" />
                            <asp:Panel ID="pnlDetail" runat="server" Width="50%">
                                <table width="100%">
                                    <tr>
                                        <td align="right" style="width: 30%">
                                            First Name:
                                        </td>
                                        <td style="padding-right:10px;">
                                            <asp:TextBox ID="tbFirstName" Width="100%" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FirstName")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Last Name:
                                        </td>
                                        <td style="padding-right:10px;">
                                            <asp:TextBox ID="tbLastName" runat="server" Width="100%" Text='<%#DataBinder.Eval(Container.DataItem, "LastName")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Web:
                                        </td>
                                        <td style="padding-right:10px;">
                                            <asp:TextBox ID="tbWeb" runat="server" Width="100%" Text='<%#DataBinder.Eval(Container.DataItem, "Web")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            eMail:
                                        </td>
                                        <td style="padding-right:10px;">
                                            <asp:TextBox ID="tbEmail" runat="server" Width="100%" Text='<%#DataBinder.Eval(Container.DataItem, "eMail")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div style="padding-top: 3px; border-top: thin solid black;" id="divUpdate">
                                    <asp:Button CssClass="btnNormal" runat="server" ID="bUpdate" Text="Update" OnClick="bUpdate_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                        OnClick="bCancel_Click" CssClass="btnNormal" runat="server" ID="bCancel" Text="Cancel" />
                                </div>
                            </asp:Panel>
                        </center>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ButtonType="Image" EditImageUrl="./edit.gif"
                    HeaderText="Edit" ShowCancelButton="false" UpdateImageUrl="./Blank.gif" ItemStyle-Width="20" />
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div></center>
    </form>
</body>
</html>

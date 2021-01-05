<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="KeyNavigationPlugIn" Codebehind="KeyNavigationPlugIn.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="Javascript/jquery-1.4.1.js" type="text/javascript"></script>

    <script src="jquery.keynavigation.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            //For navigating using left and right arrow of the keyboard
            var gridview = $("#GridView1");
            $.keynavigation(gridview);
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <h2>
        Gridview column and row navigation using up/down/right/left arrows and Enter key</h2>
    <br />
    <div style="padding-left: 50px">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
            <RowStyle BackColor="white" />
            <Columns>
                <asp:TemplateField HeaderText="Coulumn1" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtColumn1" Text='<%# Bind("Column1") %>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Coulumn2" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtColumn2" Text='<%# Bind("Column2") %>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Coulumn4" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkTest" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Coulumn3" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddTest" runat="server">
                            <asp:ListItem Value="one" Text="One"></asp:ListItem>
                            <asp:ListItem Value="two" Text="Two"></asp:ListItem>
                            <asp:ListItem Value="three" Text="Three"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Coulumn4" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkTest" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>

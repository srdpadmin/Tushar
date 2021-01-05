<%@ Page Language="C#" AutoEventWireup="true" Inherits="KeyNavExample" Codebehind="KeyNavExample.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Javascript/jquery-1.4.1.js" type="text/javascript"></script>

    <style type="text/css">
        .rowbackground
        {
            background-color: white;
        }
    </style>

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            //For navigating using left and right arrow of the keyboard
            $("input[type='text'], select").keydown(
function(event) {
    if ((event.keyCode == 39) || (event.keyCode == 9 && event.shiftKey == false)) {
        var inputs = $(this).parents("form").eq(0).find("input[type='text'], select");
        var idx = inputs.index(this);
        if (idx == inputs.length - 1) {
            inputs[0].select()
        } else {
            $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
            });
            inputs[idx + 1].parentNode.parentNode.style.backgroundColor = "Aqua";
            inputs[idx + 1].focus();
        }
        return false;
    }

    if ((event.keyCode == 37) || (event.keyCode == 9 && event.shiftKey == true)) {
        var inputs = $(this).parents("form").eq(0).find("input[type='text'], select");
        var idx = inputs.index(this);
        if (idx > 0) {
            $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
            });
            inputs[idx - 1].parentNode.parentNode.style.backgroundColor = "Aqua";
      
            inputs[idx - 1].focus();
        }
        return false;
    }
});

            //For navigating using up and down arrow of the keyboard
            $("input[type='text'], select").keydown(
function(event) {
    if ((event.keyCode == 40)) {
        if ($(this).parents("tr").next() != null) {
            var nextTr = $(this).parents("tr").next();
            var inputs = $(this).parents("tr").eq(0).find("input[type='text'], select");
            var idx = inputs.index(this);
            nextTrinputs = nextTr.find("input[type='text'], select");
            if (nextTrinputs[idx] != null) {
                $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                    $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                });
                nextTrinputs[idx].parentNode.parentNode.style.backgroundColor = "Aqua";
                nextTrinputs[idx].focus();
            }
        }
        else {
            $(this).focus();
        }
    }

    if ((event.keyCode == 38)) {
        if ($(this).parents("tr").next() != null) {
            var nextTr = $(this).parents("tr").prev();
            var inputs = $(this).parents("tr").eq(0).find("input[type='text'], select");
            var idx = inputs.index(this);
            nextTrinputs = nextTr.find("input[type='text'], select");
            if (nextTrinputs[idx] != null) {
                $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                    $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                });
                nextTrinputs[idx].parentNode.parentNode.style.backgroundColor = "Aqua";
                nextTrinputs[idx].focus();
            }
            return false;
        }
        else {
            $(this).focus();
        }
    }
});
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <h2>
        Gridview column and row navigation using up/down/right and left arrows</h2>
    <br />
    <div style="padding-left: 50px">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" ForeColor="#333333" GridLines="None">
            <RowStyle BackColor="#E3EAEB" />
            <Columns>
                <asp:TemplateField HeaderText="Coulumn1" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtColumn1" Text='<%# Bind("Column1") %>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Coulumn1" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtColumn2" Text='<%# Bind("Column2") %>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Coulumn1" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtColumn3" Text='<%# Bind("Column3") %>'></asp:TextBox>
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

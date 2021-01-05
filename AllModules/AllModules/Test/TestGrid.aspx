<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestGrid.aspx.cs" Inherits="AllModules.Test.TestGrid" %>
 
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="Edit" runat="server" Text="Edit" OnClientClick="ED('false');return false;" /> <asp:Button ID="Save" Text="Save" runat="server" />
     <asp:Button ID="Add" Text="Add" runat="server" onclick="Add_Click" />
     <asp:GridView ID="TGrid" runat="server"  AutoGenerateColumns="false"  >
     <Columns>
     <asp:TemplateField>
     <ItemTemplate>
     <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("ID") %>'  ></asp:TextBox>
      <asp:TextBox ID="fname" runat="server" Text='<%# Eval("FirstName") %>'   ></asp:TextBox>
      <asp:TextBox ID="lname" runat="server" Text='<%# Eval("LastName") %>' ReadOnly="<%# IsInEditMode %>" ></asp:TextBox>
     </ItemTemplate>
    
     </asp:TemplateField>
     </Columns>
     </asp:GridView>
     
    </div>
    </form>
        <script type="text/javascript" defer='defer'>
            var isEditOn = '<%= IsInEditMode %>';
            //ED(isEditOn);
            function ED(edit) {
                
                var div_to_disable = document.getElementById('<%=TGrid.ClientID %>').getElementsByTagName("input");
                var children = div_to_disable; //.childNodes;
                for (var i = 0; i < children.length; i++) {
                    if (Boolean(edit)) {
                        children[i].disabled = Boolean(edit);
                    }
                    else {
                        children[i].removeAttribute('disabled');
                       
                    }
                };
            }
</script>
</body>
</html>

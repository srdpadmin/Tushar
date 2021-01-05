<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InLineEdit.aspx.cs" Inherits="AllModules.Test.InLineEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="add" runat="server" Text="Add" OnClick="add_Click" />
    <asp:Button ID="del" runat="server" Text="Del" OnClick="del_Click" />
   <asp:GridView ID="gvUsers" AutoGenerateColumns="False" runat="server" 
        CellPadding="4" Font-Names="Georgia" ForeColor="#333333" GridLines="None">

    <Columns>   
        <asp:TemplateField HeaderText="User ID">
    <ItemTemplate>
        <asp:CheckBox ID="chk" runat="server" />
    </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="User ID">

    <ItemTemplate>

    <asp:Label ID="lblUserID" runat="server"    Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("UserID") %>' />
    <asp:TextBox ID="txtUserID" runat="server"  Visible='<%# IsInEditMode %>' Text='<%# Eval("UserID") %>' />

    </ItemTemplate>     

    </asp:TemplateField> 

     <asp:TemplateField HeaderText="First Name">

    <ItemTemplate>

    <asp:Label ID="lblFirstName" Visible='<%# !(bool) IsInEditMode %>' runat="server" Text='<%# Eval("FirstName") %>' />

    <asp:TextBox ID="txtFirstName" Visible='<%# IsInEditMode %>' runat="server" Text='<%# Eval("FirstName") %>' />  

    </ItemTemplate>   

    </asp:TemplateField>   

     <asp:TemplateField HeaderText="Last Name">  

    <ItemTemplate>

    <asp:Label ID="lblLastName" Visible='<%# !(bool) IsInEditMode %>' runat="server" Text='<%# Eval("LastName") %>' />   

    <asp:TextBox ID="txtLastName" Visible='<%# IsInEditMode %>' runat="server" Text='<%# Eval("LastName") %>' />  

    </ItemTemplate>     

    </asp:TemplateField>   
    </Columns>      

    </asp:GridView>
    </div>
    </form>
</body>
</html>

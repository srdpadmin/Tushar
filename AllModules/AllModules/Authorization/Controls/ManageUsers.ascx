<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageUsers.ascx.cs" Inherits="Authorization.Controls.ManageUsers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table class="TableClass" cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:50%;">
    <tr class="theadColumnWithBackground">
        <td>Users Account Management</td>
    </tr>
    <tr >
        <td style="text-align:left;padding-left:5px;"> Total registered users are:<b> <asp:Literal runat="server" ID="lblTotalUsers" /></b></td>
    </tr>
    <tr >
        <td style="text-align:left;padding-left:5px;">Total online users:<b> <asp:Literal runat="server" ID="lblOnlineUsers" /></b></td>
    </tr>
    
    <tr >
    <td>
    <asp:UpdatePanel ID="upG" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
     <asp:Repeater runat="server" ID="rptAlphabetBar"
       OnItemCommand="rptAlphabetBar_ItemCommand">
       <ItemTemplate><asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Container.DataItem %>'
        CommandArgument='<%# Container.DataItem %>' />&nbsp;&nbsp;
       </ItemTemplate>
        </asp:Repeater>
    </ContentTemplate>
    </asp:UpdatePanel>
       
        </td>
    </tr>
    <tr >
    <td style="text-align:left;padding-left:5px;">
         Use the below feature to search users by partial username or e-mail:
     </td>    
    </tr>
     <tr >
     <td style="text-align:left;padding-left:5px;">
         <asp:DropDownList runat="server" ID="ddlUserSearchTypes">
           <asp:ListItem Text="UserName" Selected="true" />
           <asp:ListItem Text="E-mail" />
        </asp:DropDownList>contains
        <asp:TextBox runat="server" ID="txtSearchText" />
        <asp:Button runat="server" ID="btnSearch" Text="Search"
           OnClick="btnSearch_Click" />
      </td>    
    </tr>
      <tr >
      <td>  &nbsp;
      </td>
      </tr>
     <tr >
      <td>    
         <asp:GridView ID="gvUsers" runat="server" SkinID="metro"
                        DataKeyNames="UserName" OnRowCreated="gvUsers_RowCreated" OnRowDeleting="gvUsers_RowDeleting">
   <Columns>
      <asp:BoundField HeaderText="UserName" DataField="UserName" />
      <asp:HyperLinkField HeaderText="E-mail" DataTextField="Email"
         DataNavigateUrlFormatString="mailto:{0}" DataNavigateUrlFields="Email" />
      <asp:BoundField HeaderText="Created" DataField="CreationDate"
         DataFormatString="{0:MM/dd/yy h:mm tt}" />
      <asp:BoundField HeaderText="Last activity" DataField="LastActivityDate"
         DataFormatString="{0:MM/dd/yy h:mm tt}" />
      <asp:CheckBoxField HeaderText="Approved" DataField="IsApproved"
         HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
       
      <%--<asp:HyperLinkField Text="<img src='../images/edit.gif' border='0' />"
         DataNavigateUrlFormatString="EditUsers.aspx?UserName={0}"
         DataNavigateUrlFields="UserName" />--%>
      <%--<asp:HyperLinkField Text="<img src='../images/edit.gif' border='0' />"
         DataNavigateUrlFormatString="EditUsers.aspx?UserName={0}"
         DataNavigateUrlFields="UserName" />--%>
      
      <asp:ButtonField CommandName="Delete" ButtonType="Link"
          Text="Delete"   />
   </Columns>
   <EmptyDataTemplate>No users found.</EmptyDataTemplate>
</asp:GridView>
 </td>    
    </tr>     
</table>

   
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRoles.ascx.cs" Inherits="Authorization.Controls.ManageRoles" %>

<table class="TableClass" cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:50%;">
    <tr class="theadColumnWithBackground" >
        <td colspan="3">Manage Roles By User</td>
    </tr>
    <tr >
        <td style="text-align:left;padding-left:5px;" colspan="3"> <asp:Label ID="ActionStatus" runat="server" ForeColor="Red" /></td>
    </tr>
    <tr >
        <td style="text-align:right;padding-left:5px; width:100px;">Select a User</td>
        <td style="text-align:left;padding-left:5px;" colspan="2"> 
        <asp:DropDownList ID="UserList" runat="server" AutoPostBack="True" 
          DataTextField="UserName" DataValueField="UserName" 
         onselectedindexchanged="UserList_SelectedIndexChanged"> 
     </asp:DropDownList> </td>       
    </tr>
    <tr>
        <td style="text-align:right;padding-left:5px;width:100px;">Assign role</td>
        <td colspan="2" style="text-align:left;">
        <asp:Repeater ID="UsersRoleList" runat="server" > 
            
          <ItemTemplate> 
               <asp:CheckBox runat="server" ID="RoleCheckBox" AutoPostBack="true" 
                    OnCheckedChanged="RoleCheckBox_CheckChanged"
                    Text='<%# Container.DataItem %>' /> 
             
          </ItemTemplate> 
        </asp:Repeater> 
        </td>
    </tr>
    <tr>
        <td style="text-align:right;padding-left:5px;">Reset Password</td>
        <td style="text-align:left;">
        <asp:CheckBox ID="passResetCheckBox" runat="server"
         AutoPostBack="true" oncheckedchanged="passResetCheckBox_CheckedChanged" />
          Reset Password
         </td>
        <td style="text-align:left;">
        <asp:Panel ID="ResetPasswordPanel" runat="server" Visible="false" >
        <asp:TextBox ID="ResetPassword" runat="server" Text="" Width="150px" />
        <asp:Button ID="Reset" runat="server" onclick="Reset_Click" Text="Reset Password" />
        </asp:Panel>
         </td>
    </tr>
    
   </table>

    
     
     

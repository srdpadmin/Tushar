<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" 
Inherits="Authorization.ChangePassword" EnableTheming="true" Theme="Default"  
MasterPageFile="~/Authorization/Authorized.Master" %>


 
<asp:Content id="sm" runat="server" ContentPlaceHolderID="main" >
<table class="TableClass" cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:36%;">
    <tr class="theadColumnWithBackground"  >
        <td>    Change Password
        </td>
    </tr>
</table>
<%--<asp:ChangePassword ID="chgPassword" runat="server" ChangePasswordTitleText=""   
CancelDestinationPageUrl='<%# ConfigurationManager.AppSettings["mykey"].ToString() + "Authorization/AdministrationPage.aspx" %>' 
ContinueDestinationPageUrl='<%# ConfigurationManager.AppSettings["mykey"].ToString() + "Authorization/AdministrationPage.aspx" %>' >
<SuccessTemplate>
            <p>Your password has been successfully changed.  </p>
        </SuccessTemplate>
</asp:ChangePassword>--%>
<div> 
        <asp:Label ID="Label1" runat="server" Text="Current password" Width="120px" 
               ></asp:Label>
        <asp:TextBox ID="txt_cpassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ValidationGroup="sum"
            ControlToValidate="txt_cpassword"  
            ErrorMessage="Please enter Current password">*</asp:RequiredFieldValidator>
        <br />
         <asp:Label ID="Label2" runat="server" Text="New password" Width="120px" ></asp:Label>
        <asp:TextBox ID="txt_npassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ValidationGroup="sum"
            ControlToValidate="txt_npassword" ErrorMessage="Please enter New password">*</asp:RequiredFieldValidator>
        <br />
        
         <asp:Label ID="Label3" runat="server" Text="Confirm password" Width="120px" ></asp:Label>

        <asp:TextBox ID="txt_ccpassword" runat="server" TextMode="Password"></asp:TextBox>   

        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  ValidationGroup="sum"
            ControlToValidate="txt_ccpassword" 
            ErrorMessage="Please enter Confirm  password">*</asp:RequiredFieldValidator>

        <asp:CompareValidator ID="CompareValidator1" runat="server"  ValidationGroup="sum"
            ControlToCompare="txt_npassword" ControlToValidate="txt_ccpassword" 
            ErrorMessage="Password Mismatch"></asp:CompareValidator>    
        <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="sum" />
    </div>
    <asp:Button ID="btn_update" runat="server" Font-Bold="True"   onclick="btn_update_Click" Text="Update" ValidationGroup="sum" CausesValidation="true" />
    <asp:Label ID="lbl_msg"  ForeColor="Red"   runat="server" Text=""></asp:Label><br />
</asp:Content>
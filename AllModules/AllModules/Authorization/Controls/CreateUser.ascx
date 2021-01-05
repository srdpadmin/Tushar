<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateUser.ascx.cs" Inherits="Authorization.Controls.CreateUser" %>

<asp:Panel ID="SignUpPanel" runat="server"> 
       <table class="TableClass" cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:40%;">
        <tr class="theadColumnWithBackground">
           <td  colspan="2"   align="center">
                       <asp:Label ID="signup" runat="server" Text="Sign Up for a new account"></asp:Label>
           </td>
        </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">
                        User Name:</asp:Label>
                 </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">* Required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">
                        Password:</asp:Label>
                </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">* Required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">
                        Confirm Password:</asp:Label>
                </td>
               <td style="text-align:left;">
                    <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                        ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                        ValidationGroup="CreateUserWizard1">* Required</asp:RequiredFieldValidator>
                </td>
            </tr>
                        <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">
                        Security Question:</asp:Label>
                </td>
                <td style="text-align:left;">
                 <asp:DropDownList ID="Question" runat="server">
                   <asp:ListItem>What is your mother's maiden name?</asp:ListItem>
                   <asp:ListItem>In what city were you born?</asp:ListItem>
                   <asp:ListItem>What is your favorite sport?</asp:ListItem>                   
                </asp:DropDownList>
                </td>                   
            </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">
                        Security Answer:</asp:Label>
                </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="Answer" runat="server" CausesValidation="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer"
                        ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                        ValidationGroup="CreateUserWizard1">* Required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
            <td style="text-align:right;">&nbsp;</td>
             <td style="text-align:left;">Personal Details</td>
            </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">
                        First Name:</asp:Label>
                 </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstName"
                        ErrorMessage="First Name is required." ToolTip="First Name is required." ValidationGroup="CreateUserWizard1">* Required</asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="MiddleNameLabel" runat="server" AssociatedControlID="MiddleName">
                        Middle Name:</asp:Label>
                 </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="MiddleName" runat="server"></asp:TextBox>                   
                </td>
            </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">
                        Last Name:</asp:Label>
                 </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="LastName"
                        ErrorMessage="Last Name is required." ToolTip="Last Name is required." ValidationGroup="CreateUserWizard1">* Required</asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="CompanyNameLabel" runat="server" AssociatedControlID="CompanyName">
                        Company Name:</asp:Label>
                 </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="CompanyName" runat="server"></asp:TextBox>                   
                </td>
            </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">
                        E-mail:</asp:Label>
                </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                        ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">* Required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;padding-left:5px;">
                    <asp:Label ID="PhoneLabel" runat="server" AssociatedControlID="Phone">
                        Phone:</asp:Label>
                 </td>
                <td style="text-align:left;">
                    <asp:TextBox ID="Phone" runat="server"></asp:TextBox>                   
                </td>
            </tr>

            <tr>
                <td align="left" colspan="2" style="text-align:left;padding-left:5px;">
                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                        ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                        ValidationGroup="CreateUserWizard1" ></asp:CompareValidator>
                         <asp:Label ID="CreateAccountResults" runat="server" ForeColor="Red" ></asp:Label> 
                    
                </td>
            </tr>            
            <tr>
                <td style="text-align:center;" colspan="2">
                    <asp:Button ID="CreateAccountButton" ValidationGroup="CreateUserWizard1" runat="server" CausesValidation="true" Text="Create User"  OnClick="CreateAccountButton_Click"></asp:Button>                    
                </td>                
            </tr>                       
        </table>
</asp:Panel>
<asp:Panel ID="showResult" runat="server" Visible="false">
<table class="TableClass" cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:36%;">
        <tr class="theadColumnWithBackground">
           <td  colspan="2"   align="center">
                       <asp:Label ID="Label16" runat="server" Text="New Account Creation"></asp:Label>
           </td>
        </tr>
            <tr>
                <td colspan="2"  style="text-align:left;padding-left:5px;">
                    <asp:Label ID="AccountCreated" runat="server" ></asp:Label> 
                    <asp:HyperLink ID="back" runat="server" NavigateUrl="~/Authorization/Login.aspx">Back to login</asp:HyperLink>                    
                 </td>
        </tr>
</table>

</asp:Panel>
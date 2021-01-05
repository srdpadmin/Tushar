<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Authorization.Login" 
 MasterPageFile="~/Authorization/UnAuthorized.Master" Theme="Default" %>

 <asp:Content ID="test" runat="server"   ContentPlaceHolderID="head">
   <style type="text/css">
   .pad{padding:10px;}
   </style>
 </asp:Content>
 
<asp:Content ID="something" runat="server" ContentPlaceHolderID="main">

    <div style="background-image:url('../Images/All-Icons.png');  width:650px; min-height:300px;float:left;">&nbsp;</div>
    <div class="fColor" style=" margin-top:3%;float:right;margin-right: 25%;" >
        <asp:Login ID="LoginControl" runat="server" VisibleWhenLoggedIn="false" DisplayRememberMe="true"                 
                   RememberMeSet="true" Width="100%" Height="100%" >
                 
            <LayoutTemplate >
           
            <table class="TableClass" style="width:100% !Important;">
            <tr class="theadColumnWithBackground">
                <td align="center" colspan="2">
               <asp:Label ID="LoginTitle" runat="server" Text="<%$appSettings:PortalName %>"> </asp:Label>
                              
                </td>
            </tr> 
            <tr>               
            <td  align="right" class="pad"   > 
            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name </asp:Label> 
            </td>
            <td align="left">
            <div  class="pad" > 
            <asp:TextBox ID="UserName" runat="server" Width="150px"  ></asp:TextBox>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                ControlToValidate="UserName" ErrorMessage="User Name is required." 
            ToolTip="User Name is required." ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>            
            </div>
            </td> 
             </tr> 
            <tr>
            <td align="right"  class="pad"   >   <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password </asp:Label></td>
            <td align="left"  class="pad" >
            <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
              ControlToValidate="Password" ErrorMessage="Password is required." 
               ToolTip="Password is required." ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
             </td>
            </tr>    
            <tr>            
            <td colspan="2" align="left">
                <asp:ValidationSummary ID="ValSum" ValidationGroup="LoginControl" runat="server"  DisplayMode="List" ShowMessageBox="False" ShowSummary="True" />                       
                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
            </td>
            </tr>                
            <tr>
            <td align="left">&nbsp;</td>
            <td align="left">
                 <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="LoginControl" />                         
            </td>
            </tr>
             <tr>
                <td align="right"  >
                    <asp:Label ID="Label1" runat="server">New User ?</asp:Label>
                </td>
                <td align="left" >
                    <a href="Register.aspx" id="A1" >Register </a>
                </td>
            </tr>
            </table>
            </LayoutTemplate>
          
        </asp:Login>
         

    </div>
  </asp:Content>
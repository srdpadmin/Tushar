<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserModules.aspx.cs" Inherits="Authorization.UserModules"
 MasterPageFile="~/Authorization/Authorized.Master" Theme="Default" %>
 
 <asp:Content ID="test" runat="server" ContentPlaceHolderID="main">
 <script type="text/javascript">
 function confirmation()
 {
   if(confirm('Are you sure you want to delete this user?'))
   {
    var evObj = document.createEventObject();
    //document.getElementById('<%=hdnButton.ClientID %>').fireEvent('onclick', evObj);
    document.getElementById('<%=hdnButton.ClientID %>').click();
   }
 }
 </script>
 <div>
    <asp:Label ID="ErrorLabel" runat="server" Visible="false"></asp:Label>
    <table cellpadding="2" border="0" cellspacing="0" width="1220px" style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAdd" Height="20px" runat="server" ImageUrl="~/Images/iADD.bmp"
                    AlternateText="Add" OnClick="Add_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEdit" Height="22px" runat="server" ImageUrl="../Images/iEdit.gif"
                    AlternateText="Edit" OnClick="Edit_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDelete" Height="22px" runat="server" ImageUrl="../Images/iDel.png"
                    AlternateText="Delete" OnClientClick="confirmation(); return false;" CausesValidation="false" ToolTip="Delete"
                     ></asp:ImageButton>
                  
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSave" Height="22px" runat="server" ImageUrl="../Images/iSave.gif"
                    AlternateText="Update" OnClick="Update_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancel" Height="22px" runat="server" ImageUrl="../Images/iCan.gif"
                    AlternateText="Cancel" OnClick="Cancel_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
             <td style="width: 5px;" align="left">
              <asp:ImageButton ID="imgResetPassword" Height="22px" runat="server" ImageUrl="../Images/unlock.jpg"
                    AlternateText="Reset Password" OnClick="ResetPassword_Click" CausesValidation="false" ToolTip="Reset Password">
                </asp:ImageButton>
            </td>
             <td style="width: 5px;" align="left">
              <asp:Button ID="hdnButton" runat="server" OnClick="Delete_Click" Visible="false" CssClass="HiddenClass"   />
            </td>
            
             <td style="width: 5px;" align="left">
              &nbsp;
            </td>
            <td style="width: 1085px;">
                <asp:HiddenField ID="hdnEditMode" Value="" runat="server" />
            </td>                      
        </tr>
    </table>
    <asp:GridView ID="umGrid" runat="server"  OnRowDataBound="umGrid_RowDataBound"
    AutoGenerateColumns="false" DataKeyNames="ID" SkinID="metro" Width="500px">
    <Columns>
    <asp:TemplateField HeaderText="Select">
    <ItemTemplate>        
     <asp:RadioButton ID="rbtnSelect" runat="server" AutoPostBack="true"  OnCheckedChanged="SelectButton_Click" />    
     </ItemTemplate> 
     <EditItemTemplate>&nbsp;</EditItemTemplate>
     </asp:TemplateField>
     
    <asp:TemplateField HeaderText="User Name">
    <ItemTemplate>
    <asp:Label ID="UserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
    <asp:HiddenField ID="hdnUserID" runat="server" Value='<%# Eval("UserID") %>' />
    </ItemTemplate>
    <EditItemTemplate>
    <asp:HiddenField ID="hdnUserID" runat="server" Value='<%# Eval("UserID") %>' />
    <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>' />
    <asp:DropDownList ID="userddl" runat="server"></asp:DropDownList>
    </EditItemTemplate>
    </asp:TemplateField>   
    <asp:TemplateField HeaderText="First Name">
    <ItemTemplate>
    <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
    <asp:TextBox ID="txtFirstName" runat="server" Text='<%# Bind("FirstName") %>'></asp:TextBox>
    </EditItemTemplate>
    </asp:TemplateField>  
    <asp:TemplateField HeaderText="Middle Name">
    <ItemTemplate>
    <asp:Label ID="lblMiddleName" runat="server" Text='<%# Eval("MiddleName") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
    <asp:TextBox ID="txtMiddleName" runat="server" Text='<%# Bind("MiddleName") %>'></asp:TextBox>
    </EditItemTemplate>
    </asp:TemplateField>  
    <asp:TemplateField HeaderText="Last Name">
    <ItemTemplate>
    <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
    </ItemTemplate>
    <EditItemTemplate>
    <asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:TextBox>
    </EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Module Access">
    <ItemTemplate>
    <asp:HiddenField ID="hdnModuleBit" runat="server" Value='<%# Eval("ModuleBit") %>' />
    <asp:Label ID="ModuleName" runat="server" ></asp:Label>
    </ItemTemplate>    
    <EditItemTemplate>    
    <asp:HiddenField ID="hdnModuleBit" runat="server" Value='<%# Eval("ModuleBit") %>' />
    <asp:DropDownCheckBoxes ID="modCheckBoxes" runat="server"  CssClass="fColor"  
                OnSelectedIndexChanged="mod_checkBoxes_SelcetedIndexChanged"               
                AddJQueryReference="True"  UseButtons="False"  
                UseSelectAllNode="True" >
                <Style2 SelectBoxWidth="150"  />
                <Texts SelectBoxCaption="Select Modules" />          
            </asp:DropDownCheckBoxes>           
    </EditItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Role Access">
    <ItemTemplate>
    <asp:HiddenField ID="hdnRoleBit" runat="server" Value='<%# Eval("RolesBit") %>' />
    <asp:Label ID="RoleName" runat="server" ></asp:Label>
    </ItemTemplate>    
    <EditItemTemplate>    
    <asp:HiddenField ID="hdnRoleBit" runat="server" Value='<%# Eval("RolesBit") %>' />
    <asp:DropDownCheckBoxes ID="rodCheckBoxes" runat="server"  CssClass="fColor"  
                OnSelectedIndexChanged="rod_checkBoxes_SelcetedIndexChanged"               
                AddJQueryReference="True"  UseButtons="False"  
                UseSelectAllNode="True" >
                <Style2 SelectBoxWidth="150"  />
                <Texts SelectBoxCaption="Select Roles" />          
            </asp:DropDownCheckBoxes>           
    </EditItemTemplate>
    </asp:TemplateField>
   <%-- OnDataBound="checkBoxes_DataBound"
   <asp:TemplateField HeaderText="Created By">
    <ItemTemplate><asp:Label ID="createdBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label></ItemTemplate>
    <EditItemTemplate><asp:Label ID="createdBy" runat="server" Text='<%# Bind("CreatedBy") %>' ReadOnly="True"></asp:Label></EditItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Created On">
    <ItemTemplate><asp:Label ID="createdOn" runat="server" Text='<%# Eval("CreatedOn") %>'></asp:Label></ItemTemplate>
    <EditItemTemplate><asp:Label ID="createdOn" runat="server" Text='<%# Bind("CreatedOn") %>' ReadOnly="True"></asp:Label></EditItemTemplate>
    </asp:TemplateField>  --%>  
    </Columns>
    </asp:GridView>
    </div>

   </asp:Content>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterTermCondition.ascx.cs" Inherits="TermsConditions.Controls.MasterTermCondition" %>

<ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>   
    <li>Create / Manage Terms & Conditions </li>
    </ul>
<asp:UpdatePanel ID="up" runat="server" >
<ContentTemplate>

<asp:RadioButtonList ID="RadioList" runat="server" RepeatDirection="Horizontal" CssClass="GridViewAllColumn"
                      AutoPostBack="true" >
<asp:ListItem Text="Terms" Value="Terms" Selected="True" />
<asp:ListItem Text="Conditions" Value="Conditions" />
</asp:RadioButtonList>

<asp:Panel ID="TermsPanel" runat="server" >

    <asp:Label ID="lblSearch" runat="server" Text="Search" />
    <asp:TextBox ID="txtSearchTerm" runat="server" Width="300px" />
    <asp:Button id="btnSearch" runat="server" Text="Search" 
        onclick="btnTermSearch_Click" />
    <asp:Panel ID="AddDeleteTermsPanel" runat="server">
    <table cellpadding="2" border="0" cellspacing="0" width="5%" style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAdd" Height="20px" runat="server" ImageUrl="~/Images/iADD.bmp"
                    AlternateText="Add" OnClick="imgAdd_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEdit" Height="22px" runat="server" ImageUrl="~/Images/iEdit.gif"
                    AlternateText="Edit" OnClick="imgEdit_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDelete" Height="22px" runat="server" ImageUrl="~/Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDelete_Click" CausesValidation="false" ToolTip="Delete"
                    ></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSave" Height="22px" runat="server" ImageUrl="~/Images/iSave.gif"
                    AlternateText="Update" OnClick="imgUpdate_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancel" Height="22px" runat="server" ImageUrl="~/Images/iCan.gif"
                    AlternateText="Cancel" OnClick="imgCancel_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
             <td style="width:100%;" align="left">
            </td>
                                  
        </tr>
    </table>
</asp:Panel>

  <asp:ObjectDataSource ID="TermsSearchODS" runat="server" 
                        TypeName="TermsConditions.BusLogic.MasterTermConditions" 
                        SelectMethod="GetAllTerms" 
                        DataObjectTypeName="TermsConditions.Data.SingleTerm"
                        UpdateMethod ="UpdateSingleTerm">
 <SelectParameters>
    <asp:ControlParameter ControlID="txtSearchTerm" ConvertEmptyStringToNull="true" Name="TermName" Type="String" />   
 </SelectParameters>
 </asp:ObjectDataSource>
 
 <asp:GridView ID="TermsGridView" runat="server"  SkinID="halfmetro" 
 DataKeyNames="ID"  > 
             <%--DataSourceID="TermsSearchODS"--%>
 <Columns > 
     <asp:TemplateField HeaderText="Select" SortExpression="ID" ItemStyle-Width="5%"  >
     <ItemTemplate>
     <asp:RadioButton ID="rbtnSelect" runat="server" AutoPostBack="true"  OnCheckedChanged="SelectTerm_Click" />    
     </ItemTemplate>    
      <EditItemTemplate>&nbsp;
       </EditItemTemplate>      
    </asp:TemplateField>    
     <asp:TemplateField HeaderText="Terms " SortExpression="TermName" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
     <ItemTemplate>
     <asp:Label ID="TermName" runat="server" Text='<%# Eval("Term") %>' Width="100%"  />
     </ItemTemplate> 
      <EditItemTemplate>
     <asp:TextBox ID="TermName" runat="server" Text='<%# Bind("Term") %>' Width="98%"  />
     </EditItemTemplate> 
     </asp:TemplateField>
 </Columns>   
 
 </asp:GridView>
 </asp:Panel>
 
 <asp:Panel ID="ConditionsPanel" runat="server" >

        
     <asp:Label ID="Label1" runat="server" Text="Search" />
    <asp:TextBox ID="txtSearchCondition" runat="server" Width="300px" />
    <asp:Button id="Button1" runat="server" Text="Search" onclick="btnConditionSearch_Click" />
    <asp:Panel ID="AddDeleteConditionsPanel" runat="server">
    <table cellpadding="2" border="0" cellspacing="0" width="5%" style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAddC" Height="20px" runat="server" ImageUrl="~/Images/iADD.bmp"
                    AlternateText="Add" OnClick="imgAddC_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEditC" Height="22px" runat="server" ImageUrl="~/Images/iEdit.gif"
                    AlternateText="Edit" OnClick="imgEditC_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDeleteC" Height="22px" runat="server" ImageUrl="~/Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDeleteC_Click" CausesValidation="false" ToolTip="Delete"
                    ></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSaveC" Height="22px" runat="server" ImageUrl="~/Images/iSave.gif"
                    AlternateText="Update" OnClick="imgUpdateC_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancelC" Height="22px" runat="server" ImageUrl="~/Images/iCan.gif"
                    AlternateText="Cancel" OnClick="imgCancelC_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
             <td style="width:100%;" align="left">
            </td>
                                  
        </tr>
    </table>
    </asp:Panel>
  <asp:ObjectDataSource ID="ConditionSearchODS" runat="server" 
                        TypeName="TermsConditions.BusLogic.MasterTermConditions" 
                        SelectMethod="GetAllConditions" 
                        DataObjectTypeName="TermsConditions.Data.SingleCondition"
                        UpdateMethod ="UpdateSingleCondition">
 <SelectParameters>
    <asp:ControlParameter ControlID="txtSearchCondition" ConvertEmptyStringToNull="true" Name="ConditionName" Type="String" />   
 </SelectParameters>
 </asp:ObjectDataSource>
   
 <asp:GridView ID="ConditionsGridView" runat="server" SkinID="halfmetro" DataKeyNames="ID"  
           > 
 <Columns > 
     <asp:TemplateField HeaderText="Select" SortExpression="ID" ItemStyle-Width="5%">
      <ItemTemplate>
     <asp:RadioButton ID="rbtnSelect" runat="server" AutoPostBack="true"  OnCheckedChanged="SelectCondition_Click" />    
     </ItemTemplate>    
      <EditItemTemplate>&nbsp;
       </EditItemTemplate>  
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Conditions " SortExpression="Condition" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
     <ItemTemplate>     
     <asp:Label ID="ConditionName" runat="server" Text='<%# Eval("Condition") %>'  Width="100%"  />
     </ItemTemplate> 
      <EditItemTemplate>
     <asp:TextBox ID="ConditionName" runat="server" Text='<%# Bind("Condition") %>' Width="98%"  />
     </EditItemTemplate> 
     </asp:TemplateField>
 </Columns>   
  
 </asp:GridView>
 </asp:Panel>
 </ContentTemplate>
</asp:UpdatePanel>
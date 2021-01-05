<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationControl.ascx.cs" Inherits="AllModules.Inventory.Controls.LocationControl" %>
<asp:UpdatePanel ID="up" runat="server" >
<ContentTemplate>

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
     <asp:TemplateField HeaderText="Location" SortExpression="LocationName" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
     <ItemTemplate>
     <asp:Label ID="TermName" runat="server" Text='<%# Eval("LocationName") %>' Width="100%"  />
     </ItemTemplate> 
      <EditItemTemplate>
     <asp:TextBox ID="TermName" runat="server" Text='<%# Bind("LocationName") %>' Width="98%"  />
     </EditItemTemplate> 
     </asp:TemplateField>
 </Columns>   
 
 </asp:GridView>
 </asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
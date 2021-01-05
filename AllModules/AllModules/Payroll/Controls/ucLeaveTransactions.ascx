<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLeaveTransactions.ascx.cs" 
Inherits="Payroll.Controls.ucLeaveTransactions" EnableTheming="true"  %>

<script type="text/javascript" >
function SetLeavesSource(SourceID)
    {
        var hidSourceID =document.getElementById("<%=LeaveTransactionsField.ClientID%>");
        hidSourceID.value = SourceID;
    }
</script>
 
<asp:Panel ID="sm" runat="server" style="padding-left:15px;padding-bottom:5px;padding-top:5px;float:left;">
<asp:HiddenField ID="LeaveTransactionsField" runat="server" />
 <asp:ImageButton runat="server" ID="AddNewbtn" AlternateText="Add New Leave Transaction" ToolTip="Add New Leave Transaction"
         OnClick="AddNewButton_Click" ImageUrl="~/Images/iAdd.PNG"   />  
 <asp:ImageButton runat="server" ID="Updatebtn" AlternateText="Update Leave Transaction" ToolTip="Update Leave Transaction"
         OnClick="Updatebtn_Click" ImageUrl="~/Images/iSave.gif" Visible="false" OnClientClick = "SetLeavesSource(this.id)" /> 
 <asp:ImageButton runat="server" ID="CancelBtn" AlternateText="Cancel Leave Transaction" ToolTip="Cancel Leave Transaction"
         OnClick="CancelBtn_Click" ImageUrl="~/Images/iCan.gif" Visible="false" /> 
  

</asp:Panel>

<asp:Panel ID="gp" runat="server">
<div>
<div style="padding-left:15px;padding-right:15px;">
<asp:GridView ID="gView" runat="server" DataKeyNames="ID"  AutoGenerateColumns="false" 
AllowPaging="true" PageSize="10" OnPageIndexChanging="gView_PageIndexChanged" SkinID="metro" Width="750px" >
<EmptyDataTemplate>

</EmptyDataTemplate>
<Columns>

    <asp:TemplateField HeaderText="Year" SortExpression="YearName" >
    
     <ItemTemplate>     
     <asp:Label ID="YearName" runat="server" Text='<%# Eval("YearName") %>'  />                
     </ItemTemplate> 
     <InsertItemTemplate>
     <asp:Label ID="YearName" runat="server" Text='<%# DateTime.Now.Year %>'  /> 
     </InsertItemTemplate>
     <EditItemTemplate>
     <asp:Label ID="YearName" runat="server" Text='<%# DateTime.Now.Year %>'   />   
    </EditItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Month" SortExpression="MonthName" >      
     <ItemTemplate>          
     <asp:Label ID="MonthName" runat="server"   Text='<%# Eval("MonthName") %>'   />           
     </ItemTemplate> 
     <InsertItemTemplate>
       <asp:Label ID="MonthName" runat="server"    Text='<%# DateTime.Now.ToString("MMMM") %>'  />  
     </InsertItemTemplate>
      <EditItemTemplate>         
     <asp:Label ID="MonthName" runat="server"    Text='<%# DateTime.Now.ToString("MMMM") %>'  />           
     </EditItemTemplate> 
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Credit" SortExpression="Credit" >      
     <ItemTemplate>          
     <asp:Label ID="Credit" runat="server"    Text='<%# Eval("Credit") %>'   />           
     </ItemTemplate> 
     <InsertItemTemplate>
     <asp:TextBox ID="Credit" runat="server"   Text='<%# Bind("Credit") %>' Width="30px"  />
     </InsertItemTemplate>
      <EditItemTemplate>        
     <asp:TextBox ID="Credit" runat="server"   Text='<%# Bind("Credit") %>' Width="30px"  />           
     </EditItemTemplate> 
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Debit" SortExpression="Debit" >      
     <ItemTemplate>          
     <asp:Label ID="Debit" runat="server"    Text='<%# Eval("Debit") %>'   />           
     </ItemTemplate> 
     <InsertItemTemplate>
     <asp:TextBox ID="Debit" runat="server"   Text='<%# Bind("Debit") %>' Width="30px" />  
     </InsertItemTemplate>
      <EditItemTemplate>        
     <asp:TextBox ID="Debit" runat="server"   Text='<%# Bind("Debit") %>' Width="30px" />           
     </EditItemTemplate> 
     </asp:TemplateField>
     
     <%-- <asp:TemplateField HeaderText="Previous Balance" SortExpression="PreviousBalance" >      
     <ItemTemplate>          
     <asp:Label ID="PreviousBalance" runat="server"    Text='<%# Eval("PreviousBalance") %>'   />           
     </ItemTemplate> 
      <EditItemTemplate>          
     <asp:Label ID="PreviousBalance" runat="server"   Text='<%# Eval("PreviousBalance") %>'  />           
     </EditItemTemplate>
     </asp:TemplateField>--%>
     
     <asp:TemplateField HeaderText="Current Balance" SortExpression="CurrentBalance" HeaderStyle-Width="40%" >      
     <ItemTemplate>          
     <asp:Label ID="CurrentBalance" runat="server"    Text='<%# Eval("CurrentBalance") %>'   />           
     </ItemTemplate> 
     <EditItemTemplate>        
     <asp:Label ID="CurrentBalance" runat="server"   Text='<%# Eval("CurrentBalance") %>'  />           
     </EditItemTemplate> 
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Comments" SortExpression="CurrentBalance" HeaderStyle-Width="50%" >      
     <ItemTemplate>          
     <asp:Label ID="Comments" runat="server"    Text='<%# Eval("Comments") %>'   />           
     </ItemTemplate> 
     <InsertItemTemplate>
     <asp:TextBox ID="Comments" runat="server"   Text='<%# Bind("Comments") %>'  />  
     </InsertItemTemplate>           
      <EditItemTemplate>        
     <asp:TextBox ID="Comments" runat="server"   Text='<%# Bind("Comments") %>'  />           
     </EditItemTemplate>
    
     </asp:TemplateField>
</Columns>
</asp:GridView>
</div>
</div>
<br />
</asp:Panel>

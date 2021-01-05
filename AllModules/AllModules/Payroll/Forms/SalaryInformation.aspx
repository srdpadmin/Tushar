<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryInformation.aspx.cs" 
Inherits="Payroll.Forms.SalaryInformation" MasterPageFile="~/Payroll/Payroll.Master" Theme="Default" %>
<%@ Register TagName="Lookup" TagPrefix="emp" Src="~/Payroll/Controls/EmployeeLookupDDL.ascx" %> 
 <asp:Content ID="SI" runat="server" ContentPlaceHolderID="main"> 
 <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:60px;"><a href="SearchEmployees.aspx" title="Payroll"><b>I&nbsp;-&nbsp;Pay</b></a> </li>
    <li> Salary Information</li>
    </ul>
 <script type="text/javascript">
     function ShowPaySlipInNewWindow(empId, dmid, showheader) {

         window.open('../Reports/SalarySlip.aspx?EmpID=' + empId + '&dmid=' + dmid + '&showHeader=' + showheader, 'NewWindow', 'toolbar=no');
     }
 </script>
 <asp:UpdatePanel ID="upL" runat="server" UpdateMode="Conditional" >
 <ContentTemplate>
 <emp:Lookup ID="empLookup" runat="server" />
 </ContentTemplate>
 </asp:UpdatePanel>
 
 <br /> 
  <asp:UpdatePanel ID="upS" runat="server" UpdateMode="Conditional" >
 <ContentTemplate>
    <asp:Panel ID="BtnPanel" runat="server" >
    <table cellpadding="2" border="0" cellspacing="0" width="1220px" style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAdd" Height="20px" runat="server" ImageUrl="~/Payroll/Images/iADD.bmp"
                    AlternateText="Add" OnClick="imgAdd_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEdit" Height="22px" runat="server" ImageUrl="~/Payroll/Images/iEdit.gif"
                    AlternateText="Edit" OnClick="imgEdit_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDelete" Height="22px" runat="server" ImageUrl="~/Payroll/Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDelete_Click" CausesValidation="false" ToolTip="Delete"
                    ></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSave" Height="22px" runat="server" ImageUrl="~/Payroll/Images/iSave.gif"
                    AlternateText="Update" OnClick="imgUpdate_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancel" Height="22px" runat="server" ImageUrl="~/Payroll/Images/iCan.gif"
                    AlternateText="Cancel" OnClick="imgCancel_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
             <td style="width: 5px;" align="left">
            </td>
            <td style="width: 1085px;">
                <asp:HiddenField ID="hdnEditMode" Value="" runat="server" />
            </td>                      
        </tr>
    </table>
     </asp:Panel>

 <asp:GridView ID="SalaryGrid" runat="server" DataSourceID="SalaryODS" 
      DataKeyNames="DMID" SkinID="metro"  AutoGenerateColumns="false"      
    onrowcommand="SalaryGrid_RowCommand" onrowdatabound="SalaryGrid_RowDataBound" >
 <Columns > 
 <asp:TemplateField HeaderText="Select" >    
     <ItemTemplate>        
     <asp:RadioButton ID="cbSelect" runat="server" AutoPostBack="true"  OnCheckedChanged="SelectButton_Click" />    
     </ItemTemplate> 
     <EditItemTemplate>&nbsp;</EditItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Year" SortExpression="YearName" >
     <ItemTemplate>     
     <asp:Label ID="YearName" runat="server" Text='<%# Eval("YearName") %>'  />           
     <asp:Label ID="EmpType" runat="server" Text='<%# Eval("EmpType") %>'  CssClass="HiddenClass" />  
     </ItemTemplate> 
     <EditItemTemplate>
     <asp:Label ID="YearName" runat="server" Text='<%# Eval("YearName") %>'   />   
     </EditItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Month" SortExpression="MonthName" >      
     <ItemTemplate>          
     <asp:LinkButton ID="MonthName" runat="server" CommandName="View"  CommandArgument='<%# Eval("EmpID") + ";" +  Eval("DMID")  %>' Text='<%# Eval("MonthName") %>' Font-Underline="true" />           
     </ItemTemplate> 
     <EditItemTemplate>          
     <asp:LinkButton ID="MonthName" runat="server" CommandName="View"  CommandArgument='<%# Eval("EmpID") + ";" +  Eval("DMID")  %>' Text='<%# Eval("MonthName") %>' Font-Underline="true" />           
     </EditItemTemplate> 
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText=" Days Worked  " SortExpression="DaysWorked" >      
     <ItemTemplate>     
    <asp:Label ID="Field1" runat="server" Text='<%# Eval("DaysWorked","{0:00}") %>' Width="100px" />
     </ItemTemplate> 
     <EditItemTemplate>
      <asp:Label ID="Field1" runat="server" Text='<%# Eval("DaysWorked","{0:00}") %>' Width="100px" />
     </EditItemTemplate>
     </asp:TemplateField>    
      
     <asp:TemplateField HeaderText="Total Paid Days" SortExpression="TotalPayableDays"  >      
     <ItemTemplate>
    <asp:Label ID="Field2" runat="server" Text='<%# Eval("TotalPayableDays") %>' Width="100px" />
     </ItemTemplate> 
     <EditItemTemplate>
     <asp:Label ID="Field2" runat="server" Text='<%# Eval("TotalPayableDays") %>' Width="100px" />
     </EditItemTemplate>
     </asp:TemplateField>   
       
     <asp:TemplateField HeaderText="Paid Leaves Taken  " SortExpression="PaidLeavesTaken" >      
     <ItemTemplate>
    <asp:Label ID="Field3" runat="server" Text='<%# Eval("PaidLeavesTaken") %>' Width="100px" />
     </ItemTemplate> 
      <EditItemTemplate>
       <asp:Label ID="Field3" runat="server" Text='<%# Eval("PaidLeavesTaken") %>' Width="100px" />
     </EditItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Created By  " SortExpression="CreatedBy" >      
     <ItemTemplate>
     <asp:Label ID="Field4" runat="server" Text='<%# Eval("CreatedBy") %>' Width="100px" />
     </ItemTemplate> 
      <EditItemTemplate>
       <asp:Label ID="Field4" runat="server" Text='<%# Eval("CreatedBy") %>' Width="100px" />
     </EditItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Last Updated By  " SortExpression="LastUpdatedBy" >      
     <ItemTemplate>
     <asp:Label ID="Field5" runat="server" Text='<%# Eval("LastUpdatedBy") %>' Width="100px" />
     </ItemTemplate> 
      <EditItemTemplate>
       <asp:Label ID="Field5" runat="server" Text='<%# Eval("LastUpdatedBy") %>' Width="100px" />
     </EditItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Print Salary Slip"   >      
     <ItemTemplate>
     <asp:LinkButton ID="PrintButton" runat="server" Text="Print" CommandArgument='<%#  Eval("EmpID") + ";" + Eval("DMID")  %>'  />
     </ItemTemplate> 
      <EditItemTemplate>
      <asp:LinkButton ID="PrintButton" runat="server" Text="Print" CommandArgument='<%#  Eval("EmpID") + ";" + Eval("DMID")  %>'  />
     </EditItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Locked" SortExpression="Locked" >     
     <ItemTemplate>
     <asp:CheckBox runat="server" ID="LockedCheck"  Enabled="false"
     Checked='<%#Eval("Locked") == DBNull.Value ? false : Convert.ToBoolean(Eval("Locked"))  %>'   />        
     </ItemTemplate>       
     <EditItemTemplate>
     <asp:CheckBox runat="server" ID="LockedCheck"  
     Checked='<%#Eval("Locked") == DBNull.Value ? false : Convert.ToBoolean(Eval("Locked"))  %>'   />
     </EditItemTemplate>
     </asp:TemplateField>
     
    <%-- <asp:TemplateField HeaderText="Print2"   >
      <ItemStyle CssClass="GridViewRowColumn" />
     <ItemTemplate>
     <asp:LinkButton ID="PrintButton2" runat="server" Text="Print2" 
     OnClientClick='<%# javascript:window.open('~/Reports/GeneratePaySlip.aspx?EmpID=' + Eval("EmpID"),'','left=600px, top=100px, width=540px, height=600px, scrollbars=no, status=no, resizable=no');return false; %>' />      
     </ItemTemplate> 
     </asp:TemplateField>--%>
     
     </Columns>
</asp:GridView>
</ContentTemplate>
 </asp:UpdatePanel>
 
 
<asp:ObjectDataSource ID="SalaryODS" runat="server" TypeName="Payroll.BusLogic.Salary"
                     DataObjectTypeName="Payroll.Data.Salary" SelectMethod="GetSalaries"
                     UpdateMethod="UpdateSalaryDetails" InsertMethod="InsertSalaryDetails"  >
  <SelectParameters>
    <asp:ControlParameter ControlID="empLookup" PropertyName="EmployeeID"  ConvertEmptyStringToNull="true" Name="EmpID" Type="String" />    
    <%--<asp:QueryStringParameter   ConvertEmptyStringToNull="true" QueryStringField="Month" Name="MonthNumber" Type="String" />--%>
 </SelectParameters>
</asp:ObjectDataSource>
 </asp:Content>
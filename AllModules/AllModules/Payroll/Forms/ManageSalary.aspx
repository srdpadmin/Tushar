﻿<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ManageSalary.aspx.cs" 
Inherits="Payroll.Forms.ManageSalary" MasterPageFile="~/Payroll/Payroll.Master" Theme="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

<%--<asp:UpdatePanel ID="MUP" runat="server" UpdateMode="Conditional">
<ContentTemplate>--%>
  <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:60px;"><a href="SearchEmployees.aspx" title="Payroll"><b>I&nbsp;-&nbsp;Pay</b></a> </li>
    <li>Manage Salary</li>
    </ul>
    <div style="height:15px;">&nbsp;</div>
    <asp:Panel ID="ButtonPanel" runat="server" >
    <table>
    <tr>    
    <td>        &nbsp;    </td>   
    <td>
    <%--<asp:ImageButton ID="AddNewSalary" runat="server" Text="Add New" onclick="AddNewSalary_Click" ImageUrl="~/Images/iAdd.PNG" /> --%>
    <asp:ImageButton ID="Back" runat="server" Text="Back" onclick="Back_Click" ToolTip="Go Back"  Visible="false" ImageUrl="~/Images/back.png"  Height="25px" Width="25px" />
    <asp:ImageButton ID="EditSalary" runat ="server" Text="Edit" ToolTip="Edit Salary" Height="23px" onclick="EditSalary_Click" Visible="false" ImageUrl="~/Images/iEdit.gif" />
    <asp:ImageButton ID="SaveNewSalary" runat="server" Text="Save"  ToolTip="Save New Salary"   onclick="SaveNewSalary_Click" Visible="false" ImageUrl="~/Images/iCommit.PNG"  />
    <asp:ImageButton ID="UpdateSalary" runat ="server" Text="Update" ToolTip="Update Salary"  onclick="UpdateSalary_Click" Visible="false" ImageUrl="~/Images/iCommit.PNG" />
    </td>
    <td>
    <asp:ImageButton ID="Cancel" runat="server" Text="Cancel" onclick="Cancel_Click"  Visible="false" ImageUrl="~/Images/iCancel.PNG" ToolTip="Cancel Current Operation"       />                
    
   <%-- <asp:ImageButton ID="DeleteSalary" runat ="server" Text="Delete" onclick="DeleteSalary_Click" Visible="false" ImageUrl="~/Images/iBin.Gif"/>--%>
    </td>
    </tr>
    </table>    
    </asp:Panel> 
    <div></div>    
    <asp:ValidationSummary id="vsum" runat="server" ValidationGroup="summaryGroup" DisplayMode="List"  />
     <asp:DetailsView ID="SDView" runat="server" AutoGenerateRows="false"
                 DataSourceID="SalaryODS" Width="60%" AutoGenerateInsertButton="false"
                 AutoGenerateDeleteButton="false" AutoGenerateEditButton="false" 
                 GridLines="None"     ondatabound="SalaryDetails_DataBound" >   
    <Fields>
    <asp:TemplateField>
    <InsertItemTemplate>
    <table  class="TableClass" >      
        <tr class="theadColumnWithBackground">
            <td colspan="6" align="center" >
                  <asp:Label ID="EmpType" Text='<%# EmpType %>' runat="server"></asp:Label>
            </td>  
        </tr>
        <tr class="theadColumn">
            <td>
               Month
            </td>            
            <td>
                Year
            </td>
            <td>
                DaysWorked
            </td>            
            <td>
               Weekly Offs
            </td>     
            <td>
              Govt\Comp
            </td>   
            <td>
                OT Hours
            </td>      
        </tr>
        <tr >
            <td>
                
                <asp:Label ID="lblID" runat="server" CssClass="HiddenClass" Text='<%# Bind("ID") %>'></asp:Label>
                <asp:DropDownList ID="CurrentMonthList" runat="server"  AutoPostBack="true"
                AppendDataBoundItems="True" onselectedindexchanged="CurrentMonthList_SelectedIndexChanged"   />                
            </td>
            <td>
                <asp:DropDownList ID="CurrentYearList" runat="server"  AutoPostBack="true"
                AppendDataBoundItems="True" onselectedindexchanged="CurrentYearList_SelectedIndexChanged"   />                
            </td>
            <td>
                <asp:Textbox ID="TxtDaysWorked" runat="server" Text='<%# Bind("DaysWorked") %>' Width="35px" CausesValidation="true" ValidationGroup="summaryGroup"  /> /
                <asp:Textbox ID="TxtTotalDays" runat="server" Enabled="false" Width="35px"   />                
                <asp:RegularExpressionValidator runat="server" ID="regex" ControlToValidate="TxtDaysWorked" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,2}(\.\d{0,1})?$" Text="*"  ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>                                                
                <asp:CompareValidator runat="server" id="CompareValidator3" ControlToValidate="TxtDaysWorked" 
                     ControlTocompare="TxtTotalDays" Operator="LessThanEqual" Type="Double" Display="Dynamic" ValidationGroup="summaryGroup" 
                      Text="*" Errormessage="* The working days cannot be greater than the month's days!" />   
                       
            </td>
            <td>
               <asp:Textbox ID="TxtWeeklyOffs" runat="server" Enabled="false" Width="20px" />
            </td>
            <td>
               <asp:Textbox ID="TxtGovtCompHoliday" runat="server" Text='<%# Bind("GovtCompHoliday") %>' Width="35px"   CausesValidation="true" />
              <asp:RegularExpressionValidator runat="server" ID="regex2" ControlToValidate="TxtGovtCompHoliday" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,2}(\.\d{0,1})?$"  Text="*" ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td>  
            <td>
               <asp:Textbox ID="TxtOTHours" runat="server" Text='<%# Bind("OTHours") %>' Width="35px" CausesValidation="true"  />
                <asp:RegularExpressionValidator runat="server" ID="regex3" ControlToValidate="TxtOTHours" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,3}(\.\d{0,1})?$"  Text="*" ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td>
        </tr>
        <tr class="theadColumnWithBackground">
            <td colspan="3" align="center" >
                    Leaves
            </td>              
            <td colspan="3" align="center" >
                    Amount
            </td>  
        </tr>  
        <tr class="theadColumn">
            <td>
                Current / Balance 
            </td>            
            <td>
                Paid 
            </td>
            <td>
                UnPaid  
            </td>
            <td>
                Paid Days
            </td>
            <td>
                Paid Amount 
            </td>
            <td>
               OT Paid
            </td>             
        </tr>
        <tr  >
            <td>              
                <asp:CheckBox ID="chkLeavesOverride" runat="server" Enabled ="true" Checked='<%# Bind("LeavesOverride") %>' />                
                <asp:Textbox ID="TxtLeavesBalanceCurrent" runat="server" Enabled="false" Width="35px"  /> /                
                <asp:Textbox ID="TxtLeavesBalancePrevious" runat="server" Width="35px" Enabled="false" /> 
            </td>            
             <td>
                <asp:Textbox ID="TxtPaidLeavesTaken" runat="server" Text='<%# Bind("PaidLeavesTaken") %>' Width="35px"  />
               <asp:CompareValidator ID="compareBalanceLeaves" runat="server" ControlToCompare="TxtPaidLeavesTaken"
                  ErrorMessage="Paid leaves cannot be greater than balance" Operator="GreaterThanEqual" Text="*" Type="Currency"
                  ControlToValidate="TxtLeavesBalancePrevious" ValidationGroup="summaryGroup" Display="Dynamic"  ></asp:CompareValidator>
            </td>
            <td>
                <asp:Textbox ID="TxtUnPaidLeavesTaken" Width="35px" runat="server" Text='<%# Bind("UnPaidLeavesTaken") %>' Enabled="false" />                
            </td>
            <td>
               <asp:Textbox ID="TxtTotalPaidDays" runat="server" Width="35px" Enabled="false"  Text='<%# Bind("TotalPayableDays") %>'  />
            </td>
            
            <td>
                <asp:Textbox ID="TxtAmountForPaidDays" runat="server" Width="50px"  Text='<%# Bind("PaidAmount") %>'  Enabled="false"  />                
            </td>
             <td>
               <asp:Textbox ID="TxtOTAmount" runat="server" Enabled="false" Text='<%# Bind("OTAmount") %>' Width="50px"  />
            </td>
        </tr>
        <tr class="theadColumnWithBackground" >
            <td colspan="3"   >
                    Advance
            </td>  
           
            <td colspan="3"   >
                    Taxes & Deduction
            </td>  
        </tr>
        <tr class="theadColumn">
            <td>
               Current / Pending 
            </td>            
            <td>
                New 
            </td>
            <td>
                Deduction
            </td>
            <td  >
               PF     
            </td>                       
            <td  >
                ESIC           
            </td>
            <td  >
               Prof Tax           
            </td>
        </tr>
        <tr  >             
            <td>                                 
                <asp:Textbox ID="TxtAdvBalanceCurrent" runat="server" Enabled="false" Width="50px"  />/
                <asp:Textbox ID="TxtAdvBalancePrevious" runat="server" Enabled="false" Width="50px"  />
            </td> 
             <td>                 
                <asp:Textbox ID="TxtAdvBalanceAdd" runat="server"   Text='<%# Bind("AdvBalanceAdd") %>' Width="50px" CausesValidation="true"  />
                <asp:RegularExpressionValidator runat="server" ID="regex4" ControlToValidate="TxtAdvBalanceAdd" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,4}(\.\d{0,2})?$"  Text="*" ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td> 
            <td>                 
                <asp:Textbox ID="TxtAdvBalanceDeduct" runat="server"    Text='<%# Bind("AdvBalanceDeduct") %>' Width="50px"  CausesValidation="true" />
                <asp:RegularExpressionValidator runat="server" ID="regex5" ControlToValidate="TxtAdvBalanceDeduct" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,4}(\.\d{0,2})?$" Text="*"  ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td> 
            <td>                 
                <asp:Textbox ID="TxtPF1" runat="server" Enabled="false" Width="50px"  Text='<%# Bind("PFDeduction") %>'  />
             </td>   
             <td>             
                <asp:Textbox ID="TxtPF2" runat="server" Enabled="false" Width="50px"  Text='<%# Bind("ESICDeduction") %>'  />
            </td>   
             <td>                 
                <asp:Textbox ID="TxtPF3" runat="server" Enabled="false" Width="50px"  Text='<%# Bind("ProfTaxDeduction") %>'  />
            </td>             
            
        </tr>     
        <tr class="theadColumnWithBackground">
            <td colspan="4" align="right" >
                    &nbsp;
            </td>       
             <td>
                   &nbsp;  <%--Bonus--%>
            </td>       
             <td>
                    Net Payable
            </td> 
                       
            <td class="HiddenClass">
                    Net Expense
            </td> 
            <td class="HiddenClass">
                    Net Income
            </td>            
             
        </tr>
        <tr  >
            <td colspan="4" align="right">                
                
                               
            </td>
            <td> 
             <asp:Button ID="calulate"  runat="server" Text="Calculate" onclick="Calculate_Click" ValidationGroup="summaryGroup"  />               
                <%--<asp:Textbox ID="txtBonus" runat="server" Enabled="false" Width="50px" Text='<%# Bind("Bonus") %>'   />--%>            
            </td>
            <td>                
                <asp:Textbox ID="TxtNetPayable" runat="server" Enabled="false" Width="50px" Text='<%# Bind("NetPayable") %>'  />            
            </td>
             
            <td class="HiddenClass">                 
                <asp:Textbox ID="TxtNetExpense"  runat="server" Enabled="false" Width="50px" Text='<%# Bind("NetExpense") %>' />
            </td>                 
            <td class="HiddenClass" >                 
                <asp:Textbox ID="TxtNetIncome"  runat="server" Enabled="false" Width="50px" Text='<%# Bind("NetIncome") %>'  />
            </td>
            
        </tr>
    </table>
    </InsertItemTemplate>
    <EditItemTemplate>
    <table  class="TableClass" >  
        <tr class="theadColumnWithBackground">
            <td colspan="6" align="center" >
                    <asp:Label ID="EmpType" Text='<%# Bind("EmpTypeName") %>' runat="server"></asp:Label>
            </td>  
        </tr>    
        <tr  class="theadColumn">
            <td>
               Month
            </td>            
            <td>
                Year
            </td>
            <td>
                DaysWorked
            </td>            
            <td>
               Weekly Offs
            </td>     
            <td>
              Govt\Comp
            </td>   
            <td>
                OT Hours
            </td>      
        </tr>
        <tr >
            <td>                
                <asp:Label ID="lblID" runat="server" CssClass="HiddenClass" Text='<%# Bind("ID") %>'></asp:Label>
                <asp:Label ID="txtMonth" runat="server"></asp:Label>                
            </td>
            <td>
               <asp:Label ID="txtYear" runat="server"></asp:Label>                 
            </td>
            <td>
                <asp:Textbox ID="TxtDaysWorked" runat="server" Text='<%# Bind("DaysWorked") %>' Width="35px" CausesValidation="true"  /> /
                <asp:Textbox ID="TxtTotalDays" runat="server" Enabled="false" Width="35px"   />                
                <asp:RegularExpressionValidator runat="server" ID="regex" ControlToValidate="TxtDaysWorked" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,2}(\.\d{0,1})?$"   Text="*" ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>                                                
                <asp:CompareValidator runat="server" id="CompareValidator3" ControlToValidate="TxtDaysWorked" 
                     ControlTocompare="TxtTotalDays" Operator="LessThanEqual" Type="Double" Display="Dynamic" ValidationGroup="summaryGroup"
                     Errormessage="* The working days cannot be greater than the month's days!" Text="*"  />   
            </td>
            <td>
               <asp:Textbox ID="TxtWeeklyOffs" runat="server" Enabled="false" Width="20px" />
            </td>
            <td>
               <asp:Textbox ID="TxtGovtCompHoliday" runat="server" Text='<%# Bind("GovtCompHoliday") %>' Width="35px"   CausesValidation="true" />
              <asp:RegularExpressionValidator runat="server" ID="regex2" ControlToValidate="TxtGovtCompHoliday" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,2}(\.\d{0,1})?$" Text="*"  ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td>  
            <td>
               <asp:Textbox ID="TxtOTHours" runat="server" Text='<%# Bind("OTHours") %>' Width="35px" CausesValidation="true"  />
                <asp:RegularExpressionValidator runat="server" ID="regex3" ControlToValidate="TxtOTHours" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,3}(\.\d{0,1})?$" Text="*"  ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td>
        </tr>
        <tr class="theadColumnWithBackground">
            <td colspan="3" align="center" >
                    Leaves
            </td>              
            <td colspan="3" align="center" >
                    Amount
            </td>  
        </tr>  
        <tr class="theadColumn">
            <td>
                Current / Balance 
            </td>            
            <td>
                Paid 
            </td>
            <td>
                UnPaid  
            </td>
            <td>
                Paid Days
            </td>
            <td>
                Paid Amount 
            </td>
            <td>
               OT Paid
            </td>             
        </tr>
        <tr  >
            <td>   
                <asp:CheckBox ID="chkLeavesOverride" runat="server" Enabled ="true" Checked='<%# Bind("LeavesOverride") %>' />             
                <asp:Textbox ID="TxtLeavesBalanceCurrent" runat="server" Enabled="false" Text='<%# Bind("LeavesBalanceCurrent") %>' Width="35px"  /> /                
                <asp:Textbox ID="TxtLeavesBalancePrevious" runat="server" Width="35px"  Text='<%# Bind("LeavesBalancePrevious") %>' Enabled="false" /> 
            </td>            
             <td>
                <asp:Textbox ID="TxtPaidLeavesTaken" runat="server" Text='<%# Bind("PaidLeavesTaken") %>' Width="35px"  />
               <asp:CompareValidator ID="compareBalanceLeaves" runat="server" ControlToCompare="TxtPaidLeavesTaken"
                   Text="*" ErrorMessage="Paid leaves cannot be greater than balance" Operator="GreaterThanEqual" Type="Currency" 
                  ControlToValidate="TxtLeavesBalancePrevious" ValidationGroup="summaryGroup" Display="Dynamic"  ></asp:CompareValidator>
            </td>
            <td>
                <asp:Textbox ID="TxtUnPaidLeavesTaken" Width="35px" runat="server" Text='<%# Bind("UnPaidLeavesTaken") %>' Enabled="false" />                
            </td>
            <td>
               <asp:Textbox ID="TxtTotalPaidDays" runat="server" Width="35px" Enabled="false"  Text='<%# Bind("TotalPayableDays") %>'  />
            </td>
            
            <td>
                <asp:Textbox ID="TxtAmountForPaidDays" runat="server" Width="50px"  Text='<%# Bind("PaidAmount") %>'  Enabled="false"  />                
            </td>
             <td>
               <asp:Textbox ID="TxtOTAmount" runat="server" Enabled="false" Text='<%# Bind("OTAmount") %>' Width="50px"  />
            </td>
        </tr>
        <tr class="theadColumnWithBackground" >
            <td colspan="3"   >
                    Advance
            </td>  
           
            <td colspan="3"   >
                    Taxes & Deduction
            </td>  
        </tr>
        <tr class="theadColumn">
            <td>
               Current / Pending 
            </td>            
            <td>
                New 
            </td>
            <td>
                Deduction
            </td>
            <td  >
               PF     
            </td>                       
            <td  >
                ESIC           
            </td>
            <td  >
               Prof Tax           
            </td>
        </tr>
        <tr  >             
            <td>                                 
                <asp:Textbox ID="TxtAdvBalanceCurrent" runat="server"  Text='<%# Bind("AdvBalanceCurrent") %>' Enabled="false" Width="50px"  />/
                <asp:Textbox ID="TxtAdvBalancePrevious" runat="server"  Text='<%# Bind("AdvBalancePrevious") %>' Enabled="false" Width="50px"  />
            </td> 
             <td>                 
                <asp:Textbox ID="TxtAdvBalanceAdd" runat="server"   Text='<%# Bind("AdvBalanceAdd") %>' Width="50px" CausesValidation="true"  />
                <asp:RegularExpressionValidator runat="server" ID="regex4" ControlToValidate="TxtAdvBalanceAdd" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,4}(\.\d{0,2})?$" Text="*"  ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td> 
            <td>                 
                <asp:Textbox ID="TxtAdvBalanceDeduct" runat="server"    Text='<%# Bind("AdvBalanceDeduct") %>' Width="50px"  CausesValidation="true" />
                <asp:RegularExpressionValidator runat="server" ID="regex5" ControlToValidate="TxtAdvBalanceDeduct" Display="Dynamic" ValidationGroup="summaryGroup"
                ValidationExpression="^\d{1,4}(\.\d{0,2})?$" Text="*"  ErrorMessage="* Should be positive and digits or decimals ( one place) only eg:28.8"/>              
            </td> 
            <td>                 
                <asp:Textbox ID="TxtPF1" runat="server" Enabled="false" Width="50px"  Text='<%# Bind("PFDeduction") %>'  />
             </td>   
             <td>             
                <asp:Textbox ID="TxtPF2" runat="server" Enabled="false" Width="50px"  Text='<%# Bind("ESICDeduction") %>'  />
            </td>   
             <td>                 
                <asp:Textbox ID="TxtPF3" runat="server" Enabled="false" Width="50px"  Text='<%# Bind("ProfTaxDeduction") %>'  />
            </td>             
            
        </tr>     
        <tr class="theadColumnWithBackground">
            <td colspan="4" align="right" >
                    &nbsp;
            </td>       
             <td>
                   &nbsp;  <%--Bonus--%>
            </td>       
             <td>
                    Net Payable
            </td> 
                       
            <td class="HiddenClass">
                    Net Expense
            </td> 
            <td class="HiddenClass">
                    Net Income
            </td>            
             
        </tr>
        <tr  >
            <td colspan="4" align="right">    &nbsp;
            </td>
            <td> 
             <asp:Button ID="calulate"  runat="server" Text="Calculate" onclick="Calculate_Click" ValidationGroup="summaryGroup"  />               
                <%--<asp:Textbox ID="txtBonus" runat="server" Enabled="false" Width="50px" Text='<%# Bind("Bonus") %>'   />--%>            
            </td>
            <td>                
                <asp:Textbox ID="TxtNetPayable" runat="server" Enabled="false" Width="50px" Text='<%# Bind("NetPayable") %>'  />            
            </td>
             
            <td class="HiddenClass">                 
                
            </td>                 
            <td class="HiddenClass" >                 
               
            </td>
            
        </tr>
    </table>
    </EditItemTemplate>
    <ItemTemplate>
    <table class="TableClass" >
        <tr class="theadColumnWithBackground">
            <td colspan="6" align="center" >
                  <asp:Label ID="EmpType" Text='<%# Eval("EmpTypeName") %>' runat="server"></asp:Label>
            </td>  
        </tr>
        <tr class="theadColumn">
            <td>
               Month
            </td>            
            <td>
                Year
            </td>
            <td>
                DaysWorked
            </td>            
            <td>
               Weekly Offs
            </td>     
            <td>
              G\C\Holiday
            </td>   
            <td>
                OT Hours
            </td>      
        </tr>
        <tr  >
            <td>
                <asp:Label ID="TxtMonth" runat="server" Text='<%# Eval("MonthName") %>' />
            </td>
            <td>
               <asp:Label ID="TxtYear" runat="server" Text='<%# Eval("YearName") %>' />
            </td>
            <td>
                <asp:Label ID="TxtDaysWorked" runat="server" Text='<%# Eval("DaysWorked") %>' />                
            </td>
              <td>
               <asp:Label ID="TxtWeeklyOffs" runat="server"   />
            </td>
            <td>
               <asp:Label ID="TxtGovtCompHoliday" runat="server" Text='<%# Eval("GovtCompHoliday") %>' Width="50px"   CausesValidation="true" />             
            </td>  
            <td>
                <asp:Label ID="TxtOTHours" runat="server" Text='<%# Eval("OTHours") %>' />                
            </td>
        </tr>
        <tr class="theadColumnWithBackground">
            <td colspan="3" align="center" >
                    Leaves
            </td>  
            <td>
            </td>
            <td colspan="3" align="center" >
                    Amount
            </td>  
        </tr>  
        <tr class="theadColumn">
            <td>
                Balance 
            </td>            
            <td>
                Paid 
            </td>
            <td>
                UnPaid  
            </td>
            <td>
                Paid Days
            </td>
            <td>
                Paid Amount 
            </td>
            <td>
               OT Paid
            </td>             
        </tr>
        <tr >
             <td>       
                                         
                 <asp:Label ID="TxtLeavesBalanceCurrent" runat="server" Width="30px" Text='<%# Eval("LeavesBalanceCurrent") %>'/> /
                 <asp:Label ID="TxtLeavesBalancePrevious" runat="server" Width="30px" Text='<%# Eval("LeavesBalancePrevious") %>'/>                  
            </td>            
            <td>
                <asp:Label ID="TxtPaidLeavesTaken" runat="server" Width="50px" Text='<%# Eval("PaidLeavesTaken") %>' />               
            </td>            
            <td>
                <asp:Label ID="TxtUnPaidLeavesTaken" Width="50px" runat="server" Text='<%# Bind("UnPaidLeavesTaken") %>'/>                
            </td>
            
            <td>
               <asp:Label ID="TxtTotalPaidDays" runat="server" Text='<%# Eval("TotalPayableDays") %>' />
            </td>
            
            <td>
                <asp:Label ID="TxtAmountForPaidDays" runat="server" Text='<%# Eval("PaidAmount") %>'  />                
            </td>
             <td>
               <asp:Label ID="TxtOTAmount" runat="server"  Text='<%# Eval("OTAmount") %>' />
            </td>
        </tr>
        <tr class="theadColumnWithBackground">
            <td colspan="3" align="center" >
                    Advance
            </td>  
           
            <td colspan="3" align="center" >
                    Taxes & Deduction
            </td>  
        </tr>
         <tr class="theadColumn">
            <td>
               Current / Pending 
            </td>            
            <td>
                New 
            </td>
            <td>
                Deduction
            </td>
            <td  >
               PF     
            </td>                       
            <td  >
                ESIC           
            </td>
            <td  >
               Prof Tax           
            </td>
        </tr>
        <tr>             
            <td>                 
                <asp:Label ID="TxtAdvBalanceCurrent" runat="server"    Width="50px" Text='<%# Eval("AdvBalanceCurrent") %>'  /> /
                <asp:Label ID="TxtAdvBalancePrevious" runat="server"    Width="50px" Text='<%# Eval("AdvBalancePrevious") %>'  />
            </td> 
             <td>                 
                <asp:Label ID="TxtAdvBalanceAdd" runat="server" Text='<%# Eval("AdvBalanceAdd") %>' />                  
            </td> 
            <td>        
                <asp:Label ID="TxtAdvBalanceDeduct" runat="server" Text='<%# Eval("AdvBalanceDeduct") %>' />                         
            </td> 
            <td>                 
                <asp:Label ID="TxtPF1" runat="server"  Width="50px"  Text='<%# Eval("PFDeduction") %>'  />
             </td>   
             <td>             
                <asp:Label ID="TxtPF2" runat="server"  Width="50px"  Text='<%# Eval("ESICDeduction") %>'  />
            </td>   
             <td>                 
                <asp:Label ID="TxtPF3" runat="server"  Width="50px"  Text='<%# Eval("ProfTaxDeduction") %>'  />
            </td>  
        </tr>        
        
        <tr class="theadColumnWithBackground">
            <td colspan="4" align="right" >
                    &nbsp;
            </td> 
             <td>                
                &nbsp;<%-- Bonus --%>           
            </td>
            <td>
                    Net Payable
            </td> 
            <td class="HiddenClass">
                    Net Expense
            </td> 
            <td class="HiddenClass">
                    Net Income
            </td>  
        </tr>
        <tr >
            <td colspan="4" >                
                  &nbsp;         
            </td> 
             <td>   
             &nbsp;             
               <%-- <asp:Textbox ID="txtBonus" runat="server" Enabled="false" Width="50px" Text='<%# Bind("Bonus") %>'   />--%>            
            </td>
             <td >                
                   <asp:Label ID="TxtNetPayable" runat="server"  Text='<%# Eval("NetPayable") %>'   /> <%--NetPayable --%>          
            </td>    
            <td class="HiddenClass">                 
               &nbsp;
            </td>  
             <td class="HiddenClass">                 
                &nbsp;
            </td>  
                 
        </tr>
    </table>
    </ItemTemplate>
    </asp:TemplateField>
    </Fields>
    </asp:DetailsView>

<%--</ContentTemplate>
</asp:UpdatePanel>--%>

<asp:ObjectDataSource ID="SalaryODS" runat="server" TypeName="Payroll.BusLogic.Salary"
                     DataObjectTypeName="Payroll.Data.SalaryData" SelectMethod="GetSalary"
                       >
  <SelectParameters>
   <asp:QueryStringParameter   ConvertEmptyStringToNull="true" QueryStringField="EmpID" Name="EmpID" Type="String" />
   <asp:QueryStringParameter   ConvertEmptyStringToNull="true" QueryStringField="DMID" Name="DMID" Type="String" />
    <%--<asp:ControlParameter ControlID="empLookup" PropertyName="EmployeeID"  ConvertEmptyStringToNull="true" Name="EmpID" Type="String" />    --%>
    <%--
    UpdateMethod="UpdateSalaryDetails" InsertMethod="InsertSalaryDetails"
    <asp:QueryStringParameter   ConvertEmptyStringToNull="true" QueryStringField="Month" Name="MonthNumber" Type="String" />--%>
 </SelectParameters>
</asp:ObjectDataSource>


</asp:Content>
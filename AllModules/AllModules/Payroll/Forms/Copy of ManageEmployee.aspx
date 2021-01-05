<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ManageEmployee.aspx.cs" 
MasterPageFile="~/Payroll/Site.Master" 
Inherits="Payroll.Forms.ManageEmployee" Theme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1"  %>

 
<asp:Content ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var BasicChecked = false;
    var AllowanceChecked = false;
    function ShowHideBasic(checked) 
    {        
        if (checked) {
            lblBD.style.display = "block";            
            txtBD.style.display = "block";           
            lblBM.style.display = "none";           
            txtBM.style.display = "none";           
        }
        else 
        {
            lblBM.style.display = "block";          
            txtBM.style.display = "block";           
            lblBD.style.display = "none";            
            txtBD.style.display = "none";
        }
        BasicChecked = checked;
    }
    function ShowHideAllowance(checked) {
        if (checked) {            
            lblAD.style.display = "block";          
            txtAD.style.display = "block";          
            lblAM.style.display = "none";          
            txtAM.style.display = "none";
        }
        else {            
            lblAM.style.display = "block";            
            txtAM.style.display = "block";            
            lblAD.style.display = "none";           
            txtAD.style.display = "none";
        }
        AllowanceChecked = checked;
    }

    function ExtraShowHide(checked1, checked2) {
    //Allowance & deductions
        if (checked1) {
            deduct1.style.display = "block";
            deduct2.style.display = "block";
            lblA.style.display = "block";
            lblAD.style.display = "block";
            lblAM.style.display = "block";
            cbA.style.display = "block";
            ShowHideAllowance(AllowanceChecked);
        }
        else {
            deduct1.style.display = "none";
            deduct2.style.display = "none";
            lblA.style.display = "none";
            lblAD.style.display = "none";
            lblAM.style.display = "none";
            cbA.style.display = "none";
            txtAD.style.display = "none";
            txtAM.style.display = "none";

        }
        //Yearly paid leaves & Balance leaves
        if (checked2) 
        {        
            lblYPL.style.display = "block";
            lblBL.style.display = "block";            
            lblYPL1.style.display = "block";
            lblBL1.style.display = "block"; 
        }
        else {
            lblYPL.style.display = "none";
            lblBL.style.display = "none";
            lblYPL1.style.display = "none";
            lblBL1.style.display = "none"; 
        }
    }

    function ShowHidetr(list) {
        var mylist = list.toString();
        switch (mylist) {
            case "1": ExtraShowHide(true,true);
                        break;
            case "2": ExtraShowHide(false,true);
                      break;
            case "3": ExtraShowHide(true, true); 
                        break;
            case "4": ExtraShowHide(false, false); 
                        break;
                
        }
      
    }
     
</script>
<style type="text/css">
.inheritNO
{
  
}
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="main" runat="server">


<%--  
.cal_Theme1 .ajax__calendar_other .ajax__calendar_day,
.cal_Theme1 .ajax__calendar_other .ajax__calendar_year
{
color:White;  
}--%>
  
 <asp:UpdatePanel ID="MUP" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Panel ID="AEP" runat="server" >
    <table style="width: 40%;">
    <tr>            
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Print" /> <%--onclick="btnPrint_Click"--%>
            </td>
            <td>
                <asp:Button ID="btnAmend" runat="server" Text="Amend" OnClick="btnAmend_Click" />
            </td>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"  />
            </td>             
            <td>
                <asp:Button ID="btnInsert" runat="server" Text="Insert" OnClick="btnInsert_Click"  /> <%--OnClick="InsertPurchaseOrder_Click"--%>
            </td>                                          
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"   /> <%--OnClick="CancelUpdateItem_Click"--%>
            </td>                    
        </tr>       
   </table>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
   
<asp:UpdatePanel ID="DUP" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<%--<asp:ValidationSummary ID="vm" runat="server" ValidationGroup="all" />--%>
<asp:DetailsView ID="EmployeeView" runat="server" GridLines="None" Width="100%" DataSourceID="ODOEV"
        AutoGenerateRows="false"  OnDataBound="EmployeeView_DataBound"  >                 
        <Fields>
            <asp:TemplateField>
                <ItemTemplate>
                    <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="margin-left:-5px;">
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblID" runat="server"   Text="Employee ID" />
                            </td>
                            <td>
                                <asp:Label ID="lblDesig" runat="server"  Text="Designation" />
                            </td>                           
                           
                             <td>
                                <asp:Label ID="lblJoinDate"   Text="Joining Date" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblCreatedOn"   Text="Created On" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblModifiedOn"   Text="Modified On" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblActive" runat="server"   Text="Active" />
                            </td>
                        </tr>          
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblid1" runat="server"   Text='<%# Eval("ID") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lbldesig1" runat="server"  Text='<%# Eval("Designation") %>' />
                            </td>                           
                            
                             <td>
                                <asp:Label ID="lbljoindate1"   Text='<%# Eval("JoiningDate", "{0:dd/MM/yyyy}") %>' runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblcreated1"   Text='<%#Eval("CreatedOn", "{0:dd/MM/yyyy}")   %>' runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblmodified1"  Text='<%#Eval("ModifiedOn", "{0:dd/MM/yyyy}")  %>' runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="hdnActive"   Text='<%# Eval("Active") %>' runat="server" CssClass="HiddenClass"/>
                                <asp:CheckBox ID="IsActive" Enabled="false" runat="server" />
                            </td>
                        </tr>                       
                    </table>
                </ItemTemplate>
                <InsertItemTemplate>
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px">
                <tr class="theadColumn">  
                            <td>
                                <asp:Label ID="lbleID" runat="server"   Text="Employee ID" />
                            </td>
                            <td>
                                <asp:Label ID="lblDesig" runat="server"  Text="Designation" />
                            </td>                                                      
                             <td>
                                <asp:Label ID="lblJoinDate"   Text="Joining Date" runat="server" />                               
                            </td>
                            <td>
                                <asp:Label ID="lblCreatedOn"   Text="Created On" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblModifiedOn"   Text="Modified On" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblActive" runat="server"   Text="Active" />
                            </td>
                        </tr>          
                <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblID" runat="server"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDesig" runat="server"  Text='<%# Bind("Designation") %>' />
                            </td>                                  
                             <td style="height:auto !Important;">
                                <asp:TextBox ID="txtJoinDate"  Text='<%# Bind("JoiningDate") %>'   runat="server"  />
                                
                                <cc1:MaskedEditExtender ID="jdE" runat="server" TargetControlID="txtJoinDate" CultureName="en-GB" 
                               MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" AcceptNegative="None" ></cc1:MaskedEditExtender>
                               
                               <cc1:MaskedEditValidator ID="jdV" runat="server" ControlToValidate="txtJoinDate" 
                                ControlExtender="jdE"  Display="Dynamic" InvalidValueMessage="Invalid Date"></cc1:MaskedEditValidator>
                            </td>
                            <td>
                                <asp:Label ID="txtCreatedOn"   Text='<%# Bind("CreatedOn") %>' runat="server" /> 
                            </td>
                             <td>
                                <asp:Label ID="txtModifiedOn"   Text='<%# Bind("ModifiedOn") %>' runat="server" /> 
                             </td>
                             <td>
                                <asp:CheckBox ID="IsActive" Enabled="true" runat="server" />
                             </td>
                             
                        </tr>
                 </table>
                </InsertItemTemplate>
                <EditItemTemplate>
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="margin-left:-3px;">
                <tr class="theadColumn">
                             <td>
                                <asp:Label ID="lbleID" runat="server"   Text="Employee ID" />
                            </td>
                            <td>
                                <asp:Label ID="lblDesig" runat="server"  Text="Designation" />
                            </td>                                                     
                             <td>
                                <asp:Label ID="lblJoinDate"   Text="Joining Date" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblCreatedOn"   Text="Created On" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblModifiedOn"   Text="Modified On" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblActive" runat="server"   Text="Active" />
                            </td>
                        </tr>
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblID" runat="server"  Text='<%# Bind("ID") %>'    />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDesig" runat="server"  Text='<%# Bind("Designation") %>' />
                            </td>                               
                             <td>
                               <asp:TextBox ID="txtJoinDate"  Text='<%# Bind("JoiningDate", "{0:dd/MM/yyyy}")%>'   runat="server"  />
                                
                                <cc1:MaskedEditExtender ID="jdE" runat="server" TargetControlID="txtJoinDate" CultureName="en-GB" 
                                MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" AcceptNegative="None" ></cc1:MaskedEditExtender>
                               
                               <cc1:MaskedEditValidator ID="jdV" runat="server" ControlToValidate="txtJoinDate" 
                                ControlExtender="jdE"  Display="Dynamic" InvalidValueMessage="Invalid Date"></cc1:MaskedEditValidator>
                            
                            </td>
                            <td>
                                <asp:Label ID="txtCreatedOn"   Text='<%# Bind("CreatedOn", "{0:dd/MM/yyyy}")%>'  runat="server" />                                
                            </td>
                             <td>
                                <asp:Label ID="txtModifiedOn"   Text='<%# Bind("ModifiedOn", "{0:dd/MM/yyyy}") %>' runat="server" />                                
                            </td>
                            <td>
                                <asp:Label ID="hdnActive"   Text='<%# Eval("Active") %>' runat="server" CssClass="HiddenClass"/>
                                <asp:CheckBox ID="IsActive" Enabled="true" runat="server" />
                            </td>
                        </tr>
                 </table>
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
</asp:DetailsView>

<asp:DetailsView ID="EmployeeDetailsView" runat="server" GridLines="None" Width="100%" DataSourceID="ODSEDV"
        AutoGenerateRows="false"  OnDataBound="EmployeeDetailsView_DataBound" >                 
        <Fields>
            <asp:TemplateField>
                <ItemTemplate>
                    <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="margin-left:-5px;">
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblFname" runat="server"   Text="First Name" />
                            </td>
                            <td>
                                <asp:Label ID="lblMname" runat="server"  Text="Middle Name" />
                            </td>                           
                             <td>
                                <asp:Label ID="lblLname" runat="server" Text="Last Name" />
                            </td>
                            <td>
                                <asp:Label ID="lblBirthDate"   Text="Birth Date" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblGender"   Text="Gender" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblMarried"   Text="Married" runat="server" />
                            </td>
                           
                        </tr>          
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblfname1" runat="server"   Text='<%# Eval("FirstName") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblmname1" runat="server"  Text='<%# Eval("MiddleName") %>' />
                            </td>                           
                             <td>
                                <asp:Label ID="lbllname1" runat="server" Text='<%# Eval("LastName") %>' />                                
                            </td>
                             <td>
                                <asp:Label ID="lbldob1"  Text='<%# Eval("BirthDate", "{0:dd/MM/yyyy}") %>' runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="hdnGender"   Text='<%# Eval("Gender") %>' runat="server"  />
                               <%-- <asp:Label ID="lblGender1" CssClass="display:none"   runat="server" />--%>
                            </td>
                            <td>
                                <asp:Label ID="hdnMarried"   Text='<%# Eval("Married") %>' runat="server" CssClass="HiddenClass"/>
                                <asp:CheckBox Enabled="false" runat="server" ID="IsMarried" />
                            </td>
                            
                        </tr>                       
                    </table>
                </ItemTemplate>
                <InsertItemTemplate>
               <table  class="TableClass"  cellpadding="0px" cellspacing="0px">
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblFname" runat="server"   Text="First Name" />
                            </td>
                            <td>
                                <asp:Label ID="lblMname" runat="server"  Text="Middle Name" />
                            </td>                           
                             <td>
                                <asp:Label ID="lblLname" runat="server" Text="Last Name" />
                            </td>
                            <td>
                                <asp:Label ID="lblBirthDate"   Text="Birth Date" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblGender"   Text="Gender" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblMarried"   Text="Married" runat="server" />
                            </td>
                           
                        </tr>          
                        <tr class="theadColumn">
                            <td>
                                <asp:TextBox ID="txtfname" runat="server"   Text='<%# Bind("FirstName") %>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtmname" runat="server"  Text='<%# Bind("MiddleName") %>' />
                            </td>                           
                             <td>
                                <asp:TextBox ID="txtlname" runat="server" Text='<%# Bind("LastName") %>' />                                
                            </td>
                             <td>
                                <asp:TextBox ID="txtdob"  Text='<%# Bind("BirthDate") %>' runat="server" />
                              <cc1:MaskedEditExtender ID="tdE" runat="server" TargetControlID="txtdob" CultureName="en-GB" 
                               MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" AcceptNegative="None" ></cc1:MaskedEditExtender>
                               
                               <cc1:MaskedEditValidator ID="ndV" runat="server" ControlToValidate="txtdob" 
                                ControlExtender="tdE"  Display="Dynamic" InvalidValueMessage="Invalid Date"></cc1:MaskedEditValidator>                            </td>                           
                             <td style="border-style:none">
                                 <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" BorderStyle="None">
                                    <asp:ListItem Value="True" Selected="True" >Male</asp:ListItem>                                                                        
                                    <asp:ListItem Value="False">Female</asp:ListItem>
                                </asp:RadioButtonList>                                
                            </td>
                            <td>
                            <asp:CheckBox ID="IsMarried" runat="server" />                               
                            </td>                            
                        </tr>                       
                    </table>
                </InsertItemTemplate>
                <EditItemTemplate>
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px">
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="lblFname" runat="server"   Text="First Name" />
                            </td>
                            <td>
                                <asp:Label ID="lblMname" runat="server"  Text="Middle Name" />
                            </td>                           
                             <td>
                                <asp:Label ID="lblLname" runat="server" Text="Last Name" />
                            </td>
                            <td>
                                <asp:Label ID="lblBirthDate"   Text="Birth Date" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblGender"   Text="Gender" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblMarried"   Text="Married" runat="server" />
                            </td>
                           
                        </tr>          
                        <tr class="theadColumn">
                            <td>
                                <asp:TextBox ID="txtfname" runat="server"   Text='<%# Bind("FirstName") %>' />
                                <asp:Label ID="lblID" runat="server" CssClass="HiddenClass" Text='<%# Bind("ID") %>'   /> 
                            </td>
                            <td>
                                <asp:TextBox ID="txtmname" runat="server"  Text='<%# Bind("MiddleName") %>' />
                            </td>                           
                             <td>
                                <asp:TextBox ID="txtlname" runat="server" Text='<%# Bind("LastName") %>' />                                
                            </td>
                             <td>
                                <asp:TextBox ID="txtdob"  Text='<%# Bind("BirthDate") %>' runat="server" />
                             <cc1:MaskedEditExtender ID="tdE" runat="server" TargetControlID="txtdob" CultureName="en-GB" 
                               MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" AcceptNegative="None" ></cc1:MaskedEditExtender>
                               
                               <cc1:MaskedEditValidator ID="ndV" runat="server" ControlToValidate="txtdob" 
                                ControlExtender="tdE"  Display="Dynamic" InvalidValueMessage="Invalid Date"></cc1:MaskedEditValidator>
                            </td>                           
                             <td style="width:15%">
                                 <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" >
                                    <asp:ListItem Value="True" Selected="True" >Male</asp:ListItem>
                                    <asp:ListItem Value="False">Female</asp:ListItem>
                                </asp:RadioButtonList>      
                                <asp:Label ID="hdnGender" runat="server" CssClass="HiddenClass" Text='<%# Bind("Gender") %>'   />                          
                            </td>
                            <td>
                            <asp:CheckBox  runat="server" ID="IsMarried" />                            
                            <asp:Label ID="hdnMarried" runat="server" CssClass="HiddenClass" Text='<%# Bind("Married") %>'   />                          
                            </td>                            
                        </tr>                       
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
</asp:DetailsView>

<asp:DetailsView ID="SalaryDetailsView" runat="server" GridLines="None" Width="100%" DataSourceID="ODSSDV"
        AutoGenerateRows="false"  OnDataBound="SalaryDetailsView_DataBound" >                 
        <Fields>
            <asp:TemplateField>
                <ItemTemplate>
                    <table  class="TableClass"  cellpadding="0px" cellspacing="0px">                      
                        <tr class="theadColumn">
                             <td>
                                <asp:Label ID="Label1" runat="server" Text="Employee Type" />
                            </td>                           
                            <td>
                            Basic Daily
                            </td>                            
                            <td id="lblBM">
                                <asp:Label ID="lblBasicMonthly" runat="server"   Text="Basic Monthly" />
                            </td>
                            <td id="lblBD">
                                <asp:Label ID="lblBasicDaily" runat="server"  Text="Basic Daily" />
                            </td>                                 
                             <td>
                            <asp:Label ID="lblAdvancePending"   Text="Advance Pending" runat="server" />
                            </td>            
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate"   Text="OverTime Rate" runat="server" />
                            </td>                                                   
                        </tr>          
                        <tr >
                            <td style="font-weight:normal;">
                                <asp:Label ID="lblemptype1" runat="server" Text='<%# Eval("EmpTypeDescription") %>' />
                                <asp:Label ID="hdnlblemptype" runat="server" Text='<%# Eval("EmpType") %>' CssClass="HiddenClass" />
                            </td>                           
                            <td>
                             <asp:CheckBox ID="cbIsBasicDaily" runat="server"  Enabled="false"  Checked='<%# Eval("IsBasicDaily") %>' />                             
                            </td>
                             <td id="txtBM">
                                <asp:Label ID="lblBasicMonthly1" runat="server"   Text='<%# Eval("BasicMonthly") %>' />
                            </td>
                            <td id="txtBD">
                            <asp:Label ID="lblBasicDaily1" runat="server" Text='<%# Eval("BasicDaily") %>' />                                
                            </td>
                            <td>
                                <asp:Label ID="lblAdvancePending1"    Text='<%# Eval("AdvancePending") %>' runat="server" />
                            </td> 
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate1"   Text='<%# Eval("OverTimeRateDescription") %>' runat="server"  />                               
                            </td>                 
                        </tr>        
                         <tr class="theadColumn" id="deduct1">
                            <td>  
                            <asp:Label ID="lblDeductProfTax"   Text="Deduct Prof Tax " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblProfessionalTaxPercent"   Text="Prof Tax %" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblDeductESIC"   Text="Deduct ESIC " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblESICTaxPercent"   Text="ESIC Tax %" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblDeductPF"   Text="Deduct PF" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblProvidentFundPercent"   Text="PF %" runat="server" />                            
                            </td>
                                                   
                         </tr>     
                         <tr id="deduct2">
                          <td>
                                <asp:CheckBox ID="cbDeductProfTax" Enabled="false"   Checked='<%# Eval("DeductProfTax") %>' runat="server" />
                          </td>
                           <td>
                                <asp:Label ID="lblProfessionalTaxPercent1"    Text='<%# Eval("ProfessionalTaxPercent") %>' runat="server" />
                          </td>
                            <td>
                                <asp:CheckBox ID="cbDeductESIC"  Enabled="false"  Checked='<%# Eval("DeductESIC") %>' runat="server" />
                          </td>
                           <td>
                                <asp:Label ID="lblESICTaxPercent1"    Text='<%# Eval("ESICTaxPercent") %>' runat="server" />
                          </td>                          
                          <td>
                                <asp:CheckBox ID="cbDeductPF" Enabled="false"   Checked='<%# Eval("DeductPF") %>' runat="server" />
                          </td>   
                          <td>
                                <asp:Label ID="lblProvidentFundPercent1"    Text='<%# Eval("ProvidentFundPercent") %>' runat="server" />
                          </td>                               
                                              
                         </tr> 
                         <tr class="theadColumn">
                             <td id="lblA">
                            Allowance Daily
                            </td>
                            <td id="lblAM">
                                <asp:Label ID="lblAllowanceMonthly" runat="server" Text="Allowance Monthly" />
                            </td>                            
                            <td id="lblAD">
                                <asp:Label ID="lblAllowanceDaily"   Text="Allowance Daily" runat="server" />
                            </td>   
                            <td id="lblBL">
                                <asp:Label ID="lblBalanceLeaves"   Text="Leaves Balance" runat="server" />
                            </td>                            
                             <td id="lblYPL">
                                <asp:Label ID="lblYearlyPaidLeaves"   Text="Yearly Paid Leaves" runat="server" />
                            </td>                    
                         </tr>
                         <tr >
                              <td id="cbA">
                                <asp:CheckBox ID="cbIsAllowanceDaily" runat="server" Enabled="false"  Checked='<%# Bind("IsAllowanceDaily") %>' />
                             </td>                      
                             <td id="txtAM">
                                    <asp:Label ID="lblAllowanceMonthly1" runat="server"  Text='<%# Eval("AllowanceMonthly") %>' />                    
                             </td>
                             <td id="txtAD">
                                    <asp:Label ID="lblAllowanceDaily1"  Text='<%# Eval("AllowanceDaily") %>' runat="server" />
                             </td>
                             <td id="lblBL1">
                                    <asp:Label ID="lblBalanceLeaves1"   Text='<%# Eval("BalanceLeaves") %>' runat="server" />                                
                             </td>                           
                             <td id="lblYPL1">
                                <asp:Label ID="lblYearlyPaidLeaves1"    Text='<%# Eval("YearlyPaidLeaves") %>' runat="server" />
                             </td>               
                         </tr>                               
                    </table>
                </ItemTemplate>
                <InsertItemTemplate>
               <table  class="TableClass"  cellpadding="0px" cellspacing="0px">                        
                         <tr class="theadColumn">    
                            <td>
                                <asp:Label ID="lblEmpType" runat="server" Text="Employee Type" />
                            </td>                                                                                              
                            <td>
                            Basic Daily
                            </td>                            
                            <td id="lblBM">
                                <asp:Label ID="lblBasicMonthly" runat="server"   Text="Basic Monthly" />
                            </td>
                            <td id="lblBD">
                                <asp:Label ID="lblBasicDaily" runat="server"  Text="Basic Daily" />
                            </td>                           
                            <td>
                                <asp:Label ID="lblAdvancePending"   Text="Advance Pending" runat="server" />
                            </td> 
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate"   Text="OverTime Rate" runat="server" />
                            </td>                                                    
                        </tr>           
                        <tr >
                             <td>
                                <asp:DropDownList ID="ddlEmpType" runat="server" onchange="ShowHidetr(this.options[this.selectedIndex].value);"  />   <%--onselectedindexchanged="ddlEmpType_SelectedIndexChanged" AutoPostBack="true"--%>                              
                            </td>    
                             <td>
                             <asp:CheckBox ID="cbIsBasicDaily" runat="server" onclick="ShowHideBasic(this.checked);" Checked='<%# Bind("IsBasicDaily") %>' />
                                                     
                             </td>
                             <td id="txtBM">
                                <asp:TextBox ID="txtBasicMonthly" runat="server"   Text='<%# Bind("BasicMonthly") %>' />
                            </td>
                             <td id="txtBD">
                                <asp:TextBox ID="txtBasicDaily" runat="server" Text='<%# Bind("BasicDaily") %>' />                                
                            </td>
                            <td>
                             <asp:Label ID="lblAdvancePending1" runat="server" Text='<%# Eval("AdvancePending") %>' />                                                              
                            </td>
                             <td colspan="2">                                
                                <asp:DropDownList ID="ddlOverTimeRate" runat="server"   />                                
                            </td>                                                                           
                        </tr>                        
                        
                           <tr class="theadColumn" id="deduct1">
                            <td>  
                            <asp:Label ID="lblDeductProfTax"   Text="Deduct Prof Tax " runat="server" />                            
                            </td>                     
                            <td>  
                            <asp:Label ID="lblProfessionalTaxPercent"   Text="Prof Tax " runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblDeductESIC"   Text="Deduct ESIC " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblESICTaxPercent"   Text="ESIC " runat="server" />                            
                            </td>
                              <td>  
                            <asp:Label ID="lblDeductPF"   Text="Deduct PF" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblProvidentFundPercent"   Text="PF" runat="server" />                            
                            </td>
                                                                                                                                              
                         </tr>  
                         <tr id="deduct2" >
                            <td>
                                <asp:CheckBox ID="cbDeductProfTax"    Checked='<%#Eval("DeductProfTax") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductProfTax"))  %>' 
                                 OnCheckedChanged="ProfTaxchecked" runat="server" AutoPostBack="true" />
                          </td>
                           <td>
                                <asp:Label ID="lblProfessionalTaxPercent1"    Text='<%# Eval("ProfessionalTaxPercent") %>' runat="server" /> Rupees
                          </td>
                           
                           <td>
                                <asp:CheckBox ID="cbDeductESIC"    Checked='<%#Eval("DeductESIC") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductESIC"))   %>' 
                                runat="server" OnCheckedChanged="ESICchecked" AutoPostBack="true" />
                          </td>
                           <td>
                                <asp:Label ID="lblESICTaxPercent1"    Text='<%# Eval("ESICTaxPercent") %>' runat="server" />%
                          </td>
                          <td>
                                <asp:CheckBox ID="cbDeductPF"    Checked='<%#Eval("DeductPF") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductPF"))   %>' 
                                runat="server" OnCheckedChanged="PFchecked" AutoPostBack="true" />
                          </td>   
                          <td>
                                <asp:Label ID="lblProvidentFundPercent1"    Text='<%# Eval("ProvidentFundPercent") %>' runat="server" />%
                          </td>                   
                                     
                         </tr> 
                       
                        
                         <tr class="theadColumn"    >
                            <td id="lblA">
                            Allowance Daily
                            </td>
                            <td id="lblAM" >
                                <asp:Label ID="lblAllowanceMonthly" runat="server" Text="Allowance Monthly" />
                            </td>                            
                            <td id="lblAD" >
                                <asp:Label ID="lblAllowanceDaily"   Text="Allowance Daily" runat="server" />
                            </td>
                             <td id="lblBL">
                                <asp:Label ID="lblBalanceLeaves"   Text="Leaves Balance" runat="server" />
                            </td>                            
                             <td id="lblYPL">
                                <asp:Label ID="lblYearlyPaidLeaves"   Text="Yearly Paid Leaves" runat="server" />
                            </td> 
                         </tr>
                          <tr>
                          <td id="cbA">
                             <asp:CheckBox ID="cbIsAllowanceDaily" runat="server" onclick="ShowHideAllowance(this.checked);" Checked='<%# Bind("IsAllowanceDaily") %>' />
                             </td>
                             <td id="txtAM">
                                <asp:TextBox ID="txtAllowanceMonthly" runat="server"  Text='<%# Bind("AllowanceMonthly") %>' Width="100px" />
                            </td>                                                       
                            <td id="txtAD">
                                <asp:TextBox ID="txtAllowanceDaily" runat="server" Text='<%# Bind("AllowanceDaily") %>'  Width="100px" />                                
                            </td>
                            <td id="lblBL1">
                             <asp:TextBox ID="txtBalanceLeaves" runat="server" Text='<%# Bind("BalanceLeaves") %>' />                                                              
                            </td>                           
                           <td id="lblYPL1">
                             <asp:TextBox ID="txtYearlyPaidLeaves" runat="server" Text='<%# Bind("YearlyPaidLeaves") %>' />                                                              
                            </td>                
                         </tr>                          
                    </table>
                </InsertItemTemplate>
                <EditItemTemplate>
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px">
                        
                       <tr class="theadColumn"> 
                            <td>
                                <asp:Label ID="lblEmpType" runat="server" Text="Employee Type" />
                            </td>                                                    
                            <td>
                            Basic Daily
                            </td>                            
                            <td id="lblBM">
                                <asp:Label ID="lblBasicMonthly" runat="server"   Text="Basic Monthly" />
                            </td>
                            <td id="lblBD">
                                <asp:Label ID="lblBasicDaily" runat="server"  Text="Basic Daily" />
                            </td>
                             <td>
                            <asp:Label ID="lblAdvancePending"   Text="Advance Pending" runat="server" />
                            </td>
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate"   Text="OverTime Rate" runat="server" />
                            </td>                                                   
                        </tr>          
                       <tr class="theadColumn">
                            <td>
                                <asp:DropDownList ID="ddlEmpType" runat="server"  onchange="ShowHidetr(this.options[this.selectedIndex].value);" />
                                <asp:Label ID="hdnEmptype" runat="server" CssClass="HiddenClass" Text='<%# Bind("EmpType") %>'   />
                            </td>  
                             <td>
                             <asp:CheckBox ID="cbIsBasicDaily" runat="server" onclick="ShowHideBasic(this.checked);" Checked='<%# Bind("IsBasicDaily") %>' />
                             <asp:Label ID="lblID" runat="server" CssClass="HiddenClass" Text='<%# Bind("ID") %>'   />                                      
                             </td>
                             <td id="txtBM">
                                <asp:TextBox ID="txtBasicMonthly" runat="server"   Text='<%# Bind("BasicMonthly") %>' />
                            </td>
                             <td id="txtBD">
                                <asp:TextBox ID="txtBasicDaily" runat="server" Text='<%# Bind("BasicDaily") %>' />                                
                            </td>                            
                             <td>
                             <asp:Label ID="lblAdvancePending1" runat="server" Text='<%# Bind("AdvancePending") %>' />                                                              
                            </td>
                             <td colspan="2">                          
                                <asp:DropDownList ID="ddlOverTimeRate" runat="server"   />  
                                <asp:Label ID="hdnOverTimeRate" runat="server" CssClass="HiddenClass" Text='<%# Bind("OverTimeRate") %>'   />                                                              
                            </td>                                                          
                                                                             
                        </tr>    
                       <tr class="theadColumn" id="deduct1" >
                            <td>  
                            <asp:Label ID="lblDeductProfTax"   Text="Deduct Prof Tax " runat="server" />                            
                            </td>                     
                            <td>  
                            <asp:Label ID="lblProfessionalTaxPercent"   Text="Prof Tax " runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblDeductESIC"   Text="Deduct ESIC " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblESICTaxPercent"   Text="ESIC " runat="server" />                            
                            </td>
                              <td>  
                            <asp:Label ID="lblDeductPF"   Text="Deduct PF" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblProvidentFundPercent"   Text="PF" runat="server" />                            
                            </td>
                         </tr>     
                         <tr id="deduct2">                          
                          <td>
                                <asp:CheckBox ID="cbDeductProfTax"    Checked='<%# Bind("DeductProfTax") %>' 
                                runat="server" OnCheckedChanged="ProfTaxchecked" AutoPostBack="true" />
                          </td>
                           <td>
                                <asp:Label ID="lblProfessionalTaxPercent1"    Text='<%# Bind("ProfessionalTaxPercent") %>' runat="server" />Rupees
                          </td>
                           <td>
                                <asp:CheckBox ID="cbDeductESIC"    Checked='<%# Bind("DeductESIC") %>' 
                                runat="server" OnCheckedChanged="ESICchecked" AutoPostBack="true" />
                          </td>
                           <td>
                                <asp:Label ID="lblESICTaxPercent1"    Text='<%# Bind("ESICTaxPercent") %>' runat="server" />%
                          </td>
                            
                          <td>
                                <asp:CheckBox ID="cbDeductPF"    Checked='<%# Bind("DeductPF") %>' 
                                runat="server" OnCheckedChanged="PFchecked" AutoPostBack="true" />
                          </td>  
                          <td>
                                <asp:Label ID="lblProvidentFundPercent1"    Text='<%# Bind("ProvidentFundPercent") %>' runat="server" />%
                          </td>                                                      
                         </tr> 
                         <tr class="theadColumn">
                            <td id="lblA">
                            Allowance Daily
                            </td>
                            <td id="lblAM">
                                <asp:Label ID="lblAllowanceMonthly" runat="server" Text="Allowance Monthly" />
                            </td>                            
                            <td id="lblAD">
                                <asp:Label ID="lblAllowanceDaily"   Text="Allowance Daily" runat="server" />
                            </td>
                          <td id="lblBL">
                                <asp:Label ID="lblBalanceLeaves"   Text="Leaves Balance" runat="server" />
                            </td>                           
                          <td id="lblYPL">
                                <asp:Label ID="lblYearlyPaidLeaves"   Text="Yearly Paid Leaves" runat="server" />
                            </td>    
                         </tr>
                          <tr>
                            <td id="cbA">
                             <asp:CheckBox ID="cbIsAllowanceDaily" runat="server" onclick="ShowHideAllowance(this.checked);" Checked='<%# Bind("IsAllowanceDaily") %>' />
                            </td>
                            <td id="txtAM">
                                <asp:TextBox ID="txtAllowanceMonthly" runat="server"  Text='<%# Bind("AllowanceMonthly") %>' Width="150px" />
                            </td>                           
                            
                            <td id="txtAD">
                                <asp:TextBox ID="txtAllowanceDaily" runat="server" Text='<%# Bind("AllowanceDaily") %>' />                                
                            </td>
                            <td id="lblBL1">
                             <asp:TextBox ID="txtBalanceLeaves" runat="server" Text='<%# Bind("BalanceLeaves") %>' />                                                              
                            </td>                            
                            <td id="lblYPL1">
                             <asp:TextBox ID="txtYearlyPaidLeaves" runat="server" Text='<%# Bind("YearlyPaidLeaves") %>' />                                                              
                            </td>                
                         </tr>                
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
</asp:DetailsView>
<asp:ObjectDataSource ID="ODOEV" runat="server" TypeName="Payroll.BusLogic.Employee" ConflictDetection="CompareAllValues"
    SelectMethod="GetEmployee" DataObjectTypeName="Payroll.Data.EmployeeData"
    OldValuesParameterFormatString="oOrder">
    <SelectParameters>
    <asp:QueryStringParameter ConvertEmptyStringToNull="true" QueryStringField="ID" Name="ID" Type="String" />    
    </SelectParameters>    
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODSEDV" runat="server" TypeName="Payroll.BusLogic.EmployeeDetails" ConflictDetection="CompareAllValues"
    SelectMethod="GetEmployeeDetails" DataObjectTypeName="Payroll.Data.EmployeeDetailsData" >
    <SelectParameters>
    <asp:QueryStringParameter ConvertEmptyStringToNull="true" QueryStringField="ID" Name="EmpID" Type="String" />    
    </SelectParameters>    
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="ODSSDV" runat="server" TypeName="Payroll.BusLogic.SalaryDetails" ConflictDetection="CompareAllValues"
    SelectMethod="GetSalaryDetails" DataObjectTypeName="Payroll.Data.SalaryDetailsData" >
    <SelectParameters>
    <asp:QueryStringParameter ConvertEmptyStringToNull="true" QueryStringField="ID" Name="EmpID" Type="String" />    
    </SelectParameters>    
</asp:ObjectDataSource>
    

 
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
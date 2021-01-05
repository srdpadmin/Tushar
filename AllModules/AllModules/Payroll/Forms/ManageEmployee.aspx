<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ManageEmployee.aspx.cs" 
MasterPageFile="~/Payroll/Payroll.Master"
Inherits="Payroll.Forms.ManageEmployee" Theme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1"  %> 
<%@ Register Src="~/Payroll/Controls/ucLeaveTransactions.ascx" TagPrefix="uc" TagName="LT" %>
<%@ Register Src="~/Payroll/Controls/ucAdvanceTransactions.ascx" TagPrefix="uc" TagName="AT" %>

 
<asp:Content ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var BasicChecked = false;// document.getElementById("<%= BasicChecked.ClientID %>").value; 
    var AllowanceChecked =false;// document.getElementById("<%= AllowanceChecked.ClientID %>").value; 
    //var forceBasicChecked = document.getElementById("<%= BasicChecked.ClientID %>");
    var show ='';
    var hide = 'none';
  
    function ShowHideBasic(checked) {
       
        if (checked) 
        {
            lblBD.style.display = "";        // Changed value 'block' to empty to make it work for all browsers
            txtBD.style.display = "";           
            lblBM.style.display = "none";           
            txtBM.style.display = "none";           
        }
        else 
        {
            lblBM.style.display = "";          
            txtBM.style.display = "";           
            lblBD.style.display = "none";            
            txtBD.style.display = "none";
        }
       
        if(document.getElementById("<%= BasicChecked.ClientID %>") != null)
        {
        document.getElementById("<%= BasicChecked.ClientID %>").value = checked;
        }
        BasicChecked=checked;
    }
    function ShowHideAllowance(checked) 
    {
        if (checked) {            
            lblAD.style.display = "";          // Changed value 'block' to empty to make it work for all browsers
            txtAD.style.display = "";          
            lblAM.style.display = "none";          
            txtAM.style.display = "none";
        }
        else {            
            lblAM.style.display = "";            
            txtAM.style.display = "";            
            lblAD.style.display = "none";           
            txtAD.style.display = "none";
        }
        if(document.getElementById("<%= AllowanceChecked.ClientID %>") != null)
        {
        document.getElementById("<%= AllowanceChecked.ClientID %>").value = checked;
        }
        AllowanceChecked=checked;
    }

    function ExtraShowHide(checked1, checked2) 
    {
    //Allowance & deductions
     
        if (checked1) 
        {
            deduct1.style.display =show;
            deduct2.style.display = show;
            lblA.style.display = show;
            lblAD.style.display = show;
            lblAM.style.display = show;
            cbA.style.display = show;
            ShowHideAllowance(AllowanceChecked);
        }
        else 
        {
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
            lblYPL.style.display = "";      // Changed value 'block' to empty to make it work for all browsers
            lblBL.style.display = "";            
            lblYPL1.style.display = "";
            lblBL1.style.display = ""; 
        }
        else {
            lblYPL.style.display = "none";
            lblBL.style.display = "none";
            lblYPL1.style.display = "none";
            lblBL1.style.display = "none"; 
        }
    }

    function ShowHidetr(list ) {
        
        var mylist = list.toString();
        switch (mylist) 
        {
            case "1": ExtraShowHide(true, true);
                break;
            case "2": ExtraShowHide(false, true);
                break;
            case "3": ExtraShowHide(true, true);
                break;
            case "4": ExtraShowHide(false, false);
                break;
        }
    }
    function ShowHidetrEdit(list, hdnEmptype) {
        // This is a case during edit we want to save the emptype in hdnEmptype
        // otherwise during postback hdnEmpty will have bind value not chagned value
        if (document.getElementById(hdnEmptype) != null) 
        {
            document.getElementById(hdnEmptype).value = list.toString(); 
        }         
        var mylist = list.toString();
        switch (mylist) {
            case "1": ExtraShowHide(true, true);
                break;
            case "2": ExtraShowHide(false, true);
                break;
            case "3": ExtraShowHide(true, true);
                break;
            case "4": ExtraShowHide(false, false);
                break;
        }
    }  
    function TriggerClick() {
    
        var Triggerbtn = document.getElementById('<%= Trigger.ClientID %>');
        Triggerbtn.click(); 
        } 
     
</script>
<style type="text/css">
.leftAndwidthLess
{  
    text-align:left !Important;
    padding-left:5px;
    
}
 
.leftAndwidth
{
    width:90px; 
    text-align:left;
}
.leftAndwidthMore
{
    width:150px; 
    text-align:left;
}
.AdjustWidth
{
    width:50px;
}
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="main" runat="server">
 <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:60px;"><a href="SearchEmployees.aspx" title="Payroll"><b>I&nbsp;-&nbsp;Pay</b></a> </li>
    <li>Manage Employee</li>
    </ul>

<asp:Button ID="Trigger" runat="server" CssClass="HiddenClass" SkinID="dd" onclick="Trigger_Click"  />
<asp:HiddenField ID="BasicChecked" runat="server" />
<asp:HiddenField ID="AllowanceChecked" runat="server" />
<script type="text/javascript">

 //document.getElementById("<%= BasicChecked.ClientID %>").value = BasicChecked;
 //document.getElementById("<%= AllowanceChecked.ClientID %>").value = AllowanceChecked;
// function saveBasicCheck()
// {
// 
//    document.getElementById("<%= BasicChecked.ClientID %>").value = BasicChecked;
//   
// }
// function saveAllowamceCheck()
// { document.getElementById("<%= AllowanceChecked.ClientID %>").value = AllowanceChecked;
// }
 </script>

<%--<asp:UpdatePanel ID="DUP" runat="server" UpdateMode="Conditional" 
 > 
  <Triggers >
 <asp:PostBackTrigger ControlID="btnUpdate"  ChildrenAsTriggers="true" />
 </Triggers> 
<ContentTemplate>
</ContentTemplate>
</asp:UpdatePanel>--%>
<asp:Panel ID="AEP" runat="server" >
    <table style="width: 10%;">
    <tr>            
            <td>
             <asp:HyperLink   NavigateUrl="~/Payroll/Forms/SearchEmployees.aspx" ID="back" runat="server" style="height:25px;width:25px;" Height="25px" Width="25px" ToolTip="Go Back">
             <asp:Image runat="server" ID="imgThumbnail" Height="25px" Width="25px" ImageUrl="~/Images/back.png" />
             </asp:HyperLink>
            <%--<a href="SearchEmployees.aspx" id="back" ><img src="../../Images/back.png" width="25px" height="25px" alt="Go Back to Employee Search"  /> </a>--%>
                <%--<asp:ImageButton ID="Back" runat="server" Text="Back" onclick="Back_Click" ToolTip="Go Back"  Visible="false" ImageUrl="~/Images/back.png" Height="25px" Width="25px" />--%>
            </td>
            <td>
                <asp:Button ID="btnAmend" runat="server" Text="Edit" OnClick="btnAmend_Click" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"   ValidationGroup="ALL" />
                <asp:Button ID="btnInsert" runat="server" Text="Create" OnClick="btnInsert_Click" ValidationGroup="ALL"  />
            </td>                                                     
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"   /> <%--OnClick="CancelUpdateItem_Click"--%>
            </td>                    
        </tr>       
   </table>      
</asp:Panel>
 

<div>
 <table class="TableClass" style="width:980px !Important; margin-left:6px;">
   <tr class="theadColumnWithBackground"> <td>Employee Details</td></tr>   
</table>
<div style="float:left;">
   <asp:DetailsView ID="EmployeeView" runat="server" GridLines="None" Width="670px" DataSourceID="ODOEV"
        AutoGenerateRows="false"  OnDataBound="EmployeeView_DataBound" CausesValidation="true" >                 
        <Fields>
            <asp:TemplateField>
                <ItemTemplate>
                    <table  class="TableClass"  cellpadding="0px" cellspacing="0px"  style="width:670px !Important; text-align:left; ">
                        <tr >
                            <td style="width:200px;" rowspan="9">
                            <%--<asp:Image ID="imgfile" runat="server" Width="136px" Height="136px" />--%>
                            <asp:HiddenField ID="FileID" runat="server" Value='<%# Bind("FileID") %>' />
                            <img src="../../ImageHandler.ashx?ID=<%# Eval("FileID") %>" height="180px" width="150px" alt="Image" />
                           <%-- <asp:Image ID="myImg" runat="server" ImageUrl='<%# "~/ImageHandler.ashx?ID" + Eval("FileID") %>' Width="136px" Height="136px" />--%>
                            <%--<asp:Image ImageUrl='<%# "data:image/jpg;base64," + Convert.ToBase64String((byte[])Eval("IMG_DATA")) %>' />  --%>
                            </td>
                            <td  class="leftAndwidth">
                                <asp:Label ID="lblID" runat="server"   Text="Employee ID" />
                            </td>
                            <td class="leftAndwidthLess">
                                <asp:Label ID="lblid1" runat="server"   Text='<%# Eval("ID") %>' />
                            </td>                           
                            <td class="leftAndwidth">
                                <asp:Label ID="lblEmployeeName1" runat="server"   Text="Emp. Name" />
                            </td>
                             <td class="leftAndwidthLess"> 
                                <asp:Label ID="lblEmployeeName" runat="server"   Text='<%# Eval("EmployeeName") %>' />
                            </td>                              
                       
                        </tr>
                        <tr >
                            <td>
                                <asp:Label ID="lblDesig" runat="server"  Text="Designation" />
                            </td>   
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lbldesig1" runat="server"  Text='<%# Eval("Designation") %>' />
                            </td>  
                            <td  >
                                <asp:Label ID="lblFname" runat="server"   Text="First Name" />
                            </td>
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblfname1" runat="server"   Text='<%# Eval("FirstName") %>' />
                            </td>
                         </tr>
                         <tr>
                          <td>
                                <asp:Label ID="lblBirthDate"   Text="Birth Date" runat="server" />
                            </td>
                            <td class="leftAndwidthLess">
                                <asp:Label ID="lbldob1"  Text='<%# Eval("BirthDate", "{0:dd/MM/yyyy}") %>' runat="server" />
                            </td>                                                                                                                                  
                            <td>
                                <asp:Label ID="lblMname" runat="server"  Text="Middle Name" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblmname1" runat="server"  Text='<%# Eval("MiddleName") %>' />
                            </td>  
                         </tr>            
                         <tr>
                          <td>
                                <asp:Label ID="lblJoinDate"   Text="Joining Date" runat="server" />
                            </td>
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lbljoindate1"   Text='<%# Eval("JoiningDate", "{0:dd/MM/yyyy}") %>' runat="server" />
                            </td>            
                            <td>
                                <asp:Label ID="lblLname" runat="server" Text="Last Name" />
                            </td>
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lbllname1" runat="server" Text='<%# Eval("LastName") %>' />                                
                            </td>
                        </tr>                        
                         <tr>
                           <td>
                                <asp:Label ID="lblActive" runat="server"   Text="Active" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="hdnActive"   Text='<%# Eval("Active") %>' runat="server" CssClass="HiddenClass"/>
                                <asp:CheckBox ID="IsActive" Enabled="false" runat="server" />
                            </td>  
                            <td>
                                <asp:Label ID="lblGender"   Text="Gender" runat="server" />
                            </td>
                            <td class="leftAndwidthLess">
                                <asp:Label ID="hdnGender"   Text='<%# Eval("Gender") %>' runat="server"  />
                               <%-- <asp:Label ID="lblGender1" CssClass="display:none"   runat="server" />--%>
                            </td>
                         </tr>
                         <tr>
                            <td>
                                <asp:Label ID="lblCreatedOn"   Text="Created On" runat="server" />
                            </td>
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblcreated1"   Text='<%#Eval("CreatedOn", "{0:dd/MM/yyyy}")   %>' runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblMarried"   Text="Married" runat="server" />
                            </td>
                             <td class="leftAndwidthLess">
                                <asp:Label ID="hdnMarried"   Text='<%# Eval("Married") %>' runat="server" CssClass="HiddenClass"/>
                                <asp:CheckBox Enabled="false" runat="server" ID="IsMarried" />
                            </td>
                         </tr>
                         <tr>
                            <td>
                                <asp:Label ID="lblModifiedOn"   Text="Modified On" runat="server" />
                            </td>
                              <td class="leftAndwidthLess">
                                <asp:Label ID="lblmodified1"  Text='<%#Eval("ModifiedOn", "{0:dd/MM/yyyy}")  %>' runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="lblPF1" runat="server"   Text="PF Number" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblPF"   Text='<%# Eval("PF") %>' runat="server" />                               
                            </td>  
                         </tr>
                        <tr>  
                         <td colspan="2">&nbsp;</td>      
                         <td>
                                <asp:Label ID="lblPAN1" runat="server"   Text="PAN Number" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblPAN"   Text='<%# Eval("PAN") %>' runat="server" />                               
                            </td>                             
                             
                        </tr>
                        <tr>       
                        <td colspan="2">&nbsp;</td>                           
                            <td>
                                <asp:Label ID="lblESIC1" runat="server"   Text="ESIC Number" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblESIC"   Text='<%# Eval("ESIC") %>' runat="server" />                               
                            </td>             
                        </tr>                                                              
                    </table>
                </ItemTemplate>
                <InsertItemTemplate>
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="width:670px !Important;">                         
                        <tr >      
                            <td style="width:200px;" rowspan="9">
                            &nbsp;
                            </td>
                            <td class="leftAndwidth">
                                <asp:Label ID="lbleID" runat="server"   Text="Employee ID" />
                            </td>
                            <td class="leftAndwidthMore">
                                <asp:Label ID="lblID" runat="server"   />
                            </td>
                            <td class="leftAndwidth">
                                <asp:Label ID="lblEmployeeName1" runat="server"   Text="Emp Name" />
                            </td>
                             <td  colspan="2" class="leftAndwidth">
                                <asp:Label ID="lblEmployeeName" runat="server"   Text='<%# Eval("EmployeeName") %>' />
                            </td>
                        </tr>
                        <tr>        
                            <td>
                                <asp:Label ID="lblDesig" runat="server"  Text="Designation"  />
                            </td> 
                            <td>
                                <asp:TextBox ID="txtDesig" runat="server"  Text='<%# Bind("Designation") %>' Width="95%" />
                            </td> 
                             <td >
                                <asp:Label ID="lblFname" runat="server"   Text="First Name" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtfname"  
                                ValidationGroup="ALL" ErrorMessage="First Name: Please provide first name" >*</asp:RequiredFieldValidator>
                            </td>
                             <td  >
                                <asp:TextBox ID="txtfname" runat="server"   Text='<%# Bind("FirstName") %>' />                               
                            </td>
                        </tr>
                        <tr>
                        <td>
                                <asp:Label ID="lblBirthDate"   Text="Birth Date" runat="server"   />
                                 <asp:RequiredFieldValidator ID="req1" runat="server"
                                 ControlToValidate="txtdob" ValidationGroup="ALL"  ErrorMessage="Date Of Birth: Please provide proper date"
                                 >*</asp:RequiredFieldValidator>
                            </td>
                             <td>
                                <asp:TextBox ID="txtdob"  Text='<%# Bind("BirthDate","{0:dd/MM/yyyy}") %>'   Width="75%" runat="server" />                               
                                 <asp:ImageButton ID="imgCal1" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <cc1:CalendarExtender ID="ce1" runat="server"   TargetControlID="txtdob"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgCal1"  />
                                <asp:RegularExpressionValidator id="req2" 
                                runat="server" 
                                ErrorMessage="Birth Date: Date needs to be in the format dd/MM/yyyy" 
                                Display="None" 
                                ControlToValidate="txtdob"  
                                ValidationGroup="ALL" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                            </td>  
                             <td>
                                <asp:Label ID="Label2" runat="server"  Text="Middle Name" />
                            </td>  
                            <td>
                                <asp:TextBox ID="txtmname" runat="server"  Text='<%# Bind("MiddleName") %>' />
                            </td>
                        </tr>    
                        <tr>                 
                            <td>
                                 <asp:Label ID="lblJoinDate"   Text="Join Date" runat="server"  />   
                                 <asp:RequiredFieldValidator ID="req3" runat="server"
                                 ControlToValidate="txtJoinDate" ValidationGroup="ALL" ErrorMessage="Joining Date: Please provide proper date"
                                 >*</asp:RequiredFieldValidator>                            
                            </td>                
                             <td >
                                <asp:TextBox ID="txtJoinDate"  Text='<%# Bind("JoiningDate", "{0:dd/MM/yyyy}") %>'    Width="75%"   runat="server"   />                                                             
                                  <asp:ImageButton ID="imgCal2" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <cc1:CalendarExtender ID="ce2" runat="server"   TargetControlID="txtJoinDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgCal2"  />
                                <asp:RegularExpressionValidator id="req4" 
                                runat="server" 
                                ErrorMessage="Joining Date: Date needs to be in the format dd/MM/yyyy" 
                                Display="None" 
                                ControlToValidate="txtJoinDate"  
                                ValidationGroup="ALL" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                            </td>
                              <td> 
                            <asp:Label ID="lblLname" runat="server" Text="Last Name" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtlname"  
                                ValidationGroup="ALL" ErrorMessage="Last Name: Please provide last name"   >*</asp:RequiredFieldValidator>
                            </td>    
                             <td>
                                <asp:TextBox ID="txtlname" runat="server" Text='<%# Bind("LastName") %>' />                                                              
                            </td> 
                        </tr>  
                                          
                        <tr>
                             <td>
                                <asp:Label ID="lblActive" runat="server"   Text="Active" />
                            </td>
                             <td>
                                <asp:CheckBox ID="IsActive" Enabled="true" runat="server" />
                             </td>    
                             <td>
                                <asp:Label ID="lblGender"   Text="Gender" runat="server" />
                            </td>
                             <td style="border-style:none; height:20px !Important;">
                                 <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" BorderStyle="None">
                                    <asp:ListItem Value="True" Selected="True" >Male</asp:ListItem>                                                                        
                                    <asp:ListItem Value="False">Female</asp:ListItem>
                                </asp:RadioButtonList>                                
                            </td>                         
                        </tr>
                         <tr>
                             <td>
                                <asp:Label ID="lblCreatedOn"   Text="Created On" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="txtCreatedOn"   Text='<%# Bind("CreatedOn") %>' runat="server" /> 
                            </td>                            
                            <td>
                                <asp:Label ID="lblMarried"   Text="Married" runat="server" />
                            </td>     
                             <td>
                            <asp:CheckBox ID="IsMarried" runat="server" />                               
                            </td> 
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblModifiedOn"   Text="Modified On" runat="server" />
                            </td>
                           
                             <td>
                                <asp:Label ID="txtModifiedOn"   Text='<%# Bind("ModifiedOn") %>' runat="server" /> 
                             </td>
                              <td>
                                <asp:Label ID="lblPF1" runat="server"   Text="PF Number" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtPF"   Text='<%# Bind("PF") %>' runat="server" />                               
                            </td>   
                        </tr>   
                        <tr>   
                            <td colspan="2">
                            <asp:FileUpload ID="fup" runat="server" Width="50%" />
                            </td>
                             <td >
                                <asp:Label ID="lblPAN1" runat="server"   Text="PAN Number" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtPAN"   Text='<%# Bind("PAN") %>' runat="server" />                               
                            </td>                             
                        </tr>   
                        <tr>
                            <td colspan="2">&nbsp;</td>
                           <td>
                                <asp:Label ID="lblESIC1" runat="server"   Text="ESIC Number" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtESIC"   Text='<%# Bind("ESIC") %>' runat="server" />                               
                            </td>                       
                        </tr>                        
                 </table>
                </InsertItemTemplate>
                <EditItemTemplate>
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="margin-left:-3px;width:670px !Important;">
                     <tr >
                           <td style="width:200px;" rowspan="9">
                            <img src="../../ImageHandler.ashx?ID=<%# Eval("FileID") %>" height="180px" width="150px" alt="Image" />
                            <asp:HiddenField ID="FileID" runat="server" Value='<%# Bind("FileID") %>' />
                            </td>
                             <td class="leftAndwidth">
                                <asp:Label ID="lbleID" runat="server"   Text="Employee ID" />
                            </td>
                            <td class="leftAndwidthLess">
                                <asp:Label ID="lblID" runat="server"  Text='<%# Bind("ID") %>'    />
                            </td>
                            <td class="leftAndwidth">
                                <asp:Label ID="lblEmployeeName1" runat="server"   Text="Emp Name" />
                            </td>
                             <td  colspan="2" class="leftAndwidthMore">
                                <asp:Label ID="lblEmployeeName" runat="server"   Text='<%# Eval("EmployeeName") %>' />
                            </td>
                           
                    </tr>
                    <tr >
                            <td>
                                <asp:Label ID="lblDesig" runat="server"  Text="Designation" />
                            </td>  
                            <td>
                                <asp:TextBox ID="txtDesig" runat="server"  Text='<%# Bind("Designation") %>' Width="95%" />
                            </td>  
                             <td class="leftAndwidth">
                                <asp:Label ID="lblFname" runat="server"   Text="First Name" />
                                <asp:RequiredFieldValidator ID="rfv3" runat="server" ControlToValidate="txtfname"  
                                ValidationGroup="ALL" ErrorMessage="First Name: Please provide first name" >*</asp:RequiredFieldValidator>                             
                            </td>
                              <td colspan="2" class="leftAndwidthMore">
                                <asp:TextBox ID="txtfname" runat="server"   Text='<%# Bind("FirstName") %>' />
                                <asp:Label ID="Label3" runat="server" CssClass="HiddenClass" Text='<%# Bind("ID") %>'    />
                                </td>                           
                    </tr>
                    <tr>
                    <td>
                                <asp:Label ID="lblBirthDate"   Text="Birth Date" runat="server" />
                                 <asp:RequiredFieldValidator ID="req5" runat="server"
                                 ControlToValidate="txtdob" ValidationGroup="ALL"  ErrorMessage="Date Of Birth: Please provide proper date"
                                 >*</asp:RequiredFieldValidator>
                            </td>
                             <td>
                                <asp:TextBox ID="txtdob"  Text='<%# Bind("BirthDate","{0:dd/MM/yyyy}") %>' runat="server"  Width="75%" />                               
                                <asp:ImageButton ID="imgCal3" runat="server" ImageUrl="~/Images/Calendar.png" />
                                <cc1:CalendarExtender ID="ce3" runat="server"   TargetControlID="txtdob"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgCal3"  />
                               <asp:RegularExpressionValidator id="req6" 
                                runat="server" 
                                ErrorMessage="Birth Date: Date needs to be in the format dd/MM/yyyy" 
                                Display="None" 
                                ControlToValidate="txtdob"  
                                ValidationGroup="ALL" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                            </td>  
                             <td>
                                <asp:Label ID="lblMname" runat="server"  Text="Middle Name" />
                            </td> 
                              <td>
                                <asp:TextBox ID="txtmname" runat="server"  Text='<%# Bind("MiddleName") %>' />
                            </td>                           
                    </tr>
                    <tr>                                                 
                             <td>
                                <asp:Label ID="lblJoinDate"   Text="Joining Date" runat="server" />
                                 <asp:RequiredFieldValidator ID="req7" runat="server"
                                 ControlToValidate="txtJoinDate" ValidationGroup="ALL" ErrorMessage="Joining Date: Please provide proper date"
                                 >*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                               <%--<asp:TextBox ID="txtJoinDate"  Text='<%# Bind("JoiningDate")%>'   runat="server" onblur="ValidateDateOf(this);"  />--%>
                                <asp:TextBox ID="txtJoinDate"  Text='<%# Bind("JoiningDate", "{0:dd/MM/yyyy}")%>'     runat="server"     Width="75%" />                               
                                 <asp:ImageButton ID="imgCal4" runat="server" ImageUrl="~/Images/Calendar.png" />
                                <cc1:CalendarExtender ID="ce4" runat="server"   TargetControlID="txtJoinDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"     PopupButtonID="imgCal4"  />
                                <asp:RegularExpressionValidator id="req8" 
                                runat="server" 
                                ErrorMessage="Joining Date: Date needs to be in the format dd/MM/yyyy" 
                                Display="None" 
                                ControlToValidate="txtJoinDate"  
                                ValidationGroup="ALL" 
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                               
                            </td>
                             <td>
                                <asp:Label ID="lblLname" runat="server" Text="Last Name" />
                                 <asp:RequiredFieldValidator ID="rfv6" runat="server" ControlToValidate="txtlname"  
                                ValidationGroup="ALL" ErrorMessage="Last Name: Please provide Last Name" >*</asp:RequiredFieldValidator>                              
                            </td>
                             <td>
                                <asp:TextBox ID="txtlname" runat="server" Text='<%# Bind("LastName") %>' />   
                               
                            </td>
                    </tr>                    
                    <tr>            
                             <td>
                                <asp:Label ID="lblActive" runat="server"   Text="Active" />
                            </td>
                            <td>
                                <asp:Label ID="hdnActive"   Text='<%# Eval("Active") %>' runat="server" CssClass="HiddenClass"/>
                                <asp:CheckBox ID="IsActive" Enabled="true" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblGender"   Text="Gender" runat="server" />
                            </td>
                            <td style="border-collapse:collapse !Important; border-style:none !Important; height:10px !Important; width:15%;"  >
                                
                                 <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal" CssClass="TableClass TableClassNoBorder"   >
                                    <asp:ListItem Value="True" Selected="True"  >Male</asp:ListItem>
                                    <asp:ListItem Value="False">Female</asp:ListItem>
                                </asp:RadioButtonList>      
                                <asp:Label ID="hdnGender" runat="server" CssClass="HiddenClass" Text='<%# Bind("Gender") %>'   />                          
                            </td>
                    </tr>
                    <tr>                            
                            <td>
                                <asp:Label ID="lblCreatedOn"   Text="Created On" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="txtCreatedOn"   Text='<%# Bind("CreatedOn", "{0:dd/MM/yyyy}")%>'  runat="server" />                                
                            </td>
                             <td>
                                <asp:Label ID="lblMarried"   Text="Married" runat="server" />
                            </td>     
                            <td>
                            <asp:CheckBox  runat="server" ID="IsMarried" />                            
                            <asp:Label ID="hdnMarried" runat="server" CssClass="HiddenClass" Text='<%# Bind("Married") %>'   />                          
                            </td>                   
                     </tr>          
                    <tr>
                           <td>
                                <asp:Label ID="lblModifiedOn"   Text="Modified On" runat="server" />
                            </td>
                             <td>
                                <asp:Label ID="txtModifiedOn"   Text='<%# Bind("ModifiedOn", "{0:dd/MM/yyyy}") %>' runat="server" />                                
                            </td>
                            <td>
                                <asp:Label ID="lblPF1" runat="server"   Text="PF" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtPF"   Text='<%# Bind("PF") %>' runat="server" />                               
                            </td>
                     </tr>
                     <tr>                           
                              <td colspan="2">
                           <asp:FileUpload ID="fup" runat="server" Width="100%" />
                            </td>
                            <td>
                                <asp:Label ID="lblPAN1" runat="server"   Text="PAN" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtPAN"   Text='<%# Bind("PAN") %>' runat="server" />                               
                            </td>                                                         
                    </tr>
                    <tr> 
                            <td colspan="2"> &nbsp;</td>   
                             <td>
                                <asp:Label ID="lblESIC1" runat="server"   Text="ESIC" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtESIC"   Text='<%# Bind("ESIC") %>' runat="server" />                               
                            </td>                   
                                                
                        </tr>                                                        
                 </table>
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
</asp:DetailsView>
</div>
<div style="float:left;">
<asp:DetailsView ID="EmployeeDetailsView" runat="server" GridLines="None" Width="100%" DataSourceID="ODSEDV"
        AutoGenerateRows="false"  OnDataBound="EmployeeDetailsView_DataBound" CausesValidation="true" >                 
        <Fields>
        
            <asp:TemplateField>
                <ItemTemplate>
                    <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="margin-left:-5px;width:300px !Important;">
                        <tr>
                            <td  class="leftAndwidth">
                                <asp:Label ID="lblAddress1" runat="server" Text="Address Line" />
                            </td>    
                             <td  class="leftAndwidthLess">  
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("AddressLine1") %>' />                                
                            </td>  
                        </tr>
                       <tr>
                            <td  >
                                <asp:Label ID="lblAddress2" runat="server" Text="Address Line" />
                            </td>    
                             <td  class="leftAndwidthLess">
                                <asp:Label ID="lblAddressLine2" runat="server" Text='<%# Eval("AddressLine2") %>' />                                
                            </td>  
                        </tr>
                        <tr>                      
                            
                            <td>
                                <asp:Label ID="lblCity1" runat="server" Text="City" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblCity" runat="server" Text='<%# Eval("City") %>' />                                
                            </td> 
                        </tr>
                        <tr>      
                             <td>
                                <asp:Label ID="lblState1" runat="server" Text="State" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblState" runat="server" Text='<%# Eval("State") %>' />                                
                            </td>                     
                       </tr>
                        <tr>
                             <td>
                                <asp:Label ID="lblCountry1" runat="server" Text="Country" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("Country") %>' />                                
                            </td> 
                        </tr>
                        <tr>                            
                             <td>
                                <asp:Label ID="lblPinCode1" runat="server" Text="PinCode" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblPinCode" runat="server" Text='<%# Eval("PinCode") %>' />                                
                            </td>                               
                        </tr>     
                        
                        <tr>                         
                             <td>
                                <asp:Label ID="lblHomePhone1" runat="server" Text="Home Phone" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblHomePhone" runat="server" Text='<%# Eval("HomePhone") %>' />                                
                            </td> 
                           </tr>  
                           <tr>                                                                      
                             <td>
                                <asp:Label ID="lblWorkPhone1" runat="server" Text="Work Phone" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblWorkPhone" runat="server" Text='<%# Eval("WorkPhone") %>' />                                
                            </td>                        
                         </tr>   
                         <tr>                         
                             <td>
                                <asp:Label ID="lblMobile1" runat="server" Text="Mobile" />
                            </td>    
                             <td class="leftAndwidthLess">
                                <asp:Label ID="lblMobile" runat="server" Text='<%# Eval("Mobile") %>' />                                
                            </td>                        
                         </tr>      
                                                                   
                    </table>
                </ItemTemplate>
                <InsertItemTemplate>
               <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="width:300px !Important;">
                          
                        <tr >                           
                             <td class="leftAndwidth">
                                <asp:Label ID="lblAddress1" runat="server" Text="Address Line" />
                            </td>    
                             <td >
                                <asp:TextBox ID="txtAddressLine1" Width="97%" runat="server" Text='<%# Bind("AddressLine1") %>' />                                
                            </td>   
                        </tr>
                        <tr>
                           
                             <td>
                                <asp:Label ID="lblAddress2" runat="server" Text="Address Line" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtAddressLine2" Width="97%" runat="server" Text='<%# Bind("AddressLine2") %>' />                                
                            </td>   
                        </tr>
                        <tr>                                                             
                            <td>
                                <asp:Label ID="lblCity" runat="server" Text="City"  />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtCity" runat="server" Text='<%# Bind("City") %>' Width="97%" />                                
                            </td>                 
                         </tr>
                        <tr>
                            
                             <td>
                                <asp:Label ID="lblState" runat="server" Text="State" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtState" runat="server" Text='<%# Bind("State") %>' Width="97%" />                                
                            </td>    
                        </tr>
                        <tr>
                             
                             <td>
                                <asp:Label ID="lblCountry" runat="server" Text="Country" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtCountry" runat="server" Text='<%# Bind("Country") %>' Width="97%" />                                
                            </td>  
                         </tr>
                        <tr>
                              
                             <td>
                                <asp:Label ID="lblPinCode" runat="server" Text="Pincode" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtPinCode" runat="server" Text='<%# Bind("Pincode") %>' Width="97%" />                                
                            </td>                    
                        </tr>                                      
                         <tr>                         
                             <td>
                                <asp:Label ID="lblHomePhone" runat="server" Text="Home Phone" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtHomePhone" runat="server" Text='<%# Bind("HomePhone") %>' Width="97%" />                                
                            </td>
                           </tr>
                           <tr>                                                                         
                             <td>
                                <asp:Label ID="lblWorkPhone" runat="server" Text="Work Phone" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtWorkPhone" runat="server" Text='<%# Bind("WorkPhone") %>' Width="97%" />                                
                            </td>                        
                         </tr>   
                         <tr>                         
                             <td>
                                <asp:Label ID="lblMobile" runat="server" Text="Mobile" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtMobile" runat="server" Text='<%# Bind("Mobile") %>' Width="97%" />                                
                            </td>                        
                         </tr>             
                    </table>
                </InsertItemTemplate>
                <EditItemTemplate>
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="margin-left:-3px;width:300px !Important;"> 
                        <tr >    
                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Bind("ID") %>'  />
                                                
                              <td class="leftAndwidth">
                                <asp:Label ID="lblAddress1" runat="server" Text="Address Line" />
                            </td>    
                              <td  >
                                <asp:TextBox ID="txtAddressLine1"   runat="server" Text='<%# Bind("AddressLine1") %>' Width="95%" />                                
                            </td>  
                        </tr>
                        <tr>                            
                             <td>
                                <asp:Label ID="lblAddress2" runat="server" Text="Address Line" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtAddressLine2"  runat="server" Text='<%# Bind("AddressLine2") %>' Width="95%" />                                
                            </td> 
                         </tr>
                        <tr>                                                       
                            <td>
                                <asp:Label ID="lblCity" runat="server" Text="City" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtCity" runat="server" Text='<%# Bind("City") %>' Width="95%" />                                
                            </td> 
                         </tr>
                        <tr>                            
                             <td>
                                <asp:Label ID="lblState" runat="server" Text="State" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtState" runat="server" Text='<%# Bind("State") %>' Width="95%" />                                
                            </td>  
                         </tr>
                         <tr>                            
                             <td>
                                <asp:Label ID="lblCountry" runat="server" Text="Country" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtCountry" runat="server" Text='<%# Bind("Country") %>' Width="95%" />                                
                            </td>  
                         </tr>
                        <tr>                                   
                             <td>
                                <asp:Label ID="lblPinCode" runat="server" Text="Pincode" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtPinCode" runat="server" Text='<%# Bind("Pincode") %>' Width="95%" />                                
                            </td>                  
                        </tr> 
                       <tr>                         
                             <td>
                                <asp:Label ID="lblHomePhone" runat="server" Text="Home Phone" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtHomePhone" runat="server" Text='<%# Bind("HomePhone") %>' Width="95%" />                                
                            </td>   
                            </tr>  
                        <tr>                                                                   
                             <td>
                                <asp:Label ID="lblWorkPhone" runat="server" Text="Work Phone" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtWorkPhone" runat="server" Text='<%# Bind("WorkPhone") %>' Width="95%" />                                
                            </td>                        
                         </tr>   
                         <tr>                         
                             <td>
                                <asp:Label ID="lblMobile" runat="server" Text="Mobile" />
                            </td>    
                             <td>
                                <asp:TextBox ID="txtMobile" runat="server" Text='<%# Bind("Mobile") %>' Width="95%" />                                
                            </td>                        
                         </tr>                                                                   
                    </table> 
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
</asp:DetailsView>
</div>
<div style="float:left; width:100%;">
<asp:ValidationSummary ID="vm" runat="server" ValidationGroup="ALL" DisplayMode="List" ShowSummary="true"  />
</div>
<br style="clear: left;" /> 
</div>

<asp:Panel ID="ODP" runat="server">
<table class="TableClass" style="width:980px !Important; margin-left:7px;">
   <tr class="theadColumnWithBackground"> <td>Employee Salary Details</td></tr>  
</table>
 
<asp:DetailsView ID="SalaryDetailsView" runat="server" GridLines="None" Width="980px" DataSourceID="ODSSDV"
        AutoGenerateRows="false"  OnDataBound="SalaryDetailsView_DataBound" CausesValidation="true" >                 
        <Fields>
            <asp:TemplateField>
                <ItemTemplate>
                    <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="width:980px;">                      
                        <tr  >
                             <td colspan="2">
                                <asp:Label ID="Label1" runat="server" Text="Employee Type" />
                            </td>                           
                            <td colspan="2" >
                            Basic Daily
                            </td>                            
                            <td id="lblBM" colspan="2">
                                <asp:Label ID="lblBasicMonthly" runat="server"   Text="Basic Monthly" />
                            </td>
                            <td id="lblBD" colspan="2">
                                <asp:Label ID="lblBasicDaily" runat="server"  Text="Basic Daily" />
                            </td>                                 
                             <td colspan="2">
                            <asp:Label ID="lblAdvancePending"   Text="Advance Pending" runat="server" />
                            </td>            
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate"   Text="OverTime Rate" runat="server" />
                            </td>                                                   
                        </tr>          
                        <tr>
                            <td style="font-weight:normal;" colspan="2">
                                <asp:Label ID="lblemptype1" runat="server" Text='<%# Eval("EmpTypeDescription") %>' />
                                <asp:Label ID="hdnlblemptype" runat="server" Text='<%# Eval("EmpType") %>' CssClass="HiddenClass" />
                            </td>                           
                            <td colspan="2"> 
                             <asp:CheckBox ID="cbIsBasicDaily" runat="server"  Enabled="false"  Checked='<%# Eval("IsBasicDaily") %>' />                             
                            </td>
                             <td id="txtBM" colspan="2">
                                <asp:Label ID="lblBasicMonthly1" runat="server"   Text='<%# Eval("BasicMonthly") %>' />
                            </td>
                            <td id="txtBD" colspan="2">
                            <asp:Label ID="lblBasicDaily1" runat="server" Text='<%# Eval("BasicDaily") %>' />                                
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblAdvancePending1"    Text='<%# Eval("PendingAdvance") %>' runat="server" />
                            </td> 
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate1"   Text='<%# Eval("OverTimeRateDescription") %>' runat="server"  />                               
                            </td>                 
                        </tr>        
                         <tr   id="deduct1">
                            <td>  
                            <asp:Label ID="lblDeductProfTax"   Text="Deduct Prof Tax " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblProfessionalTaxPercent"   Text="Prof Tax Amount" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblDeductESIC"   Text="Deduct ESIC " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblESICTaxPercent"   Text="ESIC Tax % " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblDeductPF"   Text="Deduct PF" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblProvidentFundPercent"   Text="PF % " runat="server" />                            
                            </td>  
                            <td>  
                            <asp:Label ID="lblCreditTA"   Text="Credit TA" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblTravelAllowance"   Text="TA  " runat="server" />                            
                            </td>   
                            <td>  
                            <asp:Label ID="lblCreditDA"   Text="Credit DA" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblDearnessAllowance"   Text="DA  " runat="server" />                            
                            </td>                                                 
                         </tr>     
                         <tr id="deduct2">
                          <td>
                                <asp:CheckBox ID="cbDeductProfTax" Enabled="false"   Checked='<%# Eval("DeductProfTax") %>' runat="server" />
                          </td>
                           <td>
                                <asp:Label ID="txtProfessionalTaxPercent1"    Text='<%# Eval("ProfessionalTaxPercent") %>' runat="server" />  
                                
                          </td>
                           
                           <td>
                                <asp:CheckBox ID="cbDeductESIC"  Enabled="false"   Checked='<%#Eval("DeductESIC") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductESIC"))   %>' 
                                runat="server" OnCheckedChanged="ESICchecked" AutoPostBack="true" />
                          </td>
                           <td>
                                <asp:Label ID="txtESICTaxPercent1"    Text='<%# Eval("ESICTaxPercent") %>' runat="server" />
                          </td>
                          <td>
                                <asp:CheckBox ID="cbDeductPF" Enabled="false"     Checked='<%#Eval("DeductPF") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductPF"))   %>' 
                                runat="server" OnCheckedChanged="PFchecked" AutoPostBack="true" />
                          </td>   
                          <td>
                                <asp:Label ID="txtProvidentFundPercent1"    Text='<%# Eval("ProvidentFundPercent") %>' runat="server" />
                          </td>    
                          <td>
                                <asp:CheckBox ID="cbCreditTA" Enabled="false"  Checked='<%#Eval("CreditTA") == DBNull.Value ? false : Convert.ToBoolean(Eval("CreditTA"))   %>'  
                                runat="server" OnCheckedChanged="TAchecked" AutoPostBack="true" />
                          </td>   
                          <td>
                                <asp:Label ID="txtTravelAllowance1"    Text='<%# Eval("TravelAllowance") %>' runat="server" />
                          </td> 
                           <td>
                                <asp:CheckBox ID="cbCreditDA" Enabled="false"  Checked='<%#Eval("CreditDA") == DBNull.Value ? false : Convert.ToBoolean(Eval("CreditDA"))   %>'   
                                runat="server" OnCheckedChanged="DAchecked" AutoPostBack="true" />
                          </td>   
                          <td>
                                <asp:Label ID="txtDearnessAllowance1"    Text='<%# Eval("DearnessAllowance") %>' runat="server" />
                          </td>  
                                                                                   
                         </tr> 
                         <tr  >
                             <td id="lblA" colspan="2">
                            Allowance Daily
                            </td>
                            <td id="lblAM" colspan="2">
                                <asp:Label ID="lblAllowanceMonthly" runat="server" Text="Allowance Monthly" />
                            </td>                            
                            <td id="lblAD" colspan="2">
                                <asp:Label ID="lblAllowanceDaily"   Text="Allowance Daily" runat="server" />
                            </td>   
                            <td id="lblBL" colspan="2">
                                <asp:Label ID="lblBalanceLeaves"   Text="Leaves Balance" runat="server" />
                            </td>                            
                             <td id="lblYPL" colspan="2">
                                <asp:Label ID="lblYearlyPaidLeaves"   Text="Yearly Paid Leaves" runat="server" />
                            </td>  
                                           
                         </tr>
                         <tr >
                             <td id="cbA" colspan="2">
                                <asp:CheckBox ID="cbIsAllowanceDaily" runat="server" Enabled="false"  Checked='<%# Bind("IsAllowanceDaily") %>'  />
                             </td>                      
                             <td id="txtAM" colspan="2">
                                    <asp:Label ID="lblAllowanceMonthly1" runat="server"  Text='<%# Eval("AllowanceMonthly") %>'   />                    
                             </td>
                             <td id="txtAD" colspan="2">
                                    <asp:Label ID="lblAllowanceDaily1"  Text='<%# Eval("AllowanceDaily") %>' runat="server" />
                             </td>
                             <td id="lblBL1" colspan="2">
                                    <asp:Label ID="lblBalanceLeaves1"   Text='<%# Eval("BalanceLeaves") %>' runat="server" />                                
                             </td>                           
                             <td id="lblYPL1" colspan="2">
                                <asp:Label ID="lblYearlyPaidLeaves1"    Text='<%# Eval("YearlyPaidLeaves") %>' runat="server" />
                             </td>      
                                              
                         </tr>                               
                    </table>
                </ItemTemplate>
                <InsertItemTemplate>
               <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="width:980px;">                        
                         <tr >    
                            <td colspan="2">
                                <asp:Label ID="lblEmpType" runat="server" Text="Employee Type" />
                            </td>                                                                                              
                            <td style="width:150px;" colspan="2">
                            Basic Daily
                            </td>                            
                            <td id="lblBM" style="width:150px;" colspan="2">
                                <asp:Label ID="lblBasicMonthly" runat="server"   Text="Basic Monthly" />
                            </td>
                            <td id="lblBD" style="width:150px;" colspan="2">
                                <asp:Label ID="lblBasicDaily" runat="server"  Text="Basic Daily" />
                            </td>                           
                            <td style="width:150px;" colspan="2"">
                                <asp:Label ID="lblAdvancePending"   Text="Advance Pending" runat="server" />
                            </td> 
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate"   Text="OverTime Rate" runat="server" />
                            </td>                                                    
                        </tr>           
                        <tr >
                             <td colspan="2">
                                <asp:DropDownList ID="ddlEmpType" runat="server" onchange="ShowHidetr(this.options[this.selectedIndex].value);"  />   <%--onselectedindexchanged="ddlEmpType_SelectedIndexChanged" AutoPostBack="true"--%>                              
                                <%--<asp:DropDownList ID="ddlEmpType" runat="server" onchange="ShowHidetr(this.options[this.selectedIndex].value);"  />--%>
                            </td>    
                             <td colspan="2">
                             <asp:CheckBox ID="cbIsBasicDaily" runat="server" onclick="ShowHideBasic(this.checked);" Checked='<%# Bind("IsBasicDaily") %>' />
                                                     
                             </td>
                             <td id="txtBM" colspan="2">
                                <asp:TextBox ID="txtBasicMonthly" runat="server"   Text='<%# Bind("BasicMonthly") %>' SkinID="halfWidth"/>
                            </td>
                             <td id="txtBD" colspan="2">
                                <asp:TextBox ID="txtBasicDaily" runat="server" Text='<%# Bind("BasicDaily") %>' SkinID="halfWidth"/>                                
                            </td>
                            <td colspan="2">
                             <asp:Label ID="lblAdvancePending1" runat="server" Text='<%# Eval("PendingAdvance") %>' />                                                              
                            </td>
                             <td colspan="2"  >                                
                                <asp:DropDownList ID="ddlOverTimeRate" runat="server"   />                                
                            </td>                                                                           
                        </tr>                        
                        
                           <tr   id="deduct1">
                            <td >  
                            <asp:Label ID="lblDeductProfTax"   Text="Deduct Prof Tax " runat="server" />                            
                            </td>                     
                            <td>  
                            <asp:Label ID="lblProfessionalTaxPercent"   Text="Prof Tax Amount" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblDeductESIC"   Text="Deduct ESIC " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblESICTaxPercent"   Text="ESIC %" runat="server" />                            
                            </td>
                              <td>  
                            <asp:Label ID="lblDeductPF"   Text="Deduct PF" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblProvidentFundPercent"   Text="PF %" runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblCreditTA"   Text="Credit TA" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblTravelAllowance"   Text="TA " runat="server" />                            
                            </td>   
                            <td>  
                            <asp:Label ID="lblCreditDA"   Text="Credit DA" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblDearnessAllowance"   Text="DA " runat="server" />                            
                            </td>   
                                                                                                                                              
                         </tr>  
                         <tr id="deduct2" >                          
                            <td  >
                                <asp:CheckBox ID="cbDeductProfTax"    Checked='<%#Eval("DeductProfTax") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductProfTax"))  %>' 
                                 OnCheckedChanged="ProfTaxchecked"  runat="server" AutoPostBack="true" />
                          </td>
                           <td  >
                                <asp:TextBox ID="txtProfessionalTaxPercent1" CssClass="AdjustWidth"   Text='<%# Bind("ProfessionalTaxPercent") %>' runat="server" />  
                                <asp:RangeValidator id="Range1"
                                   ControlToValidate="txtProfessionalTaxPercent1"
                                   MinimumValue="0"
                                   MaximumValue="10000"
                                   Type="Double"
                                    ErrorMessage="The Professional Tax Percent value must be greater then or equal to 0 and less than or equal to 10000"
                                   Text="*"
                                   Display="Dynamic"
                                   ValidationGroup="ALL"
                                   runat="server"/>
                          </td>
                           
                           <td >
                                <asp:CheckBox ID="cbDeductESIC"    Checked='<%#Eval("DeductESIC") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductESIC"))   %>' 
                                runat="server" OnCheckedChanged="ESICchecked" AutoPostBack="true" />
                          </td>
                           <td class="leftAndwidth">
                                <asp:TextBox ID="txtESICTaxPercent1" CssClass="AdjustWidth"   Text='<%# Bind("ESICTaxPercent") %>' runat="server" />
                                <asp:RangeValidator id="Range2"
                                   ControlToValidate="txtESICTaxPercent1"
                                   MinimumValue="0"
                                   MaximumValue="100"
                                   Type="Double"
                                   Display="Dynamic"
                                   ErrorMessage="The ESIC value must be greater then or equal to 0 and less than or equal to 100"
                                   Text="*"
                                   ValidationGroup="ALL"
                                   runat="server"/>
                          </td>
                          <td>
                                <asp:CheckBox ID="cbDeductPF"    Checked='<%#Eval("DeductPF") == DBNull.Value ? false : Convert.ToBoolean(Eval("DeductPF"))   %>' 
                                runat="server" OnCheckedChanged="PFchecked" AutoPostBack="true" />
                          </td>   
                          <td class="leftAndwidth">
                                <asp:TextBox ID="txtProvidentFundPercent1" CssClass="AdjustWidth"   Text='<%# Bind("ProvidentFundPercent") %>' runat="server" />
                                <asp:RangeValidator id="Range3"
                                   ControlToValidate="txtProvidentFundPercent1"
                                   MinimumValue="0"
                                   MaximumValue="100"
                                   Type="Double"
                                   Display="Dynamic"
                                   ErrorMessage="The Provident Fund value must be greater then or equal to 0 and less than or equal to 100"
                                   Text="*"
                                   ValidationGroup="ALL"
                                   runat="server"/>
                          </td>    
                          <td>
                                <asp:CheckBox ID="cbCreditTA" Checked='<%#Eval("CreditTA") == DBNull.Value ? false : Convert.ToBoolean(Eval("CreditTA"))   %>'  
                                runat="server" OnCheckedChanged="TAchecked" AutoPostBack="true" />
                          </td>   
                          <td class="leftAndwidth">
                                <asp:TextBox ID="txtTravelAllowance1"  CssClass="AdjustWidth"  Text='<%# Bind("TravelAllowance") %>' runat="server" />
                          </td> 
                           <td>
                                <asp:CheckBox ID="cbCreditDA" Checked='<%#Eval("CreditDA") == DBNull.Value ? false : Convert.ToBoolean(Eval("CreditDA"))   %>'   
                                runat="server" OnCheckedChanged="DAchecked" AutoPostBack="true" />
                          </td>   
                          <td class="leftAndwidth">
                                <asp:TextBox ID="txtDearnessAllowance1" CssClass="AdjustWidth"   Text='<%# Bind("DearnessAllowance") %>' runat="server" />
                          </td>  
                             
                         </tr> 
                       
                         <tr      >
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
                             <td id="lblYPL" colspan="2">
                                <asp:Label ID="lblYearlyPaidLeaves"   Text="Yearly Paid Leaves" runat="server" />
                            </td> 
                         </tr>
                          <tr>
                          <td id="cbA">
                             <asp:CheckBox ID="cbIsAllowanceDaily" runat="server" onclick="ShowHideAllowance(this.checked);" Checked='<%# Bind("IsAllowanceDaily") %>' />
                             </td>
                             <td id="txtAM">
                                <asp:TextBox ID="txtAllowanceMonthly" runat="server"  Text='<%# Bind("AllowanceMonthly") %>' SkinID="halfWidth" />
                            </td>                                                       
                            <td id="txtAD">
                                <asp:TextBox ID="txtAllowanceDaily" runat="server" Text='<%# Bind("AllowanceDaily") %>'  SkinID="halfWidth" />                                
                            </td>
                            <td id="lblBL1">
                             <asp:Label ID="txtBalanceLeaves" runat="server" Text='<%# Bind("BalanceLeaves") %>' SkinID="halfWidth" />                                                              
                            </td>                           
                           <td id="lblYPL1" colspan="2">
                             <asp:TextBox ID="txtYearlyPaidLeaves" runat="server" Text='<%# Bind("YearlyPaidLeaves") %>'  SkinID="halfWidth"
                                ToolTip="Yearly paid is for information purpose only.Does not get added or deducted from leaves balance of employee." />                                                              
                            </td>                
                         </tr>                          
                    </table>
                </InsertItemTemplate>
                <EditItemTemplate> 
                <table  class="TableClass"  cellpadding="0px" cellspacing="0px" style="width:980px;">
                        
                       <tr  > 
                            <td colspan="2">
                                <asp:Label ID="lblEmpType" runat="server" Text="Employee Type" />
                            </td>                                                    
                            <td  colspan="2">
                            Basic Daily
                            </td>                            
                            <td id="lblBM"  colspan="2" >
                                <asp:Label ID="lblBasicMonthly" runat="server"   Text="Basic Monthly" />
                            </td>
                            <td id="lblBD" colspan="2" >
                                <asp:Label ID="lblBasicDaily" runat="server"  Text="Basic Daily" />
                            </td>
                             <td  colspan="2">
                            <asp:Label ID="lblAdvancePending"   Text="Advance Pending" runat="server" />
                            </td>
                             <td colspan="2">
                                <asp:Label ID="lblOverTimeRate"   Text="OverTime Rate" runat="server" />
                            </td>                                                   
                        </tr>          
                       <tr  >
                            <td colspan="2">
                                <asp:DropDownList ID="ddlEmpType" runat="server"     />
                                <asp:HiddenField ID="hdnEmptype" runat="server" Value='<%# Bind("EmpType") %>'   />
                            </td>  
                             <td colspan="2">
                             <asp:CheckBox ID="cbIsBasicDaily" runat="server" onclick="ShowHideBasic(this.checked);" Checked='<%# Bind("IsBasicDaily") %>' />
                             <asp:Label ID="lblID" runat="server" CssClass="HiddenClass" Text='<%# Bind("ID") %>'   />                                      
                             </td>
                             <td id="txtBM" colspan="2" style="width:150px;">
                                <asp:TextBox ID="txtBasicMonthly" runat="server"   Text='<%# Bind("BasicMonthly") %>' />
                            </td>
                             <td id="txtBD" colspan="2" style="width:150px;">
                                <asp:TextBox ID="txtBasicDaily" runat="server" Text='<%# Bind("BasicDaily") %>'  />                                
                            </td>                            
                             <td colspan="2">
                             <asp:LinkButton ID="lnkAdvancePending"   runat="server" Text='<%# string.IsNullOrEmpty(Eval("PendingAdvance").ToString()) ? "0.00" : Eval("PendingAdvance","{0:N2}")    %>' OnClientClick="return false;" ></asp:LinkButton>                               
                             <asp:Panel ID="pAp" runat="server"  style="display: none; background-color:#FBFBEF" Width="60%">
                              
                             <span style="float:right"> 
                             <asp:ImageButton ID="imgCLose" runat="server" ImageUrl="~/Images/close.jpg" Height="25px" Width="25px" OnClientClick="TriggerClick();return false;" /></span>
                              <span style="float:left;width:650px;"">
                            
                                    <asp:UpdatePanel ID="up2" runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate>                                      
                                    <uc:AT ID="UCAdvanceTransaction" runat="server" />      
                                    </ContentTemplate>
                                    </asp:UpdatePanel> 
                                  
                                    </span>
                              
                             <cc1:ModalPopupExtender ID="mPop" runat="server" BackgroundCssClass="modalPopup"  BehaviorId="mPop1"
                               TargetControlID="lnkAdvancePending" PopupControlID="pAp" X="50" Y="100"   >                                    
                           </cc1:ModalPopupExtender>
                           <%--<asp:Button ID="Button2" runat="server" Text="Close" OnClientClick="TriggerClick();return false;" /> --%></asp:Panel> 
                             <%--<asp:Label ID="lblAdvancePending1" runat="server" Text='<%# Bind("AdvancePending") %>' /> --%>                                                             
                            </td>
                             <td colspan="2">                          
                                <asp:DropDownList ID="ddlOverTimeRate" runat="server"   />  
                                <asp:Label ID="hdnOverTimeRate" runat="server" CssClass="HiddenClass" Text='<%# Bind("OverTimeRate") %>'   />                                                              
                            </td>                                                          
                                                                             
                        </tr>    
                       <tr  id="deduct1" >
                            <td  >  
                            <asp:Label ID="lblDeductProfTax"   Text="Deduct Prof Tax " runat="server" />                            
                            </td>                     
                            <td>  
                            <asp:Label ID="lblProfessionalTaxPercent"  Text="Prof Tax Amount" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblDeductESIC"   Text="Deduct ESIC " runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblESICTaxPercent"   Text="ESIC %" runat="server" />                            
                            </td>
                              <td>  
                            <asp:Label ID="lblDeductPF"   Text="Deduct PF" runat="server" />                            
                            </td>
                             <td>  
                            <asp:Label ID="lblProvidentFundPercent"   Text="PF %" runat="server" />                            
                            </td>
                            <td>  
                            <asp:Label ID="lblCreditTA"   Text="Credit TA" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblTravelAllowance"   Text="TA " runat="server" />                            
                            </td>   
                            <td>  
                            <asp:Label ID="lblCreditDA"   Text="Credit DA" runat="server" />                            
                            </td>    
                             <td>  
                            <asp:Label ID="lblDearnessAllowance"   Text="DA " runat="server" />                            
                            </td>   
                         </tr>     
                         <tr id="deduct2">   
                         <asp:UpdatePanel ID="upDeduct1" runat="server" UpdateMode="Conditional">
                         <ContentTemplate>                       
                          <td >
                                <asp:CheckBox ID="cbDeductProfTax"    Checked='<%# Bind("DeductProfTax") %>' 
                                runat="server" OnCheckedChanged="ProfTaxchecked" AutoPostBack="true" />
                          </td>
                           <td class="AdjustWidth">
                                <asp:TextBox CssClass="AdjustWidth"  ID="txtProfessionalTaxPercent1"    Text='<%# Bind("ProfessionalTaxPercent") %>' runat="server" />
                                  <asp:RangeValidator id="Range1"
                                   ControlToValidate="txtProfessionalTaxPercent1"
                                   MinimumValue="0"
                                   MaximumValue="10000"
                                   Type="Double"
                                    ErrorMessage="The Professional Tax value must be greater then or equal to 0 and less than or equal to 10000"
                                   Text="*"
                                   Display="Dynamic"
                                   ValidationGroup="ALL"
                                   runat="server"/>
                          </td>
                           <td>
                                <asp:CheckBox ID="cbDeductESIC"    Checked='<%# Bind("DeductESIC") %>' 
                                runat="server" OnCheckedChanged="ESICchecked" AutoPostBack="true" />
                          </td>
                           <td>
                                <asp:TextBox ID="txtESICTaxPercent1"   CssClass="AdjustWidth"  Text='<%# Bind("ESICTaxPercent") %>' runat="server" />
                                  <asp:RangeValidator id="Range2"
                                   ControlToValidate="txtESICTaxPercent1"
                                   MinimumValue="0"
                                   MaximumValue="100"
                                   Type="Double"
                                   Display="Dynamic"
                                   ErrorMessage="The ESIC value must be greater then or equal to 0 and less than or equal to 100"
                                   Text="*"
                                   ValidationGroup="ALL"
                                   runat="server"/>
                          </td>
                            
                          <td>
                                <asp:CheckBox ID="cbDeductPF"    Checked='<%# Bind("DeductPF") %>' 
                                runat="server" OnCheckedChanged="PFchecked" AutoPostBack="true" />
                          </td>  
                          <td>
                                <asp:TextBox ID="txtProvidentFundPercent1" CssClass="AdjustWidth"   Text='<%# Bind("ProvidentFundPercent") %>' runat="server" />
                                   <asp:RangeValidator id="Range3"
                                   ControlToValidate="txtProvidentFundPercent1"
                                   MinimumValue="0"
                                   MaximumValue="100"
                                   Type="Double"
                                   Display="Dynamic"
                                   ErrorMessage="The Provident Fund value must be greater then or equal to 0 and less than or equal to 100"
                                   Text="*"
                                   ValidationGroup="ALL"
                                   runat="server"/>
                          </td>             
                          <td>
                                <asp:CheckBox ID="cbCreditTA" Checked='<%#Eval("CreditTA") == DBNull.Value ? false : Convert.ToBoolean(Eval("CreditTA"))   %>'  
                                runat="server" OnCheckedChanged="TAchecked" AutoPostBack="true" />
                          </td>   
                          <td>
                                <asp:TextBox ID="txtTravelAllowance1"  CssClass="AdjustWidth"  Text='<%# Eval("TravelAllowance") %>' runat="server" />
                          </td> 
                           <td>
                                <asp:CheckBox ID="cbCreditDA" Checked='<%#Eval("CreditDA") == DBNull.Value ? false : Convert.ToBoolean(Eval("CreditDA"))   %>'   
                                runat="server" OnCheckedChanged="DAchecked" AutoPostBack="true" />
                          </td>   
                          <td>
                                <asp:TextBox ID="txtDearnessAllowance1" CssClass="AdjustWidth"   Text='<%# Eval("DearnessAllowance") %>' runat="server" />
                          </td>  
                          </ContentTemplate></asp:UpdatePanel>                                         
                         </tr> 
                         <tr >
                            <td id="lblA" colspan="2">
                            Allowance Daily
                            </td>
                            <td id="lblAM" colspan="2">
                                <asp:Label ID="lblAllowanceMonthly" runat="server" Text="Allowance Monthly" />
                            </td>                            
                            <td id="lblAD" colspan="2">
                                <asp:Label ID="lblAllowanceDaily"   Text="Allowance Daily" runat="server" />
                            </td>
                          <td id="lblBL" colspan="2">
                                <asp:Label ID="lblBalanceLeaves"   Text="Leaves Balance" runat="server" />
                            </td>                           
                          <td id="lblYPL" colspan="2">
                                <asp:Label ID="lblYearlyPaidLeaves"   Text="Yearly Paid Leaves" runat="server" />
                            </td>    
                         </tr>
                          <tr>
                            <td id="cbA" colspan="2">
                             <asp:CheckBox ID="cbIsAllowanceDaily" runat="server" onclick="ShowHideAllowance(this.checked);" Checked='<%# Bind("IsAllowanceDaily") %>' />
                            </td>
                            <td id="txtAM" colspan="2">
                                <asp:TextBox ID="txtAllowanceMonthly" runat="server"  Text='<%# Bind("AllowanceMonthly") %>' SkinID="halfWidth" />
                            </td>                           
                            
                            <td id="txtAD" colspan="2">
                                <asp:TextBox ID="txtAllowanceDaily" runat="server" Text='<%# Bind("AllowanceDaily") %>' SkinID="halfWidth" />                                
                            </td>
                            <td id="lblBL1" colspan="2">
                             <%--<asp:TextBox ID="txtBalanceLeaves" runat="server" Text='<%# Bind("BalanceLeaves") %>'  CssClass="HiddenClass" /> --%>
                             <asp:LinkButton ID="lnkBalanceLeaves"  runat="server" Text='<%# Eval("BalanceLeaves") == DBNull.Value ? 0.0 :  Eval("BalanceLeaves") %>' OnClientClick="return false;" ></asp:LinkButton>                               
                             <asp:Panel ID="EpPP" runat="server"  style="display: none; background-color:#FBFBEF" Width="60%">
                             <span style="float:right"> 
                             <asp:ImageButton ID="imgBloseLeaves" runat="server" ImageUrl="~/Images/close.jpg" Height="25px" Width="25px" OnClientClick="TriggerClick();return false;" /></span>
                              <span style="float:left; width:650px;">
                                    <asp:UpdatePanel ID="EuPop" runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate>                                      
                                    <uc:LT ID="UCLeavesTransaction" runat="server" />      
                                    </ContentTemplate>
                                    </asp:UpdatePanel> 
                                    </span>
                             <cc1:ModalPopupExtender ID="Epop" runat="server" BackgroundCssClass="modalPopup"  BehaviorId="ModelPopUp1"
                               TargetControlID="lnkBalanceLeaves" PopupControlID="EpPP" X="50" Y="100"   >                                    
                           </cc1:ModalPopupExtender>
                          <%-- <asp:Button ID="EClose" runat="server" Text="Close" OnClientClick="TriggerClick();return false;" /> --%></asp:Panel>                                                             
                            </td>                            
                            <td id="lblYPL1" colspan="2">
                             <asp:TextBox ID="txtYearlyPaidLeaves" runat="server" Text='<%# Bind("YearlyPaidLeaves") %>' SkinID="halfWidth" 
                               ToolTip="Yearly paid is for information purpose only.Does not get added or deducted from leaves balance of employee." />                                                              
                            </td>                
                         </tr>                
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
</asp:DetailsView>
<table class="TableClass" style="width:980px !Important; margin-left:7px;">
   <tr class="theadColumnWithBackground"> <td>&nbsp;</td></tr>  
</table>
</asp:Panel>
  
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
    

 


</asp:Content>
<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="EmployeeLookup.ascx.cs" Inherits="Payroll.Controls.EmployeeLookup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
 <script type="text/javascript">

     function GetEmployeeOnDemand(source, eventargs) 
     {
         var employeeIdLabel = document.getElementById("<%= empID.ClientID %>");
        var empId = eventargs.get_value();
        employeeIdLabel.value = empId;
    }
    
//    function HideImage(txtBox) {
//        document.getElementById(txtBox).style.backgroundImage = 'none';
//    } 
//function ShowImage(txtBox) 
//        {             
//            document.getElementById(txtBox).style.backgroundImage =  'url(Images/ajax-loader2.gif)';
//            document.getElementById(txtBox).style.backgroundRepeat = 'no-repeat'; 
//            document.getElementById(txtBox).style.backgroundPosition = '95% 2%';             
//        }
//        function HideImage(txtBox) {
//            document.getElementById(txtBox).style.backgroundImage = 'none';
//        } 

</script> 

<table  style="border:solid 1px #498AF3; width:auto;">
<tr  >
     
    <td >
       <asp:Label ID="test" runat="server" Text="Employee Name"   />
    </td>            
    <td>
   <%--ServicePath="~/Service/CommonService.asmx" removed not calling service anymore only pagemethod--%>
      <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="EmployeeName" 
                            ServiceMethod="GetEmployeeOnDemand"  MinimumPrefixLength="1"
                            ServicePath="~/Payroll/MainService.asmx" 
                            CompletionInterval="1000" CompletionSetCount="20" DelimiterCharacters=";, :"
                            OnClientItemSelected="GetEmployeeOnDemand"  >
                        </asp:AutoCompleteExtender> 
        <asp:TextBox ID="employeeName" runat="server" Width="200px" OnTextChanged="EmployeeName_TextChanged" AutoPostBack="true" />      
                            
    </td>
    
    <td>
        <asp:Label ID="Label1" runat="server" Text="Employee ID" />
    </td>
    <td>
      <asp:TextBox ID="empID" runat="server" Width="50px" Enabled="false"  />
    </td>
    <td>
    <asp:Button ID="Reset" runat="server" Text="Clear" onclick="Reset_Click" /></td>
</tr>
</table>
 
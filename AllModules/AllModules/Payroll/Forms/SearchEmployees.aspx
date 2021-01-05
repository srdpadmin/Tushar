<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="SearchEmployees.aspx.cs" 
Inherits="Payroll.Forms.SearchEmployees" MasterPageFile="~/Payroll/Payroll.Master" Theme="Default" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>--%>
<asp:Content ID="headr" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function ShowHide(id) {
        var el = document.getElementById('AdvSearch');
        var expcol = document.getElementById('imgA');
        if (el.style.display == 'none') {
            el.style.display = 'block';
            /*expcol.src = "Images/imgUblue.png" */
            expcol.src = "../../Images/imgUblue.PNG"
        }
        else {
            el.style.display = 'none';
            /* expcol.src = "Images/imgDblue.png" */
            expcol.src = "../../Images/imgDblue.PNG"
        }
    }    
</script>
</asp:Content>
<asp:Content ID="x" ContentPlaceHolderID="main" runat="server">
<ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:60px;"><a href="SearchEmployees.aspx" title="Payroll"><b>I&nbsp;-&nbsp;Pay</b></a> </li>
    <li> Search Employees </li>
    
   
    </ul>
<script type="text/javascript">
    function checkid(hdnfield) {
        var x = hdnfield.value;
        alert(x);
    }
</script>
 <asp:Panel ID="SearchTable" runat="server" >
 
 <div style="border:solid 1px #498AF3; width:545px;" >     
    <asp:TextBox ID="txtSearch" runat="server" BorderStyle="None"  Height="18px" Width="500px"  ></asp:TextBox>        
    <asp:ImageButton ID="search" runat="server" CssClass="vAlign" ImageUrl="~/Images/imgSBlue.gif"   /> <%--OnClick="SearchBtnClick"--%>
    <a href="#" onclick="javascript:ShowHide(this);return false;" style="vertical-align:middle;">
    <input type="image"  src="../../Images/imgDblue.PNG" id="imgA" /></a>  
    <ajax:TextBoxWatermarkExtender ID="ajaxWaterMark" runat="server" TargetControlID="txtSearch"
     WatermarkText="Search..." ></ajax:TextBoxWatermarkExtender>
    
 </div>
<div id="AdvSearch" style="border:solid 1px #AACCEE; width:545px;display:none; margin-top:5px;">

<div style="border:solid 1px #498AF3; height:20px; background-color:#498AF3; color:White;">
<div style="margin-left:5px;margin-top:2px;">
<asp:Label ID="Label11"  runat="server" Text=" Advanced Search"  SkinID="" />
</div>
</div>

<div>
<table >
    <tr>
        <td>
            Employee ID  </td>
        <td>
            <asp:TextBox ID="EmployeeID" Width="150px" runat="server"></asp:TextBox>
            
        </td>
        <td>
           EmployeeName
        </td>
        <td>
            <asp:TextBox ID="EmployeeName" Width="150px" runat="server"></asp:TextBox>
            
        </td>
        <td>
         <asp:Button runat="server" ID="Reset" Text="Reset" onclick="Reset_Click"     />
        </td>   
    </tr>
      
</table>
 </div>
 </div>
 </asp:Panel>
<br />
<asp:UpdatePanel ID="sup" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="AddDeleteButtonPanel" runat="server">
<table>
    <tr>  
        <td style="padding-left:20px;">&nbsp;</td> 
            
        <td>
        <asp:ImageButton runat="server" ID="AddNewButton" AlternateText="Add New Employee" ToolTip="Add New Employee"
         OnClick="AddNewButton_Click" ImageUrl="~/Images/iAdd.PNG" />            
        </td>        
        <td>
        <asp:ImageButton runat="server" ID="DeleteButton" AlternateText="Delete Employee" ToolTip="Delete Employee"
         OnClick="DeleteButton_Click" ImageUrl="~/Images/iBin.gif" OnClientClick="return confirm('Employee & Salary details will be deleted! Do you want to continue ?')" />         
        </td> 
    </tr>
</table>
</asp:Panel>


 
 <asp:ObjectDataSource ID="EmpSearchODS" runat="server" 
                        TypeName="Payroll.BusLogic.Employee" 
                        SelectMethod="SearchEmployees" >
 <SelectParameters>
    <asp:ControlParameter ControlID="txtSearch" ConvertEmptyStringToNull="true" Name="searchText" Type="String" />
    <asp:ControlParameter ControlID="EmployeeName" ConvertEmptyStringToNull="true" Name="EmployeeName" Type="String" />
    <asp:ControlParameter ControlID="EmployeeID" ConvertEmptyStringToNull="true" Name="EmpID" Type="String" />
    
 </SelectParameters>
 </asp:ObjectDataSource>
 <%--OnRowCommand="SearchViewItemSelected" --%>
 
 <asp:GridView ID="EmpGV" runat="server" OnRowCommand="SearchViewItemSelected"
           DataSourceID="EmpSearchODS"   DataKeyNames="ID"  OnPageIndexChanging="EmpGV_PageIndexChanging"
           AutoGenerateColumns="false" SkinID="metro"> 
 <Columns > 
     <asp:TemplateField HeaderText="Select">
     <ItemTemplate>        
     <asp:RadioButton ID="rbtnSelect" runat="server"  AutoPostBack="true"   OnCheckedChanged="SelectButton_Click" />    
    <%-- <asp:HiddenField ID="test" runat="server" Value='<%# Eval("EmpID") %>'  onClientClick="checkid(<%= test.ClientID().value %>);/>--%>
     </ItemTemplate> 
     <EditItemTemplate>&nbsp;</EditItemTemplate>
     </asp:TemplateField>
    <asp:TemplateField HeaderText="ID" SortExpression="ID" >
     <ItemTemplate>
     <asp:HyperLink ID="cli" runat="server" NavigateUrl='<%# Eval("ID","~/Payroll/Forms/ManageEmployee.aspx?ID={0}") %>' Text='<%# Eval("ID") %>' Font-Underline="true"></asp:HyperLink>     
     </ItemTemplate> 
     </asp:TemplateField>
     <asp:TemplateField HeaderText="Salary"  >
     <ItemTemplate>
     <asp:HyperLink ID="sal" runat="server" NavigateUrl='<%# Eval("ID","~/Payroll/Forms/SalaryInformation.aspx?EmpID={0}") %>' Text="Details" Font-Underline="true"></asp:HyperLink>     
     </ItemTemplate> 
     </asp:TemplateField>
     <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName" >
     <ItemTemplate>
     <asp:Label ID="Field11" runat="server" Text='<%# Eval("EmployeeName") %>'  />
     </ItemTemplate> 
     </asp:TemplateField>
     <asp:TemplateField HeaderText="Employee Type" SortExpression="EmpTypeName" >
     <ItemTemplate>
    <asp:Label ID="empType" runat="server" Text='<%# Eval("EmpTypeName") %>'  />
     </ItemTemplate> 
     </asp:TemplateField>
     <asp:TemplateField HeaderText="OverTime Rate" SortExpression="OTRateName" >
     <ItemTemplate>
    <asp:Label ID="OTRate" runat="server" Text='<%# Eval("OTRateName") %>'  />
     </ItemTemplate> 
     </asp:TemplateField>
     <asp:TemplateField HeaderText="Basic Salary" SortExpression="BasicSalary"  >
     <ItemTemplate>
    <asp:Label ID="Field2" runat="server" Text='<%# Eval("BasicSalary") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>
     
     <%--<asp:TemplateField HeaderText="Yearly Paid Leaves  " SortExpression="YearlyPaidLeaves" >
     <ItemTemplate>
    <asp:Label ID="Field3" runat="server" Text='<%# Eval("YearlyPaidLeaves") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>--%>
   <%--  <asp:TemplateField HeaderText="Balance Leaves " SortExpression="BalanceLeaves" >
     <ItemTemplate>
    <asp:Label ID="Field4" runat="server" Text='<%# Eval("BalanceLeaves") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>  --%>  
     <%-- This one shoudl not be used, removed 
     <asp:TemplateField HeaderText="Advance Pending" SortExpression="AdvancePending" >
     <ItemTemplate>
    <asp:Label ID="Field6" runat="server" Text='<%# Eval("AdvancePending") %>' Width="100px" />
     </ItemTemplate> 
     </asp:TemplateField>--%>
    
 </Columns>   
 
 </asp:GridView>
 </ContentTemplate>
</asp:UpdatePanel>

 </asp:Content>
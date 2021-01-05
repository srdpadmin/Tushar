<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ManageEnquiry.aspx.cs"  MasterPageFile="~/Enquiry/Enquiry.Master" Theme="Default" Inherits ="Enquiry.Forms.ManageEnquiry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>
<%@ Register Src="~/Contact/Controls/Contacts.ascx" TagPrefix="uc" TagName="Contacts" %>
<asp:Content ID="header" runat="server" ContentPlaceHolderID="head">
<script type="text/javascript">
    function PrintReportInNewWindow(PO,type) {
        //window.open('/QOModule/Reports/BillingReport.aspx?QID=' + PO, 'NewWindow', 'toolbar=no');
        //window.open('/Billing/Reports/GenerateBill.aspx?ID=' + PO, 'NewWindow', 'toolbar=no');
        var rootPath = '<%= ConfigurationManager.AppSettings["BillRootPath"] %>';
        
        if (type == 'PDF') {
            window.open(rootPath + 'GenerateBill.aspx?ID=' + PO + '&Type=PDF', 'NewWindow', 'toolbar=no');
        } else {
        window.open(rootPath + 'GenerateBill.aspx?ID=' + PO + '&Type=Excel', 'NewWindow', 'toolbar=no');
        }
    }
    function IsOneDecimalPoint(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
        var parts = evt.srcElement.value.split('.');
        if (parts.length > 1 && charCode == 46)
            return false;
        return true;
    }
    function GetCustomerOnDemand(source, eventargs) {
        var CustomerLabel = document.getElementById("ctl00$ctl00$mainMaster$main$ODView$hdnCustomerID");
        var CustomerId = eventargs.get_value();
        CustomerLabel.value = CustomerId;

    }
    function GetItemsOnDemand(source, eventargs) {
        //  http://forums.asp.net/t/1069245.aspx/1
        //  document.getElementById ("hdnCompanyMasterId.ClientID").value =eventargs.get_value();
        //  document.getElementById ("hdnCompanyName.ClientID").value =  document.getElementById ("txtAutoCompleteforCompany.ClientID").value;
        //  debugger;
        var GridView = document.getElementById('<%= ItemsGridView.ClientID %>');
        var rowIndex = document.getElementById('RowIndex');
        //var ItemDescription = GridView.rows[rowIndex.value].cells[2].children[0];
        var intIndex = parseInt(rowIndex.value) + 1;

        var split = eventargs.get_value().split('|');
        
        //GridView.rows[2].cells[2].children[0].innerHTML = eventargs.get_value();
        GridView.rows[intIndex].cells[2].children[0].value = split[0];
        GridView.rows[intIndex].cells[3].children[0].value = split[1];
        GridView.rows[intIndex].cells[4].children[0].value = split[2];
        GridView.rows[intIndex].cells[5].children[0].value = split[3];
        GridView.rows[intIndex].cells[6].children[0].value = split[4];
        //GridView.rows[intIndex].cells[7].children[0].value = split[5];

        //ItemDescription.innerHTML 

    }
    function CheckOtherOption(Terms, Condition, termBox, conditionBox) {

        if (Terms.options[Terms.selectedIndex].value != 'Insert a Term') {
            termBox.disabled = false;
            termBox.value = Terms.options[Terms.selectedIndex].value;

            Terms.disabled = true;
            Terms.style.display = 'none';
        }
        if (Condition.options[Condition.selectedIndex].value != 'Insert a Condition') {

            conditionBox.disabled = false;

            conditionBox.value = Condition.options[Condition.selectedIndex].value;

            Condition.disabled = true;
            Condition.style.display = 'none';
        }

    }

    function TriggerClick() {
        var Triggerbtn = document.getElementById('<%= Trigger.ClientID %>');
        Triggerbtn.click();
    }
</script>

</asp:Content>

<asp:Content ID="order" ContentPlaceHolderID="main" runat="server">
    <div>
    
<asp:UpdatePanel ID="MUP" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Panel ID="AEP" runat="server" >
    <table style="width: 40%;">
    <tr>
            <td>
                <asp:Label ID="lblID" runat="server"  />
            </td>
             <td>
                <asp:ImageButton ID="btnPDFPrint" runat="server" ImageUrl="~/Images/pdficon.jpg" Height="40px" />                      
            </td>
             <td>
                <asp:ImageButton ID="btnExcelPrint" runat="server" ImageUrl="~/Images/excelicon.jpg" />                      
            </td>
            <td>
                <asp:Button ID="btnAmend" runat="server" Text="Edit" OnClick="btnAmend_Click" />
            </td>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"  />
            </td>             
            <td>
                <asp:Button ID="btnInsert" runat="server" Text="Create" OnClick="btnInsert_Click"  /> <%--OnClick="InsertPurchaseOrder_Click"--%>
            </td>                                          
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"   /> <%--OnClick="CancelUpdateItem_Click"--%>
            </td>                    
        </tr>       
   </table>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="Trigger" runat="server" CssClass="HiddenClass" />
<asp:UpdatePanel ID="DUP" runat="server" UpdateMode="Conditional">
<Triggers>
<asp:AsyncPostBackTrigger ControlID="Trigger" />
</Triggers>
<ContentTemplate>
<asp:Panel ID="ODP" runat="server">

<asp:DetailsView ID="ODView" runat="server" GridLines="None" Width="100%" DataSourceID="ODODS"
        AutoGenerateRows="false"  OnDataBound="ODView_DataBound" 
         >        
        <Fields>
            <asp:TemplateField>
                <ItemTemplate>
                    <table  class="TableClass"  cellpadding="0px" cellspacing="0px">
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="LblQO" runat="server"   Text="Enquiry Number" />
                            </td>                            
                            <td>
                                <asp:Label ID="LblRev" runat="server"  Text="Revision No" />
                            </td>
                           
                             <td>
                                <asp:Label ID="amR" runat="server" Text="Amend Reason" />
                            </td>
                              <td>
                                <asp:Label ID="lblDate" runat="server" Text="Enquiry Date" />
                            </td>
                             <td>
                                <asp:Label ID="LblCustomer" Width="100%" Text="Customer" runat="server" />
                            </td>  
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="QONLbl" runat="server" Text='<%# Eval("ID") %>' Width="150px"/>
                            </td>
                            <td>
                                <asp:Label ID="RevisionLbl" runat="server" Text='<%# Eval("Revision") %>' Width="150px" />
                            </td>                          
                             <td>
                                <asp:Label ID="Label3" Text='<%# Eval("AmendReason") %>' Width="95%" runat="server"></asp:Label>
                            </td> 
                            <td>
                               <asp:Label ID="Label4" Text='<%# Eval("EnquiryDate","{0:dd/MM/yyyy}")  %>' runat="server"></asp:Label>                                                              
                            </td>
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                                <asp:Label ID="hdnCustomerID" Text='<%# Eval("CustID") %>'  CssClass="HiddenClass" runat="server" />
                                <asp:Label ID="txtCustomer" Text='<%# Eval("Company") %>'  runat="server"></asp:Label>
                            </td>
                            
                        </tr>                        
                        <tr class="theadColumn">
                            <td>                                
                                <asp:Label ID="LblSuggested" Text="Product Suggested"  runat="server" />                            
                            </td> 
                            <td>
                                <asp:Label ID="LblStatus"    Text="Product Status" runat="server" />
                            </td>                                          
                            <td>
                                <asp:Label ID="LblEnStatus"  Text="Enquiry Status" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="LblFolloUp"  Text="Follow Up Status" runat="server" />
                            </td>
                            <td  style="padding-left:30px;font-weight:normal; text-align:left;" >
                              <asp:Label ID="CustomerRow1"   runat="server" />                              
                            </td> 
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="SuggestedLbl" Text='<%# Eval("ProductSuggested") %>'  runat="server"></asp:Label>
                            </td>                                                       
                           <td>
                               <asp:Label ID="StatusLbl" Text='<%# Eval("ProductStatus")  %>' runat="server"></asp:Label>                                                            
                            </td>
                              <td>
                                <asp:Label ID="EnStatusLbl" Text='<%# Eval("EnquiryStatus") %>'  runat="server"></asp:Label>
                            </td>    
                            <td>
                               <asp:Label ID="FollowUpLbl"  Text='<%# Eval("FollowUpStatus") %>' runat="server" />&nbsp;
                            </td>
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow2" runat="server"></asp:Label>                                
                            </td>
                        </tr>                        
                                                                                                                                  
                        <tr class="theadColumn">
                            <td> <asp:Label ID="LblEstimation"  Text="Estimation" runat="server" /></td>
                            <td> <asp:Label ID="LblAssignedTo"  Text="Assigned To" runat="server" /> </td>                            
                            <td > <asp:Label ID="LblClosureDate"  Text="Closure Date" runat="server" /></td>
                             <td > <asp:Label ID="LblContactPerson"  Text="Contact Person" runat="server" /></td>
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow3" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                        <tr >
                            <td> <asp:Label ID="EstimationLbl"  Text='<%# Eval("Estimation") %>' runat="server" /></td>
                            <td> <asp:Label ID="AssignedToLbl"  Text='<%# Eval("AssignedTo") %>' runat="server" /> </td>                            
                            <td> <asp:Label ID="ClosureDateLbl"  Text='<%# Eval("ClosureDate","{0:dd/MM/yyyy}")  %>' runat="server" /></td>
                             <td> <asp:Label ID="ContactPersonLbl"  Text='<%# Eval("ContactName") %>' runat="server" /> </td>                            
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow4" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                    </table>
                            
                </ItemTemplate>
                <EditItemTemplate>                   
                      <table  class="TableClass" >          
                        <tr class="theadColumn">
                           <td>
                                <asp:Label ID="LblQO" runat="server"   Text="Enquiry Number" />
                            </td>
                            <td>
                                <asp:Label ID="LblRev" runat="server"  Text="Revision No" />
                            </td>
                           
                             <td>
                                <asp:Label ID="amR" runat="server" Text="Amend Reason" />
                            </td>
                              <td>
                                <asp:Label ID="lblDate" runat="server" Text="Enquiry Date" />
                            </td>
                             <td>
                                <asp:Label ID="LblCustomer" Width="100%" Text="Customer" runat="server" />
                            </td>  
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblID" runat="server"  Text='<%# Bind("ID") %>'  />
                                <asp:HiddenField ID="hdnCreatedBy" runat="server" Value='<%# Bind("CreatedBy") %>'   />
                                <asp:HiddenField ID="hdnCreatedOn" runat="server" Value='<%# Bind("CreatedOn","{0:dd/MM/yyyy}")  %>' />
                                 <asp:HiddenField ID="hdnModifiedBy" runat="server" Value='<%# Bind("ModifiedBy") %>'   />
                                <asp:HiddenField ID="hdnModifiedOn" runat="server" Value='<%# Bind("ModifiedOn","{0:dd/MM/yyyy}")  %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblRevision" runat="server" Text='<%# Bind("Revision") %>'  />
                            </td>
                             <td>                                
                                <asp:TextBox ID="txtAmendReason"  Text='<%# Bind("AmendReason") %>' Width="95%" runat="server"></asp:TextBox> <%--Visible='<%# IsAmendEdit %>'--%>
                            </td>               
                             <td>
                               <asp:TextBox ID="txtEnquiryDate" Text='<%# Bind("EnquiryDate","{0:dd/MM/yyyy}")  %>' runat="server" />                                                              
                               <asp:ImageButton ID="imgClose3" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <asp:CalendarExtender ID="ce3" runat="server"   TargetControlID="txtEnquiryDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgClose3"  />
                               
                            </td>                                        
                            <td style="padding-left:30px;font-weight:normal; text-align:left;"> 
                            <asp:Label ID="hdnCustomerID" Text='<%# Bind("CustID") %>' CssClass="HiddenClass"  runat="server"/>    
                            <asp:UpdatePanel ID="EuPop" runat="server"  UpdateMode="Conditional">
                            <ContentTemplate> 
                             
                            <asp:LinkButton ID="EtxtCustomer"  Width="250px" runat="server" ></asp:LinkButton>
                               <asp:ModalPopupExtender ID="Epop" runat="server" BackgroundCssClass="modalPopup"
                                   TargetControlID="EtxtCustomer" PopupControlID="EpPP" X="50" Y="100" CancelControlID="EClose"  >                                    
                               </asp:ModalPopupExtender>
                               <asp:Panel ID="EpPP" runat="server"  style="display: none; background-color:#FBFBEF">
                                <uc:Contacts ID="Econtacts" runat="server" IsPopup="true" />                                                            
                                <asp:Button ID="EClose" runat="server" Text="Close" OnClientClick="TriggerClick();return false;"  />                               
                            </asp:Panel>
                            </ContentTemplate>
                            </asp:UpdatePanel>                                
                            </td>
                        </tr>                        
                        <tr class="theadColumn">
                            <tr class="theadColumn">
                            <td>                                
                                <asp:Label ID="LblSuggested" Text="Product Suggested"  runat="server" />                            
                            </td> 
                            <td>
                                <asp:Label ID="LblStatus"    Text="Product Status" runat="server" />
                            </td>                                          
                            <td>
                                <asp:Label ID="LblEnStatus"  Text="Enquiry Status" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="LblFolloUp"  Text="Follow Up Status" runat="server" />
                            </td>
                            <td  style="padding-left:30px;font-weight:normal; text-align:left;" >
                              <asp:Label ID="CustomerRow1"   runat="server" />                              
                            </td> 
                        </tr>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtProductSuggested" Text='<%# Bind("ProductSuggested") %>'  runat="server"/>
                            </td>                                                       
                            <td>
                                <asp:DropDownList ID="ddlProductStatus" runat="server" Width="100%"></asp:DropDownList>                               
                                <asp:HiddenField ID="hdnProductStatus" runat="server" Value='<%# Bind("ProductStatus") %>' />
                            </td>
                              <td>
                                <asp:DropDownList ID="ddlEnquiryStatus" runat="server" Width="100%"></asp:DropDownList>   
                                  <asp:HiddenField ID="hdnEnquiryStatus" runat="server" Value='<%# Bind("EnquiryStatus") %>' />                             
                            </td>    
                            <td>
                             <asp:DropDownList ID="ddlFollowUpStatus" runat="server" Width="100%"></asp:DropDownList>    
                               <asp:HiddenField ID="hdnFollowUpStatus" runat="server" Value='<%# Bind("FollowUpStatus") %>' />                              
                            </td>                            
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow2" runat="server"></asp:Label>                                
                            </td>
                        </tr>                        
                       
                        <tr class="theadColumn">
                           <td> <asp:Label ID="LblEstimation"  Text="Estimation" runat="server" /></td>
                            <td> <asp:Label ID="LblAssignedTo"  Text="Assigned To" runat="server" /> </td>                            
                            <td > <asp:Label ID="LblClosureDate"  Text="Closure Date" runat="server" /></td>
                             <td > <asp:Label ID="LblContactPerson"  Text="Contact Person" runat="server" /></td>
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow3" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                        <tr >
                            <td> <asp:TextBox ID="txtEstimation"  Text='<%# Bind("Estimation") %>' runat="server" /></td>
                            <td>  <asp:DropDownList ID="ddlAssignedTo" runat="server" Width="100%"></asp:DropDownList>   
                            <asp:HiddenField ID="hdnAssignedTo"  Value='<%# Bind("AssignedTo") %>' runat="server" /> </td>                            
                            <td> <asp:TextBox ID="txtClosureDate"  Text='<%# Bind("ClosureDate","{0:dd/MM/yyyy}")  %>' runat="server" />
                             <asp:ImageButton ID="imgClose1" runat="server" ImageUrl="~/Images/Calendar.png" />
                                 <asp:CalendarExtender ID="ce2" runat="server"   TargetControlID="txtClosureDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgClose1"  /></td>
                             <td> <asp:TextBox ID="txtContactName"  Text='<%# Eval("ContactName") %>' runat="server" /> </td>                            
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow4" runat="server"></asp:Label>                                
                            </td>                           
                        </tr>
                    </table>                    
                </EditItemTemplate>
                <InsertItemTemplate>
                 
                <table  class="TableClass">                       
                        
                        <tr class="theadColumn">
                            <td>
                                <asp:Label ID="LblQO" runat="server"   Text="Enquiry Number" />
                            </td>
                            <td>
                                <asp:Label ID="LblRev" runat="server"  Text="Revision No" />
                            </td>
                           
                             <td>
                                <asp:Label ID="amR" runat="server" Text="Amend Reason" />
                            </td>
                              <td>
                                <asp:Label ID="lblDate" runat="server" Text="Enquiry Date" />
                            </td>
                             <td>
                                <asp:Label ID="LblCustomer" Width="100%" Text="Customer" runat="server" />
                            </td>  
                           
                        </tr>
                        <tr>                            
                           <td>
                                <asp:Label ID="lblID" runat="server"  Text='<%# Bind("ID") %>'  />
                                <asp:HiddenField ID="hdnCreatedBy" runat="server" Value='<%# Bind("CreatedBy") %>'   />
                                <asp:HiddenField ID="hdnCreatedOn" runat="server" Value='<%# Bind("CreatedOn","{0:dd/MM/yyyy}")  %>' />
                                <asp:HiddenField ID="hdnModifiedBy" runat="server" Value='<%# Bind("ModifiedBy") %>'   />
                                <asp:HiddenField ID="hdnModifiedOn" runat="server" Value='<%# Bind("ModifiedOn","{0:dd/MM/yyyy}")  %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblRevision" runat="server" Text='<%# Bind("Revision") %>'  />
                            </td>
                             <td>                                
                                <asp:Label ID="txtAmendReason"  Text='<%# Bind("AmendReason") %>' Width="95%" runat="server"></asp:Label> <%--Visible='<%# IsAmendEdit %>'--%>
                            </td>               
                             <td>
                               <asp:TextBox ID="txtEnquiryDate" Text='<%# Bind("EnquiryDate","{0:dd/MM/yyyy}")  %>' ReadOnly="true" runat="server" />                                                              
                               <asp:ImageButton ID="calenderImg" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <asp:CalendarExtender runat="server"   TargetControlID="txtEnquiryDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="calenderImg"  />
                            </td>  
                           <td style="padding-left:30px;font-weight:normal; text-align:left;">
                           <asp:Label ID="hdnCustomerID" Text='<%# Bind("CustID") %>' CssClass="HiddenClass" runat="server"/>                             
                            <asp:UpdatePanel ID="uPop" runat="server"  UpdateMode="Conditional">
                            <ContentTemplate>
                               <asp:LinkButton ID="txtCustomer" Text="Add New Customer" Width="250px" runat="server" ></asp:LinkButton> <%--OnTextChanged="CustomerChanged" AutoPostBack="true"--%>                                                              
                               <asp:ModalPopupExtender ID="pop" runat="server" BackgroundCssClass="modalPopup"
                                   TargetControlID="txtCustomer" PopupControlID="pPP" X="50" Y="100" CancelControlID="Close"  >
                               </asp:ModalPopupExtender>   
                               <asp:Panel ID="pPP" runat="server"  Style="display: none; background-color:#FBFBEF">
                                <uc:Contacts ID="contacts" runat="server" />                                                            
                                <asp:Button ID="Close" runat="server" Text="Close" OnClientClick="TriggerClick();return false;"  />                               
                            </asp:Panel>      
                            </ContentTemplate>
                            </asp:UpdatePanel>
                               
                            </td>
                        </tr>
                        <tr class="theadColumn">
                             <tr class="theadColumn">
                            <td>                                
                                <asp:Label ID="LblSuggested" Text="Product Suggested"  runat="server" />                            
                            </td> 
                            <td>
                                <asp:Label ID="LblStatus"    Text="Product Status" runat="server" />
                            </td>                                          
                            <td>
                                <asp:Label ID="LblEnStatus"  Text="Enquiry Status" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="LblFolloUp"  Text="Follow Up Status" runat="server" />
                            </td>
                            <td  style="padding-left:30px;font-weight:normal; text-align:left;" >
                              <asp:Label ID="CustomerRow1"   runat="server" />                              
                            </td> 
                        </tr>
                        </tr>   
                        <tr>   
                            <td>
                                <asp:TextBox ID="txtProductSuggested" Text='<%# Bind("ProductSuggested") %>'  runat="server"/>
                            </td>                                                       
                            <td>
                                <asp:DropDownList ID="ddlProductStatus" runat="server" Width="100%"></asp:DropDownList>                               
                            </td>
                              <td>
                                <asp:DropDownList ID="ddlEnquiryStatus" runat="server" Width="100%"></asp:DropDownList>                                
                            </td>    
                            <td>
                             <asp:DropDownList ID="ddlFollowUpStatus" runat="server" Width="100%"></asp:DropDownList>                                  
                            </td>                            
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow2" runat="server"></asp:Label>                                
                            </td>
                        </tr>                        
                          
                        
                         <tr class="theadColumn">
                          <td> <asp:Label ID="LblEstimation"  Text="Estimation" runat="server" /></td>
                            <td> <asp:Label ID="LblAssignedTo"  Text="Assigned To" runat="server" /> </td>                            
                            <td > <asp:Label ID="LblClosureDate"  Text="Closure Date" runat="server" /></td>
                             <td > <asp:Label ID="LblContactPerson"  Text="Contact Person" runat="server" /></td>
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow3" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                        <tr >                        
                            <td> <asp:TextBox ID="txtEstimation"  Text='<%# Bind("Estimation") %>' runat="server" /></td>
                            <td> <asp:DropDownList ID="ddlAssignedTo" runat="server" Width="100%"></asp:DropDownList>    </td>                            
                            <td> <asp:TextBox ID="txtClosureDate"  Text='<%# Bind("ClosureDate","{0:dd/MM/yyyy}")  %>' runat="server" />
                                 <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/Calendar.png" />
                                 <asp:CalendarExtender ID="ce1" runat="server"   TargetControlID="txtClosureDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgClose"  />
                            </td>
                             <td> <asp:TextBox ID="txtContactName"  Text='<%# Eval("ContactName") %>' runat="server" /> </td>                
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow4" runat="server"></asp:Label>                                
                            </td>     
                        </tr>                      
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
        </Fields>              
</asp:DetailsView>
  
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
<br />
<asp:UpdatePanel ID="ITUP" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="TabPanel" runat="server">
<asp:TabContainer ID="Tab" runat="server" Width="100%">

<asp:TabPanel ID="ItemsTab" runat="server" HeaderText="Items">
<ContentTemplate>
<asp:Panel ID="ItemsBtnPanel" runat="server">
<table cellpadding="2" border="0" cellspacing="0" width="1220px" style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAdd" Height="20px" runat="server" ImageUrl="~/Images/iAdd.bmp"
                    AlternateText="Add" OnClick="imgAddItem_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEdit" Height="22px" runat="server" ImageUrl="~/Images/iEdit.gif"
                    AlternateText="Edit" OnClick="imgEditItem_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDelete" Height="22px" runat="server" ImageUrl="~/Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDeleteItem_Click" CausesValidation="false" ToolTip="Delete"
                    ></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSave" Height="22px" runat="server" ImageUrl="~/Images/iSave.gif"
                    AlternateText="Update" OnClick="imgUpdateItem_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancel" Height="22px" runat="server" ImageUrl="~/Images/iCan.gif"
                    AlternateText="Cancel" OnClick="imgCancelItem_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
             <td style="width: 5px;" align="left">
            </td>
            <td style="width: 1085px;">
                <asp:HiddenField ID="hdnEditMode" Value="" runat="server" />
            </td>                      
        </tr>
    </table> 
<asp:ValidationSummary ID="vs" ValidationGroup="validateItems" runat="server" />
</asp:Panel> 

 <asp:GridView ID="ItemsGridView" runat="server" AutoGenerateEditButton="false"
    AutoGenerateDeleteButton="false" AutoGenerateColumns="false" SkinID="metro"    
    ShowFooter="true" >  
    <Columns>        
        <asp:TemplateField HeaderText="Select">
         <ItemTemplate>
            <asp:CheckBox ID="cbSelect" EnableTheming="true" Font-Bold="true"
                runat="server"    />           
        </ItemTemplate>
        <EditItemTemplate>
         <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' CssClass="HiddenClass" ></asp:Label>
            <asp:TextBox ID="txtEnquiryID" Text='<%# Bind("EnquiryID") %>' CssClass="HiddenClass" runat="server"   />
        </EditItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Item Code / Type"  >
      
            <ItemTemplate>
                <asp:Label ID="c2" runat="Server" Width="100px" Text='<%# Eval("Code") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtCode"
                    ServiceMethod="GetItemsOnDemand" ServicePath="~/Billing/BillingService.asmx" MinimumPrefixLength="1"
                    CompletionInterval="1000" CompletionSetCount="20" DelimiterCharacters=";, :"
                    OnClientItemSelected="GetItemsOnDemand">
                </asp:AutoCompleteExtender>
                <asp:TextBox ID="txtCode" runat="server" Width="100"  Text='<%# Bind("Code") %>' /><%-- OnTextChanged="ItemChanged" AutoPostBack="true"--%>
               
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Item Description / Specification" >           
            <ItemTemplate>
                <asp:Label ID="c3" runat="Server" Width="250px" Text='<%# Eval("Description") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="txtItemDescription"
                    ServiceMethod="GetItemDescriptionsOnDemand" ServicePath="~/Billing/BillingService.asmx" MinimumPrefixLength="1"
                    CompletionInterval="1000" CompletionSetCount="20" DelimiterCharacters=";, :">
                </asp:AutoCompleteExtender>
                <asp:TextBox ID="txtItemDescription" runat="server" Width="250px" Text='<%# Bind("Description") %>' /> <%--OnTextChanged="ItemChanged" AutoPostBack="true" --%>
            </EditItemTemplate>
        </asp:TemplateField>         
        <asp:TemplateField HeaderText="Quantity"  >          
            <ItemTemplate>
             <asp:Label ID="c4" runat="Server" Width="60px" Text='<%# Eval("Quantity","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>   
             <asp:TextBox ID="txtQuantity" runat="server" Width="70px" Text='<%# Bind("Quantity","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);" />
               <asp:FilteredTextBoxExtender ID="quantityValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtQuantity" />                              
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Unit"  >           
            <ItemTemplate>
                <asp:Label ID="c5" runat="Server" Width="30px" Text='<%# Eval("Unit") %>' />                
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtUnit" runat="server" Width="30px" Text='<%# Bind("Unit") %>' />
                <asp:FilteredTextBoxExtender ID="unitValidator" runat="server" Enabled="True"
                    FilterType="LowercaseLetters, UppercaseLetters"  TargetControlID="txtUnit" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rate / Unit" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>
                <asp:Label ID="c6" runat="Server" Width="70px" Text='<%# Eval("Rate","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtRate" runat="server" Width="70px" Text='<%# Bind("Rate","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);" />
               <asp:FilteredTextBoxExtender ID="rateValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtRate" />
            </EditItemTemplate>
             <FooterTemplate>                    
                <asp:Label ID="Totals" runat="Server" Text="Totals" Font-Bold="true"   />                   
            </FooterTemplate>
        </asp:TemplateField>
       <%-- <asp:TemplateField HeaderText="Discount %" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>                
                <asp:Label ID="c7" runat="Server" Width="50px" Text='<%# Eval("Discount","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>            
               <asp:TextBox ID="txtDiscount" runat="server" Width="70px" Text='<%# Bind("Discount","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);" />
               <asp:FilteredTextBoxExtender ID="discountValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtDiscount" />
            </EditItemTemplate>
             <FooterTemplate>                    
                <asp:Label ID="lblDiscount" runat="Server" Width="35px"    />                   
            </FooterTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Tax %" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>
                <asp:Label ID="c8" runat="Server" Width="30px" Text='<%# Eval("Tax","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtTax" runat="server" Width="70px" Text='<%# Bind("Tax","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);" />
               <asp:FilteredTextBoxExtender ID="taxValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtTax" />      
            </EditItemTemplate>            
             <FooterTemplate>            
            <asp:Label ID="lblTax" runat="Server" Width="35px"   />                   
            </FooterTemplate>
        </asp:TemplateField>
        <%-- <asp:TemplateField HeaderText="Tax Amount" FooterStyle-CssClass="columnBorder">            
            <ItemTemplate>
                <asp:Label ID="c9" runat="Server" Width="70px" Text='<%# Eval("TaxAmount","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label ID="lblTaxAmount" runat="server" Width="70px" Text='<%# Bind("TaxAmount","{0:N2}") %>'  />
            </EditItemTemplate>            
             <FooterTemplate>            
            <asp:Label ID="lblTaxAmount" runat="Server" Width="35px"  />                   
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Item Total" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>
                <asp:Label ID="c10" runat="Server" Width="70px" Text='<%# Eval("TotalAmount","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label ID="lblTotalAmount" runat="server" Width="70px"  Text='<%# Bind("TotalAmount","{0:00}") %>' />
            </EditItemTemplate>
             <FooterTemplate>                               
                <asp:Label ID="lblTotalAmount" runat="Server" Width="70px"   />          
            </FooterTemplate>
        </asp:TemplateField>--%>
       
    </Columns>
    
</asp:GridView>
</ContentTemplate>
</asp:TabPanel>
<asp:TabPanel ID="TermsTab" runat="server" HeaderText="Terms & Condition">
<ContentTemplate>
<asp:Panel ID="TermsBtnPanel" runat="server">
<table cellpadding="2" border="0" cellspacing="0" width="1220px" style="height: 0px;">
        <tr>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgAddTerm" Height="20px" runat="server" ImageUrl="~/Images/iADD.bmp"
                    AlternateText="Add" OnClick="imgAddTerm_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgEditTerm" Height="22px" runat="server" ImageUrl="~/Images/iEdit.gif"
                    AlternateText="Edit" OnClick="imgEditTerm_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDeleteTerm" Height="22px" runat="server" ImageUrl="~/Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDeleteTerm_Click" CausesValidation="false" ToolTip="Delete"
                   ></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSaveTerm" Height="22px" runat="server" ImageUrl="~/Images/iSave.gif"
                    AlternateText="Update" OnClick="imgUpdateTerm_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancelTerm" Height="22px" runat="server" ImageUrl="~/Images/iCan.gif"
                    AlternateText="Cancel" OnClick="imgCancelTerm_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
             <td style="width: 5px;" align="left">
            </td>
            <td style="width: 1085px;">
                <asp:HiddenField ID="HiddenField1" Value="" runat="server" />
            </td>                      
        </tr>
    </table>
  </asp:Panel>
   <asp:GridView ID="TermsGridView" runat="server" 
    AutoGenerateEditButton="false"
    AutoGenerateDeleteButton="false" AutoGenerateColumns="false" SkinID="metro"    
    ShowFooter="true" 
    OnRowDataBound="TermsGridView_RowDataBound"
      >
    
    <Columns>     
        <asp:TemplateField HeaderText="Select" >
         <ItemStyle  Width="10%" />
         <ItemTemplate>
            <asp:CheckBox ID="cbSelect" EnableTheming="true" Font-Bold="true"
                runat="server"    />           
        </ItemTemplate>
        <EditItemTemplate>
         <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' CssClass="HiddenClass" ></asp:Label>
         <asp:TextBox ID="txtEnquiryID" Text='<%# Bind("EnquiryID") %>' CssClass="HiddenClass" runat="server"   />
        </EditItemTemplate>
        </asp:TemplateField>                            
        
        <asp:TemplateField HeaderText="Term"  >
            <ItemStyle  Width="25%" />
            <ItemTemplate>
                <asp:Label ID="ab2" runat="Server" Text='<%# Eval("Term") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <div style="float: left;">
                    <asp:DropDownList ID="Termddl" runat="server" Width="200px" DataSourceID="TODS"
                        DataTextField="Value" DataValueField="Value" />
                </div>
                <br />
                <div  style="float: left;" >
                    <asp:TextBox ID="TermBox" runat="server" Width="200px" Text='<%# Bind("Term") %>' />
                </div>
                
                <asp:ObjectDataSource ID="TODS" runat="server" SelectMethod="GetAllTermsFromCache" TypeName="TermsConditions.Common.WebCommon">
                </asp:ObjectDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Condition" >
            <ItemStyle  Width="60%" />
            <ItemTemplate>
                <asp:Label ID="ab3" runat="Server" Text='<%# Eval("Condition") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <div style="float: left;">
                    <asp:DropDownList ID="Conditionddl" runat="server" Width="500px" DataSourceID="CODS"
                        DataTextField="Value" DataValueField="Value" />
                </div>
                <div style="float: left;">
                    <asp:TextBox ID="ConditionBox" runat="server" Width="500px" Text='<%# Bind("Condition") %>' /><br />
                </div>
                
                <asp:ObjectDataSource ID="CODS" runat="server" SelectMethod="GetAllConditionsFromCache" TypeName="TermsConditions.Common.WebCommon">
                </asp:ObjectDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</ContentTemplate>
</asp:TabPanel>
</asp:TabContainer>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

<asp:ObjectDataSource ID="ODODS" runat="server" TypeName="Enquiry.BusLogic.Enquiry" ConflictDetection="CompareAllValues"
    SelectMethod="GetEnquiry" DataObjectTypeName="Enquiry.Data.EnquiryData"
    OldValuesParameterFormatString="oOrder">
    <SelectParameters>
    <asp:QueryStringParameter ConvertEmptyStringToNull="true" QueryStringField="ID" Name="ID" Type="String" />    
    </SelectParameters>    
</asp:ObjectDataSource>


    </div>
  </asp:Content>
  
   <%--<td>          
                            <asp:AutoCompleteExtender ID="ACE3" runat="server"
                                ServiceMethod="GetCustomerOnDemand"
                                ServicePath="Billing.BillingService.asmx"
                                TargetControlID="txtCustomer"
                                MinimumPrefixLength="1"
                                CompletionInterval="1000" 
                                CompletionSetCount="20" 
                                DelimiterCharacters=";, :"
                                OnClientItemSelected="GetCustomerOnDemand"
                            >
                            
                            </asp:AutoCompleteExtender>   
                                <asp:TextBox ID="txtCustomer" Text='<%# Eval("Company") %>' Width="95%" runat="server"></asp:TextBox> <%--OnTextChanged="CustomerChanged" AutoPostBack="true"
                                      <span style="display:none;">
                                <asp:HiddenField ID="hdnCustomerID" Value='<%# Bind("CustID") %>'   runat="server" /></span>
                            </td>--%>


                             <%--<asp:AutoCompleteExtender ID="ACE1" runat="server"
                                ServiceMethod="GetCustomerOnDemand"
                               ServicePath="~/Billing/BillingService.asmx"
                                TargetControlID="txtCustomer"
                                MinimumPrefixLength="1"
                                CompletionInterval="1000" 
                                CompletionSetCount="20" 
                                DelimiterCharacters=";, :"
                                OnClientItemSelected="GetCustomerOnDemand"
                            ></asp:AutoCompleteExtender>--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageQuote.aspx.cs"  MasterPageFile="~/Quotation/Quotation.Master" 
Theme="Default" Inherits ="Quotation.Forms.ManageQuote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>
<%@ Register Src="~/Contact/Controls/Contacts.ascx" TagPrefix="uc" TagName="Contacts" %>
<asp:Content ID="header" runat="server" ContentPlaceHolderID="head">
<script type="text/javascript">
    var selectedRowIndex = 0;
    var hdntxtCode;
    var IsItemSelected = false;
   
    function PrintReportInNewWindow(PO,type) {
        //window.open('/QOModule/Reports/QuotationReport.aspx?QID=' + PO, 'NewWindow', 'toolbar=no');
        //window.open('/Quotation/Reports/GenerateQuote.aspx?ID=' + PO, 'NewWindow', 'toolbar=no');
        var rootPath = '<%= ConfigurationManager.AppSettings["QuoteRootPath"] %>';
        
        if (type == 'PDF') {
            window.open(rootPath + 'GenerateQuote.aspx?ID=' + PO + '&Type=PDF', 'NewWindow', 'toolbar=no');
        } else {
        window.open(rootPath + 'GenerateQuote.aspx?ID=' + PO + '&Type=Excel', 'NewWindow', 'toolbar=no');
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
    function validatePrice(source) {        
        hdntxtCode = source.id;       
        //ctl00_ctl00_mainMaster_main_Tab_ItemsTab_ItemsGridView_ctl02_txtCode
        //ctl00_ctl00_mainMaster_main_Tab_ItemsTab_ItemsGridView_ctl03_txtCode
        var newsplit = hdntxtCode.split('_txtCode');
        selectedRowIndex  = newsplit[0].substr(newsplit[0].length - 2)
        return false;
    }
    function UpdateClearGridRow(NoRecordsFoundText, split) 
    {
        var flag = false;
        var table = document.getElementById('<%= ItemsGridView.ClientID %>');
        if (table.rows.length > 0) 
        {
            //loop the gridview table
            for (var i = 1; i < table.rows.length; i++) 
            {
                //get all the input elements
                var inputs = table.rows[i].getElementsByTagName("input");
                for (var j = 0; j < inputs.length; j++) 
                {
                    //get the txtCode
                    if (inputs[j].id == hdntxtCode) 
                    {
                        if (NoRecordsFoundText == "No Records Found") {
                            //clearing "No Records Found" so empty will be set to itemcode
                            NoRecordsFoundText = "";
                        }                        
                        //this will set the value for Master item code
                        inputs[j].value = NoRecordsFoundText;
                        inputs[j + 1].value = split[0];
                        inputs[j + 2].value = split[1];
                        inputs[j + 3].value = split[2];
                        inputs[j + 4].value = split[3];
                        inputs[j + 5].value = split[4];
                        inputs[j + 6].value = split[5];

                        flag = true;
                        break;
                    }
                }
                if (flag)
                    break;
            }
        }
     }
        function checkItemSelected(myTextBox) {            
            var split = ["", "0", "0", "0", "0", "0"];
            UpdateClearGridRow(myTextBox.value, split);
            myTextBox.value = "";
        }
        function GetItemsOnDemand(source, eventargs) 
        {
            //  http://forums.asp.net/t/1069245.aspx/1
            //  http: //forums.asp.net/t/1948727.aspx?How+to+Find+Gridview+Item+template+texbox+using+Javascript
            var split = eventargs.get_value().split('|');
            UpdateClearGridRow(eventargs._text, split);
        }
       
//        intIndex = parseInt(selectedRowIndex);
//        GridView.rows[intIndex].cells[2].children[0].value = split[0];
//        GridView.rows[intIndex].cells[3].children[0].value = split[1];
//        GridView.rows[intIndex].cells[4].children[0].value = split[2];
//        GridView.rows[intIndex].cells[5].children[0].value = split[3];
//        GridView.rows[intIndex].cells[6].children[0].value = split[4];
//        GridView.rows[intIndex].cells[7].children[0].value = split[5];
 

     
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
<ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:80px;"><a href="SearchQuotes.aspx" title="Quote"><b>I&nbsp;-&nbsp;Quote</b></a> </li>
    <li>Create / Manage Quote</li>
    </ul>
    <div>
    
<asp:UpdatePanel ID="MUP" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Panel ID="AEP" runat="server" >
    <table style="width: 5%;">
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
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return confirm('No changes will be allowed after you submit. Are you sure you want to submit this Quotation');" OnClick="btnSubmit_Click" />
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
<asp:Button ID="Trigger" runat="server" CssClass="HiddenClass" SkinID="dd"  />
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
                                <asp:Label ID="LblQO" runat="server"   Text="Quote Number" />
                            </td>
                            <td>
                                <asp:Label ID="LblRev" runat="server"  Text="Revision No" />
                            </td>
                           
                             <td>
                                <asp:Label ID="amR" runat="server" Text="Amend Reason" />
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
                                <asp:Label ID="AmendReasonLbl" Text='<%# Eval("AmendReason") %>' Width="95%" runat="server"></asp:Label>
                            </td> 
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                                <asp:Label ID="hdnCustomerID" Text='<%# Eval("CustID") %>'  CssClass="HiddenClass" runat="server" />
                                <asp:Label ID="txtCustomer" Text='<%# Eval("Company") %>'  runat="server"></asp:Label>
                            </td>
                            
                        </tr>                        
                        <tr class="theadColumn">
                            <td>                                
                                <asp:Label ID="Label2" Text="Created By"  runat="server" />                            
                            </td> 
                            <td>
                                <asp:Label ID="LblQuoteDate"    Text="QuoteDate" runat="server" />
                            </td>              
                            
                            <td>
                                <asp:Label ID="Reflbl"  Text="Reference ID" runat="server" />
                            </td>
                            <td  style="padding-left:30px;font-weight:normal; text-align:left;" >
                              <asp:Label ID="CustomerRow1"   runat="server" />                              
                            </td> 
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="CreatedByLbl" Text='<%# Eval("CreatedByName") %>'  runat="server"></asp:Label>
                            </td>                                                       
                           <td>
                               <asp:Label ID="QuoteDateLbl" Text='<%# Eval("QuoteDate","{0:dd/MM/yyyy}")  %>' runat="server"></asp:Label>                               
                               <asp:HiddenField ID="hdnCreatedOn" runat="server" Value='<%# Eval("CreatedOn","{0:dd/MM/yyyy}")  %>' />
                            </td>
                            <td>
                               <asp:Label ID="RFQIDLbl"  Text='<%# Eval("ReferenceID") %>' runat="server" />&nbsp;
                            </td>
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow2" runat="server"></asp:Label>                                
                            </td>
                        </tr>                        
                                                                                                                                  
                        <tr class="theadColumn">
                            <td> <asp:Label ID="DiscountPercentLbl"  Text="Discount %" runat="server" /></td>
                            <td> <asp:Label ID="TaxPercentLbl"  Text="Tax %" runat="server" /> </td>                            
                            <td > <asp:Label ID="QuotationTotal"  Text="Total" runat="server" /></td>
                            <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow3" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                        <tr >
                            <td> <asp:Label ID="DiscountPercentLbl1"  Text='<%# Eval("Discount") %>' runat="server" /></td>
                            <td> <asp:Label ID="TaxPercentLbl1"  Text='<%# Eval("Tax") %>' runat="server" /> </td>                            
                            <td >  <asp:Label ID="QuotationTotal1"  Text='<%# Eval("Total") %>' runat="server" /></td>
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
                                <asp:Label ID="Label7" runat="server"   Text="Quote Number" Width="150px" />
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server"  Text="Revision No" Width="150px" />
                            </td>
                            <td>
                                 <asp:Label ID="amR" Width="150px" runat="server" Text="Amend Reason" /> <%-- Visible='<%# IsAmendEdit %>'--%>
                            </td>                           
                            <td>
                                <asp:Label ID="Label1"  Text="Customer" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblID" runat="server"  Text='<%# Bind("ID") %>'  />
                                <asp:HiddenField ID="hdnCreatedBy" runat="server" Value='<%# Bind("CreatedBy") %>'   />
                                
                            </td>
                            <td>
                                <asp:Label ID="lblRevision" runat="server" Text='<%# Bind("Revision") %>'  />
                            </td>
                             <td>                                
                                <asp:TextBox ID="txtAmendReason"  Text='<%# Bind("AmendReason") %>' Width="95%" runat="server"></asp:TextBox> <%--Visible='<%# IsAmendEdit %>'--%>
                            </td>                            
                           
                            <td style="padding-left:30px;font-weight:normal; text-align:left;"> 
                            <asp:Label ID="hdnCustomerID" Text='<%# Bind("CustID") %>' CssClass="HiddenClass"  runat="server"/>    
                            <asp:UpdatePanel ID="EuPop" runat="server"  UpdateMode="Conditional">
                            <ContentTemplate> 
                             
                            <asp:LinkButton ID="EtxtCustomer"  Width="250px" runat="server" Text="Update Customer Details" ></asp:LinkButton>
                               <asp:ModalPopupExtender ID="Epop" runat="server" BackgroundCssClass="modalPopup" PopupDragHandleControlID="dragId" 
                                   TargetControlID="EtxtCustomer" PopupControlID="EpPP" X="50" Y="50" CancelControlID="EClose"  >                                    
                               </asp:ModalPopupExtender>
                               <asp:Panel ID="EpPP" runat="server"  style="display: none; background-color:#FBFBEF">
                                <div id="dragId">                                
                                <div style="float:right;">  <asp:Button ID="EClose" runat="server" Text="Select/Close" OnClientClick="TriggerClick();return false;"  />  </div>
                                </div>
                                <uc:Contacts ID="Econtacts" runat="server" IsPopup="true" />    
                                </asp:Panel>
                            </ContentTemplate>
                            </asp:UpdatePanel> 
                               
                            </td>
                        </tr>                        
                        <tr class="theadColumn">
                            <td>                                
                                <asp:Label ID="Label12" Text="Created By"  runat="server" />                            
                            </td>
                            <td>
                                <asp:Label ID="Label5"   Text="Quote Date" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label15"  Text="Reference ID" runat="server" />
                            </td>
                             <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow1" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <asp:Label ID="Label10" Text='<%# Bind("CreatedByName") %>'  runat="server"></asp:Label>
                                 <%--<asp:Label ID="Label9" Text='<%# Bind("CreatedBy") %>'  runat="server" CssClass="HiddenClass"></asp:Label>--%>
                            </td>     
                              <td>
                                <asp:TextBox ID="txtQuoteDate" Text='<%# Bind("QuoteDate","{0:dd/MM/yyyy}")  %>'  Enabled="false" runat="server"></asp:TextBox>                               
                               <asp:HiddenField ID="hdnCreatedOn" runat="server" Value='<%# Bind("CreatedOn","{0:dd/MM/yyyy}")  %>' />                                
                               <asp:ImageButton ID="imgCal1" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <asp:CalendarExtender ID="ce1" runat="server"   TargetControlID="txtQuoteDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgCal1"  />
                            </td>    
                             <td>
                               <asp:TextBox ID="txtReferencePO" Text='<%# Bind("ReferenceID") %>'  Width="60%" runat="server"></asp:TextBox>
                            </td>  
                             <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow2" runat="server"></asp:Label>                                
                            </td>   
                        </tr>                        
                       
                        <tr class="theadColumn">
                            <td> <asp:Label ID="DiscountPercentLbl"  Text="Discount %" runat="server" /></td>
                            <td> <asp:Label ID="TaxPercentLbl"  Text="Tax %" runat="server" /> </td>                             
                            <td > <asp:Label ID="Total"  Text="Total" runat="server" /></td>
                             <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow3" runat="server"></asp:Label>                                
                            </td>
                        </tr>
                        <tr >
                            <td> <asp:Label ID="DiscountPercentLbl1"  Text='<%# Eval("Discount") %>' runat="server" /></td>
                            <td> <asp:Label ID="TaxPercentLbl1"  Text='<%# Eval("Tax") %>' runat="server" /> </td>                            
                            <td> <asp:Label ID="QuotationTotal1"  Text='<%# Eval("Total") %>' runat="server" /></td>
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
                                <asp:Label ID="Label7" runat="server"   Text="Quote Number"  Width="150px" />
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server"  Text="Revision No"  Width="150px" />
                            </td>
                            <td>
                                 <asp:Label ID="amR" runat="server" Text="Amend Reason"  Width="150px"  />
                            </td>
                            <td>
                                <asp:Label ID="Label1"  Text="Customer"  runat="server" />
                            </td>
                           
                        </tr>
                        <tr>                            
                            <td>
                                <asp:Label ID="PONLbl" runat="server"   Text='<%# Eval("ID") %>' />
                            </td>                            
                            <td>
                                <asp:Label ID="RevisionLbl" runat="server" Text='<%# Eval("Revision") %>'   />
                            </td>                                              
                            <td>
                             <asp:Label ID="Label11" runat="server" Text="N/A"  />
                            </td>
                           <td style="padding-left:30px;font-weight:normal; text-align:left;">
                           <asp:Label ID="hdnCustomerID" Text='<%# Bind("CustID") %>' CssClass="HiddenClass" runat="server"/>                             
                            <asp:UpdatePanel ID="uPop" runat="server"  UpdateMode="Conditional">
                            <ContentTemplate>

                               <asp:LinkButton ID="txtCustomer" Text="Add New Customer" Width="250px" runat="server" ></asp:LinkButton> <%--OnTextChanged="CustomerChanged" AutoPostBack="true"--%>
                                                              
                               <asp:ModalPopupExtender ID="pop" runat="server" BackgroundCssClass="modalPopup" PopupDragHandleControlID="dragId" 
                                   TargetControlID="txtCustomer" PopupControlID="pPP" X="50" Y="50" CancelControlID="Close"  >
                               </asp:ModalPopupExtender>   
                               <asp:Panel ID="pPP" runat="server"  style="display: none; background-color:#FBFBEF">
                                    <div id="dragId">                                                                                            
                                    <div style="float:right;">  <asp:Button ID="Close" runat="server" Text="Select/Close" OnClientClick="TriggerClick();return false;"  />  </div>
                                    </div>
                                    <uc:Contacts ID="contacts" runat="server" IsPopup="true" />   
                                </asp:Panel>      
                            </ContentTemplate>
                            </asp:UpdatePanel>
                               
                            </td>
                        </tr>
                        <tr class="theadColumn">
                            <td>                                
                                <asp:Label ID="Label12" Text="Created By"  runat="server" />                            
                            </td>
                            <td>
                                <asp:Label ID="Label5"   Text="Quote Date " runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label15"  Text="Reference ID" runat="server" />
                            </td>
                             <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow1" runat="server"></asp:Label>                                
                            </td>  
                        </tr>   
                        <tr>   
                            <td>
                                <asp:Label ID="lblCreatedBy" Text='<%# Eval("CreatedByName") %>'   runat="server"></asp:Label>    
                            </td>    
                           <td >
                               <asp:TextBox ID="txtQuoteDate" Text='<%# Bind("QuoteDate","{0:dd/MM/yyyy}")  %>' Enabled="false" runat="server"></asp:TextBox>                               
                               <asp:HiddenField ID="hdnCreatedOn" runat="server" Value='<%# Bind("CreatedOn","{0:dd/MM/yyyy}")  %>' />
                                <asp:ImageButton ID="imgCal1" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <asp:CalendarExtender ID="ce1" runat="server"   TargetControlID="txtQuoteDate"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"     PopupButtonID="imgCal1"  />
                           </td>       
                            <td>
                                <asp:TextBox ID="txtReferencePO" Text='<%# Bind("ReferenceID") %>' runat="server"></asp:TextBox>
                            </td>                                            
                             <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow2" runat="server"></asp:Label>                                
                            </td>  
                        </tr>                        
                          
                        
                         <tr class="theadColumn">
                            <td> <asp:Label ID="DiscountPercentLbl"  Text="Discount %" runat="server" /></td>
                            <td> <asp:Label ID="TaxPercentLbl"  Text="Tax %" runat="server" /> </td>                          
                            <td > <asp:Label ID="QuotationTotal"  Text="Total" runat="server" /></td>
                             <td style="padding-left:30px;font-weight:normal; text-align:left;">
                             <asp:Label ID="CustomerRow3" runat="server"></asp:Label>                                
                            </td>  
                        </tr>
                        <tr >
                            <td> <asp:Label ID="DiscountPercentLbl1"  Text='<%# Eval("ItemDiscount") %>' runat="server" /></td>
                            <td> <asp:Label ID="TaxPercentLbl1"  Text='<%# Eval("ItemTax") %>' runat="server" /> </td>                            
                            <td > <asp:Label ID="QuotationTotal1"  Text='<%# Eval("ItemTotal") %>' runat="server" /></td>
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
                <asp:ImageButton ID="imgAdd" Height="20px" runat="server" ImageUrl="~/Quotation/Images/iADD.bmp"
                    AlternateText="Add" OnClick="imgAddItem_Click"  CausesValidation="false" ToolTip="Add ">
                </asp:ImageButton>
            </td>
           <%-- <td style="width: 5px;">
                <asp:ImageButton ID="imgEdit" Height="22px" runat="server" ImageUrl="~/Quotation/Images/iEdit.gif"
                    AlternateText="Edit" OnClick="imgEditItem_Click" ToolTip="Edit "></asp:ImageButton>
            </td>--%>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDelete" Height="22px" runat="server" ImageUrl="~/Quotation/Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDeleteItem_Click" CausesValidation="false" ToolTip="Delete"
                    ></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSave" Height="22px" runat="server" ImageUrl="~/Quotation/Images/iSave.gif"
                    AlternateText="Update" OnClick="imgUpdateItem_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancel" Height="22px" runat="server" ImageUrl="~/Quotation/Images/iCan.gif"
                    AlternateText="Cancel" OnClick="imgCancelItem_Click" CausesValidation="false" ToolTip="Cancel">
                </asp:ImageButton>
            </td>  
            <td style="width: 5px;"  >
              <asp:ImageButton ID="imgRefresh" Height="22px" runat="server" ImageUrl="~/Images/refresh.png"
                    AlternateText="Refresh" OnClick="imgRefresh_Click" CausesValidation="false" ToolTip="Refresh">
                </asp:ImageButton>
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
            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' CssClass="HiddenClass" ></asp:Label>
            <asp:Label ID="lblQuoteID" Text='<%# Bind("QuoteID") %>' CssClass="HiddenClass" runat="server"   />
            <asp:CheckBox ID="cbSelect" Visible='<%# (bool) IsInEditMode %>' EnableTheming="true" Font-Bold="true" runat="server"    />           
        </ItemTemplate>
         
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Item Code / Type"  >
      
            <ItemTemplate>             
                <asp:Label ID="c2" runat="Server" Width="100px"  Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("Code") %>' />
                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtCode"
                    ServiceMethod="GetItemsOnDemand" ServicePath="~/Quotation/QuotationService.asmx" MinimumPrefixLength="1"
                    CompletionInterval="1000" CompletionSetCount="20" DelimiterCharacters=";, :"
                    OnClientItemSelected="GetItemsOnDemand">
                </asp:AutoCompleteExtender>
                <asp:TextBox ID="txtCode" runat="server" Width="100"  onChange="checkItemSelected(this);" onfocus="javascript:return validatePrice(this);" Visible='<%# IsInEditMode %>'   Text='<%# Bind("Code") %>' />
            </ItemTemplate>
            
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Item Description / Specification" >           
            <ItemTemplate>
                <asp:Label ID="c3" runat="Server" Width="250px"  Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("Description") %>' />
                <%-- <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="txtItemDescription"
                    ServiceMethod="GetItemDescriptionsOnDemand" ServicePath="~/Quotation/QuotationService.asmx" MinimumPrefixLength="1"
                    CompletionInterval="1000" CompletionSetCount="20" DelimiterCharacters=";, :">
                </asp:AutoCompleteExtender>--%>
                <asp:TextBox ID="txtItemDescription" runat="server"   Visible='<%# IsInEditMode %>'  Width="250px" Text='<%# Bind("Description") %>' /> <%--OnTextChanged="ItemChanged" AutoPostBack="true" --%>
            </ItemTemplate>
        </asp:TemplateField>         
        <asp:TemplateField HeaderText="Quantity"  >          
            <ItemTemplate>
             <asp:Label ID="c4" runat="Server" Width="60px"  Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("Quantity","{0:N2}") %>' />
               <asp:TextBox ID="txtQuantity" runat="server" Width="70px" Text='<%# Bind("Quantity","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);"  Visible='<%# IsInEditMode %>'/>
               <asp:FilteredTextBoxExtender ID="quantityValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtQuantity" />                              
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Unit"  >           
            <ItemTemplate>
                <asp:Label ID="c5" runat="Server" Width="30px" Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("Unit") %>' />                
                <asp:TextBox ID="txtUnit" runat="server" Width="30px" Text='<%# Bind("Unit") %>' Visible='<%# IsInEditMode %>' />
                <asp:FilteredTextBoxExtender ID="unitValidator" runat="server" Enabled="True"
                    FilterType="LowercaseLetters, UppercaseLetters"  TargetControlID="txtUnit" />
             </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rate / Unit" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>
                <asp:Label ID="c6" runat="Server" Width="70px"  Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("Rate","{0:N2}") %>' />
                <asp:TextBox ID="txtRate" runat="server" Width="70px" Visible='<%# IsInEditMode %>' Text='<%# Bind("Rate","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);" />
               <asp:FilteredTextBoxExtender ID="rateValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtRate" />
            </ItemTemplate>
             <FooterTemplate>                    
                <asp:Label ID="Totals" runat="Server" Text="Totals" Font-Bold="true"   />                   
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="SubTotal" FooterStyle-CssClass="columnBorder" >                     
            <ItemTemplate>
                <asp:Label ID="c11" runat="Server" Width="70px"  Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("SubTotal","{0:N2}") %>' />               
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label ID="lblSubTotal" runat="server"  Width="70px" Text='<%# Bind("SubTotal","{0:N2}") %>'  />
            </EditItemTemplate>            
             <FooterTemplate>            
            <asp:Label ID="lblSubTotal" runat="Server" Width="35px"  />                   
            </FooterTemplate>
        </asp:TemplateField>         
           
        <asp:TemplateField HeaderText="Discount %" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>                
                <asp:Label ID="c7" runat="Server" Width="50px" Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("Discount","{0:N2}") %>' />
               <asp:TextBox ID="txtDiscount" runat="server" Visible='<%# IsInEditMode %>' Width="70px" Text='<%# Bind("Discount","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);" />
               <asp:FilteredTextBoxExtender ID="discountValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtDiscount" />
           </ItemTemplate>
              
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Discount" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>
                <asp:Label ID="c21" runat="Server" Width="70px" Text='<%# Eval("DiscountAmount","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label ID="lblDiscountAmount" runat="server"  Width="70px" Text='<%# Bind("DiscountAmount","{0:N2}") %>'  />
            </EditItemTemplate>            
             <FooterTemplate>            
            <asp:Label ID="lblTotalDiscountAmount" runat="Server" Width="35px"  />                   
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tax %" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>
                <asp:Label ID="c8" runat="Server" Width="30px" Visible='<%# !(bool) IsInEditMode %>' Text='<%# Eval("Tax","{0:N2}") %>' />
               <asp:TextBox ID="txtTax" runat="server" Width="70px" Visible='<%# IsInEditMode %>' Text='<%# Bind("Tax","{0:N2}") %>'   
                onkeypress="return IsOneDecimalPoint(event);" />
               <asp:FilteredTextBoxExtender ID="taxValidator" runat="server" Enabled="True"
                FilterType="Custom" ValidChars="01234567890." TargetControlID="txtTax" />      
           </ItemTemplate>    
             
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Tax Amount" FooterStyle-CssClass="columnBorder">            
            <ItemTemplate>
                <asp:Label ID="c9" runat="Server" Width="70px" Text='<%# Eval("TaxAmount","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label ID="lblTaxAmount" runat="server"  Width="70px" Text='<%# Bind("TaxAmount","{0:N2}") %>'  />
            </EditItemTemplate>            
             <FooterTemplate>            
            <asp:Label ID="lblTotalTaxAmount" runat="Server" Width="35px"  />                   
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Item Total" FooterStyle-CssClass="columnBorder" >            
            <ItemTemplate>
                <asp:Label ID="c10" runat="Server" Width="70px" Text='<%# Eval("Total","{0:N2}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Label ID="lblTotalAmount" runat="server" Width="70px"  Text='<%# Bind("Total","{0:00}") %>' />
            </EditItemTemplate>
             <FooterTemplate>                               
                <asp:Label ID="lblTotalAmount" runat="Server" Width="70px"   />          
            </FooterTemplate>
        </asp:TemplateField>
       
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
                <asp:ImageButton ID="imgEditTerm" Height="22px" runat="server" ImageUrl="../Images/iEdit.gif"
                    AlternateText="Edit" OnClick="imgEditTerm_Click" ToolTip="Edit "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgDeleteTerm" Height="22px" runat="server" ImageUrl="../Images/iDel.png"
                    AlternateText="Delete" OnClick="imgDeleteTerm_Click" CausesValidation="false" ToolTip="Delete"
                   ></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgSaveTerm" Height="22px" runat="server" ImageUrl="../Images/iSave.gif"
                    AlternateText="Update" OnClick="imgUpdateTerm_Click" ToolTip="Save "></asp:ImageButton>
            </td>
            <td style="width: 5px;">
                <asp:ImageButton ID="imgCancelTerm" Height="22px" runat="server" ImageUrl="../Images/iCan.gif"
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
         <asp:TextBox ID="txtQuoteID" Text='<%# Bind("QuoteID") %>' CssClass="HiddenClass" runat="server"   />
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

<asp:ObjectDataSource ID="ODODS" runat="server" TypeName="Quotation.Buslogic.Quote" ConflictDetection="CompareAllValues"
    SelectMethod="GetQuote" DataObjectTypeName="Quotation.Data.QuoteData"
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
                                ServicePath="Quotation.QuotationService.asmx"
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
                               ServicePath="~/Quotation/QuotationService.asmx"
                                TargetControlID="txtCustomer"
                                MinimumPrefixLength="1"
                                CompletionInterval="1000" 
                                CompletionSetCount="20" 
                                DelimiterCharacters=";, :"
                                OnClientItemSelected="GetCustomerOnDemand"
                            ></asp:AutoCompleteExtender>--%>
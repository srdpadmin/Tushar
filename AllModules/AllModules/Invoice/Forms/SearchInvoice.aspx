<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchInvoice.aspx.cs" Inherits="Invoice.Forms.SearchInvoice"
MasterPageFile="~/Invoice/Invoice.Master" EnableTheming="true" Theme="Default" EnableViewState="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>

<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
<%--<script src="../../Javascript/jquery-1.4.1.js" type="text/javascript"></script>--%>
<script src="../../Javascript/jquery-1.12.1.min.js" type="text/javascript"></script>
<script type="text/javascript">

    var focusChild = null;
    window.onload = FocusNavigate;
    function DoPostBack(arg, index) {
        __doPostBack(arg, index);
    }    
    
    function GetCustomerOnDemand(source, eventargs) {
         
        var CustomerLabel = document.getElementById('<%=LookupBox.ClientID %>');
        var CustomerId = document.getElementById('<%=CustID.ClientID %>');
        CustomerId = eventargs.get_value();
        __doPostBack('LookupBox', CustomerId);
    }
    
    function FocusNavigate() {

        var hdnFocusChild = document.getElementById('<%=FocussedChild.ClientID%>');
        var focusChild = hdnFocusChild.value;
        if (focusChild != null & focusChild != "") {

            document.getElementById(focusChild).focus();
            document.getElementById(focusChild).parentNode.className = "active";
            document.getElementById(focusChild).parentNode.parentNode.className = "active";
        }
//        Use This code when directly focussing GridRow from inside Jquery         
//        if (event.keyCode == 119) 
//        {
//            $("#<%=navigate.ClientID%> tbody tr td").focus();
//            var $list = $("#<%=navigate.ClientID%> tbody tr td span").eq(0);
//            if ($list.length) {
//                $list[0].parentNode.className = "active";
//                $list[0].parentNode.parentNode.className = "active";
//                $list[0].focus();
//            }
//            event.preventDefault();
//            return false;
//        }
    }
    $(window).load(function() {
        
    });
    $(document).ready(function() {
        ////////////////THIS IS FOR SEARCH ///////////////////

        $(document).keydown(function(event) {

            if (event.which == 113) { //F2

                $("#<%=LookupBox.ClientID%>").focus();
                return false;
            }
            if (event.which == 114) { //F3
                event.preventDefault();
                DoPostBack('New', 0);
                event.preventDefault();
                return false;
            }
            else if (event.which == 119) { //F1

            }
            else if (event.which == 119) { //F8

                $("#<%=NewBill.ClientID%>").click();
                return false;
            }
            else if (event.which == 27) { //F8

                DoPostBack('CloseBill', '');
                return false;
            }
        });
        $("#<%=LookupBox.ClientID%>").keydown(function(event) {



        });
        // user clicks on cell
        var active = 0;
       
        $("#<%=navigate.ClientID%> td").click(function() {

            active = $('#<%=navigate.ClientID%> td').index(this);
            rePosition();
        });
        $("#<%=navigate.ClientID%> tbody tr td").keydown(function(e) {

            reCalculate(e);
            rePosition();
            return false;
        });
        function reCalculate(e) {
            //http: //jsfiddle.net/BdVB9/
            var rows = $('#<%=navigate.ClientID%> tr').length;
            var columns = $('#<%=navigate.ClientID%> tr:eq(0) th').length;
            if (e.keyCode == 113) {
                //$("#<%=LookupBox.ClientID%>").focus();
            }
            if (e.keyCode == 38) { // move up                
                active = (active - columns >= 0) ? active - columns : active;
            }
            if (e.keyCode == 40) { // move down   
                active = (active + columns <= (rows * columns) - 1) ? active + columns : active;
            }
            if (event.keyCode == 13) {//enter 
                DoPostBack('ResultRow', $("#ctl00_ctl00_mainMaster_main_navigate tr:has(td.active) span")[0].innerHTML);
            }

        }

        function rePosition() {

            $('.active').removeClass('active');
            $('#<%=navigate.ClientID%> tbody tr td').eq(active).addClass('active');
            $('#<%=navigate.ClientID%> tbody tr td').eq(active).closest('tr').addClass('active');
        }
    });
</script>
<style type="text/css">
tr.active{border:1px solid blue;font-weight:bold; color:Yellow;background-color:Red}
td.active{border:1px solid blue;font-weight:bold;color:Yellow;background-color:Red}
td{padding:5px;text-align:center}
</style>
</asp:Content>
<asp:Content ID="main1" runat="server" ContentPlaceHolderID="main">
    <div>
    <div style="width:100%;background-color:Black; color:Lime; padding-left:10px; font-size:medium;"> 
    <span style="color:White;">Navigation Keys</span>  [Focus=F2][New Bill=F3][View Details=Enter][Close[ESC]    
    </div>
    <br />
    <asp:Label ID="Customer" runat="server" Height="25px" Font-Size="Medium" Text="Customer/Vendor (F2)"></asp:Label>
    <asp:TextBox ID="LookupBox" runat="server" Height="20px" Font-Size="Medium" Enabled="true" TabIndex="0"    AutoPostBack="false"   ></asp:TextBox> 
    <asp:HiddenField ID="CustID" runat="server" />
    <asp:HiddenField ID="FocussedChild" runat="server" />
    <asp:Button ID="NewBill" runat="server" Text="New Invoice [F3]" Height="25px" OnClick="NewBill_Click" TabIndex="1"   />
    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="LookupBox"
                    ServiceMethod="GetCustomerOnDemand" ServicePath="~/Invoice/InvoiceService.asmx" MinimumPrefixLength="1"
                    CompletionInterval="1000" CompletionSetCount="20" DelimiterCharacters=";, :"
                    OnClientItemSelected="GetCustomerOnDemand">
                </asp:AutoCompleteExtender>
     
    <asp:GridView ID="navigate" runat="server" OnRowDataBound="navigate_RowDataBound"  
     AutoGenerateColumns="false"  SkinID="halfmetro"   >
     <Columns > 
     <asp:TemplateField HeaderText="Bill#">
        <ItemTemplate>       
        <asp:Label ID="billID" runat="server" TabIndex="3" Text='<%# Eval("ID")%>'></asp:Label>         
        </ItemTemplate>
        </asp:TemplateField>
      <asp:TemplateField HeaderText="Company">
        <ItemTemplate>       
        <asp:Label ID="company" runat="server" TabIndex="4" Text='<%# Eval("Company")%>'></asp:Label>        
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Total">
        <ItemTemplate>
          <asp:Label ID="Total" runat="server" TabIndex="5" Text='<%# Eval("SubTotal")%>'></asp:Label>      
        </ItemTemplate>
       </asp:TemplateField>
        <asp:TemplateField HeaderText="Balance">
        <ItemTemplate>
          <asp:Label ID="Balance" runat="server" TabIndex="6" Text='<%# Eval("Balance")%>'></asp:Label>      
        </ItemTemplate>
       </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Date">
        <ItemTemplate>
         <asp:Label ID="BillDate" runat="server" TabIndex="7" Text='<%# Eval("BillDate", "{0:dd/MM/yyyy}") %>'></asp:Label>       
        </ItemTemplate>
       </asp:TemplateField> 
     </Columns>
     </asp:GridView> 
      
    </div>
</asp:Content>

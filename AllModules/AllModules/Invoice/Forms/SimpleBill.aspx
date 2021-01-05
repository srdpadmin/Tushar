<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleBill.aspx.cs" Inherits="Invoice.Forms.SimpleBill" 
MasterPageFile="~/Invoice/Invoice.Master" EnableTheming="true" Theme="Default" EnableViewState="true" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>
 
<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
<%--<script src="../../Javascript/jquery-1.4.1.js" type="text/javascript"></script>--%>
<script src="../../Javascript/jquery-1.12.1.min.js" type="text/javascript"></script>
<script type="text/javascript">

    var focusChild = null;
    function DoPostBack(arg, index) {
        __doPostBack(arg, index);
    }
    
    
    function GetCustomerOnDemand(source, eventargs) {
         
        var CustomerLabel = document.getElementById('<%=LookupBox.ClientID %>');
        var CustomerId = document.getElementById('<%=CustID.ClientID %>');
        CustomerId = eventargs.get_value();
        __doPostBack('LookupBox', CustomerId);
    }
    function FocusNavigate(focusedChild) {
        var navigate = document.getElementById('<%=navigate.ClientID %>');

        if (navigate != null) {
            focusChild = focusedChild;
            focusedChild.focus();
            focusedChild.parentNode.className = "active";
            //navigate.scrollIntoView(true);            
            //navigate.getElementsByTagName('tr')[1].firstChild.focus;
            //navigate.getElementsByTagName('tr')[1].getElementsByTagName('td')[0].firstChild.focus;
            //navigate.focus();
        }
    }
    $(document).ready(function() {


        ////////////////THIS IS FOR SEARCH ///////////////////
        $(document).keydown(function(event) {

            if (event.which == 113) { //F2

                $("#<%=LookupBox.ClientID%>").focus();
                return false;
            }
            else if (event.which == 116) { //F1
            if (focusChild)            
            focusChild.focus(); 
                return false;
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

            if (event.keyCode == 13) {//enter

                $("#<%=navigate.ClientID%> tbody tr td:first").focus();
                return false;
            }
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
                $("#<%=LookupBox.ClientID%>").focus();
            }
            if (e.keyCode == 38) { // move up 
                active = (active - columns >= 0) ? active - columns : active;
            }
            if (e.keyCode == 40) { // move down
                active = (active + columns <= (rows * columns) - 1) ? active + columns : active;
            }
            if (event.keyCode == 13) {//enter
                DoPostBack('ResultRow', active);
            }

        }

        function rePosition() {

            $('.active').removeClass('active');
            $('#<%=navigate.ClientID%> tbody tr td').eq(active).addClass('active');
            $('#<%=navigate.ClientID%> tbody tr td').eq(active).closest('tr').addClass('active');
        }
        ////////////////////////THIS IS FOR BILL /////////////////////

        $("end").keydown(function(event) {

            if ((event.keyCode == 39) || (event.keyCode == 9 && event.shiftKey == false)) {//left

            }
            if ((event.keyCode == 37) || (event.keyCode == 9 && event.shiftKey == false)) {//right

            }
            if ((event.keyCode == 38)) {//up

                var $list = $("#<%=ItemsGridView.ClientID%> :input[type='text']");
                if ($list.length) {
                    var val = $list.length;
                    $list[val - 1].select();
                }
            }
            if ((event.keyCode == 40)) {//down

                var $list = $("#<%=PaymentGridView.ClientID%> :input[type='text']");
                if ($list.length) {
                    $list[0].select();
                }

            }
        });

        //For navigating using left and right arrow of the keyboard
        $("#<%=ItemsGridView.ClientID%> input[type='text'], select").keydown(

                function(event) {


                    if (event.keyCode == 13)  // the enter key code
                    {

                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        DoPostBack('itemNew', idx);
                        return false;
                    }
                    if (event.ctrlKey) {

                        var parent = $(this).closest('table').attr('id');
                        if (parent == 'PaymentGridView')
                            return false;

                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);

                        DoPostBack('itemDel', idx);

                        return false;
                    }
                    if ((event.keyCode == 39) || (event.keyCode == 9 && event.shiftKey == false)) {
                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        if (idx == inputs.length - 1) {
                            inputs[0].select()
                        } else {
                            $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                            });
                            inputs[idx + 1].parentNode.parentNode.style.backgroundColor = "Aqua";
                            inputs[idx + 1].focus();
                        }
                        return false;
                    }

                    if ((event.keyCode == 37) || (event.keyCode == 9 && event.shiftKey == true)) {
                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        if (idx > 0) {
                            $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                            });
                            inputs[idx - 1].parentNode.parentNode.style.backgroundColor = "Aqua";

                            inputs[idx - 1].focus();
                        }
                        return false;
                    }

                    //For navigating using up and down arrow of the keyboard 
                    if ((event.keyCode == 40)) {
                        if ($(this).parents("tr").next() != null) {
                            var nextTr = $(this).parents("tr").next();
                            var inputs = $(this).parents("tr").eq(0).find("input[type='text'], select");
                            var idx = inputs.index(this);
                            nextTrinputs = nextTr.find("input[type='text'], select");
                            if (nextTrinputs[idx] != null) {
                                $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                    $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                                });
                                nextTrinputs[idx].parentNode.parentNode.style.backgroundColor = "Aqua";
                                nextTrinputs[idx].focus();
                            }
                            else {
                                // case when down is click and need to move go up
                                //$('end').select();
                                var $list = $("#<%=PaymentGridView.ClientID%> :input[type='text']");
                                if ($list.length) {
                                    $list[0].select();
                                }
                            }
                        }
                        else {
                            $(this).focus();
                        }
                    }

                    if ((event.keyCode == 38)) {
                        if ($(this).parents("tr").next() != null) {
                            var nextTr = $(this).parents("tr").prev();
                            var inputs = $(this).parents("tr").eq(0).find("input[type='text'], select");
                            var idx = inputs.index(this);
                            nextTrinputs = nextTr.find("input[type='text'], select");
                            if (nextTrinputs[idx] != null) {
                                $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                    $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                                });
                                nextTrinputs[idx].parentNode.parentNode.style.backgroundColor = "Aqua";
                                nextTrinputs[idx].focus();
                            }
                            else {
                                // case when up is click and need to move go up
                                $('#start').select();
                            }
                            return false;
                        }
                        else {
                            $(this).focus();
                        }
                    }
                });

        // PAYMENT GRIDVIEW
        $("#<%=PaymentGridView.ClientID%> input[type='text'], select").keydown(
                function(event) {

                    if (event.keyCode == 32)  // the space key code
                    {
                        var inputs = $(this).parents("table").eq(0).find("select");
                        var idx = inputs.index(this);
                        var noOfListItems = inputs[idx].length;
                        var curListItem = inputs[idx].selectedIndex;
                        curListItem = curListItem + 1;
                        if (curListItem == noOfListItems)
                            curListItem = 0;
                        $(this).val(curListItem);
                    }

                    if (event.keyCode == 13)  // the enter key code
                    {

                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);

                        DoPostBack('payNew', idx);
                        return false;
                    }
                    if (event.ctrlKey) {

                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);

                        DoPostBack('payDel', idx);
                        return false;
                    }
                    if ((event.keyCode == 39) || (event.keyCode == 9 && event.shiftKey == false)) {
                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        if (idx == inputs.length - 1) {
                            inputs[0].focus();
                        } else {
                            $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                            });
                            inputs[idx + 1].parentNode.parentNode.style.backgroundColor = "Aqua";
                            inputs[idx + 1].focus();
                        }
                        return false;
                    }

                    if ((event.keyCode == 37) || (event.keyCode == 9 && event.shiftKey == true)) {
                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        if (idx > 0) {
                            $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                            });
                            inputs[idx - 1].parentNode.parentNode.style.backgroundColor = "Aqua";

                            inputs[idx - 1].focus();
                        }
                        return false;
                    }

                    //For navigating using up and down arrow of the keyboard 
                    if ((event.keyCode == 40)) {
                        if ($(this).parents("tr").next() != null) {
                            var nextTr = $(this).parents("tr").next();
                            var inputs = $(this).parents("tr").eq(0).find("input[type='text'], select");
                            var idx = inputs.index(this);
                            nextTrinputs = nextTr.find("input[type='text'], select");
                            if (nextTrinputs[idx] != null) {
                                $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                    $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                                });
                                nextTrinputs[idx].parentNode.parentNode.style.backgroundColor = "Aqua";
                                nextTrinputs[idx].focus();
                            }
                            return false;
                        }
                        else {
                            $(this).focus();
                        }
                    }

                    if ((event.keyCode == 38)) {
                        if ($(this).parents("tr").next() != null) {
                            var nextTr = $(this).parents("tr").prev();
                            var inputs = $(this).parents("tr").eq(0).find("input[type='text'], select");
                            var idx = inputs.index(this);
                            nextTrinputs = nextTr.find("input[type='text'], select");
                            if (nextTrinputs[idx] != null) {
                                $(this).parents("table").eq(0).find("tr").not(':first').each(function() {

                                    $(this).attr("style", "BACKGROUND-COLOR: white; COLOR: #003399");
                                });
                                nextTrinputs[idx].parentNode.parentNode.style.backgroundColor = "Aqua";
                                nextTrinputs[idx].focus();
                            }
                            else {
                                // case when up is click and need to move go up
                                //$('end').select();
                                var $list = $("#<%=ItemsGridView.ClientID%> :input[type='text']");
                                if ($list.length) {
                                    var val = $list.length;
                                    $list[val - 1].select();
                                }
                            }
                            return false;
                        }
                        else {
                            $(this).focus();
                        }
                    }
                });
        /// END PAYMENT

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
    <br />
    <asp:Label ID="Customer" runat="server" Height="30px" Font-Size="Medium" Text="Customer/Vendor (F2)"></asp:Label>
    <asp:TextBox ID="LookupBox" runat="server" Height="25px" Font-Size="Medium" Enabled="true"    AutoPostBack="false"   ></asp:TextBox> 
    <asp:HiddenField ID="CustID" runat="server" />
    <asp:Button ID="NewBill" runat="server" Text="New Bill" OnClick="NewBill_Click" />
    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="LookupBox"
                    ServiceMethod="GetCustomerOnDemand" ServicePath="~/Invoice/InvoiceService.asmx" MinimumPrefixLength="1"
                    CompletionInterval="1000" CompletionSetCount="20" DelimiterCharacters=";, :"
                    OnClientItemSelected="GetCustomerOnDemand">
                </asp:AutoCompleteExtender>
    <asp:Panel ID="pnlSearch" runat="server">
    <asp:GridView ID="navigate" runat="server" OnRowDataBound="navigate_RowDataBound"  
     AutoGenerateColumns="false"  SkinID="halfmetro"   >
     <Columns > 
      <asp:TemplateField HeaderText="Company">
        <ItemTemplate>
        <asp:HiddenField Id="ID" runat="server" Value='<%# Eval("ID")%>' />
        <asp:Label ID="company" runat="server" TabIndex="1" Text='<%# Eval("Company")%>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Total">
        <ItemTemplate>
          <asp:Label ID="Total" runat="server" TabIndex="2" Text='<%# Eval("SubTotal")%>'></asp:Label>      
        </ItemTemplate>
       </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Date">
        <ItemTemplate>
         <asp:Label ID="BillDate" runat="server" TabIndex="3" Text='<%# Eval("BillDate", "{0:dd/MM/yyyy}") %>'></asp:Label> 
      
        </ItemTemplate>
       </asp:TemplateField> 
     </Columns>
     </asp:GridView> 
    </asp:Panel>
    <asp:Panel ID="pnlCreateBill" runat="server" Width="550px">
      <div>   
    <input type="button" id="itemNew" style="visibility:hidden;" />
    <input type="button" id="itemDel" style="visibility:hidden;" />
    <input type="button" id="payNew" style="visibility:hidden;" />
    <input type="button" id="payDel" style="visibility:hidden;" />
    </div> 
    <div>
    <asp:GridView ID="ItemsGridView" runat="server" AutoGenerateColumns="false" SkinID="metro" 
        onrowdatabound="ItemsGridView_RowDataBound" ShowFooter="true" >
    <Columns>
       
        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:HiddenField ID="lblID" runat="server" Value='<%# Bind("ID") %>'  />
                <asp:HiddenField ID="lblBillID" Value='<%# Bind("BillID") %>'  runat="server"   />
                <asp:DropDownList ID="ddlDescription" runat="server"></asp:DropDownList>
                <asp:HiddenField runat="server" ID="hdnDescription" Value='<%# Bind("Description") %>'
                 ></asp:HiddenField>
                  
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Code" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate> 
                <asp:TextBox runat="server" ID="txtCode" Width="50px" Text='<%# Bind("Code") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Unit" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtUnit" Width="50px" Text='<%# Bind("Unit") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="Rate" HeaderStyle-HorizontalAlign="Left" FooterStyle-CssClass="columnBorder">
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtRate" Width="50px" Text='<%# Bind("Rate") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>            
        </asp:TemplateField>   
         <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtQuantity" Width="50px" Text='<%# Bind("Quantity") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
             <FooterTemplate>
            <asp:Label runat="server" ID="lblSubTotal" Width="70px" Text="Total" Visible="true"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="Left" FooterStyle-CssClass="columnBorder">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblSubTotal" Width="70px" Text='<%# Bind("SubTotal") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            <FooterTemplate>
            <asp:Label runat="server" ID="lblSubTotal" ></asp:Label>
            </FooterTemplate>
        </asp:TemplateField> 
    </Columns>
    </asp:GridView>
    <%--<asp:Panel ID="pnlCarting" runat="server" >
    <div style="float:right;clear:left;">   
    <span style="padding-right:20px; font-weight:bold;"><asp:Label ID="Carting" Text="Carting" runat="server"></asp:Label></span>
    <span><asp:TextBox ID="end" runat="server" Width="80px"  /></span>
    </div>
     <div style="float:right;clear:both;">
    <span style="padding-right:20px; font-weight:bold;"><asp:Label ID="Label1" Text="Total" runat="server"></asp:Label></span>
    <span><asp:Label ID="Label2"  Width="80px" Height="25px" runat="server"></asp:Label></span>    
    </div>
    </asp:Panel>--%>
      <div style="float:right;clear:right;">
    <asp:GridView ID="PaymentGridView" runat="server" AutoGenerateColumns="false" 
        onrowdatabound="PaymentGridView_RowDataBound" SkinID="halfmetro">
    <Columns>
       <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:HiddenField ID="lblID" runat="server" Value='<%# Bind("ID") %>'  />
                        <asp:HiddenField ID="lblRefID" Value='<%# Bind("ReferenceID") %>'  runat="server"   />
                        <asp:DropDownList ID="payType" runat="server">
                        <asp:ListItem Text="Cash" Selected="True" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Cheque" Value="1"></asp:ListItem>
                        <asp:ListItem Text="NEFT" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtAmount" Width="70px" Text='<%# Bind("Amount") %>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Balance" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBalance" Width="75px"  Text='<%# Bind("Balance") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
            
    </Columns>
    </asp:GridView>
   </div>
    </div>
     </asp:Panel>
    </div>
</asp:Content>

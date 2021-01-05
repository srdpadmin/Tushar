<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageInvoice.aspx.cs" Inherits="Invoice.Forms.ManageInvoice"  
MasterPageFile="~/Invoice/Invoice.Master" EnableTheming="true" Theme="Default" EnableViewState="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>

<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
<%--<script src="../../Javascript/jquery-1.4.1.js" type="text/javascript"></script>--%>
<script src="../../Javascript/jquery-1.12.1.min.js" type="text/javascript"></script>
<script src="../../Javascript/ExpandSelect_1.00.js" type="text/javascript"></script>
<script type="text/javascript">
    function DoPostBack(arg, index) {
        __doPostBack(arg, index);
    }
    $(document).ready(function() {

        $(document).keydown(function(event) {
            if (event.which == 116)  // F5 key refresh
            {
                DoPostBack('Refresh', 0);
                event.preventDefault();
                return false;
            }
            if (event.which == 113) { //F2
                
                var $list = $("#<%=ItemsGridView.ClientID%> select");
                if ($list.length) {
                    var val = $list.length;
                    $list[val - 1].focus();
                }
                return false;
            }
            if (event.which == 114) { //F3
                event.preventDefault();
                DoPostBack('Save', 0);
               
                return false;
            }
            if (event.which == 27) { //ESC
                event.preventDefault();
                $("#<%=btnExit.ClientID%>").trigger('click');  
               
            }

        });
        $("#<%=ddlCustomer.ClientID%>").keydown(function(event) {

            if ((event.keyCode == 39) || (event.keyCode == 9 && event.shiftKey == false)) {//left

            }
            if ((event.keyCode == 37) || (event.keyCode == 9 && event.shiftKey == false)) {//right

            }
            if ((event.keyCode == 38)) {//up

                //                var $list = $("#<%=ItemsGridView.ClientID%> :input[type='text']");
                //                if ($list.length) {
                //                    var val = $list.length;
                //                    $list[val - 1].select();
                //                }
            }
            if ((event.keyCode == 40)) {//down

                var $list = $("#<%=ItemsGridView.ClientID%> :input[type='text'], select");
                if ($list.length) {
                    $list[0].focus();
                }

            }
            if (event.keyCode == 13)  // the enter key code
            {
                debugger;
                var $list = $("#<%=ItemsGridView.ClientID%> select");
                if ($list.length) {

                    $list[0].focus();

                    return false;
                }
            }
            if (event.keyCode == 32)  // the space key code
            {
                ExpandSelect($(this));

            }
        });



        //For navigating using left and right arrow of the keyboard
        $("#<%=ItemsGridView.ClientID%> input[type='text'], select").keydown(

                function(event) {

                    if (event.keyCode == 32)  // the space key code
                    {

                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        if (idx > -1) {
                            ExpandSelect($(this));
                        }

                    }
                    if (event.keyCode == 13)  // the enter key code
                    {
                        // should only work on Text boxes, otherwise wierd behaviour on dropdowns
                        var inputs = $(this).parents("table").eq(0).find("input[type='text']");
                        var idx = null;
                        idx = inputs.index(this);
                        if (idx != null && idx > -1) {
                            DoPostBack('itemNew', idx);
                            return false;
                        }

                    }
                    if (event.keyCode == 116)  // F5 key refresh
                    {
                        DoPostBack('Refresh', 0);
                        event.preventDefault();
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
                            inputs[1].select()
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

                });

        $("#<%=ItemsGridView.ClientID%> input[type='text']").keydown(

                function(event) {

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
                                $('#<%=ddlCustomer.ClientID%>').focus();
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
                    var parent = $(this).closest('table').attr('id');
                    if (parent == 'ItemsGridView')
                        return false;

                    if (event.keyCode == 116)  // F5 key refresh
                    {
                        DoPostBack('Refresh', 0);
                        event.preventDefault();
                        return false;
                    }
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
                        // should only work on Text boxes, otherwise wierd behaviour on dropdowns
                        var inputs = $(this).parents("table").eq(0).find("input[type='text']");
                        var idx = null;
                        idx = inputs.index(this);
                        if (idx != null && idx > -1) {
                            DoPostBack('payNew', idx);
                        }

                        return false;
                    }
                    if (event.ctrlKey) {

                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        if (idx > 0) {
                            DoPostBack('payDel', idx);
                        }

                        return false;
                    }
                    if ((event.keyCode == 39) || (event.keyCode == 9 && event.shiftKey == false)) {
                        var inputs = $(this).parents("table").eq(0).find("input[type='text'], select");
                        var idx = inputs.index(this);
                        if (idx == inputs.length - 1) {
                            inputs[1].focus();
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


                });

        $("#<%=PaymentGridView.ClientID%> input[type='text']").keydown(
                function(event) {
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
</asp:Content>
<asp:Content ID="main1" runat="server" ContentPlaceHolderID="main">
  
    <%--<div>   
    <input type="button" id="itemNew" style="visibility:hidden;" />
    <input type="button" id="itemDel" style="visibility:hidden;" />
    <input type="button" id="payNew" style="visibility:hidden;" />
    <input type="button" id="payDel" style="visibility:hidden;" />
    </div> --%>
    <div style="width:50%;">
    <div style="width:100%;background-color:Black; color:Lime; padding-left:10px; font-size:medium;"> 
    <span style="color:White;">Navigation Keys</span>  [Focus=F2] [Refresh=F5][New Row=Enter][Delete Row=Ctrl]Close[ESC]    
    </div>
    <table style="width:100%;">
    <tr><td><asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="DoPostBack('Save',0);return false;" /></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td><asp:Button ID="btnExit" runat="server" Text="Close" OnClientClick="DoPostBack('Close', 0);return false;" /></td>
    </tr>
    <tr style="width:100%;background-color:#4da6ff; color:White; padding-left:10px; ">
    <td>Customer</td>
    <td><asp:DropDownList ID="ddlCustomer" runat="server"></asp:DropDownList></td>    
    <td colspan="2">Invoice #<asp:Label ID="lblInvoice" runat="server" Text="<%#  BillID %>"></asp:Label></td> 
    <td>Date<asp:Label ID="Label1" runat="server"></asp:Label></td>         
    <td><asp:Label ID="lblDate" runat="server"></asp:Label></td>
    </tr>
    </table> 
  
    <asp:GridView ID="ItemsGridView" runat="server" AutoGenerateColumns="false" SkinID="metro" 
        onrowdatabound="ItemsGridView_RowDataBound" ShowFooter="true" >
    <Columns>
       
        <asp:TemplateField HeaderText="Description -[F2]" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:HiddenField ID="lblID" runat="server" Value='<%# Bind("ID") %>'  />
                <asp:HiddenField ID="lblBillID" Value='<%# Bind("BillID") %>'  runat="server"   />
                 <asp:HiddenField ID="hdnCode" Value='<%# Bind("Code") %>'  runat="server"   />
                <asp:DropDownList ID="ddlDescription" runat="server"></asp:DropDownList>
                
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField>
      <%--  <asp:TemplateField HeaderText="Code" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate> 
                <asp:TextBox runat="server" ID="txtCode" Width="50px" Text='<%# Bind("Code") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField>--%>
       
        <asp:TemplateField HeaderText="Unit" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtUnit" Width="50px" Text='<%# Bind("Unit") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtQuantity" Width="50px" Text='<%# Bind("Quantity") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
             
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rate" HeaderStyle-HorizontalAlign="Left" FooterStyle-CssClass="columnBorder">
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtRate" Width="50px" Text='<%# Bind("Rate") %>'></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>       
            <FooterTemplate>
            <asp:Label runat="server" ID="lblSubTotal1" Width="70px" Text="Total" Visible="true"></asp:Label>
            </FooterTemplate>     
        </asp:TemplateField>   
         
        <asp:TemplateField HeaderText="Sub Total" HeaderStyle-HorizontalAlign="Left" FooterStyle-CssClass="columnBorder">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblSubTotal" Width="70px" Text='<%# Bind("SubTotal") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            <FooterTemplate>
            <asp:Label runat="server" ID="lblSubTotal" Text="<%# Total %>" ></asp:Label>
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
                        <asp:HiddenField ID="lblRefID" Value='<%# Bind("BillID") %>'  runat="server"   />
                         <asp:HiddenField ID="hdnPayType" Value='<%# Bind("Type") %>'  runat="server"   />
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
                        <asp:TextBox runat="server" ID="txtAmount" Width="98px" Text='<%# Bind("Amount") %>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Balance" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBalance" Width="103px"  Text='<%# Bind("Balance") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
            
    </Columns>
    </asp:GridView>
   </div>
    </div>
   
 </asp:Content>
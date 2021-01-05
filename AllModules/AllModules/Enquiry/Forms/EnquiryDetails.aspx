<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnquiryDetails.aspx.cs" Inherits="Enquiry.Forms.EnquiryDetails" 
Theme="Default" MasterPageFile ="~/Enquiry/Enquiry.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
.paddingLeftRight
{
    padding:5px;
}
</style>
</asp:Content>
<asp:Content ID="main" runat="server" ContentPlaceHolderID="main">
    <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:100px;"><a href="Enquiries.aspx" title="All Enquiries"><b>I&nbsp;-&nbsp;Enquire</b></a> </li>
    <li>Enquiry Details</li>
    </ul>
    <asp:ImageButton ID="Back" runat="server" Text="Back" onclick="Back_Click" ToolTip="Go Back"  ImageUrl="~/Images/back.png"  Height="25px" Width="25px" />
    
        <asp:ModalPopupExtender ID="pop" runat="server" BackgroundCssClass="modalPopup" TargetControlID="createBtn"
            PopupControlID="pPP" X="50" Y="100" CancelControlID="imgClose">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pPP" runat="server" Style="display: none; background-color: #FBFBEF">
            <div style="float: right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/close.jpg"
                    Height="25px" Width="25px" OnClientClick="return false;" />
            </div>
            <div style="float: left; width: 550px;">
                <table width="540px">
                    <tbody>
                        
                        <tr>
                            <td>
                                <asp:Label ID="cname" runat="server" Text="Add a message"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" >
                                    <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>       
                                    <asp:ListItem Text="Closed" Value="2" ></asp:ListItem>                                
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Call Back"></asp:Label>
                            </td>
                             <td>
                               <asp:TextBox ID="txtCB" Width="70px"  runat="server"></asp:TextBox>
                               <asp:ImageButton ID="imgC" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <ajax:CalendarExtender ID="Ce31" runat="server"   TargetControlID="txtCB"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgC"   />
                            </td>
                       </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="Message" runat="server"  TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox>                               
                            </td> 
                       </tr>
                       <tr> 
                            <td colspan="5"> 
                            <asp:CompareValidator ID="CompareEndTodayValidator" Display="Dynamic" Operator="GreaterThanEqual" type="Date" 
                            ErrorMessage="The Call Back Date must be later than today" runat="server"  ControltoValidate="txtCB" 
                            ValidationGroup="Val"    />
                            </td>
                       </tr>
                       <tr> 
                            <td> <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="Val" OnClick="btnSubmit_OnClick" /></td>
                       </tr>
                       </tbody>
                </table>
             </div>
            </asp:Panel>
    <div style="margin-left:5px">    
    <div >
    <asp:Repeater ID="mainRepeater" runat="server"   >        
       <ItemTemplate>
       <table  class="TableClass"  cellpadding="0px" cellspacing="0px"  style="table-layout:fixed;"   >
       <tr class="theadColumnWithBackground">       
           <td>Company</td> 
           <td>Contact Person</td>           
           <td>Email</td>           
           <td>Enquiry Source</td>        
           <td>Status</td>     
           <td>CallBack</td> 
       </tr>
       <tr>
           <td><asp:Label ID="lblCompany" runat="server" Text='<%# Eval("Company") %>' ></asp:Label></td>       
           <td><asp:Label ID="lblName" runat="server" Text='<%# Eval("EName") %>' ></asp:Label></td>          
           <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("Email") %>' ></asp:Label></td> 
           <td><asp:Label ID="Label6" runat="server" Text='<%# Eval("EnquiryTypeName") %>' ></asp:Label></td>
           <td style="width:148px;"><asp:Label ID="Label8" runat="server" Width="112px" Text='<%# Eval("EnquiryStatusName") %>' ></asp:Label></td>             
           <td  style="min-width:40px;max-width:40px; ">
           <asp:Label ID="Label9" runat="server"    Text='<%# Eval("CallBackDate","{0:dd/MM/yyyy}")   %>' 
           CssClass='<%# Eval("EnquiryStatusName").Equals("Closed") ? "blueClass" : (( (Convert.IsDBNull(Eval("CallBackDate")) ? DateTime.Now.Date : Convert.ToDateTime(Eval("CallBackDate"))) - DateTime.Now.Date ).TotalDays >= 0 ? "greenClass" : "redClass") %>'></asp:Label>
           </td>
       </tr>
        <tr class="theadColumnWithBackground">
        <td>Subject</td>
        <td  style="text-align:center;" colspan="2">Message</td>
        <td>Phone</td>        
        <td>Updated By</td>
        <td>Modified On</td>    
       
       </tr>
        <tr>
        <td  ><asp:Label ID="Label4"  runat="server" Text='<%# Eval("Subject") %>' ></asp:Label></td>       
        <td   colspan="2"  ><asp:Label ID="Label7" runat="server" Text='<%# Eval("Message") %>' ></asp:Label></td>       
        <td  ><asp:Label ID="Label2" runat="server"  Text='<%# Eval("TelePhone") %>' ></asp:Label></td>       
        <td  style="min-width:40px;max-width:40px;"><asp:Label ID="Label5" runat="server"     Text='<%# Eval("CreatedByName") %>' ></asp:Label></td>           
        <td style="width:148px;"><asp:Label ID="Label3" runat="server" Width="80px" Text='<%# Eval("CreatedOn","{0:dd/MM/yyyy}") %>' ></asp:Label></td>        
        
       </tr>
       </table>
       </ItemTemplate>
       </asp:Repeater>
    </div>
    <div >
    <asp:Repeater ID="childRepeater" runat="server"   >  
      <ItemTemplate>   
      <table class="TableClass" style="table-layout:fixed;">      
       <tr>
        <td colspan="4" style="text-align:left; padding-left:10px;" ><asp:Label ID="Label2" runat="server" Width="80%" Text='<%# Eval("Message") %>' ></asp:Label></td>       
        <td style="min-width:40px;max-width:40px; " ><asp:Label ID="Label5" runat="server"  Text='<%# Eval("ModifiedByName") %>' ></asp:Label></td>           
        <td style="min-width:40px;max-width:40px; " ><asp:Label ID="Label9" runat="server" Text='<%# Eval("ModifiedOn","{0:dd/MM/yyyy}")   %>' ></asp:Label></td>
       </tr>
       </table>
        
        </ItemTemplate> 
    </asp:Repeater>      
    <br />
     <asp:Button ID="createBtn" runat="server" Text="Add New Message" />
    </div>
    </div>
</asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Enquiries.aspx.cs" Inherits="Enquiry.Forms.Enquiries"
Theme="Default" MasterPageFile ="~/Enquiry/Enquiry.Master"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp"  %>
<%@ Register TagName="dash" TagPrefix="enq" Src="~/Enquiry/Controls/ucEnquiryDashboard.ascx" %> 

<asp:Content ID="headr" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function ShowHide(id) {
        var el = document.getElementById('AdvSearch');
        var expcol = document.getElementById('imgA');
        if (el.style.display == 'none') {
            el.style.display ="";
            /*expcol.src = "Images/imgUblue.png" */
            expcol.src = "../../Images/up.png"
        }
        else {
            el.style.display = 'none';
            /* expcol.src = "Images/imgDblue.png" */
            expcol.src = "../../Images/down.png"
        }
    }
     
</script> 
</asp:Content>
 <asp:Content ID="main" runat="server" ContentPlaceHolderID="main">
  <ul id="breadcrumb">
    <li><a href="../../Default.aspx" title="Navigation"><img src="../../Images/breadcrumbs_home.png" alt="Navigation" class="home" /></a></li>
    <li style="width:100px;"><a href="Enquiries.aspx" title="All Enquiries"><b>I&nbsp;-&nbsp;Enquire</b></a> </li>
    <li>Enquiries</li>
  </ul>
  <asp:Panel ID="SearchTable" runat="server" >
 <div style="border:solid 1px #498AF3; width:650px; " >     
    <asp:TextBox ID="txtSearch" runat="server"  BorderStyle="None" BorderWidth="0px"  Height="18px" Width="560px"   ></asp:TextBox>        
    <asp:ImageButton ID="search" runat="server" CssClass="vAlign" ImageUrl="~/Images/imgSBlueBig.png" ValidationGroup="Val"  OnClick="SearchBtnClick" ToolTip="Save Search" Height="25px" Width="25px"     />    
    <img src="../../Images/down.png" class="vAlign" id="imgA"  onclick="javascript:ShowHide(this);return false;" style="height:25px;width:25px;border-style: none;" />
    <asp:ImageButton ID="reset" runat="server" CssClass="vAlign" ImageUrl="~/Images/refresh.png" ToolTip="Clear Search" OnClick="ClearBtnClick" Height="25px" Width="25px"   />
    <ajax:TextBoxWatermarkExtender ID="ajaxWaterMark" runat="server" TargetControlID="txtSearch"
     WatermarkText="Search..." ></ajax:TextBoxWatermarkExtender>    
 </div>
<div id="AdvSearch" style="border:solid 1px #AACCEE; width:640px;display:none; margin-top:5px;">
<div style="border:solid 1px #498AF3; height:20px; background-color:#498AF3; color:White;">
<div style="margin-left:5px;margin-top:2px;">
<asp:Label ID="lblSearchControl"  runat="server" Text=" Advanced Search"  SkinID="" />
</div>
</div>

<div>
<table >
     <tr>
        <td>
            Enquiry #  
         </td>
        <td>            
            <asp:TextBox ID="txtIDSearch" runat="server" Width="150px" ></asp:TextBox>
        </td>
        <td >
            Created By  
        </td>
        <td>
            <asp:TextBox ID="txtCreatedBySearch" Width="70px" runat="server"></asp:TextBox>            
        </td>
         <td >
            Status
        </td>
        <td> <asp:DropDownList ID="ddlStatusSearch" runat="server" >
                <asp:ListItem Text="ALL" Value="All" Selected="True"></asp:ListItem> 
                <asp:ListItem Text="Open" Value="0"  ></asp:ListItem>
                <asp:ListItem Text="Pending" Value="1" ></asp:ListItem>
                <asp:ListItem Text="Closed" Value="2"  ></asp:ListItem>                                  
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            Contact/Company Name
        </td>
        <td>
            <asp:TextBox ID="txtCompanySearch" Width="150px" runat="server"></asp:TextBox>
        </td>
        <td >
          From Date 
        </td>
        <td>
         <span><asp:TextBox ID="txtFromSearch" Width="70px"  runat="server"></asp:TextBox>
         <asp:ImageButton ID="imgPop1" runat="server" ImageUrl="~/Images/Calendar.png" />
         <ajax:CalendarExtender ID="ceTo" runat="server"   TargetControlID="txtFromSearch"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgPop1"  />
        </span>
        </td>
         <td> 
          To Date
          </td>
         <td>
         <span><asp:TextBox ID="txtToSearch" Width="70px"  runat="server"  ></asp:TextBox>
         <asp:ImageButton ID="imgPop2" runat="server" ImageUrl="~/Images/Calendar.png" />
         <ajax:CalendarExtender ID="ceFrom" runat="server"   TargetControlID="txtToSearch"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgPop2"  />
         </span>
         </td>     
        </tr>
        <tr>
        <td colspan="5">  
         <asp:CompareValidator ID="cmpVal1" ControlToCompare="txtToSearch"  ValidationGroup="Val"   EnableClientScript="true" 
         ControlToValidate="txtFromSearch" Type="Date" Operator="LessThanEqual"   Display="Dynamic" 
         ErrorMessage="To Date cannot be before than From Date" runat="server"></asp:CompareValidator>
        </td>
        </tr>
</table>
 </div>
 </div>
  </asp:Panel> 
    <div>
       <table width="60%">
       <tr>
       <td style="width:10%">
        <asp:Panel ID="AddDeleteButtonPanel" runat="server">
        <table>
        <tr>  
           
        <td>
         <asp:ImageButton runat="server" ID="DeleteButton" AlternateText="Delete Enquiry" ToolTip="Delete Enquiry"
         OnClick="DeleteButton_Click" ImageUrl="~/Images/iBin.gif" OnClientClick="return confirm('Are you sure you want to delete this Enquiry, Even the Details will be deleted! Do you want to continue ?')" />         
        </td>         
        <td>
        <asp:ImageButton runat="server" ID="AddNewButton" AlternateText="Add New Enquiry" ToolTip="Add New Enquiry"
         OnClientClick="return false;" ImageUrl="~/Images/iAdd.PNG" />          
        </td>       
        </tr>
        </table>
        </asp:Panel>
       </td>
          <td ><asp:LinkButton ID="lnkOpen" runat="server" onclick="lnkOpen_Click"></asp:LinkButton></td>
          <td><asp:LinkButton ID="lnkPending" runat="server" onclick="lnkPending_Click"></asp:LinkButton></td>
          <td><asp:LinkButton ID="lnkClosed" runat="server" onclick="lnkClosed_Click"></asp:LinkButton></td>
          <td><asp:LinkButton ID="lnkCallback" runat="server" onclick="lnkCallback_Click"></asp:LinkButton></td>
   
       </tr>   
       
       </table>  
         
        <asp:ModalPopupExtender ID="pop" runat="server" BackgroundCssClass="modalPopup" TargetControlID="AddNewButton"
            PopupControlID="pPP" X="50" Y="100" CancelControlID="imgClose">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pPP" runat="server" Style="display: none; background-color: #FBFBEF">
            <div style="float: right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/close.jpg"
                    Height="25px" Width="25px" OnClientClick="return false;" />
            </div>
            <div style="float: left; width: 650px;">
                <table width="500px">
                    <tbody>
                         
                        <tr>
                            <td>
                                <asp:Label ID="cname" runat="server" Text="Contact Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="company" runat="server" Text="Company"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="phone" runat="server" Text="Phone"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="email" runat="server" Text="Email "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Enquiry Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlList" runat="server">
                                    <asp:ListItem Text="Demo" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Sales" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Support" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="JustDial" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Internal" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Subject"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Enquiry Status"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false">
                                    <asp:ListItem Text="Open" Value="0" Selected="True"></asp:ListItem>                                   
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="CallBack Date"></asp:Label>
                            </td>
                            <td>
                               <asp:TextBox ID="txtCB" Width="70px"  runat="server"></asp:TextBox>
                               <asp:ImageButton ID="imgC" runat="server" ImageUrl="~/Images/Calendar.png" />
                               <ajax:CalendarExtender ID="Ce31" runat="server"   TargetControlID="txtCB"   CssClass="MyCalendar"
                                   Format="dd/MM/yyyy"    PopupButtonID="imgC"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Message"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr> 
                            <td colspan="4"> 
                            <asp:CompareValidator ID="CompareEndTodayValidator" Display="Dynamic" Operator="GreaterThanEqual" type="Date" 
                            ErrorMessage="The Call Back Date must be later than today" runat="server"  ControltoValidate="txtCB" 
                            ValidationGroup="Val"    />
                            </td>
                       </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="Val" OnClick="btnSubmit_OnClick" />
                            </td>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </asp:Panel>
         
        <asp:GridView ID="egv" runat="server" SkinID="metro" AutoGenerateColumns="false" 
        DataKeyNames="ID" OnPageIndexChanging="egv_PageIndexChanging" AllowPaging="true" PageSize="20">
        <Columns>
        <asp:TemplateField>
        <ItemTemplate>
        <asp:RadioButton ID="rbtnSelect" runat="server" AutoPostBack="true"  OnCheckedChanged="btnSelect_Click"     />       
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
            <asp:Image ID="imgCollapsible" ImageUrl="~/Images/iADD.bmp"  runat="server" />            
            <ajax:CollapsiblePanelExtender ID="cpe" runat="Server"  
             TargetControlID="pnlOrderItem" CollapsedSize="0" Collapsed="True"  
             ExpandControlID="imgCollapsible"     CollapseControlID="imgCollapsible"
             AutoCollapse="False" AutoExpand="False" ScrollContents="false" 
             ImageControlID="imgCollapsible" ExpandedImage="~/images/iSUB.bmp" 
             CollapsedImage="~/images/iADD.bmp" ExpandDirection="Vertical" />             
              </ItemTemplate>  
        </asp:TemplateField>
        <asp:TemplateField>
        <ItemTemplate>
        <asp:HyperLink  ID="txt" runat="server" Text="View"  NavigateUrl='<%# Eval("ID","~/Enquiry/Forms/EnquiryDetails.aspx?ID={0}" ) %>' />
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ID"       HeaderText="Enquiry #" />
        <asp:BoundField DataField="EName"       HeaderText="Contact Name" />
        <asp:BoundField DataField="Company"     HeaderText="Company Name" />       
        <asp:BoundField DataField="Subject"     HeaderText="Subject"/>
        <asp:BoundField DataField="EnquiryTypeName" HeaderText="Enquiry Type" />
        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" />
        <asp:BoundField DataField="CreatedOn"   HeaderText="Created On" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="ModifiedByName" HeaderText="Modified By" />
        <asp:BoundField DataField="ModifiedOn"   HeaderText="Modified On" DataFormatString="{0:dd/MM/yyyy}" />
         <asp:BoundField DataField="EnquiryStatusName"     HeaderText="Status" /> 
         <asp:TemplateField HeaderText="CallBack">
         <ItemTemplate>
         <asp:Label  id="lbl1" runat="Server" Text='<%# Eval("CallBackDate","{0:dd/MM/yyyy}") %>'
          CssClass='<%# Eval("EnquiryStatusName").Equals("Closed") ? "blueClass" : (( (Convert.IsDBNull(Eval("CallBackDate")) ? DateTime.Now.Date : Convert.ToDateTime(Eval("CallBackDate"))) - DateTime.Now.Date ).TotalDays >= 0 ? "greenClass" : "redClass") %>'></asp:Label>
         </ItemTemplate>
         </asp:TemplateField>
        
             
        <asp:TemplateField>
            <ItemTemplate>        
                <tr>                   
                    <td colspan="100%"> 
                      <asp:Panel ID="pnlOrderItem" runat="server"  >
                      <table class="TableClass">
                      <tr>
                      <td style="width:25%">Email</td>
                      <td style="width:25%"><asp:Label ID="email" runat="server" Text='<%# Bind("Email") %>'></asp:Label>&nbsp;</td>
                      <td  style="width:25%">Phone</td>
                      <td  style="width:25%"><asp:Label ID="telephone" runat="server" Text='<%# Bind("Telephone") %>'></asp:Label>&nbsp;</td>
                      </tr>
                      <tr>
                      <td  colspan="1" >Message</td>
                      <%--<td  colspan="100%" >&nbsp;</td>--%>
                       <td colspan="100%"  ><asp:Label ID="Label2" runat="server" Text='<%# Bind("Message") %>'>&nbsp;</asp:Label></td>
                       </tr>
                      
                      </table>                     
                     
                      </asp:Panel>
                    </td>
                </tr> 
            </ItemTemplate>
        </asp:TemplateField> 
        </Columns>
        </asp:GridView>
        
    </div>
</asp:Content>
 

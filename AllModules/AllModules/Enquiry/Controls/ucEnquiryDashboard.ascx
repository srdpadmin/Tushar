<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEnquiryDashboard.ascx.cs" 
Inherits="Enquiry.Controls.ucEnquiryDashboard"  %>


<div>
  <table class="TableClass" style="width:50% !Important;">  
  <%--<tr>
  <th>Open</th>
  <th>Pending</th>
  <th>Closed</th>
  <th>Callback Required</th>
  </tr>--%>
  <tr>
  <td ><asp:LinkButton ID="open" runat="server"></asp:LinkButton></td>
  <td><asp:LinkButton ID="pending" runat="server"></asp:LinkButton></td>
  <td><asp:LinkButton ID="closed" runat="server"></asp:LinkButton></td>
  <td><asp:LinkButton ID="callback" runat="server"></asp:LinkButton></td>
  </tr>
  </table>
  </div>

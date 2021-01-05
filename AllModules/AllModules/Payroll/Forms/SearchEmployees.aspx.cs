using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Payroll.BusLogic;

namespace Payroll.Forms
{
    public partial class SearchEmployees : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Payroll/Forms/ManageEmployee.aspx");
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = EmpGV.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string empID = Convert.ToString(EmpGV.DataKeys[row.RowIndex].Value);
                Response.Redirect("~/Payroll/Forms/ManageEmployee.aspx?ID="  + empID);
                //+ "&" + "EmpName=" + appendQuery[1]);
            }
            
        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = EmpGV.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string ID = Convert.ToString(EmpGV.DataKeys[row.RowIndex].Value);
                Employee ord = new Employee();
                int[] IDs = ord.DeleteEmployee(ID);
                if (IDs[1] > 0)
                {
                    EmpGV.DataBind();
                }
            }
             
        }

        protected void SelectEmployee_Click(object sender, EventArgs e)
        {
            GridViewRow row = EmpGV.SelectedRow;
            if (row != null)
            {
                string empID = Convert.ToString(EmpGV.DataKeys[row.RowIndex].Value);
                Response.Redirect("~/Payroll/Forms/SalaryInformation.aspx?EmpID=" + empID);
                                  //+ "&" + "EmpName=" + appendQuery[1]);
            }

        }

        protected void SelectButton_Click(object sender, EventArgs e)
        {
            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            EmpGV.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            EmpGV.SelectedIndex = row.RowIndex;
        }

        protected void SearchViewItemSelected(Object sender, GridViewCommandEventArgs e)
        {
            // If multiple buttons are used in a GridView control, use the
            // CommandName property to determine which button was clicked.
            if (e.CommandName == "Select")
            {               
                string[] appendQuery = (e.CommandArgument).ToString().Split(';');
                Response.Redirect("~/Payroll/Forms/ManageEmployee.aspx?ID=" + appendQuery[0]);
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {            
            EmployeeName.Text = string.Empty;
            EmployeeID.Text = string.Empty;
            txtSearch.Text = string.Empty;
        }

        protected void EmpGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EmpGV.PageIndex = e.NewPageIndex;
            EmpGV.DataBind();
        }
    }
}

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

namespace AllModules.Payroll.Controls
{
    public partial class EmployeeLookupDDL : System.Web.UI.UserControl
    {
        public string EmployeeName
        {
            get { return emplddlList.SelectedItem.Value; }
             
        }

        public string EmployeeID
        {
            get { return empID.Text; }
            set { empID.Text = value; }
        }

        public event EventHandler empChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Employee emp = new Employee();
                emplddlList.DataSource = emp.GetEmployees();
                emplddlList.DataTextField = "Value";
                emplddlList.DataValueField = "Key";
                emplddlList.DataBind();
            }
        }

        protected void emplddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            empID.Text = emplddlList.SelectedValue;
            

            if (empChanged != null)
            {
                empChanged(new Object(), e);
            }
        }
        protected void Reset_Click(object sender, EventArgs e)
        {
            empID.Text = string.Empty;
            emplddlList.SelectedIndex = 0;

            if (empChanged != null)
            {
                empChanged(this, EventArgs.Empty);
            }
        }
         
    }
}
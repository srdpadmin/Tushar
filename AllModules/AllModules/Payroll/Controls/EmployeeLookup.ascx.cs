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

namespace Payroll.Controls
{
    public partial class EmployeeLookup : System.Web.UI.UserControl
    {
        public string EmployeeName
        {
            get { return employeeName.Text; }
            set { employeeName.Text = value; }
        }

        public string EmployeeID
        {
            get { return empID.Text; }
            set { empID.Text = value; }
        }

        public event EventHandler empChanged;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void EmployeeName_TextChanged(object sender, EventArgs e)
        {
            if (empChanged != null)
            {
                empChanged(new Object(), e);
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            empID.Text = string.Empty;
            employeeName.Text = string.Empty;
            if (empChanged != null)
            {
                empChanged(this, EventArgs.Empty);
            }
        }
    }
}
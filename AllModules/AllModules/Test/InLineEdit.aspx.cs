using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllModules.Test
{
    public partial class InLineEdit : System.Web.UI.Page
    {
         
        private bool isEditMode = true;
        protected bool IsInEditMode
        {
            get { return this.isEditMode; }
            set { this.isEditMode = value; }
        }
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            List<Person> list = new List<Person>();
            if (ViewState["list"] == null)
            {

                list.Add(new Person("abc", "def", "hig"));
                list.Add(new Person("abc", "def", "hig"));
                list.Add(new Person("abc", "def", "hig"));
            }
            else
                list = (List<Person>)ViewState["list"];
            gvUsers.DataSource = list;
            gvUsers.DataBind();
        }

        protected void add_Click(object sender, EventArgs e)
        {
            List<Person> list = new List<Person>();

            foreach (GridViewRow gvr in gvUsers.Rows)
            {
                TextBox fname = (TextBox)gvr.FindControl("txtFirstName");
                TextBox userid = (TextBox)gvr.FindControl("txtUserID");
                TextBox lname = (TextBox)gvr.FindControl("txtLastName");               
                list.Add(new Person(fname.Text, lname.Text, userid.Text));
            }
            list.Add(new Person("","",""));
            ViewState["list"] = list;
            BindData();
        }
        protected void del_Click(object sender, EventArgs e)
        {
            List<Person> list = new List<Person>();

            foreach (GridViewRow gvr in gvUsers.Rows)
            {
                TextBox fname = (TextBox)gvr.FindControl("txtFirstName");
                TextBox userid = (TextBox)gvr.FindControl("txtUserID");
                TextBox lname = (TextBox)gvr.FindControl("txtLastName");
                CheckBox chk = (CheckBox)gvr.FindControl("chk");
                if(!chk.Checked)
                list.Add(new Person(fname.Text, lname.Text, userid.Text));
            }
            
            ViewState["list"] = list;
            BindData();
        }
    }
        
}

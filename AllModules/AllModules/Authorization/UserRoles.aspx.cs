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
using System.Collections.Generic;

namespace Authorization
{
    public partial class UserRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var years = new int[3];
                var currentYear = DateTime.Now.Year;
                Dictionary<int, string> dict = new Dictionary<int, string>();
                for (int i = 0; i < years.Length; i++)
                {
                    dict.Add(currentYear--,Convert.ToString(currentYear--));
                }

                checkBoxes1.DataSource = dict;
                checkBoxes1.DataTextField = "Key";
                checkBoxes1.DataValueField = "Value";
                checkBoxes1.DataBind();
            }
            
        }
        protected void checkBoxes_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem item in (checkBoxes1 as ListControl).Items)
            {
                if (item.Text == "2009")
                {
                    item.Selected = true;
                }
                    
            }
        }
        protected void checkBoxes_SelcetedIndexChanged(object sender, EventArgs e)
        {
            checkBoxes1.Texts.SelectBoxCaption = string.Empty;
            foreach (ListItem item in (checkBoxes1 as ListControl).Items)
            {
                if(item.Selected)
                checkBoxes1.Texts.SelectBoxCaption += item.Text + ",";
            }
        }
        protected void testBtn_OnClick(object sender, EventArgs e)
        {
            smText.Text = string.Empty;
            foreach (ListItem item in (checkBoxes1 as ListControl).Items)
            {
                if (item.Selected)
                    smText.Text+=item.Text + ",";
            }
        }
    }
}

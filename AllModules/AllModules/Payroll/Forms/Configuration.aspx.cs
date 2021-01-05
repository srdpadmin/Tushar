using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AllModules.Payroll.BusLogic;
using AllModules.Payroll.Data;

namespace AllModules.Payroll.Forms
{
    public partial class Configuration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateConfiguration();
            }
        }

        public void PopulateConfiguration()
        {
            clsConfiguration config = new clsConfiguration();
            clsConfigurationData cData = config.GetConfiguration();
            DateTime dt = new DateTime(1,1,1);
            lblModifiedOn.Text = cData.ModifiedOn.ToString();
            if (cData.ModifiedOn.CompareTo(dt.Date) != 0)
            {
                txtProfTax.Text = cData.ProfessionalTax.ToString();
                txtPF.Text = cData.ProvidentFund.ToString();
                txtESIC.Text = cData.ESIC.ToString();
                txtTA.Text = cData.TravelAllowance.ToString();
                txtDA.Text = cData.DearnessAllowance.ToString();
                hdnID.Value = cData.ID.ToString();
            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "UniqueID", "alert(\"No configuration found\");", true);

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            clsConfigurationData cData = new clsConfigurationData();
            cData.ProfessionalTax   = txtProfTax.Text != string.Empty ? Convert.ToSingle(txtProfTax.Text):0.0f;
            cData.ProvidentFund     = txtPF.Text != string.Empty ? Convert.ToSingle(txtPF.Text) : 0.0f;
            cData.ESIC              = txtESIC.Text != string.Empty ? Convert.ToSingle(txtESIC.Text) : 0.0f;
            cData.TravelAllowance   = txtTA.Text != string.Empty ? Convert.ToSingle(txtTA.Text) : 0.0f;
            cData.DearnessAllowance = txtDA.Text != string.Empty ? Convert.ToSingle(txtDA.Text) : 0.0f;
            cData.ModifiedOn = DateTime.Now;
        
            DateTime dt = new DateTime(1, 1, 1);
            bool success = false;
            if (cData.ProfessionalTax > 0 || cData.ESIC > 0 || cData.ProvidentFund > 0 || cData.TravelAllowance > 0 || cData.DearnessAllowance > 0)
            {
                clsConfiguration config = new clsConfiguration();
                if (Convert.ToDateTime(lblModifiedOn.Text).CompareTo(dt) == 0 )
                {  //insert
                    success =config.InsertConfiguration(cData);

                }
                else
                {     //update
                    cData.ID = Convert.ToInt32(hdnID.Value);
                   success= config.UpdateConfiguration(cData);
                }
                string successStr = "alert(\"Configuration Updated\");";
                string errorStr = "alert(\"Configuration Failed to Update\");";
                if (success)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "UniqueID",successStr , true);
                    PopulateConfiguration();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "UniqueID", errorStr, true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "UniqueID", "alert(\"No configuration to Update\");", true);
            }

        }
    }
}
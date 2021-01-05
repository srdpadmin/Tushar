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
using CoreAssemblies;

namespace AllModules
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {             
            //This solves issue with hitting back button
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoServerCaching();
            HttpContext.Current.Response.Cache.SetNoStore();
            //headImage.ImageUrl = Server.MapPath("~") + "\\Images\\Temp-Copy.png";
            headImage.ImageUrl = "~/Images/Temp-Copy1.png";

            // Culture for date
            //System.Globalization.CultureInfo vCulture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CreateSpecificCulture("en-GB").Clone();
            //vCulture.DateTimeFormat.ShortDatePattern = "dd/mm/yyyy";
            //System.Threading.Thread.CurrentThread.CurrentCulture = vCulture; 
           
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        { 
            
          Response.Redirect("~/Authorization/UserModules.aspx");
            
        }
        protected void CreateUser_Click(object sender, EventArgs e)
        { 
          Response.Redirect("~/Authorization/AdministrationPage.aspx?Selection=1");
            
        }
        protected void ManageUsers_Click(object sender, EventArgs e)
        { 
          Response.Redirect("~/Authorization/AdministrationPage.aspx?Selection=2");            
        }
        protected void ManageUserRoles_Click(object sender, EventArgs e)
        { 
          Response.Redirect("~/Authorization/AdministrationPage.aspx?Selection=3");            
        }
        protected void lnkChangePassword_Click(object sender, EventArgs e)
        { 
          Response.Redirect("~/Authorization/ChangePassword.aspx");            
        }
        protected void MainMenu_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("~/Default.aspx");
        }
        protected void MainSite_Click(object sender, EventArgs e)
        {
           
            Response.Redirect("www.srdpltd.in");
        }
        
         
    }
}

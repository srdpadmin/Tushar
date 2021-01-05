using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Authorization.BusLogic;

namespace Authorization.Controls
{
    public partial class ManageUsers : System.Web.UI.UserControl
    {
        private MembershipUserCollection allRegisteredUsers = Membership.GetAllUsers();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                lblOnlineUsers.Text = Membership.GetNumberOfUsersOnline().ToString();
                lblTotalUsers.Text = allRegisteredUsers.Count.ToString();
                string[] alph = "A;B;C;D;E;F;G;J;K;L;M;N;O;P;Q;R;S;T;U;V;W;X;Y;Z;All".Split(';');
                rptAlphabetBar.DataSource = alph;
                rptAlphabetBar.DataBind();
                BindAllUsers(false);
            }
            
        }

        protected void rptAlphabetBar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            gvUsers.Attributes.Add("SearchByEmail", false.ToString());
            if (e.CommandArgument.ToString().Length == 1)
            {
                gvUsers.Attributes.Add("SearchText", e.CommandArgument.ToString() + "%");
                this.BindAllUsers(false);
            }
            else
            {
                gvUsers.Attributes.Add("SearchText", "");
                this.BindAllUsers(false);
            }
        }
        private void BindAllUsers(bool reloadAllUsers)
        {
            //MembershipUserCollection allUsers = null;
            if (reloadAllUsers)
                allRegisteredUsers = Membership.GetAllUsers();
            string searchText = "";
            if (!string.IsNullOrEmpty(gvUsers.Attributes["SearchText"]))
                searchText = gvUsers.Attributes["SearchText"];
            bool searchByEmail = false;
            if (!string.IsNullOrEmpty(gvUsers.Attributes["SearchByEmail"]))
                searchByEmail = bool.Parse(gvUsers.Attributes["SearchByEmail"]);
            if (searchText.Length > 0)
            {
                if (searchByEmail)
                    allRegisteredUsers = Membership.FindUsersByEmail(searchText);
                else
                    allRegisteredUsers = Membership.FindUsersByName(searchText);
            }
            //else
            //{
            //    allRegisteredUsers = allRegisteredUsers;
            //}
            gvUsers.DataSource = allRegisteredUsers;
            gvUsers.DataBind();
            upG.Update();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bool searchByEmail = (ddlUserSearchTypes.SelectedValue == "E-mail");
            gvUsers.Attributes.Add("SearchText", "%" + txtSearchText.Text + "%");
            gvUsers.Attributes.Add("SearchByEmail", searchByEmail.ToString());
            BindAllUsers(false);
        }
        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string userName  = gvUsers.DataKeys[e.RowIndex].Value.ToString();
            string providerUserKey = Membership.GetUser(userName).ProviderUserKey.ToString();
            Membership.DeleteUser(userName,true);          
            
            Users user = new Users();
            int x = user.DeleteUserProfile(Convert.ToInt32(providerUserKey));
            UserModule um = new UserModule();
            um.DeleteUserModules("",providerUserKey.ToString());
            BindAllUsers(true);
            lblTotalUsers.Text = allRegisteredUsers.Count.ToString();
        }

        protected void gvUsers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton btn = e.Row.Cells[6].Controls[0] as ImageButton;
                LinkButton btn = e.Row.Cells[5].Controls[0] as LinkButton;
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this user?') == false) return false;";
            }
        }

    }
}
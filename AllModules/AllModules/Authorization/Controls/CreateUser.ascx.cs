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
    public partial class CreateUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        } 
        protected void CreateAccountButton_Click(object sender, EventArgs e)
        {
            
            MembershipCreateStatus createStatus;

            MembershipUser newUser = Membership.CreateUser(UserName.Text, Password.Text, Email.Text, Question.SelectedValue, Answer.Text, true, null, out createStatus);

            switch (createStatus)
            {
                case MembershipCreateStatus.Success:
                    AccountCreated.Text = "The  account " + UserName.Text + " was successfully created!";
                    

                    //string[] selectedUsersRoles = Roles.GetRolesForUser(UserName.Text);
                    Users user = new Users();
                    user.InsertNewUserProfile(Convert.ToInt32(newUser.ProviderUserKey), UserName.Text, FirstName.Text, MiddleName.Text, LastName.Text, CompanyName.Text, Email.Text, Phone.Text);
                    //Roles.AddUserToRole(UserName.Text, "supervisor");
                    SignUpPanel.Visible = false;
                    showResult.Visible = true;
                    break;
                   
                case MembershipCreateStatus.DuplicateUserName:
                    CreateAccountResults.Text = "There already exists a user with this username.";
                    break;

                case MembershipCreateStatus.DuplicateEmail:
                    CreateAccountResults.Text = "There already exists a user with this email address.";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    CreateAccountResults.Text = "There email address you provided in invalid.";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    CreateAccountResults.Text = "There security answer was invalid.";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    CreateAccountResults.Text = "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character.";

                    break;
                default:
                    CreateAccountResults.Text = "There was an unknown error; the user account was NOT created.";
                    break;
            }
        }

        private void CreateUserProfile()
        {

        }
    }
}
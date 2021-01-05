using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Test
{
	public partial class _Default : System.Web.UI.Page
	{

		private int iCurrentEdit = -1;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				GridView1.DataSource = CreateTable();
				GridView1.DataBind();
			}
			else
			{
				if (ViewState["editRowIndex"] != null) // indicates we have values from edit which needs to be saved in viewstate
				{
					int iEditRowID = int.Parse((ViewState["editRowIndex"] == null ? "-2" : ViewState["editRowIndex"].ToString())); // get current edit row index

					// get all elements from edit template
					TextBox tbF = (TextBox)GridView1.Rows[iEditRowID].FindControl("tbFirstName");
					TextBox tbL = (TextBox)GridView1.Rows[iEditRowID].FindControl("tbLastName");
					TextBox tbW = (TextBox)GridView1.Rows[iEditRowID].FindControl("tbWeb");
					TextBox tbE = (TextBox)GridView1.Rows[iEditRowID].FindControl("tbEmail");

					// save all values into viewstate for future use
					if (tbF != null)
					{
						ViewState.Add("fname", tbF.Text);
						ViewState.Add("lname", tbL.Text);
						ViewState.Add("web", tbW.Text);
						ViewState.Add("email", tbE.Text);
					}
				}
			}
		}

		private DataTable CreateTable()
		{
			DataTable table = new DataTable();

			DataColumn fName = new DataColumn("FirstName");
			DataColumn lName = new DataColumn("LastName");
			DataColumn email = new DataColumn("eMail");
			DataColumn web = new DataColumn("Web");
			DataColumn userID = new DataColumn("UserID");
			table.Columns.Add(fName);
			table.Columns.Add(lName);
			table.Columns.Add(email);
			table.Columns.Add(web);
			table.Columns.Add(userID);

			DataRow row = null;
			String[,] sData = new string[,] 
            {{"Jay","Thakar","jay@Thakar.info","http://www.thakar.info","1"},
            {"Johnson","Thomas","jthomas@yahoo.com","http://www.Microsoft.com","2"},
            {"Samuel","Tony","sTony@Cisco.com","http://www.Cisco.com","3"},
            {"Methew","Carlson","MCarlson@MySpace.com","http://www.MySpace.com","4"},
            {"Johnson","Thomas","jthomas@yahoo.com","http://www.Microsoft.com","5"},
            {"Samuel","Tony","sTony@Cisco.com","http://www.Cisco.com","6"},
            {"Methew","Carlson","MCarlson@MySpace.com","http://www.MySpace.com","7"},
            {"Paul","Cook","pCook@680News.com","http://www.680News.com","8"}};
			for (int iCtr = 0; iCtr < sData.Length / 5; iCtr++)
			{
				row = table.NewRow();
				row["FirstName"] = sData[iCtr, 0];
				row["LastName"] = sData[iCtr, 1];
				row["eMail"] = sData[iCtr, 2];
				row["Web"] = sData[iCtr, 3];
				table.Rows.Add(row);
			}
			return table;
		}

		protected void GridView1_RowEditing(Object sender, GridViewEditEventArgs e)
		{
			GridView1.EditIndex = e.NewEditIndex;
			GridView1.DataSource = CreateTable();
			GridView1.DataBind();
			ViewState.Add("editRowIndex", e.NewEditIndex);
			ImageButton editButton;

			foreach (GridViewRow row in GridView1.Rows)
			{
				if (row.RowIndex != e.NewEditIndex)
				{
					editButton = (ImageButton)(row.Cells[1].Controls[0]);
					editButton.ImageUrl = "./edit_off.gif";
					editButton.Enabled = false;
				}
				else
				{
					ViewState.Add("EditRowID", row.DataItemIndex.ToString());
					if (ViewState["fname"] != null) // indicates we have values from previous editing session which are not saved
					{
						// get elements from edit template
						TextBox tbF = (TextBox)row.FindControl("tbFirstName");
						TextBox tbL = (TextBox)row.FindControl("tbLastName");
						TextBox tbW = (TextBox)row.FindControl("tbWeb");
						TextBox tbE = (TextBox)row.FindControl("tbEmail");

						// restore previous values
						tbF.Text = ViewState["fname"].ToString();
						tbL.Text = ViewState["lname"].ToString();
						tbW.Text = ViewState["web"].ToString();
						tbE.Text = ViewState["email"].ToString();
					}
				}
			}
		}
		
		protected void bUpdate_Click(object sender, EventArgs e)
		{
			// Put data update code here
			TextBox tbF = (TextBox)GridView1.Rows[GridView1.EditIndex].FindControl("tbFirstName");
			TextBox tbL = (TextBox)GridView1.Rows[GridView1.EditIndex].FindControl("tbLastName");
			TextBox tbW = (TextBox)GridView1.Rows[GridView1.EditIndex].FindControl("tbWeb");
			TextBox tbE = (TextBox)GridView1.Rows[GridView1.EditIndex].FindControl("tbEmail");

			// now you have all values entered by user
			// set those values in dataset and update
			// or generate UPDATE sql statement here and save values in db.
			// more details / example can be found at 
			// http://msdn.microsoft.com/en-us/library/ms972948.aspx
			// OR
			// http://www.aspdotnetcodes.com/GridView_Insert_Edit_Update_Delete.aspx

			refreshData();
		}
		
		protected void bCancel_Click(object sender, EventArgs e)
		{
			// Put data Cancel code here
			refreshData();
		}
		
		protected void GridView1_PageIndexChanging(Object objDGName, GridViewPageEventArgs e)
		{
			
			GridView1.DataSource = CreateTable();
			GridView1.PageIndex = e.NewPageIndex;
			GridView1.EditIndex = -1;
			GridView1.DataBind();

			if (iCurrentEdit != -1)
			{
				GridView1_RowEditing(GridView1, new GridViewEditEventArgs(iCurrentEdit));
			}
		}
		
		protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow ||
				e.Row.RowType == DataControlRowType.EmptyDataRow)
			{
				int iEditRowID = int.Parse((ViewState["EditRowID"] == null ? "-2" : ViewState["EditRowID"].ToString()));
				if (iEditRowID != -2 && e.Row.DataItemIndex == iEditRowID)
				{
					iCurrentEdit = e.Row.RowIndex;
					
					//TextBox tbF = (TextBox)e.Row.FindControl("tbFirstName");
					//

					//ViewState.Add("fname", tbF.Text);
					//
				}
				else if (iEditRowID != -2)
				{
					ImageButton editButton;
					editButton = (ImageButton)(e.Row.Cells[1].Controls[0]);
					editButton.ImageUrl = "./edit_off.gif";
					editButton.Enabled = false;
				}
			}
		}

		private void refreshData()
		{
			GridView1.EditIndex = -1;
			ViewState["EditRowID"] = null;
			GridView1.DataSource = CreateTable();
			GridView1.DataBind();

			// clear viewstate
			ViewState["fname"] = null;
			ViewState["lname"] = null;
			ViewState["web"] = null;
			ViewState["email"] = null;
			ViewState["editRowIndex"] = null;
		}
	}
}// end class


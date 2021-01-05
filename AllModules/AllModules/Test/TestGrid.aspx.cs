using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
 


namespace AllModules.Test
{
    public partial class TestGrid : System.Web.UI.Page
    {
        public bool IsInEditMode
        {
            get
            {
                if (ViewState["IsEditMode"] == null)
                    return true;
                return (bool)ViewState["IsEditMode"];
            }
            set
            {
                ViewState["IsEditMode"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateGrid();
            }
            else
            {
                // update data values back into the datatable object
            }
        }
        public void populateGrid()
        {
            ViewState["Items"] = TestClass.CreateTable();
            TGrid.DataSource = ViewState["Items"];
            TGrid.DataBind();
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            IsInEditMode = false;
            populateGrid();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            DataTable ds = (DataTable)ViewState["Items"];
            int index = 0;
             
            if (ds != null)
            {
                if (ds.Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Rows)
                    {
                        index = Convert.ToInt32(dr["ID"]);
                    }
                }
                index += 1;
                DataRow drNewRow = ds.NewRow();
                drNewRow["ID"] = index;
                ds.Rows.Add(drNewRow);
                //ds.Tables[0].AcceptChanges();

                ViewState["Items"] = ds;
                TGrid.DataSource = ds;
                TGrid.DataBind();
                
            }
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class KeyNavExample : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid(10);
    }

    private void BindGrid(int rowcount)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new System.Data.DataColumn("Column1", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("Column2", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("Column3", typeof(String)));
        for (int i = 1; i < rowcount + 1; i++)
        {
            dr = dt.NewRow();
            dr[0] = "Row" + i.ToString() + " Col1"   ;
            dr[1] = "Row" + i.ToString() + " Col2";
            dr[2] = "Row" + i.ToString() + " Col3";
            dt.Rows.Add(dr);
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
}

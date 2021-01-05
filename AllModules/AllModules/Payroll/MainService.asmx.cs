using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using Payroll.BusLogic;
using AjaxControlToolkit;

namespace Payroll
{
    /// <summary>
    /// Summary description for MainService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MainService : System.Web.Services.WebService
    {

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetEmployeeOnDemand(string prefixText)
        {
            List<string> alist = new List<string>();
            try
            {
                Employee oEd = new Employee();
                DataSet ds = (DataSet)oEd.SearchEmployees(prefixText,null, null);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dRow in ds.Tables[0].Rows)
                    {
                        string item = AutoCompleteExtender.CreateAutoCompleteItem(dRow["EmployeeName"].ToString(), dRow["EmpID"].ToString());
                        alist.Add(item);
                    }
                }

            }
            catch (Exception ecx)
            {

            }
            return alist.ToArray();
        }

    }
}

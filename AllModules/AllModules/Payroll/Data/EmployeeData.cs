using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Payroll.Data
{   
    public class EmployeeData
    {
        public int      ID              { get; set; }
        public string   Designation     { get; set; }       
        public int      EmployeeProfile { get; set; }
        public string   FirstName       { get; set; }
        public string   MiddleName      { get; set; }
        public string   LastName        { get; set; }
        public bool     Gender          { get; set; }
        public bool     Married         { get; set; }
        public DateTime BirthDate       { get; set; }
        public DateTime JoiningDate     { get; set; }
        public int      RowVersion      { get; set; }
        public string   CreatedBy       { get; set; }
        public string   ModifiedBy      { get; set; }
        public bool     Active          { get; set; }
        public DateTime CreatedOn       { get; set; }
        public DateTime ModifiedOn      { get; set; }
        //Emptype is for display only no mapping with db
        public string   EmpTypeName     { get; set; }
        public int      FileID          { get; set; }        
        public string   PF              { get; set; }
        public string   ESIC            { get; set; }
        public string   PAN             { get; set; }
    }
    
}

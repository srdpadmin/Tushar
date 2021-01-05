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
    public class EmployeeDetailsData
    {
        public int      ID          { get; set; }
        public int      EmpID       { get; set; }        
        public string   AddressLine1    { get; set; }
        public string   AddressLine2    { get; set; }
        public string   City        { get; set; }
        public string   State       { get; set; }
        public string   Country     { get; set; }
        public string   PinCode     { get; set; }
        public string   HomePhone   { get; set; }
        public string   Mobile      { get; set; }
        public string   WorkPhone   { get; set; }        
        public string   CreatedBy   { get; set; }
        public string   ModifiedBy  { get; set; }
        public string   EmpTypeName { get; set; }
        public DateTime CreatedOn   { get; set; }
        public DateTime ModifiedOn  { get; set; }

    }
}

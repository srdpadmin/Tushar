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

namespace TermsConditions.Data
{
    public class MasterConditions
    {
        public MasterConditions() { }
        private int _conditionSerial;
        private string _condition;
        private DateTime _createdOn;

        public int ID
        {
            get { return _conditionSerial; }
            set { _conditionSerial = value; }
        }
        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        public DateTime ModifiedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }
    }
}

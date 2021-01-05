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
    public class MasterTerms
    {
        public MasterTerms() { }
        private int _termID;
        private string _termName;
        private DateTime _createdOn;

        public int ID
        {
            get { return _termID; }
            set { _termID = value; }
        }
        public string Term
        {
            get { return _termName; }
            set { _termName = value; }
        }

        public DateTime ModifiedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }
    }
}

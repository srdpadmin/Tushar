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
using Contact.BusLogic;

namespace Contact.Common
{
    public class WebCommon
    {

        public static object GetAllContactsFromCache()
        {
            if (HttpContext.Current.Cache["ContactCache"] == null)
            {
                Contacts v = new Contacts();
                HttpContext.Current.Cache["ContactCache"] = v.GetAllContacts();
            }
            return HttpContext.Current.Cache["ContactCache"];

        }
    }
}

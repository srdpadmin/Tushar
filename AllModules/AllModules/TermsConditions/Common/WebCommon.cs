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
using TermsConditions.BusLogic;

namespace TermsConditions.Common
{
    public class WebCommon
    {
        public static object GetAllTermsFromCache()
        {
            if (HttpContext.Current.Cache["TermsCache"] == null)
            {
                MasterTermConditions mtc = new MasterTermConditions();
                HttpContext.Current.Cache["TermsCache"] = mtc.GetAllTerms();
            }
            return HttpContext.Current.Cache["TermsCache"];

        }
        public static object GetAllConditionsFromCache()
        {
            if (HttpContext.Current.Cache["ConditionsCache"] == null)
            {
                MasterTermConditions mtc = new MasterTermConditions();
                HttpContext.Current.Cache["ConditionsCache"] = mtc.GetAllConditions();
            }
            return HttpContext.Current.Cache["ConditionsCache"];

        }
    }
}

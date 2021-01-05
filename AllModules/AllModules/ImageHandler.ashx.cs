using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using CoreAssemblies;
using Payroll.BusLogic;
using AllModules.Common;

namespace QOModule
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //string sql = "select FileID from Employee where ID=";
            if (context.Request.QueryString["id"] != null)
            {                
                DbFactory factory = new DbFactory(EnumClass.ModuleName.ACL);                
                Files files = new Files();
                string x = context.Request.QueryString["id"];
                FilesData fs = files.GetFilesData(x);
                if (fs != null && fs.OleAttach != null)
                {
                    context.Response.AddHeader("content-disposition", "attachment; filename=" + fs.FileName);
                    context.Response.ContentType = fs.FileType;
                    context.Response.BinaryWrite((byte[])fs.OleAttach);
                }
            }
              
            //http://www.mikesdotnetting.com/Article/123/Storing-Files-and-Images-in-Access-with-ASP.NET
                 
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

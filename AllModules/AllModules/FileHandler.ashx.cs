using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using AllModules.Common;

namespace AllModules
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FileHandler : IHttpHandler
    {
        
    
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            HttpResponse r = context.Response; 
            string fileID = context.Request.QueryString["FID"];
            
            // Get Data From Database. And write file.
            if (fileID != "0")
            {
                try
                {
                    Files files = new Files();
                    FilesData fileData = files.GetFilesData(fileID);
                    context.Response.Clear();
                    context.Response.AddHeader("content-disposition", "attachment; filename=" + fileData.FileName);
                    context.Response.ContentType = fileData.FileType;
                    context.Response.BinaryWrite((byte[])fileData.OleAttach);
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(ex.Message);
                }
                finally
                {
                    context.Response.End();
                }
            }
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

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
using System.Collections.Generic;
using System.IO;
namespace SalaryManagement
{

    public static class ErrorLog
    {
       public static void addError(string a, string b)
       {
           string c = a + b;
           InitializeErrorList();
           if (HttpContext.Current.Items["ErrorLog"] != null)
           {
               List<Errors> elist = (List<Errors>)HttpContext.Current.Items["ErrorLog"];
               elist.Add(new Errors(a, b));
               HttpContext.Current.Items["ErrorLog"] = elist;
               
               HttpContext.Current.Trace.Write(c);
           }
           
           AppendToFile(c);
       }
       public static void InitializeErrorList()
       {
           if (HttpContext.Current.Items["ErrorLog"] == null)
           {
               HttpContext.Current.Items["ErrorLog"] = new List<Errors>();
           }
       }

       public static List<Errors> getErrorList()
       {
           return (List<Errors>)HttpContext.Current.Items["ErrorLog"];

       }
       public static void AppendToFile(string err)
       {
        try
        {
            string drive = ConfigurationManager.AppSettings["DriveLetter"].ToString();
            string Directory = ConfigurationManager.AppSettings["ErrorDirectory"].ToString();
            string pathfile = drive + @":\" + Directory + @"\" + "ErrorLog.txt";  //ASP.NET D Server
            //string pathfile = drive + @":\" + Directory + @"" + "ErrorLog.txt";    //LOCALHOST
            pathfile.Replace(@"\\",@"\");  //ASP.NET D Server
            //pathfile.Replace(@"\\\", @"\"); //Localhost
            if (!File.Exists(pathfile))
            {
                File.Create(pathfile).Close();
            }
            using (StreamWriter w = File.AppendText(pathfile))
            {
                w.WriteLine("\r\nLog Entry : " + err);                              
                w.WriteLine("__________________________");
                w.Flush();
                w.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cannot Initialise Error Log. Errors cannot be recorded!");
        }
       }
            
    }

    public class Errors 
    {
        private string functionName;

        public string FunctionName
        {
            get { return functionName; }
            set { functionName = value; }
        }

        private string msg;
        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
        public Errors(string function,string Message)
        {
            msg = Message;
            functionName = function;
        }
        
    }

    public class ErrorODS
    {
        public ErrorODS() { }

        public List<Errors> getErrorList()
        {
            return ErrorLog.getErrorList();
        }
    }
  
}

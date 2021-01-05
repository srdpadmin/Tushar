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
using ENM = CoreAssemblies.EnumClass;
using CoreAssemblies;
using System.IO;
using System.Reflection;

namespace AllModules
{
    public class Validate
    {
        
        public static bool UserModuleAccess(int uID, ENM.Modules moduleToCheck)
        {
            string cacheKey = "ModuleRole_" + uID.ToString(); //Update the key with userID
            string sessionValue = string.Empty;
            string[] strArry;
            byte moduleBit = 0;
            byte rolesBit = 0;
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                sessionValue = HttpContext.Current.Cache[cacheKey].ToString();
                strArry = sessionValue.Split('|');
                moduleBit = Convert.ToByte(strArry[0]);
            }
            else
            {
                Authorization.BusLogic.UserModule um = new Authorization.BusLogic.UserModule();
                um.GetUserModuleRolesByID(uID, ref moduleBit, ref rolesBit);
                HttpContext.Current.Cache[cacheKey] = moduleBit.ToString() + "|" + rolesBit.ToString();
            }
            //long result = Convert.ToByte(ENM.Modules.Payroll) | Convert.ToByte(ENM.Modules.Quotation);
            return (moduleBit & Convert.ToByte(moduleToCheck)) == Convert.ToByte(moduleToCheck);
        }
        public static bool UserRoleAccess(int uID, ENM.Roles roleToCheck)
        {
            string cacheKey = "ModuleRole_"+ uID.ToString(); //Update the key with userID
            string sessionValue = string.Empty;
            string[] strArry;
            byte moduleBit = 0;
            byte rolesBit = 0;
         
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                sessionValue = HttpContext.Current.Cache[cacheKey].ToString();
                strArry = sessionValue.Split('|');
                rolesBit = Convert.ToByte(strArry[1]);
            }
            else
            {
                //Save to Session
                Authorization.BusLogic.UserModule um = new Authorization.BusLogic.UserModule();
                um.GetUserModuleRolesByID(uID, ref moduleBit, ref rolesBit);
                HttpContext.Current.Cache[cacheKey] = moduleBit.ToString() + "|" + rolesBit.ToString();
            }
            //long result = Convert.ToByte(ENM.Modules.Payroll) | Convert.ToByte(ENM.Modules.Quotation);
            return (rolesBit & Convert.ToByte(roleToCheck)) == Convert.ToByte(roleToCheck);
        }
        public static void ClearRoleModuleAccess(int uID)
        {
            string cacheKey = "ModuleRole_" + uID.ToString(); //Update the key with userID           
            HttpContext.Current.Cache.Remove(cacheKey);
        }
        public static void WriteToEventLog(Exception e, string method)
        {

            string path = ConfigurationManager.AppSettings["LogFilePath"].ToString();

            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {

                    sw.WriteLine(method + ":" + e.Message);
                    sw.WriteLine();
                }
            }
            else
            {
                // This text is always added, making the file longer over time 
                // if it is not deleted. 
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(method + ":" + e.Message);
                    sw.WriteLine();
                }
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreAssemblies;
using System.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace Enquiry.BusLogic
{
    public class EnquiryTerm
    {
         private string sql;
         public EnquiryTerm() 
        {              
             
        }

         public DataSet GetEnquiryTerms(string EnquiryID)
        {

            sql = "select * From EnquiryTerm where EnquiryID=";

            if (string.IsNullOrEmpty(EnquiryID))
            {
                sql += "0";
            }
            else
            {
                sql += EnquiryID;
            }

            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }
    }
    }

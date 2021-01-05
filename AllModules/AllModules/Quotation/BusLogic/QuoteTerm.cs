using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreAssemblies;
using System.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace Quotation.BusLogic
{
    public class QuoteTerm
    {
         private string sql;
        public QuoteTerm() 
        {              
             
        }

        public DataSet GetQuoteTerms(string OrderID)
        {
            
            sql ="select * From QuoteTerm where QuoteID=";
            DbFactory factory =null;
              DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(OrderID))
            {
                sql += "0";
            }
            else
            {
                sql += OrderID;
            }
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);          
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
            }
            catch (System.Data.Common.DbException exc)
            {
                AllModules.Errors.LogError(exc);
            }	
            catch (Exception exc)
            {
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return ds;
        }
    }
    }

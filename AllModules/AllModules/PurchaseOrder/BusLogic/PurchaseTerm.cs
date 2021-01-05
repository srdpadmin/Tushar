using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreAssemblies;
using System.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace PurchaseOrder.BusLogic
{
    public class PurchaseTerm
    {
         private string sql;
        public PurchaseTerm() 
        {              
             
        }

        public DataSet GetPurchaseTerms(string OrderID)
        {
            
            sql ="select * From PurchaseTerm where PurchaseID=";
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
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.PurchaseOrder);          
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

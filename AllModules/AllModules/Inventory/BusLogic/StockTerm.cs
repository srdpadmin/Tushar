using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreAssemblies;
using System.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace Inventory.BusLogic
{
    public class StockTerm
    {
         private string sql;
         public StockTerm() 
        {              
             
        }

         public DataSet GetStockTerms(string OrderID)
        {

            sql = "select * From StockTerm where StockID=";
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
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);          
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

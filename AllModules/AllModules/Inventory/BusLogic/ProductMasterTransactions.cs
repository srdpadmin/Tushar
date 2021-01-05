using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASM=CoreAssemblies;
using CoreAssemblies;
using ENM=CoreAssemblies.EnumClass;
using System.Data;
using System.Data.Common;
using Inventory.Data;
using AllModules;
 
 

namespace Inventory.BusLogic
{
    public class ProductMasterTransactions
    {
        public int InsertProductMasterTransaction(ProductMasterTransactionsData pmt)
        {
            int x = 0;
            ProductMasterTransactionsData pmCurrent = new ProductMasterTransactionsData();
            pmCurrent.ProductMasterID = pmt.ProductMasterID;
            GetCurrentBalance(pmCurrent);           
            pmt.Balance = pmCurrent.Balance + pmt.Credit - pmt.Debit;

            string insertSql = ASM.Core.SetClassPropertiesValuesToSql(pmt, "INSERT", "ProductMasterTransactions");
            
            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Inventory);
                factory.UseTransaction = true;
                x = factory.RunCommand(insertSql);
                factory.CommitTransaction();
            }
            catch (System.Data.Common.DbException exc)
            {
                Errors.LogError(exc);
                x = -1;
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
                x = -1;
            }
            finally
            {
                factory.Close();
            }

            return x;
        }

        public DataTable GetProductMasterTransactions(int ProductMasterID)
        {           
            string sql = "SELECT * from ProductMasterTransactions  where  ProductMasterID = ";
            if (ProductMasterID == 0)
            {
                sql += " 0";
            }
            else
            {
                sql += ProductMasterID;
            }
            sql += " Order by CreatedOn Desc";

            CoreAssemblies.DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
            }
            catch (System.Data.Common.DbException exc)
            {
                Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return ds.Tables[0];

        }

        public void GetCurrentBalance(ProductMasterTransactionsData pmt)
        {

            string sql;
            sql = "select top 1 * from ProductMasterTransactions where ProductMasterID =" + pmt.ProductMasterID;
            
            CoreAssemblies.DbFactory factory = null;
            try
            {
                  
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                DbDataReader reader = factory.GetDataReader(sql);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pmt.ProductMasterID = CoreAssemblies.Core.ResolveInt(reader["ProductMasterID"]) != null ? Convert.ToInt32(reader["ProductMasterID"]) : 0;
                        pmt.ItemCode = CoreAssemblies.Core.ResolveString(reader["Code"]);
                        pmt.Credit = CoreAssemblies.Core.ResolveSingle(reader["Credit"]) != null ? Convert.ToSingle(reader["Credit"]) : 0;
                        pmt.Debit = CoreAssemblies.Core.ResolveSingle(reader["Debit"]) != null ? Convert.ToSingle(reader["Debit"]) : 0;
                        pmt.Balance = CoreAssemblies.Core.ResolveSingle(reader["Balance"]) != null ? Convert.ToSingle(reader["Balance"]) : 0;
                        pmt.TransactionID = CoreAssemblies.Core.ResolveInt(reader["TransactionID"])!= null ? Convert.ToInt32(reader["TransactionID"]) : 0;
                        pmt.TransactionType = CoreAssemblies.Core.ResolveString(reader["TransactionType"]);
                       
                    }
                    reader.Close();
                }
            }
            catch (System.Data.Common.DbException exc)
            {
                Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
        }
        
    }
}

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
using ASM = CoreAssemblies;
using System.Collections;
using System.Collections.Generic;
using Billing.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace Billing.BusLogic
{
    public class BillItemDetails
    {
        public DataSet GetBilling()
        {
             ASM.DbFactory factory=null;
            DataSet ds = new DataSet();
            string sql = "SELECT tp.Id, tp.TrackId, tp.WorkBill, tp.Challan, tp.Quantity, tp.CreatedBy, tp.TransactionTypeID, ";
            sql += "(select Top 1 (Description) from TransactionType TT  where TT.ID=tp.TransactionTypeID) AS [Transaction], ";
            sql += "tp.ReturnableTypeID, (select Top 1 (Description) from MaterialType MT  where MT.ID=tp.ReturnableTypeID) AS Returnable, ";
            sql += "tp.StatusTypeID, (select Top 1 (Description) from StatusType ST  where ST.ID=tp.StatusTypeID) AS Status FROM Billing AS tp; ";

            try
            {
            factory= new CoreAssemblies.DbFactory(ENM.ModuleName.Billing);            
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
        public ArrayList GetTrackIDS()
        {
            string sql = "SELECT distinct TrackID from Bill";
            ASM.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Billing);
            DbDataReader dbreader = null;
            ArrayList ar = new ArrayList();
            ar.Add("New Track ID");
            try
            {
                dbreader = factory.GetDataReader(sql);
                if (dbreader.HasRows)
                {

                    while (dbreader.Read())
                    {
                        ar.Add(dbreader["TrackID"].ToString());
                    }
                }
                dbreader.Close();
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
            return ar;

        }
        public int INSERT(BillItemDetails Billing)
        {

            string query = ASM.Core.SetClassPropertiesValuesToSql(Billing, "INSERT", "Bill");
            string query2 = "Select @@Identity";
            string[] queries = new string[] { query, query2 };
             ASM.DbFactory factory=null;
            int[] ids = new int[2];
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Billing);
                ids = factory.Runcommands(queries);
            }
            catch (System.Data.Common.DbException exc)
            {
                ids[1] = -1;
                AllModules.Errors.LogError(exc);
            }	
             catch (Exception exc)
            {
                 ids[1] = -1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
                
            
            return ids[1];
        }
        public int UPDATE(BillItemDetails Billing)
        {
            string query = ASM.Core.SetClassPropertiesValuesToSql(Billing, "UPDATE", "Bill");
            //query =   query.Replace("Id=" + Billing.Id.ToString() + ",", "");
            int id = -1;
             ASM.DbFactory factory=null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Billing);
                id = factory.RunCommand(query);
            }
            catch (System.Data.Common.DbException exc)
            {
                id  = -1;
                AllModules.Errors.LogError(exc);
            }	
            catch (Exception exc)
            {
                id=-1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return id;
        }

        public IDictionary<int, string> Getenums(string enumTable)
        {
            string sql = "SELECT * from " + enumTable;
            ASM.DbFactory factory = null;
            DbDataReader dbreader = null;
            Dictionary<int, string> dict = new Dictionary<int, string>();

            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Billing);
                dbreader = factory.GetDataReader(sql);
                if (dbreader.HasRows)
                {
                    while (dbreader.Read())
                    {
                        dict.Add(Convert.ToInt32(dbreader["ID"]), dbreader["Description"].ToString());
                    }
                }
                dbreader.Close();
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
            return dict;

        }
    }
}

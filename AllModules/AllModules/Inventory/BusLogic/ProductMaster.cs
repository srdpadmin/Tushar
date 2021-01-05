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
using System.Data.OleDb;
using System.Collections.Generic;
using ASM = CoreAssemblies;
using Inventory.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;
using System.Collections;
using System.Text;

namespace Inventory.BusLogic
{
    public class ProductMaster
    {
        private string sql;
        public Dictionary<string, string> GetProductMasterWithId()
        {
             
            Dictionary<string, string> dict = new Dictionary<string, string>();
            sql = "SELECT P.*,(select U.UserName from [Users] as U where P.CreatedBy=U.UserID) as CreatedByName from ProductMaster P ";
            ASM.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
            DbDataReader reader =factory.GetDataReader(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                   
                   dict.Add(reader["ID"].ToString(), reader["Code"].ToString());               

                };
            }
            
            return dict;
        }

        public DataSet GetAllProducts()
        {
            sql = "Select P.*,(select U.UserName from [Users] as U where P.CreatedBy=U.UserID) as CreatedByName,";
            sql += "(Select top 1 Balance from ProductMasterTransactions where P.ID =ProductMasterID Order By CreatedOn DESC) as BalanceValue ";
            sql+= " from ProductMaster P";
            ASM.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
            DataSet ds = new DataSet();
            try
            {
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
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

        public DataSet GetProductMasterForCache(string prefixText)
        {            
            sql = "select Code,Description,Unit,Rate,Balance as Quantity,Discount,Tax From ProductMaster where Code Like '%" + prefixText + "%'";
            ASM.DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
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

        public DataSet GetProductMasterByDescriptionFromCache(string prefixText)
        {
            sql = "select Code,Description,Unit,Rate,Balance as Quantity,Discount,Tax From ProductMaster where Description Like '%" + prefixText + "%' Order by Description";
            ASM.DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
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

        

        #region CRUD
        public int InsertProductMaster(int userID)
        {
            string[] sql = new string[2];
            //string query = "Insert into ProductMaster(Code,ModifiedOn,CreatedBy,CreatedOn) values(?,?,?,?)";
            ASM.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
            ProductMasterData pmd = new ProductMasterData();
            pmd.Code = "TestCode";
            pmd.Description = "Please Use Correct Code";
            pmd.CreatedBy = userID;             
            pmd.CreatedOn = DateTime.Now;
            pmd.ModifiedOn = DateTime.Now;
            

            string query = ASM.Core.SetClassPropertiesValuesToSql(pmd, "INSERT", "ProductMaster");
            string query1 = "Select @@Identity";
            sql[0] = query;
            sql[1]= query1;
            int[] ID = new int[2];
            try
            {
                ID=factory.RuncommandsWithTransaction(sql, true, null);
                //using (OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BillingDbProvider"].ToString()))
                //{
                //    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                //    {

                //        cmd.Parameters.AddWithValue("Code", Code);
                //        cmd.Parameters.AddWithValue("ModifiedOn", DateTime.Parse(DateTime.Now.ToString()));
                //        cmd.Parameters.AddWithValue("CreatedBy", userID);
                //        cmd.Parameters.AddWithValue("CreatedOn", DateTime.Parse(DateTime.Now.ToString()));
                //        conn.Open();
                //        cmd.ExecuteNonQuery();
                //        cmd.CommandText = query2;
                //        ID = (int)cmd.ExecuteScalar();
                //    }
                //}
            }
            catch (Exception exc)
            {
                ID[1] = -1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return ID[1];
        }
        public int UpdateProductMaster(ProductMasterData pmd)
        {
            sql =ASM.Core.SetClassPropertiesValuesToSql(pmd, "UPDATE", "ProductMaster");

            //string query = "Update ProductMaster SET Code=? ,Description=? ,Balance=? ,Type=? ,Unit=?,Rate=? ,Location=? where ID=" + ProdId.ToString();
            ////string query = "Insert into Blogs(Title,UserID) values(@blogTitle,@CreatedById)";
            //string query2 = "Select @@Identity";
            sql = sql.Replace("Balance=" + pmd.Balance + ",", "");
            int ID = 0;
            ASM.DbFactory factory = null;
            try
            {
                  factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                ID=factory.RunCommand(sql);

                //using (OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BillingDbProvider"].ToString()))
                //{
                //    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                //    {

                //        cmd.Parameters.AddWithValue("Code", Code);
                //        cmd.Parameters.AddWithValue("Description", Description);
                //        cmd.Parameters.AddWithValue("Balance", Balance);
                //        cmd.Parameters.AddWithValue("Type", Type);
                //        cmd.Parameters.AddWithValue("Unit", Unit);
                //        cmd.Parameters.AddWithValue("Rate", Rate);
                //        cmd.Parameters.AddWithValue("Location", Location);
                //        conn.Open();
                //        cmd.ExecuteNonQuery();
                //        cmd.CommandText = query2;
                //        ID = cmd.ExecuteNonQuery();
                //    }
                //}
            }
            catch (Exception exc)
            {
                ID = -1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return ID;
        }
        public int DeleteProductMaster(int catId)
        {
            sql= "Delete from ProductMaster where ID=" + catId.ToString();
            //string query = "Insert into Blogs(Title,UserID) values(@blogTitle,@CreatedById)";
            //string query2 = "Select @@Identity";
            int ID = 0;
            ASM.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                ID = factory.RunCommand(sql);

                //using (OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["BillingDbProvider"].ToString()))
                //{
                //    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                //    {
                //        conn.Open();
                //        cmd.ExecuteNonQuery();
                //        cmd.CommandText = query2;
                //        ID = cmd.ExecuteNonQuery();
                //    }
                //}
            }
            catch (Exception exc)
            {
                ID = -1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return ID;
        }
        #endregion
    }
}

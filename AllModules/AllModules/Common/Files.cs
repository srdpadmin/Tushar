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
using System.Data.Common;
using System.Data.OleDb;
using CoreAssemblies;

namespace AllModules.Common
{
    public class Files
    {
        public byte[] GetFileStream(string fileID)
        {
            string sql;
             FilesData fsData = new FilesData();
            sql = "select * from Files where ID =" + fileID;

            CoreAssemblies.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CoreAssemblies.Core.SetClassProperties(fsData, reader);
                }
            }
            reader.Close();
            factory.Close();
            return fsData.OleAttach;
        }
        public FilesData GetFilesData(string fileID)
        {
            string sql;
            FilesData fsData = new FilesData();
            if (fileID != string.Empty)
            {
                sql = "select * from Files where ID =" + fileID;
            }
            else
            {
                sql = "select * from Files where ID =0";
            }

            CoreAssemblies.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CoreAssemblies.Core.SetClassProperties(fsData, reader);
                }
            }
            
            reader.Close();
            factory.Close();
            return fsData;
        }

        public string GetFileName(string fileID)
        {
            string sql;
            string fileName = string.Empty;
            if (fileID != string.Empty)
            {
                sql = "select FileName from Files where ID =" + fileID;
            }
            else
            {
                sql = "select FileName from Files where ID =0";
            }

            CoreAssemblies.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    fileName = reader["FileName"].ToString();
                    break;
                }
            }

            reader.Close();
            factory.Close();
            return fileName;
        }
        public int InsertNewFile(FilesData fs)
        {             
            string query = "Insert into Files(FileName,FileType,FileSize,CreatedOn,OleAttach) values (?,?,?,?,?)";
            //string query = "Insert into Blogs(Title,UserID) values(@blogTitle,@CreatedById)"; ,Type,Size
            string query2 = "Select @@Identity";
            int ID = 0;
            OleDbConnection conn = null;
            try
            {

                conn = new OleDbConnection(DbFactory.AccessConnectionStringName);
                 
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {                        
                    cmd.Parameters.AddWithValue("FileName", fs.FileName);
                    cmd.Parameters.AddWithValue("FileType", fs.FileType);
                    cmd.Parameters.AddWithValue("FileSize", fs.FileSize);
                    cmd.Parameters.AddWithValue("CreatedOn", DateTime.Parse(DateTime.Now.ToString()));
                    cmd.Parameters.AddWithValue("OleAttach", fs.OleAttach);
                    //cmd.Parameters.AddWithValue("Attachment", Attachment);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = query2;
                    ID = (int)cmd.ExecuteScalar();
                }
                
            }
            catch (Exception ex)
            {
                //do something with ex.Message

            }
            finally
            {
                conn.Close();
            }
            return ID;
        }
        public int ReplaceFile(FilesData fs,int fileID)
        {
            int id = 0;
            string query = "Update Files SET FileName=?,FileType=?,FileSize=?,CreatedOn=?, OleAttach=? where ID=" + fileID;
            //string query = "Update Files SET FileName=@FileName,FileType=@FileType,FileSize=@FileSize,CreatedOn=@CreatedOn,OleAttach=@OleAttach where PostID=@PostID";
            //  string query = "Insert into Blogs(Title,UserID) values(@blogTitle,@CreatedById)"; ,Type,Size
            CoreAssemblies.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
            try
            {
            Object[] values = new Object[5];
            values[0] = fs.FileName;
            values[1] = fs.FileType;
            values[2] = fs.FileSize;
            values[3] = fs.CreatedOn;
            values[4] = fs.OleAttach;
            
            string[] names = new string[5];
            names[0] = "FileName";
            names[1] = "FileType";
            names[2] = "FileSize";
            names[3] = "CreatedOn";
            names[4] = "OleAttach";

             factory = new CoreAssemblies.DbFactory(ENM.ModuleName.ACL);
             id = factory.RunCommandWithParametersList(query, names, values);
             
            //OleDbConnection conn = null;
            //int ID = 0;
            //try
            //{

            //    conn = new OleDbConnection(DbFactory.PayrollConnectionStringName);
                 
            //        using (OleDbCommand cmd = new OleDbCommand(query, conn))
            //        {
            //            cmd.Parameters.AddWithValue("FileName", fs.FileName);
            //            //cmd.Parameters.AddWithValue("FileType", fs.FileType);
            //            cmd.Parameters.AddWithValue("FileSize", fs.FileSize);
            //            cmd.Parameters.AddWithValue("CreatedOn", DateTime.Parse(DateTime.Now.ToString()));
            //            cmd.Parameters.AddWithValue("OleAttach", fs.OleAttach);
            //            //cmd.Parameters.AddWithValue("Attachment", Attachment);
            //            conn.Open();
            //            ID = cmd.ExecuteNonQuery();

            //        }
                 
            }
            catch (Exception ex)
            {
                //do something with ex.Message
                 
            }
            finally
            {
                factory.Close();
            }
            return id;
        }
    }
}

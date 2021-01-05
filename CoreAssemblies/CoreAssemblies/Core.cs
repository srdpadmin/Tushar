using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

namespace CoreAssemblies
{
    public class Core
    {
        /// <summary>
        /// This class will set all the properties in the collection (class) that is passed in)
        /// </summary>   
        public static void SetClassProperties(object cls, DbDataReader oSQLDR)
        {
            //Add all the column names to array in order to check if returned in resultset
            ArrayList arrColumnsNames = new ArrayList();
            Nullable<System.DateTime> dteDate = default(Nullable<System.DateTime>);
            for (int iCol = 0; iCol <= oSQLDR.FieldCount - 1; iCol++)
            {
                //Added toUpper due to values been case sensitive
                arrColumnsNames.Add(oSQLDR.GetName(iCol).ToUpper());
            }

            //Iterate through all the properties and set the ones that are returned in the resultset
            foreach (System.Reflection.PropertyInfo propInfo in cls.GetType().GetProperties())
            {
                //Check that array list of column names contains the property
                //Added toUpper due to values been case sensitive

                if (arrColumnsNames.Contains(propInfo.Name.ToUpper()))
                {
                    //Check that the property is not readonly

                    if (propInfo.CanWrite)
                    {
                        // Check if value returned is null or is nothing


                        if ((oSQLDR[propInfo.Name] != null) & (!object.ReferenceEquals(oSQLDR[propInfo.Name], DBNull.Value)))
                        {
                            if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.String)))
                            {
                                propInfo.SetValue(cls, Convert.ToString(ResolveString(oSQLDR[propInfo.Name])), null);

                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.Int32)))
                            {
                                propInfo.SetValue(cls, Convert.ToInt32(ResolveInt(oSQLDR[propInfo.Name])), null);

                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.Decimal)))
                            {
                                propInfo.SetValue(cls, Convert.ToDecimal(String.Format("{##0.00}", ResolveDecimal(oSQLDR[propInfo.Name]))), null);

                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.Double)))
                            {
                                propInfo.SetValue(cls, Convert.ToDouble(String.Format("{##0.00}", ResolveDouble(oSQLDR[propInfo.Name]))), null);
                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.Single)))
                            {
                                 propInfo.SetValue(cls, Convert.ToSingle(String.Format("{0:0.00##}",ResolveSingle(oSQLDR[propInfo.Name]))),null);
                                //propInfo.SetValue(cls, float.Parse(ResolveSingle(oSQLDR[propInfo.Name]),String.Format("{##0.00}" )));
                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.Boolean)))
                            {
                                propInfo.SetValue(cls, Convert.ToBoolean(ResolveInt(oSQLDR[propInfo.Name])), null);


                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.DateTime)))
                            {

                                //Convert the date back from UTC to Local time where required
                                //'propInfo.SetValue(cls, CDate(ResolveDate(oSQLDR[propInfo.Name))), Nothing)
                                dteDate = ResolveDate(oSQLDR[propInfo.Name]);
                                //var _with1 = dteDate;
                                //if (_with1.HasValue)
                                //    dteDate = ConvertUTCDateToLocalDate(_with1.Value);
                                propInfo.SetValue(cls,dteDate, null);

                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.Byte[])))
                            {
                                propInfo.SetValue(cls, oSQLDR[propInfo.Name], null);
                            }

                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(System.Nullable<DateTime>)))
                            {

                                //Convert the date back from UTC to Local time where required
                                dteDate = ResolveDate(oSQLDR[propInfo.Name]);
                                //var _with2 = dteDate;
                                //if (_with2.HasValue)
                                //    dteDate = ConvertUTCDateToLocalDate(_with2.Value);
                                propInfo.SetValue(cls, Convert.ToDateTime(dteDate), null);


                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(Nullable<int>)))
                            {
                                propInfo.SetValue(cls, Convert.ToInt32(ResolveInt(oSQLDR[propInfo.Name])), null);

                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(Nullable<bool>)))
                            {
                                propInfo.SetValue(cls, Convert.ToBoolean(ResolveInt(oSQLDR[propInfo.Name])), null);

                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(Nullable<decimal>)))
                            {
                                propInfo.SetValue(cls, Convert.ToDecimal(string.Format("{##0.00}", ResolveDecimal(oSQLDR[propInfo.Name]))), null);

                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(Nullable<double>)))
                            {
                                propInfo.SetValue(cls, Convert.ToDouble(string.Format("{##0.00}", ResolveDouble(oSQLDR[propInfo.Name]))), null);
                            }
                            else if (object.ReferenceEquals(propInfo.PropertyType, typeof(Nullable<float>)))
                            {
                                propInfo.SetValue(cls, Convert.ToSingle(String.Format("{0:0.00##}", ResolveSingle(oSQLDR[propInfo.Name]))), null);

                            }
                        }
                        else
                        {
                            propInfo.SetValue(cls, null, null);

                        }
                    }

                }
            }
        }

        /// <summary>
        /// This method is useful when you do not have ID, and during a transaction you intend
        /// to pass the generated id as a parameter.
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="typeOfSql"></param>
        /// <param name="tableName"></param>
        /// <param name="xProperties"></param>
        /// <param name="addPropAsParam"></param>
        /// <returns></returns>
        public static string SetClassPropertiesValuesToSqlUpdated
            (object cls, string typeOfSql, string tableName,ArrayList xProperties,string addPropAsParam)
        {

            //Add all the column names to array in order to check if returned in resultset
            ArrayList arrColumnsNames = new ArrayList();
            ArrayList arrColumnValues = new ArrayList();
            Hashtable typehash = new Hashtable();
            //Nullable<System.DateTime> dteDate = default(Nullable<System.DateTime>);

            string sql = string.Empty;
            string id = string.Empty;
            if (xProperties == null)
            {
                xProperties = new ArrayList();
                xProperties.Add("ID");
            }

            //Iterate through all the properties and create sql out of it depending upon the clause passed
            foreach (System.Reflection.PropertyInfo propInfo in cls.GetType().GetProperties())
            {
                if (propInfo.GetValue(cls, null) != null && !xProperties.Contains(propInfo.Name))
                {
                    arrColumnsNames.Add(propInfo.Name);
                    switch (propInfo.PropertyType.Name)
                    {
                        case "String": arrColumnValues.Add("'" + propInfo.GetValue(cls, null) + "'");
                            break;
                        case "Int32": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Double": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Single": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Decimal": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Boolean": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "DateTime": arrColumnValues.Add("'" + propInfo.GetValue(cls, null) + "'");
                            break;
                    }
                }
                else if (propInfo.GetValue(cls, null) != null && addPropAsParam == propInfo.Name)
                {
                    arrColumnsNames.Add(propInfo.Name);
                    arrColumnValues.Add("@" + addPropAsParam);
                }
                else if (propInfo.GetValue(cls, null) != null && propInfo.Name.ToLower() == "id")
                {
                    id = propInfo.GetValue(cls, null).ToString();
                }
            }
            StringBuilder columnNames = new StringBuilder();
            StringBuilder columnValues = new StringBuilder();
            StringBuilder update = new StringBuilder();
            for (int i = 0; i < arrColumnsNames.Count; i++)
            {
                switch (typeOfSql)
                {
                    case "INSERT": columnNames.Append(arrColumnsNames[i] + ",");
                        columnValues.Append(arrColumnValues[i] + ",");
                        break;
                    case "UPDATE":
                        update.Append(arrColumnsNames[i].ToString() + "=" + arrColumnValues[i].ToString() + ",");
                        break;
                }
            }

            switch (typeOfSql)
            {
                case "INSERT":
                    columnNames.Remove(columnNames.Length - 1, 1);
                    columnValues.Remove(columnValues.Length - 1, 1);
                    sql = "INSERT INTO [" + tableName + "] (";
                    sql += columnNames.ToString() + ") VALUES (";
                    sql += columnValues.ToString() + ");";
                    break;
                case "UPDATE":
                    update.Remove(update.Length - 1, 1);
                    sql = "Update [" + tableName + "] SET ";
                    sql += update.ToString();
                    sql += " where ID=" + id + ";";
                    break;
            }

            return sql;
        }

        public static string SetClassPropertiesValuesToSql(object cls,string typeOfSql, string tableName)
        {

            //Add all the column names to array in order to check if returned in resultset
            ArrayList arrColumnsNames = new ArrayList();
            ArrayList arrColumnValues = new ArrayList();
            Hashtable typehash = new Hashtable();
            //Nullable<System.DateTime> dteDate = default(Nullable<System.DateTime>);

            string sql = string.Empty;
            string id = string.Empty;
          
            //Iterate through all the properties and create sql out of it depending upon the clause passed
            foreach (System.Reflection.PropertyInfo propInfo in cls.GetType().GetProperties())
            {
                if (propInfo.GetValue(cls, null) != null & propInfo.Name.ToLower() != "id")
                {
                    arrColumnsNames.Add(propInfo.Name);
                    switch (propInfo.PropertyType.Name)
                    {
                        case "String": arrColumnValues.Add("'" + propInfo.GetValue(cls, null) + "'");   
                            break;
                        case "Int32": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Int64": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Double": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Single": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;                         
                        case "Decimal": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "Boolean": arrColumnValues.Add(propInfo.GetValue(cls, null));
                            break;
                        case "DateTime": arrColumnValues.Add("'" + propInfo.GetValue(cls, null) + "'"); 
                            break;
                    }
                                                      
                }
                else if(propInfo.GetValue(cls, null) != null & propInfo.Name.ToLower() == "id")
                {
                    id= propInfo.GetValue(cls, null).ToString();                   
                }
            }
            StringBuilder columnNames = new StringBuilder();
            StringBuilder columnValues = new StringBuilder();
            StringBuilder update = new StringBuilder();
            for (int i = 0; i < arrColumnsNames.Count; i++)
            {
                switch (typeOfSql)
                {
                    case "INSERT": columnNames.Append(arrColumnsNames[i] + ",");
                        if(arrColumnValues[i] != null)
                        columnValues.Append(arrColumnValues[i] + ",");
                        break;
                    case "UPDATE":
                        if (arrColumnValues[i] != null)
                        update.Append(arrColumnsNames[i].ToString() + "=" + arrColumnValues[i].ToString()+ ",");
                        break;
                }
            }
            
            switch (typeOfSql)
            {
                case "INSERT":
                    columnNames.Remove(columnNames.Length - 1, 1);
                    columnValues.Remove(columnValues.Length - 1, 1);
                    sql = "INSERT INTO [" + tableName + "] (";
                    sql += columnNames.ToString() + ") VALUES (";
                    sql += columnValues.ToString() + ");";
                    break;
                case "UPDATE": 
                    update.Remove(update.Length - 1, 1);
                    sql = "Update [" + tableName + "] SET ";
                    sql += update.ToString();
                    sql += " where ID=" + id + ";";  
                    break;
            }

            return sql;
        }

        public static ArrayList GetSQLFromDataTableOld(object cls, DataTable DT, string tableName)
        {

            //Add all the column names to array in order to check if returned in resultset
            ArrayList arrColumnsNames = new ArrayList();
            int index = 0;
            string sql = string.Empty;
           
 
            //Iterate through all the properties and create sql out of it depending upon the clause passed
            foreach (System.Reflection.PropertyInfo propInfo in cls.GetType().GetProperties())
            { 
                    arrColumnsNames.Add(propInfo.Name);                                    
            }
            //DataView dv = new DataView(DT, "", "", DataViewRowState.Deleted);
            DataTable deletedTable = DT.GetChanges(DataRowState.Deleted);
            DataTable modifiedTable = DT.GetChanges(DataRowState.Modified);
            DataTable insertedTable = DT.GetChanges(DataRowState.Added);
            ArrayList returnRows = new ArrayList();
            if (deletedTable != null)
            {
                foreach (DataRow dr in deletedTable.Rows)
                {                     
                    sql = "Delete from " + tableName + " where ID=" + dr[0, DataRowVersion.Original].ToString();
                    returnRows.Add(sql);
                    
                }
            }

            if (modifiedTable != null)
            {
                foreach (DataRow dr in modifiedTable.Rows)
                {
                    sql = "UPDATE " + tableName + " SET ";
                    index = 0;
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        if (arrColumnsNames.Contains(dc.ColumnName.ToString()))
                        {
                            sql += dc.ColumnName.ToString() + "=" + EvaluateDataTypeValue(dc.DataType.Name, dr[dc].ToString());
                        }
                        index += 1;
                    }
                    sql = sql.Remove(sql.Length - 1, 1);
                    returnRows.Add(sql);
                }
            }
            if (insertedTable != null)
            {
                string columnNames = string.Empty;
                string columnValues = string.Empty;
                foreach (DataRow dr in insertedTable.Rows)
                {
                    sql = "INSERT INTO " + tableName + "(";
                    index=0;
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        if (arrColumnsNames.Contains(dc.ColumnName.ToString())) 
                        {
                            sql += dc.ColumnName.ToString() + "=" + EvaluateDataTypeValue(dc.DataType.Name, dr[dc].ToString());
                         if((dc.ColumnName.ToString() != "ID") && !string.IsNullOrEmpty(dr[dc].ToString()))
                         {
                             columnNames += dc.ColumnName.ToString() + ",";
                             columnValues += EvaluateDataTypeValue(dc.DataType.Name, dr[dc].ToString());
                         }                           
                        }
                        index+=1;
                    }
                    columnNames = columnNames.Remove(columnNames.Length - 1, 1);
                    columnValues = columnValues.Remove(columnValues.Length - 1, 1);
                    sql += columnNames + ")";
                    sql += "VALUES (" + columnValues + ")";
                    returnRows.Add(sql);
                }
            }
            return returnRows;
        }

        public static ArrayList GetSQLFromDataTable(object cls, DataTable DT, string tableName)
        {

            //Add all the column names to array in order to check if returned in resultset
            ArrayList arrColumnsNames = new ArrayList();
            int index = 0;
            string sql = string.Empty;
            int defaultValue = 0;

            //Iterate through all the properties and create sql out of it depending upon the clause passed
            foreach (System.Reflection.PropertyInfo propInfo in cls.GetType().GetProperties())
            {
                arrColumnsNames.Add(propInfo.Name);
            }
            //DataView dv = new DataView(DT, "", "", DataViewRowState.Deleted);
            DataTable deletedTable = DT.GetChanges(DataRowState.Deleted);
            DataTable modifiedTable = DT.GetChanges(DataRowState.Modified);
            DataTable insertedTable = DT.GetChanges(DataRowState.Added);
            ArrayList returnRows = new ArrayList();
            if (deletedTable != null)
            {
                foreach (DataRow dr in deletedTable.Rows)
                {
                    sql = "Delete from " + tableName + " where ID=" + dr[0, DataRowVersion.Original].ToString();
                    returnRows.Add(sql);                    
                }
            }

            if (modifiedTable != null)
            {
                foreach (DataRow dr in modifiedTable.Rows)
                {
                    sql = "UPDATE " + tableName + " SET ";
                    string orderId = string.Empty;
                    string billId = string.Empty;
                    string quoteId = string.Empty;
                    string id = string.Empty;
                    string enquiryId = string.Empty;
                    string stockId = string.Empty;
                    //string referenceId = string.Empty;
                    index = 0;
                    foreach (DataColumn dc in dr.Table.Columns)
                    {                        
                        if (arrColumnsNames.Contains(dc.ColumnName.ToString()))
                        {
                            switch (dc.ColumnName.ToString().ToLower())
                            {
                                // this should be taken care using parameter array
                                // Get the list of elements name to exclude during query creation
                                // we are doing it this way to make it a common code
                                // used if else instead of switch to collect list of values for them
                                case "purchaseid": orderId = dr[dc].ToString();
                                    break;
                                case "id": id = dr[dc].ToString();
                                    break;
                                case "billid": billId = dr[dc].ToString();
                                    break;
                                case "quoteid": quoteId = dr[dc].ToString();
                                    break;
                                case "enquiryid":enquiryId = dr[dc].ToString();
                                    break;
                                case "stockId": stockId = dr[dc].ToString();
                                    break;
                                //case "referenceid": referenceId = dr[dc].ToString();
                                //    break;
                                default:
                                    if(dr[dc].ToString() != string.Empty)
                                        sql += dc.ColumnName.ToString() + "=" + EvaluateDataTypeValue(dc.DataType.Name, dr[dc].ToString());                                                       
                                    break;
                            }                            
                        }
                        index += 1;
                    }
                    sql = sql.Remove(sql.Length - 1, 1);
                    sql += " where id=" + id;
                    if (orderId != string.Empty)
                    {
                        sql += " And PurchaseID=" + orderId;
                    }
                    else if (billId != string.Empty)
                    {
                        sql += " And BillID=" + billId;
                    }
                    else if (quoteId != string.Empty)
                    {
                        sql += " And QuoteID=" + quoteId;
                    }
                    else if (enquiryId != string.Empty)
                    {
                        sql += " And EnquiryId=" + enquiryId;
                    }
                    else if (stockId != string.Empty)
                    {
                        sql += " And StockId=" + stockId;
                    }
                    //else if (referenceId != string.Empty)
                    //{
                    //    sql += " And ReferenceId=" + stockId;
                    //}

                    returnRows.Add(sql);
                }
            }
            if (insertedTable != null)
            {
                string columnNames = string.Empty;
                string columnValues = string.Empty;
                foreach (DataRow dr in insertedTable.Rows)
                {
                    sql = "INSERT INTO " + tableName + "(";
                    index = 0;
                    foreach (DataColumn dc in dr.Table.Columns)
                    {                       
                        if (arrColumnsNames.Contains(dc.ColumnName.ToString()))
                        {
                            switch (dc.ColumnName.ToString().ToLower())
                            {
                                // This is case of insert, so orderid,billid,enquiryid will always be null/0
                                // And RuncommandsWithTransaction will fill the value for this parameter
                                case "id": break;
                                case "purchaseid" :

                                    Int32.TryParse(dr[dc].ToString(), out defaultValue);
                                    if (defaultValue == 0)
                                    {
                                        columnNames += dc.ColumnName.ToString() + ",";
                                        columnValues += "@PurchaseID" + " ,";
                                    }
                                    else
                                    {
                                        columnNames += dc.ColumnName.ToString() + ",";
                                        columnValues += dr[dc].ToString() + " ,";
                                    }
                                        break;
                                case "billid":
                                        Int32.TryParse(dr[dc].ToString(), out defaultValue);
                                        if (defaultValue == 0)
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += "@BillID" + " ,";
                                        }
                                        else
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += dr[dc].ToString() + " ,";
                                        }
                                        break;
                                case "quoteid":
                                        Int32.TryParse(dr[dc].ToString(), out defaultValue);
                                        if (defaultValue == 0)
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += "@QuoteID" + " ,";
                                        }
                                        else
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += dr[dc].ToString() + " ,";
                                        }
                                        break;
                                case "stockid":
                                        Int32.TryParse(dr[dc].ToString(), out defaultValue);
                                        if (defaultValue == 0)
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += "@StockID" + " ,";
                                        }
                                        else
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += dr[dc].ToString() + " ,";
                                        }
                                        break;
                                case "enquiryid":
                                        Int32.TryParse(dr[dc].ToString(), out defaultValue);
                                        if (defaultValue == 0)
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += "@EnquiryID" + " ,";
                                        }
                                        else
                                        {
                                            columnNames += dc.ColumnName.ToString() + ",";
                                            columnValues += dr[dc].ToString() + " ,";
                                        }
                                        break;

                                //case "referenceid":
                                //        Int32.TryParse(dr[dc].ToString(), out defaultValue);
                                //        if (defaultValue == 0)
                                //        {
                                //            columnNames += dc.ColumnName.ToString() + ",";
                                //            columnValues += "@ReferenceID" + " ,";
                                //        }
                                //        else
                                //        {
                                //            columnNames += dc.ColumnName.ToString() + ",";
                                //            columnValues += dr[dc].ToString() + " ,";
                                //        }
                                //        break;
                                default :
                                        
                                        if(!string.IsNullOrEmpty(dr[dc].ToString()))
                                        {
                                        columnNames += dc.ColumnName.ToString() + ",";
                                        columnValues += EvaluateDataTypeValue(dc.DataType.Name, dr[dc].ToString());
                                        }
                                        break;
                            }                       
                        }
                        index += 1;
                    }
                    //Correction if the row is empty, should not allow to insert
                    if (columnNames.Length > 0)
                    {
                        columnNames = columnNames.Remove(columnNames.Length - 1, 1);
                        columnValues = columnValues.Remove(columnValues.Length - 1, 1);
                        sql += columnNames + ")";
                        sql += "VALUES (" + columnValues + ")";
                        returnRows.Add(sql);
                        columnNames = string.Empty;
                        columnValues = string.Empty;
                    }
                    
                }
            }
            return returnRows;
        }

        public static string EvaluateDataTypeValue(string DataColumnTypeName,string value)
        {
            string sql = string.Empty;
            switch (DataColumnTypeName)
            {
                case "String": sql = "'" + value + "' ,";
                    break;
                case "Int32": sql = value + " ,";
                    break;
                case "Double": sql = value + " ,";
                    break;
                case "Single": sql = value + " ,";
                    break;
                case "Decimal": sql = value + " ,";
                    break;
                case "Boolean": sql = value + " ,";
                    break;
                case "DateTime": sql = "'" + value + "' ,";
                    break;
            }
            return sql;
        }

        /// <summary>
        /// This class will set all the properties in the collection (class) that is passed in)
        /// </summary>
        public static void SetClassPropertiesFromClass(ref object oDestinationClass, ref object oOriginalClass)
        {

            System.Reflection.PropertyInfo[] oOriginalClassProperties = oOriginalClass.GetType().GetProperties();
            System.Type oDestinationClassType = oDestinationClass.GetType();


            try
            {
                if (oOriginalClassProperties == null | oDestinationClassType.GetProperties() == null)
                {
                    return;
                }

                foreach (System.Reflection.PropertyInfo oOriginalClassProperty in oOriginalClassProperties)
                {
                    System.Reflection.PropertyInfo oDestinationClassProperty = oDestinationClassType.GetProperty(oOriginalClassProperty.Name);


                    if (oDestinationClassProperty != null && oDestinationClassProperty.Name != "DefaultRole")
                    {
                        if (oDestinationClassProperty.PropertyType.IsAssignableFrom(oOriginalClassProperty.PropertyType) == true & oDestinationClassProperty.CanWrite == true)
                        {
                            oDestinationClassProperty.SetValue(oDestinationClass, oOriginalClassProperty.GetValue(oOriginalClass, null), null);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string QuoteTheQuote(string sValue)
        {
            return sValue.Replace("'", "''");
        }

        public static Nullable<DateTime> ResolveDate(object dteDateTime)
        {

            //************************************************************
            //Name:          ResolveDate       

            //Purpose:       If the past in parameter is NULL return nothing, else return a value of type Date  
            //Returns:       A Date  
            //*************************************************************

            if (object.ReferenceEquals(dteDateTime, System.DBNull.Value))
            {
                return null;
            }
            else if (object.ReferenceEquals(dteDateTime, String.Empty))
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(dteDateTime);
            }

        }

        public static DateTime ResolveDate(string dteDateTime)
        {

            //************************************************************
            //Name:          ResolveDate       

            //Purpose:       If the past in parameter is NULL return nothing, else return a value of type Date  
            //Returns:       A Date  
            //*************************************************************
            //CultureInfo.InvariantCulture
            //new CultureInfo("en-GB")
            
            if (object.ReferenceEquals(dteDateTime, String.Empty))
            {
                return DateTime.MinValue;
            }
            else
            {
                string[] dates = dteDateTime.Split(' ');
                return DateTime.Parse(dates[0], new CultureInfo("en-GB"));
            }

        }
        public static string ResolveDateToUK(DateTime dteDateTime)
        {            
            CultureInfo gb = CultureInfo.GetCultureInfo("en-GB");
            string gbDate = string.Empty;
            if (object.ReferenceEquals(dteDateTime, String.Empty))
            {
                gbDate= DateTime.Now.ToString(gb.DateTimeFormat);
            }
            else
            {
                gbDate = dteDateTime.ToString(gb.DateTimeFormat);  
            }
            return gbDate;
        }

        public static DateTime ResolveDateToUS(string gbDate)
        {
            CultureInfo us = CultureInfo.GetCultureInfo("en-GB");
            DateTime usDate;
            if (object.ReferenceEquals(gbDate, String.Empty))
            {
                usDate = DateTime.MinValue;
            }
            else
            {
                usDate = DateTime.Parse(gbDate, us.DateTimeFormat);
            }
            //Return a UK date which will automatically get convert to US date
            return usDate;
        }

        public static Nullable<int> ResolveInt(object intVal)
        {

            //************************************************************
            //Name:          ResolveInt       
            //Purpose:       If the past in parameter is NULL return nothing, else return a value of type Int  
            //Returns:       An Integer 
            //*************************************************************

            if (object.ReferenceEquals(intVal, System.DBNull.Value))
            {
                return null;
            }            
            else
            {
                return Convert.ToInt32(intVal);
            }


        }

        public static Nullable<Int64> ResolveLong(object intVal)
        {

            //************************************************************
            //Name:          ResolveInt           

            //Purpose:       If the past in parameter is NULL return nothing, else return a value of type Int  
            //Returns:       An Long Integer
            //*************************************************************

            if (object.ReferenceEquals(intVal, System.DBNull.Value))
            {
                return null;
            }
            else
            {
                return (Int64)intVal;
            }


        }

        public static Nullable<decimal> ResolveDecimal(object decVal)
        {

            //************************************************************
            //Name:          ResolveDecimal         

            //Purpose:       If the past in parameter is NULL return 0 (decimal types are generally used to process money values, so appropiate we return 0 here) , else return a value of type decimal  
            //Returns:       A decimal  
            //*************************************************************

            if (object.ReferenceEquals(decVal, System.DBNull.Value))
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(decVal);
            }

        }

        public static Nullable<bool> ResolveBoolean(object boolVal)
        {

            //************************************************************
            //Name:          ResolveBoolean         

            //Purpose:       If the past in parameter is NULL return false, else return a value of type boolean 
            //Returns:       A boolean 
            //*************************************************************

            if (object.ReferenceEquals(boolVal, System.DBNull.Value))
            {
                return null;
            }
            else
            {
                return Convert.ToBoolean(boolVal);
            }

        }

        public static Nullable<double> ResolveDouble(object dblVal)
        {

            //************************************************************
            //Name:          ResolveDouble          

            //Purpose:       If the past in parameter is NULL return 0, else return a value of type double
            //Returns:       A double 
            //*************************************************************

            if (object.ReferenceEquals(dblVal, System.DBNull.Value))
            {
                return null;
            }
            else
            {
                return Convert.ToDouble(dblVal);
            }

        }

        public static Nullable<float> ResolveSingle(object dblVal)
        {

            //************************************************************
            //Name:          ResolveDouble          

            //Purpose:       If the past in parameter is NULL return 0, else return a value of type double
            //Returns:       A double 
            //*************************************************************

            if (object.ReferenceEquals(dblVal, System.DBNull.Value))
            {
                return null;
            }
            else
            {
                return Convert.ToSingle(dblVal);
            }

        }

        public static string ResolveString(object lngVal)
        {
            string functionReturnValue = null;

            //************************************************************
            //Name:          ResolveString / was g_IsNull
            //Purpose:       If the value enter is null or empty return a blank
            //Returns:       A string

            //Parameters:    IngVal -  Variant
            //*************************************************************

            if (object.ReferenceEquals(lngVal, DBNull.Value))
            {
                functionReturnValue = "";
            }
            else if (string.IsNullOrEmpty(Convert.ToString(lngVal)))
            {
                functionReturnValue = "";
            }
            else
            {
                functionReturnValue = Convert.ToString(lngVal).TrimEnd();// String.RTrim(Convert.ToString(lngVal));
            }
            return functionReturnValue;

        }

        public static void getEnumStatusTypes()
        {

            //List<EnumObject> lstEnum = new List<EnumObject>();
            ////Array arr = System.Enum.GetValues(typeof(EnumClass.StatusType));
            //string[] arr = Enum.GetNames(typeof(EnumClass.StatusType));
            //for (int i = 0; i <= (arr.Length - 1); i++)
            //{
            //    EnumObject clsEnum = new EnumObject();
            //    clsEnum.EnumDisplay = arr[0];                
            //    clsEnum.EnumValue = int.Parse(arr[i]);
            //    lstEnum.Add(clsEnum);
            //}
            //return lstEnum;
        }
        

        #region Big freaking list of mime types
        // combination of values from Windows 7 Registry and 
        // from C:\Windows\System32\inetsrv\config\applicationHost.config
        // some added, including .7z and .dat
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
        {".323", "text/h323"},
        {".3g2", "video/3gpp2"},
        {".3gp", "video/3gpp"},
        {".3gp2", "video/3gpp2"},
        {".3gpp", "video/3gpp"},
        {".7z", "application/x-7z-compressed"},
        {".aa", "audio/audible"},
        {".AAC", "audio/aac"},
        {".aaf", "application/octet-stream"},
        {".aax", "audio/vnd.audible.aax"},
        {".ac3", "audio/ac3"},
        {".aca", "application/octet-stream"},
        {".accda", "application/msaccess.addin"},
        {".accdb", "application/msaccess"},
        {".accdc", "application/msaccess.cab"},
        {".accde", "application/msaccess"},
        {".accdr", "application/msaccess.runtime"},
        {".accdt", "application/msaccess"},
        {".accdw", "application/msaccess.webapplication"},
        {".accft", "application/msaccess.ftemplate"},
        {".acx", "application/internet-property-stream"},
        {".AddIn", "text/xml"},
        {".ade", "application/msaccess"},
        {".adobebridge", "application/x-bridge-url"},
        {".adp", "application/msaccess"},
        {".ADT", "audio/vnd.dlna.adts"},
        {".ADTS", "audio/aac"},
        {".afm", "application/octet-stream"},
        {".ai", "application/postscript"},
        {".aif", "audio/x-aiff"},
        {".aifc", "audio/aiff"},
        {".aiff", "audio/aiff"},
        {".air", "application/vnd.adobe.air-application-installer-package+zip"},
        {".amc", "application/x-mpeg"},
        {".application", "application/x-ms-application"},
        {".art", "image/x-jg"},
        {".asa", "application/xml"},
        {".asax", "application/xml"},
        {".ascx", "application/xml"},
        {".asd", "application/octet-stream"},
        {".asf", "video/x-ms-asf"},
        {".ashx", "application/xml"},
        {".asi", "application/octet-stream"},
        {".asm", "text/plain"},
        {".asmx", "application/xml"},
        {".aspx", "application/xml"},
        {".asr", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".atom", "application/atom+xml"},
        {".au", "audio/basic"},
        {".avi", "video/x-msvideo"},
        {".axs", "application/olescript"},
        {".bas", "text/plain"},
        {".bcpio", "application/x-bcpio"},
        {".bin", "application/octet-stream"},
        {".bmp", "image/bmp"},
        {".c", "text/plain"},
        {".cab", "application/octet-stream"},
        {".caf", "audio/x-caf"},
        {".calx", "application/vnd.ms-office.calx"},
        {".cat", "application/vnd.ms-pki.seccat"},
        {".cc", "text/plain"},
        {".cd", "text/plain"},
        {".cdda", "audio/aiff"},
        {".cdf", "application/x-cdf"},
        {".cer", "application/x-x509-ca-cert"},
        {".chm", "application/octet-stream"},
        {".class", "application/x-java-applet"},
        {".clp", "application/x-msclip"},
        {".cmx", "image/x-cmx"},
        {".cnf", "text/plain"},
        {".cod", "image/cis-cod"},
        {".config", "application/xml"},
        {".contact", "text/x-ms-contact"},
        {".coverage", "application/xml"},
        {".cpio", "application/x-cpio"},
        {".cpp", "text/plain"},
        {".crd", "application/x-mscardfile"},
        {".crl", "application/pkix-crl"},
        {".crt", "application/x-x509-ca-cert"},
        {".cs", "text/plain"},
        {".csdproj", "text/plain"},
        {".csh", "application/x-csh"},
        {".csproj", "text/plain"},
        {".css", "text/css"},
        {".csv", "text/csv"},
        {".cur", "application/octet-stream"},
        {".cxx", "text/plain"},
        {".dat", "application/octet-stream"},
        {".datasource", "application/xml"},
        {".dbproj", "text/plain"},
        {".dcr", "application/x-director"},
        {".def", "text/plain"},
        {".deploy", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dgml", "application/xml"},
        {".dib", "image/bmp"},
        {".dif", "video/x-dv"},
        {".dir", "application/x-director"},
        {".disco", "text/xml"},
        {".dll", "application/x-msdownload"},
        {".dll.config", "text/xml"},
        {".dlm", "text/dlm"},
        {".doc", "application/msword"},
        {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        {".dot", "application/msword"},
        {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
        {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
        {".dsp", "application/octet-stream"},
        {".dsw", "text/plain"},
        {".dtd", "text/xml"},
        {".dtsConfig", "text/xml"},
        {".dv", "video/x-dv"},
        {".dvi", "application/x-dvi"},
        {".dwf", "drawing/x-dwf"},
        {".dwp", "application/octet-stream"},
        {".dxr", "application/x-director"},
        {".eml", "message/rfc822"},
        {".emz", "application/octet-stream"},
        {".eot", "application/octet-stream"},
        {".eps", "application/postscript"},
        {".etl", "application/etl"},
        {".etx", "text/x-setext"},
        {".evy", "application/envoy"},
        {".exe", "application/octet-stream"},
        {".exe.config", "text/xml"},
        {".fdf", "application/vnd.fdf"},
        {".fif", "application/fractals"},
        {".filters", "Application/xml"},
        {".fla", "application/octet-stream"},
        {".flr", "x-world/x-vrml"},
        {".flv", "video/x-flv"},
        {".fsscript", "application/fsharp-script"},
        {".fsx", "application/fsharp-script"},
        {".generictest", "application/xml"},
        {".gif", "image/gif"},
        {".group", "text/x-ms-group"},
        {".gsm", "audio/x-gsm"},
        {".gtar", "application/x-gtar"},
        {".gz", "application/x-gzip"},
        {".h", "text/plain"},
        {".hdf", "application/x-hdf"},
        {".hdml", "text/x-hdml"},
        {".hhc", "application/x-oleobject"},
        {".hhk", "application/octet-stream"},
        {".hhp", "application/octet-stream"},
        {".hlp", "application/winhlp"},
        {".hpp", "text/plain"},
        {".hqx", "application/mac-binhex40"},
        {".hta", "application/hta"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".htt", "text/webviewhtml"},
        {".hxa", "application/xml"},
        {".hxc", "application/xml"},
        {".hxd", "application/octet-stream"},
        {".hxe", "application/xml"},
        {".hxf", "application/xml"},
        {".hxh", "application/octet-stream"},
        {".hxi", "application/octet-stream"},
        {".hxk", "application/xml"},
        {".hxq", "application/octet-stream"},
        {".hxr", "application/octet-stream"},
        {".hxs", "application/octet-stream"},
        {".hxt", "text/html"},
        {".hxv", "application/xml"},
        {".hxw", "application/octet-stream"},
        {".hxx", "text/plain"},
        {".i", "text/plain"},
        {".ico", "image/x-icon"},
        {".ics", "application/octet-stream"},
        {".idl", "text/plain"},
        {".ief", "image/ief"},
        {".iii", "application/x-iphone"},
        {".inc", "text/plain"},
        {".inf", "application/octet-stream"},
        {".inl", "text/plain"},
        {".ins", "application/x-internet-signup"},
        {".ipa", "application/x-itunes-ipa"},
        {".ipg", "application/x-itunes-ipg"},
        {".ipproj", "text/plain"},
        {".ipsw", "application/x-itunes-ipsw"},
        {".iqy", "text/x-ms-iqy"},
        {".isp", "application/x-internet-signup"},
        {".ite", "application/x-itunes-ite"},
        {".itlp", "application/x-itunes-itlp"},
        {".itms", "application/x-itunes-itms"},
        {".itpc", "application/x-itunes-itpc"},
        {".IVF", "video/x-ivf"},
        {".jar", "application/java-archive"},
        {".java", "application/octet-stream"},
        {".jck", "application/liquidmotion"},
        {".jcz", "application/liquidmotion"},
        {".jfif", "image/pjpeg"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpb", "application/octet-stream"},
        {".jpe", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".jsx", "text/jscript"},
        {".jsxbin", "text/plain"},
        {".latex", "application/x-latex"},
        {".library-ms", "application/windows-library+xml"},
        {".lit", "application/x-ms-reader"},
        {".loadtest", "application/xml"},
        {".lpk", "application/octet-stream"},
        {".lsf", "video/x-la-asf"},
        {".lst", "text/plain"},
        {".lsx", "video/x-la-asf"},
        {".lzh", "application/octet-stream"},
        {".m13", "application/x-msmediaview"},
        {".m14", "application/x-msmediaview"},
        {".m1v", "video/mpeg"},
        {".m2t", "video/vnd.dlna.mpeg-tts"},
        {".m2ts", "video/vnd.dlna.mpeg-tts"},
        {".m2v", "video/mpeg"},
        {".m3u", "audio/x-mpegurl"},
        {".m3u8", "audio/x-mpegurl"},
        {".m4a", "audio/m4a"},
        {".m4b", "audio/m4b"},
        {".m4p", "audio/m4p"},
        {".m4r", "audio/x-m4r"},
        {".m4v", "video/x-m4v"},
        {".mac", "image/x-macpaint"},
        {".mak", "text/plain"},
        {".man", "application/x-troff-man"},
        {".manifest", "application/x-ms-manifest"},
        {".map", "text/plain"},
        {".master", "application/xml"},
        {".mda", "application/msaccess"},
        {".mdb", "application/x-msaccess"},
        {".mde", "application/msaccess"},
        {".mdp", "application/octet-stream"},
        {".me", "application/x-troff-me"},
        {".mfp", "application/x-shockwave-flash"},
        {".mht", "message/rfc822"},
        {".mhtml", "message/rfc822"},
        {".mid", "audio/mid"},
        {".midi", "audio/mid"},
        {".mix", "application/octet-stream"},
        {".mk", "text/plain"},
        {".mmf", "application/x-smaf"},
        {".mno", "text/xml"},
        {".mny", "application/x-msmoney"},
        {".mod", "video/mpeg"},
        {".mov", "video/quicktime"},
        {".movie", "video/x-sgi-movie"},
        {".mp2", "video/mpeg"},
        {".mp2v", "video/mpeg"},
        {".mp3", "audio/mpeg"},
        {".mp4", "video/mp4"},
        {".mp4v", "video/mp4"},
        {".mpa", "video/mpeg"},
        {".mpe", "video/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpf", "application/vnd.ms-mediapackage"},
        {".mpg", "video/mpeg"},
        {".mpp", "application/vnd.ms-project"},
        {".mpv2", "video/mpeg"},
        {".mqv", "video/quicktime"},
        {".ms", "application/x-troff-ms"},
        {".msi", "application/octet-stream"},
        {".mso", "application/octet-stream"},
        {".mts", "video/vnd.dlna.mpeg-tts"},
        {".mtx", "application/xml"},
        {".mvb", "application/x-msmediaview"},
        {".mvc", "application/x-miva-compiled"},
        {".mxp", "application/x-mmxp"},
        {".nc", "application/x-netcdf"},
        {".nsc", "video/x-ms-asf"},
        {".nws", "message/rfc822"},
        {".ocx", "application/octet-stream"},
        {".oda", "application/oda"},
        {".odc", "text/x-ms-odc"},
        {".odh", "text/plain"},
        {".odl", "text/plain"},
        {".odp", "application/vnd.oasis.opendocument.presentation"},
        {".ods", "application/oleobject"},
        {".odt", "application/vnd.oasis.opendocument.text"},
        {".one", "application/onenote"},
        {".onea", "application/onenote"},
        {".onepkg", "application/onenote"},
        {".onetmp", "application/onenote"},
        {".onetoc", "application/onenote"},
        {".onetoc2", "application/onenote"},
        {".orderedtest", "application/xml"},
        {".osdx", "application/opensearchdescription+xml"},
        {".p10", "application/pkcs10"},
        {".p12", "application/x-pkcs12"},
        {".p7b", "application/x-pkcs7-certificates"},
        {".p7c", "application/pkcs7-mime"},
        {".p7m", "application/pkcs7-mime"},
        {".p7r", "application/x-pkcs7-certreqresp"},
        {".p7s", "application/pkcs7-signature"},
        {".pbm", "image/x-portable-bitmap"},
        {".pcast", "application/x-podcast"},
        {".pct", "image/pict"},
        {".pcx", "application/octet-stream"},
        {".pcz", "application/octet-stream"},
        {".pdf", "application/pdf"},
        {".pfb", "application/octet-stream"},
        {".pfm", "application/octet-stream"},
        {".pfx", "application/x-pkcs12"},
        {".pgm", "image/x-portable-graymap"},
        {".pic", "image/pict"},
        {".pict", "image/pict"},
        {".pkgdef", "text/plain"},
        {".pkgundef", "text/plain"},
        {".pko", "application/vnd.ms-pki.pko"},
        {".pls", "audio/scpls"},
        {".pma", "application/x-perfmon"},
        {".pmc", "application/x-perfmon"},
        {".pml", "application/x-perfmon"},
        {".pmr", "application/x-perfmon"},
        {".pmw", "application/x-perfmon"},
        {".png", "image/png"},
        {".pnm", "image/x-portable-anymap"},
        {".pnt", "image/x-macpaint"},
        {".pntg", "image/x-macpaint"},
        {".pnz", "image/png"},
        {".pot", "application/vnd.ms-powerpoint"},
        {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
        {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
        {".ppa", "application/vnd.ms-powerpoint"},
        {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
        {".ppm", "image/x-portable-pixmap"},
        {".pps", "application/vnd.ms-powerpoint"},
        {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
        {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
        {".ppt", "application/vnd.ms-powerpoint"},
        {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
        {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
        {".prf", "application/pics-rules"},
        {".prm", "application/octet-stream"},
        {".prx", "application/octet-stream"},
        {".ps", "application/postscript"},
        {".psc1", "application/PowerShell"},
        {".psd", "application/octet-stream"},
        {".psess", "application/xml"},
        {".psm", "application/octet-stream"},
        {".psp", "application/octet-stream"},
        {".pub", "application/x-mspublisher"},
        {".pwz", "application/vnd.ms-powerpoint"},
        {".qht", "text/x-html-insertion"},
        {".qhtm", "text/x-html-insertion"},
        {".qt", "video/quicktime"},
        {".qti", "image/x-quicktime"},
        {".qtif", "image/x-quicktime"},
        {".qtl", "application/x-quicktimeplayer"},
        {".qxd", "application/octet-stream"},
        {".ra", "audio/x-pn-realaudio"},
        {".ram", "audio/x-pn-realaudio"},
        {".rar", "application/octet-stream"},
        {".ras", "image/x-cmu-raster"},
        {".rat", "application/rat-file"},
        {".rc", "text/plain"},
        {".rc2", "text/plain"},
        {".rct", "text/plain"},
        {".rdlc", "application/xml"},
        {".resx", "application/xml"},
        {".rf", "image/vnd.rn-realflash"},
        {".rgb", "image/x-rgb"},
        {".rgs", "text/plain"},
        {".rm", "application/vnd.rn-realmedia"},
        {".rmi", "audio/mid"},
        {".rmp", "application/vnd.rn-rn_music_package"},
        {".roff", "application/x-troff"},
        {".rpm", "audio/x-pn-realaudio-plugin"},
        {".rqy", "text/x-ms-rqy"},
        {".rtf", "application/rtf"},
        {".rtx", "text/richtext"},
        {".ruleset", "application/xml"},
        {".s", "text/plain"},
        {".safariextz", "application/x-safari-safariextz"},
        {".scd", "application/x-msschedule"},
        {".sct", "text/scriptlet"},
        {".sd2", "audio/x-sd2"},
        {".sdp", "application/sdp"},
        {".sea", "application/octet-stream"},
        {".searchConnector-ms", "application/windows-search-connector+xml"},
        {".setpay", "application/set-payment-initiation"},
        {".setreg", "application/set-registration-initiation"},
        {".settings", "application/xml"},
        {".sgimb", "application/x-sgimb"},
        {".sgml", "text/sgml"},
        {".sh", "application/x-sh"},
        {".shar", "application/x-shar"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".sitemap", "application/xml"},
        {".skin", "application/xml"},
        {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
        {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
        {".slk", "application/vnd.ms-excel"},
        {".sln", "text/plain"},
        {".slupkg-ms", "application/x-ms-license"},
        {".smd", "audio/x-smd"},
        {".smi", "application/octet-stream"},
        {".smx", "audio/x-smd"},
        {".smz", "audio/x-smd"},
        {".snd", "audio/basic"},
        {".snippet", "application/xml"},
        {".snp", "application/octet-stream"},
        {".sol", "text/plain"},
        {".sor", "text/plain"},
        {".spc", "application/x-pkcs7-certificates"},
        {".spl", "application/futuresplash"},
        {".src", "application/x-wais-source"},
        {".srf", "text/plain"},
        {".SSISDeploymentManifest", "text/xml"},
        {".ssm", "application/streamingmedia"},
        {".sst", "application/vnd.ms-pki.certstore"},
        {".stl", "application/vnd.ms-pki.stl"},
        {".sv4cpio", "application/x-sv4cpio"},
        {".sv4crc", "application/x-sv4crc"},
        {".svc", "application/xml"},
        {".swf", "application/x-shockwave-flash"},
        {".t", "application/x-troff"},
        {".tar", "application/x-tar"},
        {".tcl", "application/x-tcl"},
        {".testrunconfig", "application/xml"},
        {".testsettings", "application/xml"},
        {".tex", "application/x-tex"},
        {".texi", "application/x-texinfo"},
        {".texinfo", "application/x-texinfo"},
        {".tgz", "application/x-compressed"},
        {".thmx", "application/vnd.ms-officetheme"},
        {".thn", "application/octet-stream"},
        {".tif", "image/tiff"},
        {".tiff", "image/tiff"},
        {".tlh", "text/plain"},
        {".tli", "text/plain"},
        {".toc", "application/octet-stream"},
        {".tr", "application/x-troff"},
        {".trm", "application/x-msterminal"},
        {".trx", "application/xml"},
        {".ts", "video/vnd.dlna.mpeg-tts"},
        {".tsv", "text/tab-separated-values"},
        {".ttf", "application/octet-stream"},
        {".tts", "video/vnd.dlna.mpeg-tts"},
        {".txt", "text/plain"},
        {".u32", "application/octet-stream"},
        {".uls", "text/iuls"},
        {".user", "text/plain"},
        {".ustar", "application/x-ustar"},
        {".vb", "text/plain"},
        {".vbdproj", "text/plain"},
        {".vbk", "video/mpeg"},
        {".vbproj", "text/plain"},
        {".vbs", "text/vbscript"},
        {".vcf", "text/x-vcard"},
        {".vcproj", "Application/xml"},
        {".vcs", "text/plain"},
        {".vcxproj", "Application/xml"},
        {".vddproj", "text/plain"},
        {".vdp", "text/plain"},
        {".vdproj", "text/plain"},
        {".vdx", "application/vnd.ms-visio.viewer"},
        {".vml", "text/xml"},
        {".vscontent", "application/xml"},
        {".vsct", "text/xml"},
        {".vsd", "application/vnd.visio"},
        {".vsi", "application/ms-vsi"},
        {".vsix", "application/vsix"},
        {".vsixlangpack", "text/xml"},
        {".vsixmanifest", "text/xml"},
        {".vsmdi", "application/xml"},
        {".vspscc", "text/plain"},
        {".vss", "application/vnd.visio"},
        {".vsscc", "text/plain"},
        {".vssettings", "text/xml"},
        {".vssscc", "text/plain"},
        {".vst", "application/vnd.visio"},
        {".vstemplate", "text/xml"},
        {".vsto", "application/x-ms-vsto"},
        {".vsw", "application/vnd.visio"},
        {".vsx", "application/vnd.visio"},
        {".vtx", "application/vnd.visio"},
        {".wav", "audio/wav"},
        {".wave", "audio/wav"},
        {".wax", "audio/x-ms-wax"},
        {".wbk", "application/msword"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wcm", "application/vnd.ms-works"},
        {".wdb", "application/vnd.ms-works"},
        {".wdp", "image/vnd.ms-photo"},
        {".webarchive", "application/x-safari-webarchive"},
        {".webtest", "application/xml"},
        {".wiq", "application/xml"},
        {".wiz", "application/msword"},
        {".wks", "application/vnd.ms-works"},
        {".WLMP", "application/wlmoviemaker"},
        {".wlpginstall", "application/x-wlpg-detect"},
        {".wlpginstall3", "application/x-wlpg3-detect"},
        {".wm", "video/x-ms-wm"},
        {".wma", "audio/x-ms-wma"},
        {".wmd", "application/x-ms-wmd"},
        {".wmf", "application/x-msmetafile"},
        {".wml", "text/vnd.wap.wml"},
        {".wmlc", "application/vnd.wap.wmlc"},
        {".wmls", "text/vnd.wap.wmlscript"},
        {".wmlsc", "application/vnd.wap.wmlscriptc"},
        {".wmp", "video/x-ms-wmp"},
        {".wmv", "video/x-ms-wmv"},
        {".wmx", "video/x-ms-wmx"},
        {".wmz", "application/x-ms-wmz"},
        {".wpl", "application/vnd.ms-wpl"},
        {".wps", "application/vnd.ms-works"},
        {".wri", "application/x-mswrite"},
        {".wrl", "x-world/x-vrml"},
        {".wrz", "x-world/x-vrml"},
        {".wsc", "text/scriptlet"},
        {".wsdl", "text/xml"},
        {".wvx", "video/x-ms-wvx"},
        {".x", "application/directx"},
        {".xaf", "x-world/x-vrml"},
        {".xaml", "application/xaml+xml"},
        {".xap", "application/x-silverlight-app"},
        {".xbap", "application/x-ms-xbap"},
        {".xbm", "image/x-xbitmap"},
        {".xdr", "text/plain"},
        {".xht", "application/xhtml+xml"},
        {".xhtml", "application/xhtml+xml"},
        {".xla", "application/vnd.ms-excel"},
        {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
        {".xlc", "application/vnd.ms-excel"},
        {".xld", "application/vnd.ms-excel"},
        {".xlk", "application/vnd.ms-excel"},
        {".xll", "application/vnd.ms-excel"},
        {".xlm", "application/vnd.ms-excel"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
        {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".xlt", "application/vnd.ms-excel"},
        {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
        {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
        {".xlw", "application/vnd.ms-excel"},
        {".xml", "text/xml"},
        {".xmta", "application/xml"},
        {".xof", "x-world/x-vrml"},
        {".XOML", "text/plain"},
        {".xpm", "image/x-xpixmap"},
        {".xps", "application/vnd.ms-xpsdocument"},
        {".xrm-ms", "text/xml"},
        {".xsc", "application/xml"},
        {".xsd", "text/xml"},
        {".xsf", "text/xml"},
        {".xsl", "text/xml"},
        {".xslt", "text/xml"},
        {".xsn", "application/octet-stream"},
        {".xss", "application/xml"},
        {".xtp", "application/octet-stream"},
        {".xwd", "image/x-xwindowdump"},
        {".z", "application/x-compress"},
        {".zip", "application/x-zip-compressed"},
       

        };

        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        } 
        #endregion
    }
}
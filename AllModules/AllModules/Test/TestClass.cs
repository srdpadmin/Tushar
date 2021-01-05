using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AllModules.Test
{
    public static class TestClass
    {
        public static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn ID = new DataColumn("ID");
            DataColumn fName = new DataColumn("FirstName");
            DataColumn lName = new DataColumn("LastName");
            DataColumn email = new DataColumn("eMail");
            DataColumn web = new DataColumn("Web");
            DataColumn userID = new DataColumn("UserID");
            table.Columns.Add(ID);
            table.Columns.Add(fName);
            table.Columns.Add(lName);
            table.Columns.Add(email);
            table.Columns.Add(web);
            table.Columns.Add(userID);

            DataRow row = null;
            String[,] sData = new string[,] 
            {{"Jay","Thakar","jay@Thakar.info","http://www.thakar.info","1"},
            {"Johnson","Thomas","jthomas@yahoo.com","http://www.Microsoft.com","2"},
            {"Samuel","Tony","sTony@Cisco.com","http://www.Cisco.com","3"},
            {"Methew","Carlson","MCarlson@MySpace.com","http://www.MySpace.com","4"},
            {"Johnson","Thomas","jthomas@yahoo.com","http://www.Microsoft.com","5"},
            {"Samuel","Tony","sTony@Cisco.com","http://www.Cisco.com","6"},
            {"Methew","Carlson","MCarlson@MySpace.com","http://www.MySpace.com","7"},
            {"Paul","Cook","pCook@680News.com","http://www.680News.com","8"}};
            for (int iCtr = 0; iCtr < sData.Length / 5; iCtr++)
            {
                row = table.NewRow();
                row["ID"] = iCtr + 1;
                row["FirstName"] = sData[iCtr, 0];
                row["LastName"] = sData[iCtr, 1];
                row["eMail"] = sData[iCtr, 2];
                row["Web"] = sData[iCtr, 3];
                table.Rows.Add(row);
            }
            return table;
        }
    }

    [Serializable]
    public class Person
    {

        private string firstName = string.Empty;
        private string userId = string.Empty;
        private string lastName = string.Empty;
        public string UserID
        {
            get { return userId; }
            set { userId = value; }
        }


        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public Person(string firstname, string lastname, string userid)
        {
            firstName = firstname;
            userId = userid;
            LastName = lastname;

        }
    }
}

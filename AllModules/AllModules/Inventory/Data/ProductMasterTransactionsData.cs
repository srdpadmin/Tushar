using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Data
{
    public class ProductMasterTransactionsData
    {

        public int      ID              { get; set; }
        public int      ProductMasterID { get; set; }
        public string   TransactionType { get; set; }
        public int      TransactionID   { get; set; }
        public int      TransactionReferenceID { get; set; }
        public string   ItemCode        { get; set; }
        public float    Credit          { get; set; }
        public float    Debit           { get; set; }
        public float    Balance         { get; set; }
        public DateTime CreatedOn       { get; set; }

    }
}
using System;
namespace Billing.Data
{
    public class BillData
    {
        public int      ID { get; set; }
        public string   ReferenceID { get; set; }
        public int      Status { get; set; }
        public int      CustID { get; set; }
        public int      Revision { get; set; } 
        public float    Tax { get; set; }
        public float    TaxAmount { get; set; }
        public float    Discount { get; set; }
        public float    DiscountAmount { get; set; }
        public float    SubTotal { get; set; }
        public float    Total { get; set; } 
        public int      CreatedBy { get; set; }
        public int      AmendedBy { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string   AmendReason { get; set; }

    }
}

using System;
namespace Quotation.Data
{
    public class QuoteData
    {
        public int      ID { get; set; }
        public string   ReferenceID { get; set; }
        public int      CustID { get; set; }
        //public int      TotalValue { get; set; }
        public int      CreatedBy { get; set; }
        public int      AmendedBy { get; set; }
       
        public float    Tax { get; set; }
        public float    TaxAmount { get; set; }
        public float    Discount { get; set; }
        public float    DiscountAmount { get; set; }
        public float    SubTotal { get; set; }
        public float    Total { get; set; }

        public int      Revision { get; set; }
        public int      Status { get; set; }
        public string   AmendReason { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}

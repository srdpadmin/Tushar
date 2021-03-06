using System;
namespace Quotation.Data
{
    public class QuoteItemData
    {
        public int      ID { get; set; }
        public int      QuoteID { get; set; }
        public string   Code { get; set; }
        public string   Description { get; set; }
        public float    Quantity { get; set; }
        public float    Balance { get; set; }
        public string   Unit { get; set; }
        public float    Rate { get; set; }
        public float    Tax { get; set; }
        public float    TaxAmount { get; set; }
        public float    Discount { get; set; }
        public float    DiscountAmount { get; set; }
        public float    Total { get; set; }
        public float    SubTotal { get; set; }
        public int      CreatedBy { get; set; }
        public int      ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
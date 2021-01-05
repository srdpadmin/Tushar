using System;
namespace Enquiry.Data
{
    public class EnquiryItemData
    {
        public int      ID { get; set; }
        public int      EnquiryID { get; set; }
        public string   Code { get; set; }
        public string   Description { get; set; }
        public float    Quantity { get; set; }        
        public string   Unit { get; set; }
        public float    Rate { get; set; }
        public float    Tax { get; set; }        
        public int      CreatedBy { get; set; }
        public int      ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
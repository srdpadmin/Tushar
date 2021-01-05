using System;
namespace Enquiry.Data
{
    public class EnquiryData
    {
        public int ID { get; set; }       //system generated,hidden
        public int CustID { get; set; }
        public int Revision { get; set; }
        public string AmendReason { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ProductSuggested { get; set; }
        public int ProductStatus { get; set; }
        public int EnquiryStatus { get; set; }
        public int FollowUpStatus { get; set; }                      
        public string Estimation { get; set; }
        public int AssignedTo { get; set; }      
       
        public DateTime EnquiryDate { get; set; } 
        public DateTime ClosureDate { get; set; }
        public DateTime CreatedOn { get; set; }      //system generated,hidden
        public DateTime ModifiedOn { get; set; }      //system generated,hidden
        public int CreatedBy { get; set; }           //system generated,hidden
        public int ModifiedBy { get; set; }          //system generated,hidden

    }
}

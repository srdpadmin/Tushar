using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enquiry.Data
{
    public class EnquiriesData
    { 
        public int ID { get; set; }            
        public string EName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public int EnquiryType { get; set; }
        public int Status { get; set; }        
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime? CallBackDate { get; set; }  
        public DateTime CreatedOn { get; set; }       
        public DateTime ModifiedOn { get; set; }      
        public int CreatedBy { get; set; }           
        public int ModifiedBy { get; set; }
        public int AssignedTo { get; set; }      
    }
}

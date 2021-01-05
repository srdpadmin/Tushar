using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CoreAssemblies
{
    public class EnumClass
    {
        public enum TransactionType
        {
            [Description("Receive")]
            Receive = 1,
            [Description("Dispatch")]
            Dispatch = 2
        }

        public enum StatusType
        {
            [Description("Intermediate")]
            Intermediate = 1,
            [Description("Final")]
            Final = 2
        }

        public enum MaterialType
        {
            [Description("Returable")]
            Returable = 1,
            [Description("Non Returable")]
            NonReturable = 2
        }

        public enum Foo
        {
            [Description("Foo - Something")]
            Something,
            [Description("Foo - Anything")]
            Anything
        }

        public enum PageMode
        {
            Insert =0,
            Edit = 1,

        }
        public enum ModuleName
        { 
            ACL=1,
            Payroll=2,
            Quotation=4,
            Billing=8,
            Enquiry=12,
            Inventory=16,
            PurchaseOrder = 32
        }

        
        public enum Modules
        {   // Allways in a factor of 2
            Payroll = 1,
            Quotation = 2,
            Billing = 4,
            Enquiry = 8,
            Inventory = 16,
            PurchaseOrder = 32

        }
        public enum Roles
        {          
            Admin = 1,
            Supervisor = 2 
        }

        public enum EmployeeType
        {
            MonthlyWithBenefits = 1,
            MontlyWithoutBenefits= 2,
            DailyWithBenefits = 3,
            DailyWithoutBenefits= 4,
            
        }
        public enum ContactType
        {
            Customer=1,
            Vendor =2,
            CustomerAndVendor=3 
        }
        public enum OverTimeRate
        {       
            ZeroTimesBasic =0,
            OneTimesBasic =1,
            TwoTimesBasic=2,
            ThreeTimesBasic=3,
            OneTimesTotal=4
        }

        public enum MonthsOfYear
        {
            January = 1, February, March = 3, April = 4,
            May = 5, June = 6, July = 7, August = 8, September = 9,
            October = 10, November = 11, December = 12
        }
        public enum EnquiryStatus
        {
            Open =0,Pending=1,Closed=2
        }

        // Validate in Enum table 

        public enum Status
        {
            Draft = 0, Submitted = 1
        }
         public enum StockType
        {
            Receive = 0, Return = 1, Transfer=2, Issue=3,Deliver=4
        }

         public enum EnquiryType
         {
             Demo = 1,
             Sales = 2,
             Support = 3,
             JustDial = 4,
             Internal = 5
         }

         public enum PaymentType
         {
             Cash = 1,
             Cheque = 2
         }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllModules.Payroll.Data
{
    public class clsConfigurationData
    {
        public int ID { get; set; }
        public float ProvidentFund      { get; set; }
        public float ESIC               { get; set; }
        public float ProfessionalTax    { get; set; }
        public float TravelAllowance    { get; set; }
        public float DearnessAllowance  { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
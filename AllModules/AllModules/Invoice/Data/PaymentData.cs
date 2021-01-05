using System;
namespace Invoice.Data
{
    public class PaymentData
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int BillID { get; set; }
        public string PaymentDate { get; set; }
        public int Type { get; set; }
        public float Amount { get; set; }
        public float Balance { get; set; }
    }
}
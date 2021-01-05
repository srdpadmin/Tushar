namespace PurchaseOrder.Data
{
    public class PurchaseTermData
    {
        public int ID { get; set; }
        public int PurchaseID { get; set; }
        public int Status { get; set; }
        public string Term { get; set; }
        public string Condition { get; set; }

    }
}
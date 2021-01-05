using System;
namespace Inventory.Data
{
    public class ProductMasterData
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public float Balance { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
        public float Rate { get; set; }
        public float Tax { get; set; }
        public float Discount { get; set; }
        public string Location { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

    }
}
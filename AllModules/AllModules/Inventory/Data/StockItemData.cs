using System;

namespace Inventory.Data
{

    public class StockItemData
    {
        public int      ID              { get; set; }
        public int      StockID         { get; set; }        
        public int      ReferenceID     { get; set; }
        public int      StockType       { get; set; }
        public string   Code            { get; set; }
        public string   Description     { get; set; }
        public float    Quantity        { get; set; }        
        public string   Unit            { get; set; }
        public float    Rate            { get; set; }
        public float    Tax             { get; set; }
        public float    TaxAmount       { get; set; }
        public float    Discount        { get; set; }
        public float    DiscountAmount  { get; set; }
        public float    Total           { get; set; }
        public float    SubTotal        { get; set; }       
        public int      CreatedBy       { get; set; }
        public int      ModifiedBy      { get; set; }
        public DateTime CreatedOn       { get; set; }
        public DateTime ModifiedOn      { get; set; }
        
    }
}
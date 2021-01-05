using System;

namespace Inventory.Data
{

    public class StockData
    {
        public int      ID                      { get; set; }
        public int      Status                  { get; set; }
        public string   ReferenceID             { get; set; }
        public int      LocationID              { get; set; }
        public int      VendorID                { get; set; }
        public int      Revision                { get; set; }
        public int      StockType               { get; set; }
        public string   AmendReason             { get; set; }
        public string   TrackingNumber          { get; set; }       
        public string   ModeOfDelivery          { get; set; }
        public string   ModeOfPayment           { get; set; }
        public string   SenderPhone             { get; set; }
        public string   SenderName              { get; set; }
        public string   DeliveryBy              { get; set; }
        public string   DeliveryTo              { get; set; }
        public string   DeliveryByPhone         { get; set; }
        public string   DeliveryByCompany       { get; set; }       
        public string   Notes                   { get; set; }
        public int      FileID                  { get; set; }
        public int      CreatedBy               { get; set; }
        public int      AmendedBy               { get; set; }
        public float    Tax                     { get; set; }
        public float    TaxAmount               { get; set; }
        public float    Discount                { get; set; }
        public float    DiscountAmount          { get; set; }
        public float    SubTotal                { get; set; }
        public float    Total                   { get; set; } 
        public DateTime TransactionDate         { get; set; }
        public DateTime CreatedOn               { get; set; }
        public DateTime ModifiedOn              { get; set; }

    }
}
public class SalaryData
{ 
    public int 		    ID		                { get; set; } 
    public int 		    EmpID		            { get; set; } 
    public int 		    DMID		            { get; set; }
    public int          EmpType                 { get; set; }
    public string       EmpTypeName             { get; set; }
    public float        BasicDaily              { get; set; }
    public float        AllowanceDaily          { get; set; } 
    public float 		PFDeduction		        { get; set; } 
    public float 		ESICDeduction		    { get; set; } 
    public float 		ProfTaxDeduction		{ get; set; }
    public float        TravelAllowance         { get; set; }
    public float        DearnessAllowance       { get; set; }
    public float 		LeavesBalancePrevious   { get; set; } 
    public float 		LeavesBalanceCurrent	{ get; set; } 
    public float 		PaidLeavesTaken		    { get; set; } 
    public float 		UnPaidLeavesTaken		{ get; set; }
    public bool         LeavesOverride          { get; set; }     
    public float 		TotalPayableDays		{ get; set; } 
    public float 		GovtCompHoliday		    { get; set; } 
    public float 		DaysWorked		        { get; set; } 
    public float 		OTHours		            { get; set; } 
    public float 		OTAmount		        { get; set; }    
    public float 		PaidAmount		        { get; set; } 
    public float 		NetPayable		        { get; set; } 
    public float 		AdvBalancePrevious		{ get; set; } 
    public float 		AdvBalanceCurrent		{ get; set; }
    public float        AdvBalanceAdd           { get; set; }
    public float        AdvBalanceDeduct        { get; set; } 
    public float 		NetIncome		        { get; set; }  //unused
    public int 		    Revision		        { get; set; } 
    public int 		    CreatedBy		        { get; set; } 
    public int 		    LastUpdatedBy		    { get; set; } 
    public bool 		Locked		            { get; set; }  
}
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Payroll.Common;
using Payroll.BusLogic;
using Payroll.Data;
using ENM = CoreAssemblies.EnumClass;
using CoreAssemblies;
using AllModules;

 
namespace Payroll
{
    /// <summary>
    /// Summary description for PayrollService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
   
    public class PayrollService : System.Web.Services.WebService
    {

        private float CalculateBasicAndAllowanceAndOTRate(SalaryData sal, SalaryDetailsData SDData, CurrentMonthDetails cmd)
        {
            float OTRateValue =0;

            //if (sal.ID == 0)  // Case of first time insert- called one time only
            //        sal.LeavesCreditTaken = cmd.LeavesCreditTaken;
             // Calculate BasicDaily, AllowanceDaily and OTRate depending upon month selected.
             
            sal.EmpType = SDData.EmpType;

            if (!SDData.IsBasicDaily)
            {
                sal.BasicDaily = SDData.BasicMonthly / cmd.TotalDaysOfMonth;
            }
            else
            {
                sal.BasicDaily = SDData.BasicDaily;
            }
            if (!SDData.IsAllowanceDaily)
            {
                sal.AllowanceDaily = SDData.AllowanceMonthly / cmd.TotalDaysOfMonth;
            }
            else
            {
                sal.AllowanceDaily = SDData.AllowanceDaily;
            }
            //if (SDData.EmpType == (int)ENM.EmployeeType.MonthlyWithBenefits ||
            //    SDData.EmpType == (int)ENM.EmployeeType.MontlyWithoutBenefits)
            //{
            //    if (SDData.BasicMonthly > 0 && SDData.BasicDaily == 0)
            //    {
            //        sal.BasicDaily = SDData.BasicMonthly / cmd.TotalDaysOfMonth;
            //    }
            //    else
            //    {
            //        sal.BasicDaily = SDData.BasicDaily;
            //    }
            //    if (SDData.AllowanceMonthly > 0 && SDData.AllowanceDaily == 0)
            //    {
            //        sal.AllowanceDaily = SDData.AllowanceMonthly / cmd.TotalDaysOfMonth;
            //    }
            //    else
            //    {
            //        sal.AllowanceDaily = SDData.AllowanceDaily;
            //    }
            //    if (SDData.EmpType == (int)ENM.EmployeeType.MontlyWithoutBenefits)
            //    {
            //        sal.AllowanceDaily = 0;
            //    }
            //}               
            //else
            //{
            //    sal.BasicDaily = SDData.BasicDaily;
            //    sal.AllowanceDaily = 0;
            //}
             
           
            switch ((ENM.OverTimeRate)SDData.OverTimeRate)
            {
                case ENM.OverTimeRate.ZeroTimesBasic:   OTRateValue = 0;
                    break;
                case ENM.OverTimeRate.OneTimesBasic: OTRateValue = (sal.BasicDaily) / 8;
                    break;
                case ENM.OverTimeRate.TwoTimesBasic: OTRateValue = (2 * (sal.BasicDaily)) / 8;
                    break;
                case ENM.OverTimeRate.ThreeTimesBasic: OTRateValue = (3 * (sal.BasicDaily)) / 8;
                    break;
                case ENM.OverTimeRate.OneTimesTotal: OTRateValue = ((sal.BasicDaily + sal.AllowanceDaily)) / 8;
                    break;
            }
            return OTRateValue;
        }

        [WebMethod]
        public SalaryData CalculateSalary(SalaryData salary, CurrentMonthDetails cmd)
        {
            float daysNotWorked = 0;
            float paidDays = 0;
            float paidAmount =0;
            float OTRate =0;
            float TA =0;
            float DA =0;

            
            try
            { 
            // salary is being populated with proper values from SalaryDetails Data
            SalaryDetails SalDetails = new SalaryDetails();
            SalaryDetailsData SDData = SalDetails.PopulateSalaryDetails(salary.EmpID);
            OTRate = CalculateBasicAndAllowanceAndOTRate(salary,SDData,cmd);
            //Visit again on this
            //TA = SDData.CreditTA ? salary.BasicDaily * (SDData.TravelAllowance) : 0;
            //DA = SDData.CreditDA ? salary.BasicDaily * (SDData.DearnessAllowance / 100) : 0;

            //Calculations
            if (salary.EmpType == (int)ENM.EmployeeType.MonthlyWithBenefits ||
                salary.EmpType == (int)ENM.EmployeeType.MontlyWithoutBenefits)
                {
                    
                    TA = SDData.CreditTA ? SDData.TravelAllowance :0;
                    DA = SDData.CreditDA ? SDData.DearnessAllowance :0;

                    paidDays = salary.DaysWorked + salary.GovtCompHoliday;
                    //paidDays = paidDays > 0 ? paidDays + cmd.WeeklyOffs : 0;
                    daysNotWorked = cmd.TotalDaysOfMonth - paidDays;

                    if (salary.LeavesBalancePrevious >  0)
                    {                        
                        if (daysNotWorked > 0)
                        {
                            //Check for Manual PAID LEAVES LOGIC HERE 
                            // PaidLeaves are set in UI, we only deduct it from LeavesBalancePrevious
                            // PaidLeaves are > 0 then deduct from Balance & add to paiddays
                            salary.PaidLeavesTaken = salary.PaidLeavesTaken > daysNotWorked ? daysNotWorked : salary.PaidLeavesTaken;
                            if (salary.LeavesBalancePrevious >= salary.PaidLeavesTaken) // && daysNotWorked > 0)
                            {                                
                                salary.LeavesBalanceCurrent = salary.LeavesBalancePrevious - salary.PaidLeavesTaken;
                                salary.UnPaidLeavesTaken = daysNotWorked - salary.PaidLeavesTaken;
                                paidDays += salary.PaidLeavesTaken;
                            }
                            else
                            {
                                salary.PaidLeavesTaken = 0;
                                salary.UnPaidLeavesTaken = daysNotWorked;
                                salary.LeavesBalanceCurrent = salary.LeavesBalancePrevious;
                                paidDays += salary.PaidLeavesTaken;
                            }
                        }
                        else
                        {
                            // Clearing values which are present in UI
                            salary.PaidLeavesTaken = daysNotWorked;
                            salary.UnPaidLeavesTaken = daysNotWorked;
                            salary.LeavesBalanceCurrent = salary.LeavesBalancePrevious;
                            paidDays += daysNotWorked;
                        }
                    }
                    else
                    {
                        //float unpaidLeaves = MD.WorkingDays - daysworked - govtCompHoliday;
                        salary.UnPaidLeavesTaken = daysNotWorked;
                        salary.LeavesBalanceCurrent = salary.LeavesBalancePrevious;
                        salary.PaidLeavesTaken = 0;
                    }

                    salary.TotalPayableDays = paidDays;
                    //if (totalpaiddays >= 0)
                    //{
                    //    paidAmount = salary.EmpType == (int)ENM.EmployeeType.MonthlyWithBenefits ?
                    //        (((salary.BasicDaily + salary.AllowanceDaily)) * totalpaiddays) + (TA + DA ):
                    //        (salary.BasicDaily * totalpaiddays);
                        
                    //   salary.PaidAmount= paidAmount;
                    //}

                    //Overtime calculations
                    if (salary.OTHours > 0)
                    {
                        salary.OTAmount = salary.OTHours * OTRate;
                    }
                    
                    salary.AdvBalanceCurrent = salary.AdvBalancePrevious + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;
                   
                    // this is good for monthy with benefit
                    if (paidDays > 0)
                    {
                        
                        if (salary.EmpType == (int)ENM.EmployeeType.MonthlyWithBenefits)
                        {
                            paidAmount = (salary.BasicDaily * paidDays);
                            // IMP: PF is always 12% of Basic fixed regardless of days worked
                            if (SDData.DeductPF)
                            {
                                salary.PFDeduction = paidAmount * (SDData.ProvidentFundPercent / 100);
                            }
                            if (SDData.DeductProfTax)
                            {
                                salary.ProfTaxDeduction = SDData.ProfessionalTaxPercent;
                            }
                            if (SDData.DeductESIC)
                            {
                                salary.ESICDeduction = paidAmount * (SDData.ESICTaxPercent / 100);
                            }
                            salary.PaidAmount           = ((salary.BasicDaily + salary.AllowanceDaily) * paidDays) ;
                            salary.NetPayable           = salary.PaidAmount + salary.OTAmount - salary.PFDeduction - salary.ProfTaxDeduction - salary.ESICDeduction + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;
                            //netpayable += bonus; //Bonus removed                            
                        }
                        else if (salary.EmpType == (int)ENM.EmployeeType.MontlyWithoutBenefits)
                        {                             
                             salary.PFDeduction         = 0.0F;
                             salary.ProfTaxDeduction    = 0.0F;
                             salary.ESICDeduction       = 0.0F;
                             salary.PaidAmount          = (salary.BasicDaily  * paidDays) ;
                             salary.NetPayable          = salary.PaidAmount + salary.OTAmount + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;
                            //netpayable += bonus; //Bonus removed                            
                        }
                    }
                }
            else if (salary.EmpType == (int)ENM.EmployeeType.DailyWithoutBenefits)
                {                    
                    paidDays = salary.DaysWorked + salary.GovtCompHoliday ;                  
                    // Paid days calculation, no leaves calculations required                   
                    
                    salary.TotalPayableDays = paidDays;
                    //Overtime calculations
                    if (salary.OTHours > 0)
                    {
                        salary.OTAmount = salary.OTHours * OTRate;
                    }
                    
                    salary.AdvBalanceCurrent = salary.AdvBalancePrevious + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;
                    
                    // this is good for monthy with benefit
                    if (paidDays > 0)
                    {
                        
                        salary.PFDeduction              = 0.0F;
                        salary.ProfTaxDeduction         = 0.0F;
                        salary.ESICDeduction            = 0.0F;
                        salary.PaidAmount               = (salary.BasicDaily * paidDays) ;
                        salary.NetPayable               = salary.PaidAmount + salary.OTAmount + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;
                        //netpayable += bonus; //Bonus removed                                         
                    }
                } // DailyWithBenefits
            else if (salary.EmpType == (int)ENM.EmployeeType.DailyWithBenefits)
                { 

                     paidDays = salary.DaysWorked + salary.GovtCompHoliday ;   
                   // Copied this calculation from Monthly with Benefit 
                    // UPTO Paid days calculation
                    if (salary.LeavesBalancePrevious > 0)
                    {
                        daysNotWorked = cmd.TotalDaysOfMonth - paidDays;
                        if (daysNotWorked > 0)
                        {
                            // Check for Manual PAID LEAVES LOGIC HERE 
                            // PaidLeaves are set in UI, we only deduct it from LeavesBalancePrevious
                            // PaidLeaves are > 0 then deduct from Balance & add to paiddays
                            if (salary.LeavesBalancePrevious > salary.PaidLeavesTaken && daysNotWorked > 0)
                            {
                                //salary.PaidLeavesTaken =differnceOfWorkedDaysLeaves;
                                salary.LeavesBalanceCurrent = salary.LeavesBalancePrevious - salary.PaidLeavesTaken;
                                salary.UnPaidLeavesTaken = daysNotWorked - salary.PaidLeavesTaken;
                                paidDays += salary.PaidLeavesTaken;
                            }
                            else
                            {
                                salary.PaidLeavesTaken = 0;
                                salary.UnPaidLeavesTaken = daysNotWorked;
                                salary.LeavesBalanceCurrent = salary.LeavesBalancePrevious;
                                paidDays += salary.PaidLeavesTaken;
                            }                                                        
                        }
                    }
                    else
                    {                       
                        salary.UnPaidLeavesTaken = daysNotWorked;
                        salary.PaidLeavesTaken = 0;
                    }
                    // Paid days calculation
                    salary.TotalPayableDays = paidDays;
                    //if (totalpaiddays >=0)
                    //{
                    //    paidAmount = (salary.BasicDaily * totalpaiddays) +  (TA + DA);                       
                    //}
                     //Overtime calculations
                     if (salary.OTHours > 0)
                    {
                        salary.OTAmount = salary.OTHours * OTRate;
                    }                  

                    salary.AdvBalanceCurrent = salary.AdvBalancePrevious + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;
                    // Calculating PF and ESIC and Prof Tax here 
                    if (paidDays > 0)
                    {
                        paidAmount = (salary.BasicDaily * paidDays);
                        // IMP: PF is always 12% of Basic fixed regardless of days worked
                        if (SDData.DeductPF)
                        {
                            salary.PFDeduction = paidAmount * (SDData.ProvidentFundPercent / 100);
                        }
                        if (SDData.DeductProfTax)
                        {
                            salary.ProfTaxDeduction = SDData.ProfessionalTaxPercent;
                        }
                        if (SDData.DeductESIC)
                        {
                            salary.ESICDeduction = paidAmount * (SDData.ESICTaxPercent / 100);
                        }
                         
                        salary.PaidAmount                   = ((salary.BasicDaily + salary.AllowanceDaily) * paidDays) ;
                        salary.NetPayable                   = salary.PaidAmount + salary.OTAmount - salary.PFDeduction - salary.ProfTaxDeduction - salary.ESICDeduction + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;
                           
                        //salary.PFDeduction = paidAmount * (SDData.ProvidentFundPercent / 100);
                        //salary.ProfTaxDeduction = paidAmount * (SDData.ProfessionalTaxPercent / 100);
                        //salary.ESICDeduction = paidAmount * (SDData.ESICTaxPercent / 100);
                        //salary.NetPayable = paidAmount + salary.OTAmount - salary.PFDeduction - salary.ProfTaxDeduction - salary.ESICDeduction + salary.AdvBalanceAdd - salary.AdvBalanceDeduct;                        
                    }
                }
            }
            catch (System.Data.Common.DbException exc)
            {
                Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
            finally
            {
                 
            }

                return salary;
        }
        
        public int InsertUpdateSalaryToDb(SalaryData salary,bool mode,CurrentMonthDetails cmd)
        {
            int[] id= new int[2];
            Salary sal = new Salary();
            ArrayList sql = new ArrayList();
            string[] queries = null;
            bool forceAddAdvanceTransaction = false;
            bool forceAddLeavesTransaction = false;
 
            try
            {
                 	
            if (mode)
            {//Insert
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(salary, "INSERT", "Salary"));                
            }
            else
            {//Update               
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(salary, "UPDATE", "Salary"));
            }
            //Check for DMID exists?

            DateMaster dm = new DateMaster();
            if (!dm.ExistingDMIDCheck(cmd.DMID))
            {               
                string str="Insert into DateMaster (DM,iMonth,iYear,sMonth) Values ";
                str += " (" + cmd.DMID + "," + cmd.SelectedMonth + "," + cmd.SelectedYear + ",'" +
                        Enum.GetName(typeof(ENM.MonthsOfYear), cmd.SelectedMonth) +"')";
                sql.Add(str);
            }

            // Leaves Adjustment when Edited
            Leaves leavesAdjustment = new Leaves(salary.EmpID, salary.DMID);
            // Get top 1 system generated=1 leave
            
            if (leavesAdjustment.SystemGenerated == 1 && (leavesAdjustment.Credit != 0 || leavesAdjustment.Debit != 0))
            {
                // pass in the current leaves balance to leaves object, deduct/add the salary leaves which is to be deleted 
                float? leavesBalance = leavesAdjustment.GetCurrentBalance(salary.EmpID);
                leavesAdjustment.AdjustLeaves(leavesBalance.Value);
                salary.LeavesBalancePrevious = leavesAdjustment.CurrentBalance;
                forceAddLeavesTransaction = true;
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(leavesAdjustment, "INSERT", "Leaves"));
                
            }
                
                 
            // Advance Adjustment when Edited
            Advance advAdjustment = new Advance(salary.EmpID, salary.DMID,1);
            // Get top 1 system generated=1 leave for the same month, even if it is saved last time, so we can adjust it
            if (advAdjustment.SystemGenerated == 1 && (advAdjustment.Credit !=0 || advAdjustment.Debit !=0))
            {
                float? previousBalance = advAdjustment.GetCurrentBalance(salary.EmpID);
                advAdjustment.AdjustAdvance(previousBalance.Value);
                salary.AdvBalancePrevious = advAdjustment.CurrentBalance;
                forceAddAdvanceTransaction = true;                
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(advAdjustment, "INSERT", "Advance"));
            }

            // Add new Salary Leaves to Transactions only when PaidLeavesTaken > 0 or Debit > 0            
            if (forceAddLeavesTransaction|| salary.PaidLeavesTaken > 0)
            {
                Leaves newleave = new Leaves();
                newleave.DMID = cmd.DMID;
                newleave.EmpID = salary.EmpID;
                newleave.Credit = 0;
                newleave.Debit = salary.PaidLeavesTaken;
                newleave.CurrentBalance = salary.LeavesBalancePrevious - salary.PaidLeavesTaken;                
                newleave.MonthName = DateTime.Now.ToString("MMMM");
                newleave.iYear = DateTime.Now.Year;
                newleave.PreviousBalance = salary.LeavesBalancePrevious;
                newleave.CreatedOn = DateTime.Now;
                newleave.SystemGenerated = 1; //open for new adjustment
                newleave.Comments =newleave.MonthName + " "+newleave.iYear.ToString() + " Salary Leaves Debited";                
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(newleave, "INSERT", "Leaves")); 
            }

            if (forceAddAdvanceTransaction || salary.AdvBalanceAdd !=0 || salary.AdvBalanceDeduct !=0)
            {
                // Add new Advance to Transactions
                Advance newAdvance = new Advance();
                newAdvance.DMID = cmd.DMID;
                newAdvance.EmpID = salary.EmpID;
                newAdvance.Credit = salary.AdvBalanceAdd;
                newAdvance.Debit = salary.AdvBalanceDeduct;
                newAdvance.CurrentBalance = salary.AdvBalancePrevious + newAdvance.Credit - newAdvance.Debit;                
                newAdvance.MonthName = DateTime.Now.ToString("MMMM");
                newAdvance.iYear = DateTime.Now.Year;
                newAdvance.PreviousBalance = salary.AdvBalancePrevious;
                newAdvance.CreatedOn = DateTime.Now;
                newAdvance.SystemGenerated = 1; //open for new adjustment
                newAdvance.Comments = "Advance";
                if (salary.AdvBalanceAdd != 0)
                {
                    newAdvance.Comments += " Credited";
                }
                if (salary.AdvBalanceDeduct != 0)
                {
                    newAdvance.Comments += " Debited";
                }
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(newAdvance, "INSERT", "Advance"));
            }
            }
            catch (System.Data.Common.DbException exc)
            {
                Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
            finally
            {
               
            }

            try
            {
                queries = (string[])sql.ToArray(typeof(string));
                DbFactory factory = new DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                id = factory.Runcommands(queries);
                factory.CommitTransaction();
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
                id[0] = -1;
            }
            return id[0];
        }
    }
}

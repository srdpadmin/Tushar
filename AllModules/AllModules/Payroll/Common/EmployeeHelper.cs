using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using ASM = CoreAssemblies;
using ENM=CoreAssemblies.EnumClass;
namespace Payroll.Common
{
    public class EmployeeHelper
    {
        static List<TEmployeeType> EmployeeTypes = null;
        static List<TOverTimeRate> OverTimeRates = null;
        public static List<TEmployeeType> GetEmployeeTypes()
        {
            if (EmployeeTypes == null)
            {
                EmployeeTypes = new List<TEmployeeType>();
                EmployeeTypes.Add(new TEmployeeType("Monthly with Benefits", (int)ENM.EmployeeType.MonthlyWithBenefits));
                EmployeeTypes.Add(new TEmployeeType("Monthly without Benefits", (int)ENM.EmployeeType.MontlyWithoutBenefits));
                EmployeeTypes.Add(new TEmployeeType("Daily with Benefits", (int)ENM.EmployeeType.DailyWithBenefits));
                EmployeeTypes.Add(new TEmployeeType("Daily without Benefits", (int)ENM.EmployeeType.DailyWithoutBenefits));
               
            }
            return EmployeeTypes;
            
        }

        public static List<TOverTimeRate> GetOverTimeRates()
        {
            if (OverTimeRates == null)
            {
                OverTimeRates = new List<TOverTimeRate>();
                OverTimeRates.Add(new TOverTimeRate("0 X Basic", (int)ENM.OverTimeRate.ZeroTimesBasic));
                OverTimeRates.Add(new TOverTimeRate("1 X Basic", (int)ENM.OverTimeRate.OneTimesBasic));
                OverTimeRates.Add(new TOverTimeRate("2 X Basic", (int)ENM.OverTimeRate.TwoTimesBasic));
                OverTimeRates.Add(new TOverTimeRate("3 X Basic", (int)ENM.OverTimeRate.ThreeTimesBasic));
                OverTimeRates.Add(new TOverTimeRate("1 X Total", (int)ENM.OverTimeRate.OneTimesTotal));
                
            }
            return OverTimeRates;

        }
            
    }

    public class SalaryHelper
    {
        static List<Month> monthsinYear  = null;
        static List<TYear> totalYears = null;
        static List<TYear> totalFinYears = null;
        public SalaryHelper()
        {
            
        }

        public static List<TYear> GetListOfYear()
        {
            string[,] years = new string[11, 2] { {"2015","2015"},{"2016", "2016" },{ "2017", "2017" },
                                                 { "2018", "2018" }, { "2019", "2019" },
                                                 { "2020", "2020" }, { "2021", "2021" },
                                                 { "2022", "2022" }, { "2023", "2023" },
                                                 { "2024", "2024" }, { "2025", "2025" }};
            if(totalYears == null)
            {
                totalYears = new List<TYear>();
                for (int i = 0; i < years.GetLength(0); i++)
                {
                    totalYears.Add(new TYear(years[i, 0], Convert.ToInt16(years[i, 1]),i));
                }         
            }

            return totalYears;

        }
        public static List<TYear> GetListOfFinancialYears()
        {
            string[,] years = new string[11, 2] { { "2014-2015", "2015" },{ "2015-2016", "2016" },
                                                 { "2016-2017", "2017" },
                                                 { "2017-2018", "2018" }, { "2018-2019", "2019" },
                                                 { "2019-2020", "2020" }, { "2020-2021", "2021" },
                                                 { "2021-2022", "2022" }, { "2022-2023", "2023" },
                                                 { "2023-2024", "2024" }, { "2024-2025", "2025"}};
            if (totalFinYears == null)
            {
                totalFinYears = new List<TYear>();
                for (int i = 0; i < years.GetLength(0); i++)
                {
                    totalFinYears.Add(new TYear(years[i, 0], Convert.ToInt16(years[i, 1]), i));
                }
            }

            return totalFinYears;

        }

        public static List<Month> GetMonthsInAYear()
        {
            if (monthsinYear == null)
            {
                string[,] months = new string[12, 2] { { "January", "1" }, 
                                                   { "February", "2" },
                                                   {"March", "3"},
                                                   {"April","4"},
                                                   {"May","5"},
                                                   {"June","6"},
                                                   {"July","7"},
                                                   {"August","8"},
                                                   {"September","9"},
                                                   {"October","10"},
                                                   {"November","11"},
                                                   {"December","12"}};
            
                monthsinYear = new List<Month>();                                     
                for (int i=0;i<12;i++)
                {
                    monthsinYear.Add(new Month(months[i,0],Convert.ToInt16(months[i,1])));
                }         
            }

            return monthsinYear;
        }

        public static IDictionary<String, Int32> ConvertEnumToDictionary<MonthsOfYear>()
        {
            if (typeof(MonthsOfYear).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(MonthsOfYear)).Cast<int>().ToDictionary(currentItem => Enum.GetName(typeof(MonthsOfYear), currentItem));
        }

        

        public static int GetWorkingDaysForMonth(int month, int year, ref int saturdays)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = GetLastDayOfMonth(month,year); 
            DateTime tmp = start; 
            int count = 0; 
            while (tmp <= end) 
            {
                if (tmp.DayOfWeek != DayOfWeek.Saturday)
                {
                    count++;
                }
                else
                {
                    saturdays += 1;
                }
                tmp = tmp.AddDays(1);
            }
            return count;
        }

        public static DateTime GetLastDayOfMonth(int iMonth, int iyear)
        {
            // set return value to the last day of the month 
            // for any date passed in to the method 
            // create a datetime variable set to the passed in date 
            DateTime dtTo = new DateTime(iyear, iMonth, 1);
            // overshoot the date by a month 
            dtTo = dtTo.AddMonths(1);
            // remove all of the days in the next month 
            // to get bumped down to the last day of the 
            // previous month 
            dtTo = dtTo.AddDays(-(dtTo.Day));
            // return the last day of the month 
            return dtTo;
        }

        private static int GetTotalDaysOfMonth(int iMonth, int year)
        {
            // set return value to the last day of the month 
            // for any date passed in to the method 
            // create a datetime variable set to the passed in date 
            DateTime dtTo = new DateTime(year, iMonth, 1);
            // overshoot the date by a month 
            dtTo = dtTo.AddMonths(1);
            // remove all of the days in the next month 
            // to get bumped down to the last day of the 
            // previous month 
            dtTo = dtTo.AddDays(-(dtTo.Day));
            // return the last day of the month 
            return dtTo.Day;
        }

        //Old
        public static void PopulateMonthDetails(MonthDetails md)
        {
            int saturdays = 0;            
            md.WorkingDays = GetWorkingDaysForMonth(md.SelectedMonth, md.SelectedYear, ref saturdays);
            md.WeeklyOffs = saturdays;
            md.TotalDaysOfMonth = GetTotalDaysOfMonth(md.SelectedMonth, md.SelectedYear);             
        }
        //New
        public static void PopulateMonthDetails(CurrentMonthDetails md)
        {           
            int saturdays = 0;
            md.WorkingDays = GetWorkingDaysForMonth(md.SelectedMonth, md.SelectedYear, ref saturdays);
            md.WeeklyOffs = saturdays;
            md.TotalDaysOfMonth = GetTotalDaysOfMonth(md.SelectedMonth, md.SelectedYear);
            md.DMID = Convert.ToInt32(md.SelectedYear.ToString() + md.SelectedMonth.ToString("00"));
        }

        public static void sGetMonthNameYearFromDMID(string DMID, ref string month, ref string year)
        {
            month= DMID.ToString().Substring(4, 2);
            year = DMID.ToString().Substring(0, 4);
        }
        public static void iGetMonthNameYearFromDMID(int DMID, ref int month, ref int year)
        {
            month = Convert.ToInt32(DMID.ToString().Substring(4, 2));
            year  = Convert.ToInt32(DMID.ToString().Substring(0, 4));
        }
        public static string GetDMIDfromMonthYear(int smonth, int year)
        {
            return (year.ToString() + smonth.ToString("00"));
        }
    }

    public class Month
    {
        public string   MonthName   { get; set; }
        public int      MonthValue  { get; set; }
        public Month(string monthName, int monthValue)
        {
            MonthName = monthName;
            MonthValue = monthValue;
        }
    }
    public class TYear
    {
        public string YearName { get; set; }
        public int YearValue { get; set; }
        public int Index { get; set; }
        public TYear(string yearName, int yearValue, int index)
        {
            YearName = yearName;
            YearValue = yearValue;
            Index = index;

        }
    }

    public class TEmployeeType
    {
        public string EmployeeTypeName { get; set; }
        public int EmployeeTypeValue { get; set; }
        
        public TEmployeeType(string employeeTypeName, int employeeTypeValue)
        {
            EmployeeTypeName = employeeTypeName;
            EmployeeTypeValue = employeeTypeValue;
        }
    }

    public class TOverTimeRate
    {
        public string RateName { get; set; }
        public int RateValue { get; set; }
        
        public TOverTimeRate(string rateName, int rateValue)
        {
            RateName = rateName;
            RateValue = rateValue;
        }
    }

   
    [Serializable]
    public class CurrentMonthDetails
    {
        //This object needs to be populated during page load and persist in viewstate

        public int DMID                 { get; set; }
        public int TotalDaysOfMonth     { get; set; }
        public int WorkingDays          { get; set; }
        public int WeeklyOffs           { get; set; }        
        public int SelectedMonth        { get; set; }
        public int SelectedYear         { get; set; }
        public float LeavesBalancePrevious { get; set; }
        public float AdvBalancePrevious { get; set; }
        public float BasicDaily         { get; set; }
        public float BasicMonthly       { get; set; }
        public float AllowanceMontly    { get; set; }
        public float AllowanceDaily     { get; set; }
        //public float LeavesCreditTaken { get; set; }
        //public int SelectedMonth    { get { return selectedMonth; } set { selectedMonth = value; } }
        
    }

    public class MonthDetails
    {
        public ENM.EmployeeType CurrentMonthEmpType { get; set; }
        public ENM.OverTimeRate ActualOTRate { get; set; }
        public string   EmpID                { get; set; }
        public bool     Active              { get; set; }        
        public int     TotalDaysOfMonth     { get; set; }
        public int     WorkingDays          { get; set; }
        public int     WeeklyOffs           { get; set; }
        public int     SelectedMonth        { get; set; }
        public int     SelectedYear         { get; set; }
        public float ActualBalanceLeaves  { get; set; }
        public float ActualAdvancedPending{ get; set; }
        public float AdvancedPending      { get; set; }
        public float BalanceLeaves        { get; set; }
        public float UnpaidLeaves         { get; set; }
        public float PaidLeaves           { get; set; }
        public float GovtCompHoliday      { get; set; }
        public float Basic                { get; set; }
        public float BasicDaily           { get; set; }
        public float BasicMonthly         { get; set; }
        public float ActualAllowance      { get; set; }
        public float AllowanceDaily       { get; set; }
        public float OverTimeRate         { get; set; }
        public float PF                   { get; set; }
        public float ESIC                 { get; set; }
        public float ProfTax              { get; set; } 
        public float CalulatedAdvance     { get; set; }
        public float BaseNewAdvance       { get; set; }
        public float BaseAdvanceDeduction { get; set; }
        public float BasePaidLeaves       { get; set; }
        public float PaidExpenseNew         { get; set; }
        public float PaidExpenseSubtract    { get; set; }
        public float PaidExpenseCurrent     { get; set; }
        public float UnpaidExpenseCurrent   { get; set; }
        public float UnpaidExpenseNew       { get; set; }
        public float ExpenseDeductionCurrent { get; set; }
        public float ExpenseDeductionNew    { get; set; }
        public float NetPayableExpense      { get; set; }
        public float TotalIncome                { get; set; }
        public float Bonus              { get; set; }
        public float ExtraBonus             { get; set; }
    }

   
}



namespace MolyCoreWeb.Models.DBEntitiy
{
    public class BusinessIndicator
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double LEI_CCI { get; set; }
        public double LEI_Ex_Trend { get; set; }
        public double CEI_CCI { get; set; }
        public double CEI_Ex_Trend { get; set; }
        public double LAG_CCI { get; set; }
        public double LAG_Ex_Trend { get; set; }
        public double BCS_Composite_Score { get; set; }
        public string? BCS_Signal { get; set; }
    }


}

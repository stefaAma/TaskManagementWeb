namespace TaskManagement.ViewModels.DailyTasks
{
    public class Report
    {
        public Dictionary<string, decimal> Categories { get; set; } = new Dictionary<string, decimal>();
        public decimal PercentageCompletion { get; set; } = 0;
        public float TotalEffort { get; set; } = 0;
    }
}

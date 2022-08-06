namespace PlanIt.Application.Dtos.Plan
{
    public class CreatePlanDto
    {
        public DateTime FirstSchedulableDate { get; set; }
        public DateTime LastSchedulableDate { get; set; }
    }
}


namespace UsaPopulation.Domain.Models
{
    public class PopulationProgressionOutputModel
    {
        public int Year { get; }
        public int YearlyIncrement { get; }
        public double PercentChange { get; }

        public PopulationProgressionOutputModel(int year, int yearlyIncrement, double percentChange)
        {
            Year = year;
            YearlyIncrement = yearlyIncrement;
            PercentChange = percentChange;
        }
    }
}

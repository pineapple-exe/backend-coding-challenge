
namespace UsaPopulation.Domain.Models
{
    public class PopulationsDiffOutputModel
    {
        public string StateA { get; }
        public string StateB { get; }
        public int Diff { get; }
        public int Year { get; }

        public PopulationsDiffOutputModel(string stateA, string stateB, int diff, int year)
        {
            StateA = stateA;
            StateB = stateB;
            Diff = diff;
            Year = year;
        }
    }
}

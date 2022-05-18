
namespace UsaPopulation.Domain.Models
{
    public class StateOutputModel
    {
        public string Name { get; }
        public int Population { get; }

        public StateOutputModel(string name, int population)
        {
            Name = name;
            Population = population;
        }
    }
}

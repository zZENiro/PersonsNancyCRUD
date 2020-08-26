using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.Abstractions.Models
{
    public class Person
    {
        private readonly IAgeComputingFactory _ageComputingFactory;

        public int Id { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; } 

        public int Age { get => _ageComputingFactory.ComputeAge(Birthday); }

        public Person(IAgeComputingFactory ageComputingFactory)
        {
            _ageComputingFactory = ageComputingFactory;
        }
    }
}

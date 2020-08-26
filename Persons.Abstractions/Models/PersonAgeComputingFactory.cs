using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.Abstractions.Models
{
    public class PersonAgeComputingFactory : IAgeComputingFactory
    {
        public int ComputeAge(DateTime birthday) => DateTime.Now.Year - birthday.Year;
    }
}

using System;

namespace Persons.Abstractions.Models
{
    public interface IAgeComputingFactory
    {
        int ComputeAge(DateTime birthday);
    }
}

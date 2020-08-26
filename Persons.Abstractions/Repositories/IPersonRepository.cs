using Persons.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons.Abstractions.Repositories
{
    public interface IPersonRepository
    {
        Person Find(Guid id);

        void Insert(Person newPerson);
    }
}

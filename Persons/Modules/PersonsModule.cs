using Nancy;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persons.Abstractions.Repositories;
using Serilog;
using Persons.Abstractions.Models;
using System.Text.Json;

namespace Persons.Modules
{
    public class PersonsModule : NancyModule
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger _logger;
        private readonly IAgeComputingFactory _ageComputingFactory;

        public PersonsModule(IPersonRepository personRepository, ILogger logger, IAgeComputingFactory ageComputingFactory) : base("/api/v1/")
        {
            _personRepository = personRepository;
            _logger = logger;
            _ageComputingFactory = ageComputingFactory;

            Get("/persons/{id:guid}", GetPerson);
        }

        private object GetPerson(dynamic parameters)
        {
            Person requireePerson = _personRepository.Find(Guid.Parse(parameters.id));
            Person result = null;

            if (requireePerson != null)
            {
                result = new Person(_ageComputingFactory)
                {
                    Birthday = requireePerson.Birthday,
                    GUID = requireePerson.GUID,
                    Id = requireePerson.Id,
                    Name = requireePerson.Name
                };

                var jsonResult = JsonSerializer.Serialize(result, typeof(Person));

                _logger.Information("[{Timestamp:HH:mm:ss}]", jsonResult);
                return jsonResult;
            }

            _logger.Information("[{Timestamp:HH:mm:ss}] didn't found");
            return null;
        }
    }
}

using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using Persons.Abstractions.Models;
using Persons.Abstractions.Repositories;
using Persons.Models;
using Persons.Models.Validation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persons.Modules
{
    public class PersonsInsertModule : NancyModule
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger _logger;
        private readonly IAgeComputingFactory _ageComputingFactory;

        public PersonsInsertModule(IPersonRepository personRepository, ILogger logger, IAgeComputingFactory ageComputingFactory) : base("api/v1/")
        {
            _personRepository = personRepository;
            _logger = logger;
            _ageComputingFactory = ageComputingFactory;

            Post("/persons", InsertPerson);
        }

        private object InsertPerson(dynamic newPerson)
        {
            ViewModelPerson userInfo = this.Bind<ViewModelPerson>();
            ModelValidationResult validationResult = this.Validate(userInfo);

            if (!validationResult.IsValid)
            {
                _logger.Error("[{Timestamp:HH:mm:ss} is not validated]", JsonSerializer.Serialize(userInfo, typeof(ViewModelPerson)));

                return Negotiate.WithModel(validationResult).WithStatusCode(HttpStatusCode.BadRequest);
            }

            var inserteePerson = new Person(_ageComputingFactory) { GUID = Guid.NewGuid().ToString("D"), Birthday = DateTime.Parse(userInfo.Birthday), Name = userInfo.Name};

            _personRepository.Insert(inserteePerson);

            _logger.Information("[{Timestamp:HH:mm:ss} added to database]", JsonSerializer.Serialize(inserteePerson, typeof(Person)));

            // Created + заголовок Location /api/v1/persons/{person_id}, если команда выполнена
            return $"Created {HttpStatusCode.OK} {ModulePath}/persons/{inserteePerson.GUID}";
        }
    }
}

using LBMLibrary.Entity;
using LBMLibrary.Business;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace LBMLibrary.Service.Implementation
{
    public class PersonService : BaseService
    {
        private readonly PersonBusiness _personBusiness;

        public PersonService(PersonBusiness personBusiness)
        {
            _personBusiness = personBusiness;
        }

        public async Task<bool> AddAsync(Person person)
        {
            return await base.AddAsync(person, _personBusiness);
        }

        public async Task<Person> GetAsync(Expression<Func<Person, bool>>? expression = null)
        {
            return await base.GetAsync(_personBusiness, expression);
        }

        public async Task<IReadOnlyCollection<Person>> ListAsync(Expression<Func<Person, bool>>? expression = null)
        {
            return await base.ListAsync(_personBusiness, expression);
        }

        public bool Update(Person person)
        {
            return base.Update(person, _personBusiness);
        }

        public async Task<bool> CommitAsync(bool commit, bool clearTrack = false)
        {
            return await base.CommitAsync(_personBusiness, commit, clearTrack);
        }

        public bool IsValid(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.Name) && string.IsNullOrWhiteSpace(person.Nickname))
            {
                throw new ArgumentNullException("Campo Nome e Apelido vazios, preencha qualquer um dos campos ou ambos");
            }

            return true;
        }

        public IEnumerable<EntityEntry> TrackedItens()
        {
            IEnumerable<EntityEntry> trackedItens = TrackedItensList(_personBusiness);
            return trackedItens;
        }

        public void ClearTrack()
        {
            ClearTrack(_personBusiness);
        }

        public bool ObjectIsUnchanged(Guid id)
        {
            return base.ObjectIsUnchanged<Person, PersonBusiness>(_personBusiness, id);
        }

        public bool PersonIsUnchanged(Person personOriginal, Person personEdited)
        {
            return (personOriginal.Name == personEdited.Name) && (personOriginal.Nickname == personEdited.Nickname);
        }
    }
}

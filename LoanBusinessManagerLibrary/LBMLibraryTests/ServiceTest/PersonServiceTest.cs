using FBMLibrary.Repository;

namespace FBMLibraryTests.ServiceTest
{
    public class PersonServiceTest
    {
        public FBMContext Context { get; set; }
        public PersonService personService;

        [TearDown]
        public void TearDown()
        {
            Context.Database.EnsureDeleted();
        }

        [SetUp]
        public async Task SetUp()
        {
            Context = await TestsSettings.GetContext(DatabaseOptions.InMemoryDatabase);
            
            Context.Database.EnsureCreated();

            IFBMUoW uow = new FBMUoW(Context);
            PersonBusiness personBusiness = new PersonBusiness(uow);
            personService = new PersonService(personBusiness);
        }

        [Test]
        public async Task Add_NewPerson_Success()
        {
            #region Arrange
            Person person = TestsSettings.GetPerson();
            #endregion

            #region Act
            bool created = await personService.AddAsync(person);
            #endregion

            #region Assert
            Assert.IsTrue(created);
            #endregion
        }

        [Test]
        public async Task Get_Person_Success()
        {
            #region Arrange
            Person person = TestsSettings.GetPerson();
            await personService.AddAsync(person);
            #endregion

            #region Act
            Person personFromDb = await personService.GetAsync(x => x.Id == person.Id);
            #endregion

            #region Assert
            Assert.That(personFromDb.Id, Is.EqualTo(person.Id));
            Assert.That(personFromDb.Name, Is.EqualTo(person.Name));
            Assert.That(personFromDb.Nickname, Is.EqualTo(person.Nickname));
            #endregion
        }

        [Test]
        public async Task Update_Person_Success()
        {
            #region Arrange
            Person person = TestsSettings.GetPerson();
            await personService.AddAsync(person);

            Person personBeforeUpdate = await personService.GetAsync(x => x.Id == person.Id);
            personBeforeUpdate.Name = "Novo Nome";
            personBeforeUpdate.Nickname = "Novo Apelido";
            personBeforeUpdate.PaymentStatus = PaymentStatus.Quitado;
            #endregion

            #region Act
            bool updated = personService.Update(personBeforeUpdate);

            Person personAfterUpdate = await personService.GetAsync(x => x.Id == person.Id);
            #endregion

            #region Assert
            Assert.IsTrue(updated);
            Assert.That(personBeforeUpdate.Id, Is.EqualTo(personAfterUpdate.Id));
            Assert.That(personBeforeUpdate.Name, Is.EqualTo(personAfterUpdate.Name));
            Assert.That(personBeforeUpdate.Nickname, Is.EqualTo(personAfterUpdate.Nickname));
            #endregion
        }
    }
}

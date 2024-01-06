using FBMLibraryTests.ConfigTests;
using JBMDatabase.Enum;
using Microsoft.EntityFrameworkCore;

namespace FBMLibraryTests
{
    public class Tests
    {
        //public FBMContextTest FBMContext { get; set; }
        public DbContext FBMContext { get; set; }

        [SetUp]
        public async Task Setup()
        {
            FBMContext = await TestsSettings.GetContext(DatabaseOptions.SqlServer);
        }

        [Test]
        public void CREATE_Database()
        {
            bool created = FBMContext.Database.EnsureCreated();
            Assert.That(created, Is.True);
        }

        [Test]
        public void DELETE_Database()
        {
            bool deleted = FBMContext.Database.EnsureDeleted();
            Assert.That(deleted, Is.True);
        }

        [Test]
        public void DELETE_THAN_CREATE_Database()
        {
            bool deleted = FBMContext.Database.EnsureDeleted();
            bool created = FBMContext.Database.EnsureCreated();
            Assert.That(deleted && created, Is.True);
        }
    }
}
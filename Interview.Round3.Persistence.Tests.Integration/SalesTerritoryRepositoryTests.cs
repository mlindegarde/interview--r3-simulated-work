using System.Collections.Generic;
using FluentAssertions;
using Interview.Round3.Model;
using Xunit;

namespace Interview.Round3.Persistence.Tests.Integration
{
    // Unit tests give us a quick and easy way to see if our code works.  It can
    // be very tedious running the program and navigating to the part of the project
    // that runs the new code you just wrote.  This saves time and can prevent
    // regression isssues.
    public class SalesTerritoryRepositoryTests
    {
        #region Member Variables
        private const string ConnectionString = "Data Source=10.5.0.13;Initial Catalog=Adventureworks; Persist Security Info=False; User Id=InterviewUser; Password=InterviewUser;";

        // The subject under test (SUT)
        private readonly SalesTerritoryRepository _sut;
        #endregion

        #region Setup
        // The constructor gets called by the XUnit test runner proper to executing
        // each test method.  This means each test mothod will have a new instance
        // of the repository.
        public SalesTerritoryRepositoryTests()
        {
            _sut = new SalesTerritoryRepository(ConnectionString);
        }
        #endregion

        #region Tests
        [Fact]
        public void should_load_all_sales_territories()
        {
            // arrange
            List<SalesTerritory> result;

            // act
            result = _sut.GetAllTerritories();

            // assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(11);
            result[0].Geographies.Should().NotBeNullOrEmpty();
        }
        #endregion
    }
}

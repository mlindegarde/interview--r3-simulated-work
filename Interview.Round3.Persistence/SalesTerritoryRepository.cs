using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interview.Round3.Model;

namespace Interview.Round3.Persistence
{
    // Inherit the BaseRepository which simplifies the code to read from a SQL
    // database.  Notice that the BaseRepository has methods that support the
    // async / await opperators.  If you are comfortable using them, I would
    // suggesting taking that approach.
    //
    // When adding your code to read from the database, please keep the single
    // responsibility principle in mind.  If you aren't loading data specificly
    // related to sales territories, does it belong here?

    public sealed class SalesTerritoryRepository : BaseRepository
    {
        #region Constructor
        public SalesTerritoryRepository(string connectionString)
            : base(connectionString)
        {
        }
        #endregion

        #region Read Methods
        // This method will load all territories.  If we wanted to support filtering
        // we would want to create a new method call something like:
        //   GetTerritoriesByCountry(int countryId)
        // The method's parameters would then get passed into the SqlCommand.Parameters
        // collection where you see "null" below.
        public List<SalesTerritory> GetAllTerritories()
        {
            // We tend to make heavy use of lambdas.  The second and third
            // parameters in the ReadMany call use them to populate the parameters
            // of the SqlCommand object and to actually read the object from the
            // database.

            return 
                ReadMany(@"
                    SELECT
                        SalesTerritoryKey AS Id,
                        SalesTerritoryRegion AS Region,
                        SalesTerritoryCountry AS Country,
                        SalesTerritoryGroup AS [Group]
                    FROM DimSalesTerritory;",
                    null,
                    ReadSalesTerritory) // <- lambda expression syntax
                    .Select(
                        st =>
                        {
                            st.Geographies = GetGeographiesBySalesTerritory(st.Id);
                            return st;
                        }).ToList();
        }

        public List<Geography> GetGeographiesBySalesTerritory(int salesTerritoryId)
        {
            return
                ReadMany(@"
                    SELECT
                        GeographyKey AS Id,
                        City,
                        StateProvinceCode,
                        StateProvinceName,
                        CountryRegionCode,
                        EnglishCountryRegionName AS CountryRegionName,
                        IpAddressLocator
                    FROM DimGeography
                    WHERE SalesTerritoryKey = @SalesTerritoryId;",
                    parameters => parameters.AddWithValue("SalesTerritoryId", salesTerritoryId),
                    ReadGeography);
        }
        #endregion

        #region Utility Methods
        private SalesTerritory ReadSalesTerritory(SqlDataReader reader)
        {
            return new SalesTerritory
            {
                Id = (int)reader["Id"],
                Region = reader["Region"].ToString(),
                Country = reader["Country"].ToString(),
                Group = reader["Group"].ToString(),
            };
        }

        private Geography ReadGeography(SqlDataReader reader)
        {
            return new Geography
            {
                Id = (int)reader["Id"],
                City = reader["City"].ToString(),
                StateProvinceCode = reader["StateProvinceCode"].ToString(),
                StateProvinceName = reader["StateProvinceName"].ToString(),
                CountryRegionCode = reader["CountryRegionCode"].ToString(),
                CountryRegionName = reader["CountryRegionName"].ToString(),
                IpAddressLocator = reader["IpAddressLocator"].ToString()
            };
        }
        #endregion
    }
}

namespace Interview.Round3.Web.Models
{
    public class SalesTerritoryViewModel
    {
        #region Ids
        public int SalesTerritoryId { get; set; }
        public int GeographyId { get; set; }
        #endregion

        #region SalesTerritory Properties
        public string Country { get; set; }
        public string Region { get; set; }
        #endregion

        #region Geography Properties
        public string City { get; set; }
        #endregion
    }
}

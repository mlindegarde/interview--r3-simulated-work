namespace Interview.Round3.Model
{
    public class Geography
    {
        #region Ids
        public int Id { get; set; }
        #endregion

        #region Properties
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string StateProvinceName { get; set; }
        public string CountryRegionCode { get; set; }
        public string CountryRegionName { get; set; }
        public string IpAddressLocator { get; set; }
        #endregion
    }
}

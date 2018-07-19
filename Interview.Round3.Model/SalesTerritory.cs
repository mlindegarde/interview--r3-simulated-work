using System.Collections.Generic;

namespace Interview.Round3.Model
{
    // This is the domain model for the sales territory data.  Whenever you are
    // working with data it should happen in terms of the domain model.  In this
    // case the domain model closely matches the database schema.  Typically
    // you won't display domain models: instead use ViewModels that are defind
    // closer to the UI code.

    public class SalesTerritory
    {
        #region Ids
        public int Id { get; set; }
        #endregion

        #region Propreties
        public string Region { get; set; }
        public string Country { get; set; }
        public string Group { get; set; }
        #endregion

        #region Navigation Properties
        public List<Geography> Geographies { get; set; }
        #endregion
    }
}

using System.ComponentModel;

namespace assessment_platform_developer.Models {

    public enum CanadianProvinces {
        [Description("Select Province")] SelectProvince,
        Alberta,
        [Description("British Columbia")] BritishColumbia,
        Manitoba,
        NewBrunswick,
        [Description("Newfoundland and Labrador")] NewfoundlandAndLabrador,
        [Description("Nova Scotia")] NovaScotia,
        Ontario,
        [Description("Prince Edward Island")] PrinceEdwardIsland,
        Quebec,
        Saskatchewan,
        [Description("Northwest Territories")] NorthwestTerritories,
        Nunavut,
        Yukon
    }
}
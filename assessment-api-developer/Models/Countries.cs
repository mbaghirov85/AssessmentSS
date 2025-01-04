using System.ComponentModel;

namespace assessment_platform_developer.Models {

    public enum Countries {
        [Description("Select country")] SelectCountry,
        Canada,
        [Description("United States")] UnitedStates
    }
}
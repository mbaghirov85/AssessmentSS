using System.ComponentModel;

namespace AssessmentPlatformDeveloper.Models {

    public enum Countries {
        [Description("Select country")] SelectCountry,
        Canada,
        [Description("United States")] UnitedStates
    }
}
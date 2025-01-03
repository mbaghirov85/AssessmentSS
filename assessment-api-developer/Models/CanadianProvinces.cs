﻿using System.ComponentModel;

namespace AssessmentPlatformDeveloper.Models {

    public enum CanadianProvinces {
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AssessmentPlatformDeveloper.Models
{
    public enum Countries
    {
        Canada,
        [Description("United States")] UnitedStates
    }
}
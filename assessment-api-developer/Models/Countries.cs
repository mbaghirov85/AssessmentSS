using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace assessment_platform_developer.Models
{
    public enum Countries
    {
        Canada,
        [Description("United States")] UnitedStates
    }
}
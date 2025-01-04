using System;
using System.ComponentModel;
using System.Reflection;

namespace assessment_platform_developer.Helpers {

    public static class EnumExtensions {

        /// <summary>
        /// Retrieves the description attribute of an enum value.
        /// If no description is found, returns the enum name.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The description of the enum value or its name if no description is provided.</returns>
        public static string GetEnumDescription(Enum value) {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }
    }
}
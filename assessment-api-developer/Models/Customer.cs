using System;
using Newtonsoft.Json;

namespace AssessmentPlatformDeveloper.Models {

    [Serializable]
    public class Customer {

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Zip")]
        public string Zip { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Notes")]
        public string Notes { get; set; }

        [JsonProperty("ContactName")]
        public string ContactName { get; set; }

        [JsonProperty("ContactPhone")]
        public string ContactPhone { get; set; }

        [JsonProperty("ContactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty("ContactTitle")]
        public string ContactTitle { get; set; }

        [JsonProperty("ContactNotes")]
        public string ContactNotes { get; set; }
    }
}
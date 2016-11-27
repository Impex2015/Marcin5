using System.Collections.Generic;
using Newtonsoft.Json;

namespace Marcin5.entity
{
    public class CustomerModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("custom_variables")]
        public List<object> CustomVariables { get; set; }
    }
}
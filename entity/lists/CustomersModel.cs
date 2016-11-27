using System.Collections.Generic;
using Newtonsoft.Json;

namespace Marcin5.entity
{
    public abstract class CustomersModel
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("items")]
        public List<CustomerModel> Items { get; set; }
    }
}
   

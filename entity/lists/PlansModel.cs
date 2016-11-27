using System.Collections.Generic;
using iugu.net.Entity;
using Newtonsoft.Json;

namespace Marcin5.entity.lists
{
    public class PlansModel
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("items")]
        public List<PlanModel> Items { get; set; }
    }
}

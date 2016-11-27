using System.Collections.Generic;
using iugu.net.Entity;
using Newtonsoft.Json;

namespace Marcin5.entity.lists
{
    public class InvoicesModel
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("items")]
        public List<InvoiceModel> Items { get; set; }
    }
}

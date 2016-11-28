using System.Collections.Generic;
using iugu.net.Entity;
using Newtonsoft.Json;

namespace Marcin5.response
{
    public class Facets
    {
        [JsonProperty("facets")]
        public List<Facet> facets;
    }

    public class Facet
    {
        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("count")]
        public string Count { get; set; }
    }

    public class SubscriptionListResponse
    {
        [JsonProperty("facets")]
        public Facet Facets { get; set; }

        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("items")]
        public List<SubscriptionModel> Items { get; set; }

    }
}

//    public class Feat
//    {
//        [JsonProperty("name")]
//        public string name { get; set; }
//        [JsonProperty("value")]
//        public int value { get; set; }
//    }

//    public class SubscriptionFeatures
//    {
//        public Feat feat { get; set; }
//    }

//    public class SubscriptionSubitem
//    {
//        public string id { get; set; }
//        public string description { get; set; }
//        public int quantity { get; set; }
//        public int price_cents { get; set; }
//        public string price { get; set; }
//        public string total { get; set; }
//        public bool? recurrent { get; set; }
//    }

//    public class SubscriptionLog
//    {
//        public string id { get; set; }
//        public string description { get; set; }
//        public string notes { get; set; }
//        public string created_at { get; set; }
//    }

//    class SubscriptionListResponseMessage
//    {
//        [JsonProperty("id")]
//        public string id { get; set; }
//        [JsonProperty("suspend")]
//        public bool suspended { get; set; }
//        [JsonProperty("plan_identifier")]
//        public string plan_identifier { get; set; }
//        [JsonProperty("price_cents")]
//        public int price_cents { get; set; }
//        [JsonProperty("currency")]
//        public string currency { get; set; }

//        [JsonPropertyCollection("features")]
//        public SubscriptionFeatures features { get; set; }


//        [JsonProperty("expires_at")]
//        public object expires_at { get; set; }
//        [JsonProperty("created_at")]
//        public string created_at { get; set; }
//        [JsonProperty("updated_at")]
//        public string updated_at { get; set; }
//        [JsonProperty("customer_name")]
//        public string customer_name { get; set; }
//        [JsonProperty("customer_email")]
//        public string customer_email { get; set; }
//        [JsonProperty("cycled_at")]
//        public object cycled_at { get; set; }
//        [JsonProperty("credits_min")]
//        public int credits_min { get; set; }
//        [JsonProperty("credits_cycle")]
//        public object credits_cycle { get; set; }
//        [JsonProperty("customer_id")]
//        public string customer_id { get; set; }
//        [JsonProperty("plan_name")]
//        public string plan_name { get; set; }
//        [JsonProperty("customer_ref")]
//        public string customer_ref { get; set; }
//        [JsonProperty("plan_ref")]
//        public string plan_ref { get; set; }
//        [JsonProperty("active")]
//        public bool active { get; set; }
//        [JsonProperty("in_trial")]
//        public object in_trial { get; set; }
//        [JsonProperty("credits")]
//        public int credits { get; set; }
//        [JsonProperty("credits_based")]
//        public bool credits_based { get; set; }

//        [JsonProperty("recent_invoices")]
//        public object recent_invoices { get; set; }

//        [JsonProperty("")]
//        public List<SubscriptionSubitem> subitems { get; set; }
//        [JsonProperty("")]
//        public List<SubscriptionLog> logs { get; set; }
//        public List<CustomVariables> custom_variables { get; set; }
//    }
//}

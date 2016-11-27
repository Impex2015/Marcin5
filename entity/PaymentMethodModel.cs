namespace Marcin5.entity
{
    // TODO: Precisa de documentação
    public class PaymentMethodModel
    {
        public string customer_id { get; set; }
        public PaymentMethodData data { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string item_type { get; set; }
    }

    // TODO: Precisa de refatoração, nomes fora do padrão .Net, sem documentação também
    public class PaymentMethodData
    {
        public string brand { get; set; }
        public string display_number { get; set; }
        public string holder_name { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }
}

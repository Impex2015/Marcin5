using System.Collections.Generic;
using System.Threading.Tasks;
using Marcin5.entity;
using Marcin5.entity.lists;

namespace Marcin5.lib
{
    public class PaymentMethod : APIResource
    {
        private string CustomerID { get; }
        
        public PaymentMethod(string customerid)
        {
            CustomerID = customerid;
            BaseURI = $"/customers/{CustomerID}/payment_methods";
        }
        
        public async Task<List<PaymentMethodModel>> GetAsync()
        {
            var retorno = await GetAsync<List<PaymentMethodModel>>().ConfigureAwait(false);
            return retorno;
        }
        
        public async Task<PaymentMethodModel> GetAsync(string id)
        {
            var retorno = await GetAsync<PaymentMethodModel>(id).ConfigureAwait(false);
            return retorno;
        }

        /// <summary>
        /// Cria uma Forma de Pagamento de Cliente.
        /// </summary>
        /// <param name="token">(opcional)	Token de Pagamento, pode ser utilizado em vez de enviar os dados da forma de pagamento</param>
        /// <param name="description">Descrição</param>
        /// <param name="setAsDefault">(opcional)	Transforma a forma de pagamento na padrão do cliente</param>
        public async Task<PaymentMethodModel> CreateAsync(string token, string description, bool? setAsDefault)
        {
            var paymentmethod = new
            {
                description = description,
                set_as_default = setAsDefault,
                token = token
            };
            var retorno = await PostAsync<PaymentMethodModel>(paymentmethod).ConfigureAwait(false);
            return retorno;
        }
        
        public async Task<PaymentMethodModel> DeleteAsync(string id)
        {
            var retorno = await DeleteAsync<PaymentMethodModel>(id).ConfigureAwait(false);
            return retorno;
        }

        public async Task<PaymentMethodModel> PutAsync(string id, PaymentMethodModel model)
        {
            var retorno = await PutAsync<PaymentMethodModel>(id, model).ConfigureAwait(false);
            return retorno;
        }
    }
}

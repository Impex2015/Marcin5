using System.Threading.Tasks;
using Marcin5.request;
using Marcin5.response;

namespace Marcin5.lib
{
    /// <summary>
    /// O processo para cobrança transparente funciona da seguinte maneira:
    /// Primeiramente, os dados de cartão de crédito do cliente são enviados a Iugu através que conexão segura SSL.A Iugu então retorna um token que representa o meio de pagamento desse cliente.
    /// Esse token é então utilizado para que seja feita uma cobrança através deste meio de pagamento.
    /// Pronto! Pagamento efetuado!
    /// </summary>
    public class PaymentToken : APIResource
    {
        public PaymentToken()
        {
            BaseURI = "/payment_token";
        }
        
        /// <summary>
        /// O Token é uma representação do meio de pagamento do cliente (por ex: seu cartão de crédito), sendo totalmente seguro, de forma que não é possível que 
        /// alguém consiga as informações do cartão de crédito do cliente utilizando esse token. O token é gerado para uma transação específica, tornando-o ainda mais seguro.
        /// <see cref="https://iugu.com/referencias/api#tokens-e-cobranca-direta"/>
        /// </summary>
        /// <param name="requestMessage">Parametros de entrada da requestMessage</param>
        /// <returns>Resposta da Api para o PaymentToken</returns>
        public async Task<PaymentTokenResponseMessage> CreateAsync(PaymentTokenRequestMessage requestMessage)
        {
            var retorno = await PostAsync<PaymentTokenResponseMessage>(requestMessage).ConfigureAwait(false);
            return retorno;
        }
    }
}

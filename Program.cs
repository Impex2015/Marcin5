using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iugu.net;
using iugu.net.Entity;
using iugu.net.Request;
using iugu.net.Response;
using Marcin5.entity;
using Marcin5.lib;
using Marcin5.request;
using Marcin5.response;
using Marcin5.entity.lists;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentMethod = Marcin5.lib.PaymentMethod;

namespace Marcin5
{
    class Program
    {
        private static readonly string APP_KEY = ConfigurationManager.AppSettings["iuguApiKey"];

        static void Main(string[] args)
        {
            #region Cleanup TEST environment

            //DeleteAllSubscriptions();
            //CancelAllInvoices();
            //DeleteAllInvoices();
            //DeleteAllPlans();
            //DeleteAllCustomers();

            #endregion

            //Create Customer
            //var customer = CreateCustomer();
            //Create Payment Method
            //var paymentMethod = CreatePaymentMethod(customer.ID);
            //Attach Payment Method to Customer
            //var result = AttachPaymentMethod(customer.ID, paymentMethod.Id, "My First Credit Card");
            //Create a second payment method
            //var paymentMethod2 = CreatePaymentMethod(customer.ID);
            //Attach Payment Method to Customer
            //var result2 = AttachPaymentMethod(customer.ID, paymentMethod2.Id, "My Second Credit Card");
            //Chage the customer/card this will charge the first card and create a Paid invoice
            //var chargeResult = ChargeCard(customer, result.id);


            //Read Funcionalities
            //Look-up a customer
            var customerInfo = new Customer().GetAsync("6D0122D8B76C44FB8E5123A372951C98").Result;
            var custJson = JsonConvert.SerializeObject(customerInfo);
            var custJsonFormatted = JToken.Parse(custJson).ToString(Formatting.Indented);
            Console.WriteLine(custJsonFormatted);
            
            //Look-up payment methods for this customer
            var paymentMethods = new PaymentMethod("6D0122D8B76C44FB8E5123A372951C98").GetAsync().Result;
            foreach (var paymentMethod in paymentMethods)
            {
                var json = JsonConvert.SerializeObject(paymentMethod);
                var jsonFormatted = JToken.Parse(json).ToString(Formatting.Indented);
                Console.WriteLine(jsonFormatted);
            }
            
            
            Console.ReadLine();

        }

        private static CustomerModel CreateCustomer()
        {
            //Create a customer
            var custom = new List<CustomVariables>
            {
                new CustomVariables {name = "Tipo", value = "Pizzaria"},
                new CustomVariables {name = "Gerente", value = "Fernando Chilvarguer"}
            };

            var crm = new CustomerRequestMessage
            {
                CustomVariables = custom,
                Name = "Luiz Chilvarguer",
                CpfOrCnpj = "211.537.258-19",
                Email = "fernando@impex.com",
                Notes = "This is the best customer ever"
            };

            using (var customer = new Customer())
            {
                var myClient = customer.CreateAsync(crm).Result;
                return myClient;
            }
        }
        private static PaymentTokenResponse CreatePaymentMethod(string apiToken)
        {
            var paymentInfo = new PaymentInfoModel
            {
                FirstName = "FernandoCC",
                LastName = "ChilvarguerCC",
                Month = "01",
                Year = "2020",
                Number = "4111111111111111",
                VerificationValue = "222"
            };

            var paymentMethodRequest = new PaymentTokenRequestMessage
            {
                AccountId = apiToken,
                Method = "credit_card",
                PaymentData = paymentInfo,
                Test = true
            };

            using (var paymentMethod = new PaymentToken())
            {
                var myToken = paymentMethod.CreateAsync(paymentMethodRequest).Result;
                return myToken;
            }
        }
        private static PaymentMethodModel AttachPaymentMethod(string customerID, string paymentToken, string cardName)
        {
            var paymentMethod = new PaymentMethod(customerID);
            var paymentMethodModel = paymentMethod.CreateAsync(paymentToken, cardName, true).Result;
            return paymentMethodModel;
        }
        private static ChargeResponseMessage ChargeCard(CustomerModel customer, string paymentToken)
        {
            var chargeRequest = new ChargeRequestMessage
            {
                CustomerId = customer.ID,
                CustomerPaymentMethodId = paymentToken,
                Email = customer.Email,
                InvoiceItems =new InvoiceItem[]{new InvoiceItem {Description = "1 Coupon Campaign", PriceCents = 8900, Quantity = 8}},
                PayerCustomer = new PayerModel() { Name = "Restaurante ABC", CpfOrCnpj = "222.222.222/01", Address = new AddressModel() {City = "São Paulo", Country = "Brasil", State = "São Paulo", Street = "Rua Caropa", Number = "90", ZipCode = "05447000"}, PhonePrefix = "11", Phone = "3021-0999", Email = customer.Email},
                DiscountCents = 10800
            };

            ChargeResponseMessage chargeTokenResponse;

            using (var apiClient = new Charge())
            {
                chargeTokenResponse = apiClient.CreateAsync(chargeRequest).Result;
            }
            return chargeTokenResponse;
        }

        #region Cleanup TEST environment methods

        private static void DeleteAllSubscriptions()
        {
            //Get all subscriptions ID
            var x = new Subscription();
            var subscriptions = x.GetAsync<SubscriptionListResponse>().Result;
            foreach (var subs in subscriptions.Items)
            {
                DeleteSubscription(subs.id);
            }
        }

        private static void DeleteAllCustomers()
        {
            //Get all subscriptions ID
            var x = new Customer();
            var customers = x.GetAsync().Result;
            foreach (var cust in customers.Items)
            {
                DeleteCustomer(cust.ID);
            }
        }

        private static void DeleteAllPlans()
        {
            var plan = new Plans();
            var plans = plan.GetAsync().Result;
            foreach (var pl in plans.Items)
            {
                DeletePlan(pl.id);
            }
        }

        private static void CancelAllInvoices()
        {
            var x = new Invoice();
            var invoices = x.GetAsync().Result;
            foreach (var item in invoices.Items)
            {
                CancelInvoice(item.id);
            }
        }

        private static void DeleteAllInvoices()
        {
            var x = new Invoice();
            var invoices = x.GetAsync().Result;
            foreach (var item in invoices.Items)
            {
                if (item.status == "canceled")
                    DeleteInvoice(item.id);
            }
        }

        private static void DeleteSubscription(string ID)
        {
            var subs = new Subscription();
            var x = subs.DeleteAsync(ID).Result;
        }

        private static void DeleteCustomer(string ID)
        {
            var subs = new Customer();
            var x = subs.DeleteAsync(ID).Result;
        }

        private static void DeletePlan(string id)
        {
            var plan = new Plans();
            var planModel = plan.DeleteAsync(id).Result;
        }

        private static void DeleteInvoice(string id)
        {
            var invoice = new Invoice();
            using (invoice)
            {
                try
                {
                    var inv = invoice.DeleteAsync(id).Result;
                }
                catch (Exception)
                {
                }
            }
        }

        private static void CancelInvoice(string id)
        {
            var x = new Invoice();
            try
            {
                var z = x.CancelAsync(id).Result;
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
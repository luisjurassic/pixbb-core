using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixBB.Core;
using PixBB.Core.Models;

namespace PixBB.Test
{
    [TestClass]
    public class GeneralTests
    {
        private PixClient Client { get; }

        public GeneralTests()
        {
            Client = Config.GetPixClient();
        }

        [TestMethod]
        public async Task CriarCobranca()
        {
            try
            {
                var retorno = await Client.ChargeAsync(new Billing()
                {
                    Calendar = new Calendar(3600),
                    Price = new Price(10),
                    Key = "7f6844d0-de89-47e5-9ef7-e0a35a681615",
                    PayerRequest = "Serviço prestado"
                });

                Assert.IsNotNull(retorno, "A cobrança não foi criada");

                Console.WriteLine($"Cobrança registrada. txid: {retorno.TransactionId} link: {retorno.ImageContentQRcode}");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task ConsultaCobranca()
        {
            try
            {
                var retorno = await Client.GetChargeAsync("transactionId");

                Assert.IsNotNull(retorno, "A cobrança não foi encontrada");

                Console.WriteLine($"Cobrança encontrada. txid: {retorno.TransactionId}");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task RevisarCobranca()
        {
            try
            {
                var retorno = await Client.ReviewChargeAsync(new Billing()
                {
                    Calendar = new Calendar(3600),
                    Price = new Price(12),
                    TransactionId = "transactionId",
                    Key = "7f6844d0-de89-47e5-9ef7-e0a35a681615",
                    PayerRequest = "Serviço prestado"
                });

                Assert.IsNotNull(retorno, "A cobrança não foi atualizada");

                Console.WriteLine($"Cobrança atualizada. txid: {retorno.TransactionId}");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task Pagar()
        {
            try
            {
                var retorno = await Client.PayAsync("imageContentQRcode");

                Assert.IsNotNull(retorno, "Pagamento não foi realizado");

                Console.WriteLine($"Cobrança paga. e2eId: {retorno.EndToEndId}");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task ConsultaPagamento()
        {
            try
            {
                var retorno = await Client.GetPixPaidAsync("endToEndId");

                Assert.IsNotNull(retorno, "Pagamento não foi encontrado");

                Console.WriteLine($"Cobrança paga. e2eId: {retorno.EndToEndId} txid: {retorno.TransactionId}");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task Devolucao()
        {
            try
            {
                var idDevolucao = Guid.NewGuid().ToString();
                var retorno = await Client.RequestReturnBillingAsync(new ReturnBilling()
                {
                    Id = idDevolucao, //Id unico por devolução
                    EndToEndId = "endToEndId", //Esse cód. é gerado pelo PSP após o pagamento da cobrança 
                    Price = 10
                });

                Assert.IsNotNull(retorno, "Devolução não foi realziada");

                Console.WriteLine($"Cobrança devolvida. rtrId: {retorno.ReturnId} status: {retorno.Status}");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task ConsultarDevolucao()
        {
            try
            {
                var retorno = await Client.GetDevolutionAsync(new ReturnBilling()
                {
                    Id = "idDevolucao", //Id da solicitação de devolução
                    EndToEndId = "endToEndId" //Esse cód. é gerado pelo PSP após o pagamento da cobrança
                });

                Assert.IsNotNull(retorno, "Devolução não foi encontrada");
                Console.WriteLine($"Cobrança devolvida. rtrId: {retorno.ReturnId} status: {retorno.Status}");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
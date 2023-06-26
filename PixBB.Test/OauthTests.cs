using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixBB.Core;

namespace PixBB.Test
{
    [TestClass]
    public class OauthTests
    {
        private PixClient Client { get; }

        public OauthTests()
        {
            Client = Config.GetPixClient();
        }

        [TestMethod]
        public async Task Auth()
        {
            try
            {
                var retorno = await Client.AuthenticatedAsync();

                Assert.IsFalse(retorno, "Erro ao autenticar na API");
                
                Console.WriteLine($"Autenticação realizada com sucesso");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
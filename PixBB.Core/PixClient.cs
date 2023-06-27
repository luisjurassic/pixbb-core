using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixBB.Core.Enumerators;
using PixBB.Core.Exceptions;
using PixBB.Core.Models;
using PixBB.Core.Utils;

namespace PixBB.Core;

/// <summary>
/// Classe que atua como cliente para os serviços pix.
/// </summary>
public class PixClient
{
    /// <summary>
    /// Chave de api usada na autenticação do serviço.
    /// </summary>
    private PixOptions PixOptions { get; }

    /// <summary>
    /// Token de acesso para authenticação.
    /// </summary>
    private string AccessToken { get; set; }

    /// <summary>
    /// Tipo do token de acesso.
    /// </summary>
    private string AccessTokenType { get; set; }

    /// <summary>
    /// Data e hora em que o token de autenticação expira.
    /// </summary>
    private DateTime AccessExpiredToken { get; set; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="PixClient"/>.
    /// </summary>
    /// <param name="pixOptions">Chave de api usada na autenticação do serviço.</param> 
    public PixClient(PixOptions pixOptions)
    {
        PixOptions = pixOptions;
    }

    /// <summary> 
    /// Método de autenticação da API.
    /// </summary>
    /// <returns>Se autenticado com sucesso</returns>
    public bool Authenticated()
    {
        var result = AuthenticatedAsync().Result;
        return result;
    }

    /// <summary>
    /// Método de autenticação da API como uma operação assíncrona.
    /// </summary>
    /// <returns>Se autenticado com sucesso</returns>
    public async Task<bool> AuthenticatedAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(AccessToken) && AccessExpiredToken > DateTime.Now)
                return true;

            PixHttpClient client = new(PixUrlBase.GetOauthUri(PixOptions.ApiEnvironment));

            var credentials = $"{PixOptions.ClientId}:{PixOptions.ClientSecret}";
            var encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            client.AuthenticationHeader = new AuthenticationHeaderValue("Basic", encodedAuth);

            var retorno = await client
                .PostAsync<AccessToken>(new Dictionary<string, string>
                {
                    {"grant_type", PixOptions.GrantType},
                    {"scope", string.Join(" ", PixOptions.Scope)},
                }, MimeTypes.Form);

            AccessTokenType = retorno.TokenType;
            AccessToken = retorno.Token;
            AccessExpiredToken = DateTime.Now.AddSeconds(retorno.ExpiresIn);

            return false;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Criar uma cobrança imediata, neste caso, o txid deve ser definido pelo PSP.
    /// </summary>
    /// <param name="billing">Dados para criação da cobrança instantânea</param>
    /// <returns>Dados da cobrança</returns>
    public Billing Charge(Billing billing)
    {
        var result = ChargeAsync(billing).Result;
        return result;
    }

    /// <summary>
    /// Criar uma cobrança imediata, neste caso, o txid deve ser definido pelo PSP como uma operação assíncrona.
    /// </summary>
    /// <param name="billing">Dados para criação da cobrança instantânea</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <returns>Dados da cobrança</returns>
    public async Task<Billing> ChargeAsync(Billing billing, CancellationToken cancellationToken = default)
    {
        try
        {
            await AuthenticatedAsync();

            var client = CreateInstance();

            var result = await client
                .PrepareClient("cob/", $"gw-dev-app-key={PixOptions.DeveloperApplicationKey}")
                .PutAsync<Billing>(billing, MimeTypes.Json, cancellationToken);

            return result;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Consulta uma cobrança imediata pelo transactionId.
    /// </summary>
    /// <param name="transacaoId">Identificador da transação</param>
    /// <returns>Dados da cobrança</returns>
    public Billing GetCharge(string transacaoId)
    {
        var result = GetChargeAsync(transacaoId).Result;
        return result;
    }

    /// <summary>
    /// Consulta uma cobrança imediata pelo transactionId como uma operação assíncrona.
    /// </summary>
    /// <param name="transacaoId">Identificador da transação</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <returns>Dados da cobrança</returns>
    public async Task<Billing> GetChargeAsync(string transacaoId, CancellationToken cancellationToken = default)
    {
        try
        {
            await AuthenticatedAsync();

            var client = CreateInstance();

            var result = await client
                .PrepareClient($"cob/{transacaoId}", $"gw-dev-app-key={PixOptions.DeveloperApplicationKey}")
                .GetAsync<Billing>(MimeTypes.Json, cancellationToken);

            return result;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza uma cobrança imediata, neste caso, o txid deve ser definido pelo PSP.
    /// </summary>
    /// <param name="billing">Dados para criação da cobrança instantânea</param>
    /// <returns>Dados da cobrança</returns>
    public Billing ReviewCharge(Billing billing)
    {
        var result = ReviewChargeAsync(billing).Result;
        return result;
    }

    /// <summary>
    /// Atualiza uma cobrança imediata, neste caso, o txid deve ser definido pelo PSP como uma operação assíncrona.
    /// </summary>
    /// <param name="billing">Dados para criação da cobrança instantânea</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <returns>Dados da cobrança</returns>
    public async Task<Billing> ReviewChargeAsync(Billing billing, CancellationToken cancellationToken = default)
    {
        try
        {
            await AuthenticatedAsync();

            var client = CreateInstance();

            var result = await client
                .PrepareClient($"cob/{billing.TransactionId}", $"gw-dev-app-key={PixOptions.DeveloperApplicationKey}")
                .PatchAsync<Billing>(billing, MimeTypes.Json, cancellationToken);

            return result;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Solicita a devolução de uma cobrança imediata, através de um e2eid do PixPaid e do ID da devolução.
    /// </summary>
    /// <param name="devolucao">Dados para a devolução da cobrança instantânea</param>
    /// <returns>Dados da cobrança</returns>
    public ReturnBillingResponse RequestReturnBilling(ReturnBilling devolucao)
    {
        var result = RequestReturnBillingAsync(devolucao).Result;
        return result;
    }

    /// <summary>
    /// Solicita a devolução de uma cobrança imediata, através de um e2eid do PixPaid e do ID da devolução como uma operação assíncrona.
    /// </summary>
    /// <param name="devolucao">Dados para a devolução da cobrança instantânea</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <returns>Dados da cobrança</returns>
    public async Task<ReturnBillingResponse> RequestReturnBillingAsync(ReturnBilling devolucao, CancellationToken cancellationToken = default)
    {
        try
        {
            await AuthenticatedAsync();

            var client = CreateInstance();

            var result = await client
                .PrepareClient($"pix/{devolucao.EndToEndId}/devolucao/{devolucao.Id}", $"gw-dev-app-key={PixOptions.DeveloperApplicationKey}")
                .PutAsync<ReturnBillingResponse>(devolucao, MimeTypes.Json, cancellationToken);

            return result;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Consulta a devolução de uma cobrança imediata, através de um e2eid do PixPaid e do ID da devolução.
    /// </summary>
    /// <param name="devolucao">Dados para a devolução da cobrança instantânea</param>
    /// <returns>Dados da cobrança</returns>
    public ReturnBillingResponse GetDevolution(ReturnBilling devolucao)
    {
        var result = GetDevolutionAsync(devolucao).Result;
        return result;
    }

    /// <summary>
    /// Consulta a devolução de uma cobrança imediata, através de um e2eid do PixPaid e do ID da devolução como uma operação assíncrona.
    /// </summary>
    /// <param name="devolucao">Dados para a devolução da cobrança instantânea</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <returns>Dados da cobrança</returns>
    public async Task<ReturnBillingResponse> GetDevolutionAsync(ReturnBilling devolucao, CancellationToken cancellationToken = default)
    {
        try
        {
            await AuthenticatedAsync();

            var client = CreateInstance();

            var result = await client
                .PrepareClient($"pix/{devolucao.EndToEndId}/devolucao/{devolucao.Id}", $"gw-dev-app-key={PixOptions.DeveloperApplicationKey}")
                .GetAsync<ReturnBillingResponse>(MimeTypes.Json, cancellationToken);

            return result;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Efetua a simulação do pagamento de um QR Code gerado em homologação.
    /// Recurso exclusivo do ambiente de homologação.
    /// </summary>
    /// <param name="textQRCode">O Conteudo do campo <see cref="Billing.ImageContentQRcode"/>, recebido na resposta da criação da cobrança</param>
    /// <returns>Os dados de pagamento</returns>
    public PixPaid Pay(string textQRCode)
    {
        var result = PayAsync(textQRCode).Result;
        return result;
    }

    /// <summary>
    /// Efetua a simulação do pagamento de um QR Code gerado em homologação. Recurso exclusivo do ambiente de homologação como uma operação assíncrona.
    /// </summary>
    /// <param name="textQRCode">O Conteudo do campo <see cref="Billing.ImageContentQRcode"/>, recebido na resposta da criação da cobrança</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <returns>Os dados de pagamento</returns>
    public async Task<PixPaid> PayAsync(string textQRCode, CancellationToken cancellationToken = default)
    {
        try
        {
            await AuthenticatedAsync();

            PixHttpClient client = new PixHttpClient("https://api.sandbox.bb.com.br/testes-portal-desenvolvedor/v1")
            {
                AuthenticationHeader = new AuthenticationHeaderValue(AccessTokenType, AccessToken)
            };

            var result = await client
                .PrepareClient("boletos-pix/pagar", "gw-dev-app-key=95cad3f03fd9013a9d15005056825665")
                .PostAsync<PixPaidResponse>(new {pix = textQRCode}, MimeTypes.Json, cancellationToken);

            var Payment = await GetPixPaidAsync(result.EndToEndId, cancellationToken);

            return Payment;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Consultar o pagamento de um cobrança, através de um e2eid do PixPaid.
    /// </summary>
    /// <param name="e2eId">O Id fim a fim da transação</param>
    /// <returns>Dados do pagamento</returns>
    public PixPaid GetPixPaid(string e2eId)
    {
        var result = GetPixPaidAsync(e2eId).Result;
        return result;
    }

    /// <summary>
    /// Consultar o pagamento de um cobrança, através de um e2eid do PixPaid como uma operação assíncrona.
    /// </summary>
    /// <param name="e2eId">O Id fim a fim da transação</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <returns>Dados do pagamento</returns>
    public async Task<PixPaid> GetPixPaidAsync(string e2eId, CancellationToken cancellationToken = default)
    {
        try
        {
            await AuthenticatedAsync();

            var client = CreateInstance();

            var result = await client
                .PrepareClient($"pix/{e2eId}", $"gw-dev-app-key={PixOptions.DeveloperApplicationKey}")
                .GetAsync<PixPaid>(MimeTypes.Json, cancellationToken);

            return result;
        }
        catch (HttpException ex)
        {
            throw new PixException(ex);
        }
        catch (Exception ex)
        {
            throw new PixException(ex.Message);
        }
    }

    /// <summary>
    /// Cria uma instancia de <see cref="PixHttpClient"/> pronta para auxiliar nas requisições via HttpClient.
    /// </summary>
    /// <returns>Uma instancia de <see cref="PixHttpClient"/></returns>
    private PixHttpClient CreateInstance()
    {
        PixHttpClient client = new PixHttpClient(PixUrlBase.GetBaseUri(PixOptions.ApiEnvironment, PixOptions.ApiVersion))
        {
            AuthenticationHeader = new AuthenticationHeaderValue(AccessTokenType, AccessToken),
            Certificate = PixOptions.Certificate
        };
        return client;
    }
}
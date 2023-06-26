using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using PixBB.Core.Enumerators;
using PixBB.Core.Exceptions;

namespace PixBB.Core.Utils;

/// <summary>
/// Contem metodos para realizar a comunicação via HttpClient.
/// </summary>
internal class PixHttpClient
{
    /// <summary>
    /// Url base para a comunicação
    /// </summary>
    public Uri UrlBase { get; internal set; }

    /// <summary>
    /// Url completa para a solicitação
    /// </summary>
    public Uri Url { get; internal set; }

    /// <summary>
    /// Lista de status de erro da requisição.
    /// </summary>
    private readonly int[] _requestNotAllowedStatusRange = {400, 401, 403, 404, 422, 500, 503};

    /// <summary>
    /// Contem o tipo e a chave do token de acesso para comunicação com a API.
    /// </summary>
    public AuthenticationHeaderValue AuthenticationHeader { get; set; }

    /// <summary>
    /// Certficado de assinatura digital
    /// </summary>
    public X509Certificate2 Certificate { get; set; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="PixHttpClient"/>.
    /// </summary>
    /// <param name="urlBase">Url base da comunicação</param>
    public PixHttpClient(string urlBase)
    {
        UrlBase = new Uri(urlBase);
    }

    /// <summary>
    /// Realiza uma consulta no endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="mimeType">Tipo de dados</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T">O tipo de dados de retorno</typeparam>
    /// <returns>Os dados</returns>
    public async Task<T> GetAsync<T>(MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<T>(null, HttpMethod.Get, mimeType, cancellationToken);
        return result;
    }

    /// <summary>
    /// Realiza o envio de dados para o endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="mimeType">Tipo de dados</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T">O tipo de dados de retorno</typeparam>
    /// <returns>Os dados</returns>
    public async Task<T> PostAsync<T>(MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        return await PostAsync<T>(null, mimeType, cancellationToken);
    }

    /// <summary>
    /// Realiza o envio de dados para o endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="obj">O objeto que sera enviado no corpo da requisição</param>
    /// <param name="mimeType">Tipo de dados</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T">O tipo de dados de retorno</typeparam>
    /// <returns>Os dados</returns>
    public async Task<T> PostAsync<T>(object obj, MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<T>(obj, HttpMethod.Post, mimeType, cancellationToken);
        return result;
    }

    /// <summary>
    /// Realiza o envio de um comando de DELETE para o endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        await SendAsync(HttpMethod.Delete, cancellationToken);
    }

    /// <summary>
    /// Realiza o envio de um comando de PATCH para o endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="mimeType">Tipo de dados</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T">O tipo de dados de retorno</typeparam>
    /// <returns>Os dados</returns>
    public async Task<T> PatchAsync<T>(MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        return await PatchAsync<T>(null, mimeType, cancellationToken);
    }

    /// <summary>
    /// Realiza o envio de um comando de PATCH para o endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="obj">O objeto que sera enviado no corpo da requisição</param>
    /// <param name="mimeType">Tipo de dados</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T">O tipo de dados de retorno</typeparam>
    /// <returns>Os dados</returns>
    public async Task<T> PatchAsync<T>(object obj, MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<T>(obj, new HttpMethod("PATCH"), mimeType, cancellationToken);
        return result;
    }

    /// <summary>
    /// Realiza o envio de um comando de PUT para o endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="mimeType">Tipo de dados</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T">O tipo de dados de retorno</typeparam>
    /// <returns>Os dados</returns>
    public async Task<T> PutAsync<T>(MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        return await PutAsync<T>(null, mimeType, cancellationToken);
    }

    /// <summary>
    /// Realiza o envio de um comando de PUT para o endereço especificado na <see cref="UrlBase"/> de forma assíncrona.
    /// </summary>
    /// <param name="obj">O objeto que sera enviado no corpo da requisição</param>
    /// <param name="mimeType">Tipo de dados</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T">O tipo de dados de retorno</typeparam>
    /// <returns>Os dados</returns>
    public async Task<T> PutAsync<T>(object obj, MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<T>(obj, new HttpMethod("PUT"), mimeType, cancellationToken);
        return result;
    }

    private HttpClient CreateHttpClient()
    {
        HttpClient client = new HttpClient();
        if (Certificate != null)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            handler.ClientCertificates.Add(Certificate);

            client = new HttpClient(handler);
        }

        if (AuthenticationHeader is not null)
            client.DefaultRequestHeaders.Authorization = AuthenticationHeader;

        return client;
    }

    /// <summary>
    /// Envia uma solicitação HTTP como uma operação assíncrona.
    /// </summary>
    /// <param name="httpMethod">O protocolo HTTP</param>
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    private async Task SendAsync(HttpMethod httpMethod, CancellationToken cancellationToken = default)
    {
        string content = string.Empty;

        HttpClient client = CreateHttpClient();

        if (Url == null)
            Url = UrlBase;

        var request = new HttpRequestMessage(httpMethod, Url);

        ServicePointManager.ServerCertificateValidationCallback += (_, _, _, _) => true;
        HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                bool haveToEnsureSuccessStatusCode = false;
                if (CheckIfStatusCodeIsError(response.StatusCode))
                {
                    haveToEnsureSuccessStatusCode = true;
                    content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (haveToEnsureSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw CreateHttpException(response, content, ex);
        }
    }

    /// <summary>
    /// Envia uma solicitação HTTP como uma operação assíncrona.
    /// </summary>
    /// <param name="obj">O objeto que sera enviado no corpo da requisição</param>
    /// <param name="httpMethod">O protocolo HTTP</param>
    /// <param name="mimeType">Tipo de dados</param> 
    /// <param name="cancellationToken">Token de notificação de cancelamento de threads gerenciadas.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private async Task<T> SendAsync<T>(object obj, HttpMethod httpMethod, MimeTypes mimeType = MimeTypes.Json, CancellationToken cancellationToken = default)
    {
        string content = string.Empty;
        T result = default;

        HttpClient client = CreateHttpClient();

        if (Url == null)
            Url = UrlBase;

        var request = new HttpRequestMessage(httpMethod, Url);
        if (obj != null && mimeType == MimeTypes.Json)
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, GetMineType(mimeType));
        else if (obj != null && mimeType == MimeTypes.Form)
            request.Content = new FormUrlEncodedContent((IDictionary<string, string>) obj);

        ServicePointManager.ServerCertificateValidationCallback += (_, _, _, _) => true;
        HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
        try
        {
            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                bool haveToEnsureSuccessStatusCode = false;
                if (CheckIfStatusCodeIsError(response.StatusCode))
                {
                    haveToEnsureSuccessStatusCode = true;
                    content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (haveToEnsureSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }

            return result;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw CreateHttpException(response, content, ex);
        }
    }

    /// <summary>
    /// Verifica o status da solicitação.
    /// </summary>
    /// <param name="responseStatusCode">O status da requisição</param>
    /// <returns>Se a solicitação foi bem-sucedida</returns>
    private bool CheckIfStatusCodeIsError(HttpStatusCode responseStatusCode)
    {
        return _requestNotAllowedStatusRange.Any(r => r == (int) responseStatusCode);
    }

    /// <summary>
    /// Cria uma instancia da exceção do tipo <see cref="HttpException"/>. 
    /// </summary>
    /// <param name="response">A Mensagem de resposta, incluindo o código de status e os dados</param>
    /// <param name="content">O conteudo da resposta</param>
    /// <param name="ex">A exceção original</param>
    /// <returns>Uma instancia de <see cref="HttpException"/></returns>
    private static HttpException CreateHttpException(HttpResponseMessage response, string content, Exception ex)
    {
        Uri requestUri = null;
        string method = null;
        HttpRequestHeaders requestHeaders = null;
        var requestMessage = response.RequestMessage;
        if (requestMessage != null)
        {
            requestUri = requestMessage.RequestUri;
            method = requestMessage.Method.ToString();
            requestHeaders = requestMessage.Headers;
        }

        return new HttpException(
            requestUri,
            method,
            response.ReasonPhrase,
            requestHeaders,
            content,
            response.StatusCode,
            response.Headers,
            ex);
    }

    /// <summary>
    /// Retorna o tipo de conteudo(MIME-type) que sera usado na requisição. 
    /// </summary>
    /// <param name="mimeType">O tipo de conteudo(MIME-type)</param>
    private static string GetMineType(MimeTypes mimeType)
    {
        return mimeType switch
        {
            MimeTypes.Json => "application/json",
            MimeTypes.Form => "application/x-www-form-urlencoded",
            _ => throw new NotImplementedException()
        };
    }
}

/// <summary>
/// Contem metodos para utilizar a classe <see cref="PixHttpClient"/>.
/// </summary>
internal static class PixHttpClientHelper
{
    /// <summary>
    /// Gera a url com a chave do App.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="endPoint">O endpoint da requisição</param>
    /// <param name="query">Paramatros que seram adicionados na url</param>
    /// <returns>A url pronta pra requisição</returns>
    internal static PixHttpClient PrepareClient(this PixHttpClient client, string endPoint, params string[] query)
    {
        var url = $"{client.UrlBase.AbsoluteUri.TrimEnd('/')}/{endPoint.TrimStart('/')}";

        UriBuilder uriBuilder = new UriBuilder(url);
        var queryString = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var key in query)
        {
            var par = key.Split('=');
            if (par.Length == 2)
                queryString[par[0]] = par[1];
        }

        uriBuilder.Query = queryString.ToString();

        client.Url = uriBuilder.Uri;

        return client;
    }
}
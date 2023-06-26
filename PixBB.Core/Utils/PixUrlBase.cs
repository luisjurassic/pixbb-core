using PixBB.Core.Enumerators;

namespace PixBB.Core.Utils;

/// <summary>
/// Classe que contem as ulr's padrões conforme o ambiente definido.
/// </summary>
internal abstract class PixUrlBase
{
    /// <summary>
    /// URL padrão do serviço de sandbox.
    /// </summary>
    private const string SANDBOX_BASE_URL = "https://api.sandbox.bb.com.br";

    /// <summary>
    /// URL de autenticação padrão do serviço de sandbox.
    /// </summary>
    private const string SANDBOX_OAUTH_URL = "https://oauth.sandbox.bb.com.br";

    /// <summary>
    /// Url padrão do serviço de produção.
    /// </summary>
    private const string PRODUCTION_BASE_URL = "https://api.bb.com.br";

    /// <summary>
    /// URL de autenticação padrão do serviço de produção.
    /// </summary>
    private const string PRODUCTION_OAUTH_URL = "https://oauth.bb.com.br";

    /// <summary>
    /// Método que traz a url correta de acordo com a versão e ambiente definidos.
    /// </summary>
    /// <param name="environment">Ambiente de execução. <see cref="ApiEnvironment"/></param>
    /// <param name="version">Versão da API do PIX</param>
    /// <returns></returns>
    internal static string GetBaseUri(ApiEnvironment environment, ApiVersion version)
    {
        switch (environment)
        {
            default:
            case ApiEnvironment.Sandbox:
                return $"{SANDBOX_BASE_URL}/pix/{version.ToString().ToLower()}/";
            case ApiEnvironment.Production:
                return $"{PRODUCTION_BASE_URL}/pix/{version.ToString().ToLower()}/";
        }
    }

    /// <summary>
    /// Método que traz a url de autenticação correta de acordo com a versão e ambiente definidos.
    /// </summary>
    /// <param name="environment">Ambiente de execução. <see cref="ApiEnvironment"/></param>
    /// <returns></returns>
    internal static string GetOauthUri(ApiEnvironment environment)
    {
        switch (environment)
        {
            default:
            case ApiEnvironment.Sandbox:
                return $"{SANDBOX_OAUTH_URL}/oauth/token/";
            case ApiEnvironment.Production:
                return $"{PRODUCTION_OAUTH_URL}/oauth/token/";
        }
    }
}
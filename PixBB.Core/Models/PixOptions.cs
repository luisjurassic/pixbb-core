using System.Security.Cryptography.X509Certificates;
using PixBB.Core.Constants;
using PixBB.Core.Enumerators;

namespace PixBB.Core.Models;

/// <summary>
/// Classe responsavel pela configuração e preparação da comunicação da API.
/// </summary>
public class PixOptions
{
    /// <summary>
    /// É a credencial para acionar as APIS do Banco do Brasil.
    /// </summary>
    public string DeveloperApplicationKey { get; set; }

    /// <summary>
    /// É o identificador público e único no OAuth do Banco do Brasil.
    /// </summary>    
    public string ClientId { get; set; }

    /// <summary>
    /// É conhecido apenas para sua aplicação e o servidor de autorização. Por isso, tome muito cuidado com seu armazenamento.
    /// Em caso de suspeita de fraude, deverá acessar suas Credenciais dentro da sua Aplicação e realizar a troca do mesmo.
    /// </summary>    
    public string ClientSecret { get; set; }

    /// <summary>
    /// Typo de fluxo de requisição. Padrão 'client_credentials'.
    /// </summary>
    public string GrantType { get; set; } = "client_credentials";

    /// <summary>
    /// Permissões de acesso aos recursos da API.
    /// </summary>
    public string[] Scope { get; set; } =
    {
        PixScopes.CobRead,
        PixScopes.PixRead,
        PixScopes.CobWrite,
        PixScopes.PixWrite,
        PixScopes.CobVRead,
        PixScopes.CobVWrite
    };

    /// <summary>
    /// Tipo de ambiente.
    /// </summary>    
    public ApiEnvironment ApiEnvironment { get; set; } = ApiEnvironment.Sandbox;

    /// <summary>
    /// Versão da API do PIX.
    /// </summary>    
    public ApiVersion ApiVersion { get; set; } = ApiVersion.V1;

    /// <summary>
    /// Certficado de assinatura digital
    /// </summary>
    public X509Certificate2 Certificate { get; set; }
}
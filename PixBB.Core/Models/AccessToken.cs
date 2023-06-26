using Newtonsoft.Json;

namespace PixBB.Core.Models;

/// <summary>
/// Classe contendo dados de acesso a API
/// </summary>
public class AccessToken
{
    /// <summary>
    /// Token de autenticação
    /// </summary>
    [JsonProperty("access_token")]
    public string Token { get; set; }

    /// <summary>
    /// Tipo de token
    /// </summary>
    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    /// Tempo de vida do token, especificado em segundos a partir da data de autenticação. Default: 600
    /// </summary>
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("scope")]
    public string Scope { get; set; }
}
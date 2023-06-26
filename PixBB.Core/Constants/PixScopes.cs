namespace PixBB.Core.Constants;

/// <summary>
/// Classe com variaveis constantes de permissões
/// </summary>
public abstract class PixScopes
{
    /// <summary>
    /// Permissão para alteração de cobranças
    /// </summary>
    public const string CobWrite = "cob.write";

    /// <summary>
    /// Permissão para consulta de cobranças
    /// </summary>
    public const string CobRead = "cob.read";

    /// <summary>
    /// Permissão para alteração de cobranças com vencimento
    /// </summary>
    public const string CobVWrite = "cobv.write";

    /// <summary>
    /// Permissão para consulta de cobranças com vencimento
    /// </summary>
    public const string CobVRead = "cobv.read";

    /// <summary>
    /// Permissão para consulta de pix
    /// </summary>
    public const string PixRead = "pix.read";

    /// <summary>
    /// Permissão para alteração de PixPaid
    /// </summary>
    public const string PixWrite = "pix.write";

    /// <summary>
    /// Permissão para consulta de lotes de cobranças com vencimento
    /// </summary>
    public const string LoteCobVRead = "lotecobv.read";

    /// <summary>
    /// Permissão para alteração de lotes de cobranças com vencimento
    /// </summary>
    public const string LoteCobVWrite = "lotecobv.write";

    /// <summary>
    /// Permissão para consulta do webhook
    /// </summary>
    public const string WebhookRead = "webhook.read";

    /// <summary>
    /// Permissão para alteração do webhook
    /// </summary>
    public const string WebhookWrite = "webhook.write";

    /// <summary>
    /// Permissão para consulta de payloads
    /// </summary>
    public const string PayloadLocationRead = "payloadlocation.read";

    /// <summary>
    /// Permissão para alteração de payloads
    /// </summary>
    public const string PayloadLocationWrite = "payloadlocation.write";
}
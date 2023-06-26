using Newtonsoft.Json;

namespace PixBB.Core.Models;

/// <summary>
/// Classe que representa os dados de retorno de um pagamento.
/// </summary>
internal class PixPaidResponse
{
    /// <summary>
    /// Codigo do pagamento
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }
    
    /// <summary>
    /// Id fim a fim da transação
    /// </summary>
    [JsonProperty("texto")]
    public string EndToEndId { get; set; }
}


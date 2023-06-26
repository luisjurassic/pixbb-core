using System;
using Newtonsoft.Json;
using PixBB.Core.Converters;
using PixBB.Core.Enumerators;

namespace PixBB.Core.Models;

/// <summary>
/// Classe que representa uma devolução da cobrança imediata.
/// </summary>
public class ReturnBillingResponse
{
    /// <summary>
    /// Id gerado pelo cliente para representar unicamente uma devolução
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Id da devolução que transita na PACS004.
    /// </summary>
    [JsonProperty("rtrId")]
    public string ReturnId { get; set; }

    /// <summary>
    /// Valor a devolver
    /// </summary>
    [JsonProperty("valor")]
    [JsonConverter(typeof(DecimalFormatConverter))]
    public decimal Price { get; set; }

    /// <summary>
    /// Natureza
    /// </summary>
    [JsonProperty("natureza")]
    public string natureza { get; set; }

    /// <summary>
    /// Descrição
    /// </summary>
    [JsonProperty("descricao")]
    public string Description { get; set; }

    /// <summary>
    /// Horário da devolução
    /// </summary>
    [JsonProperty("horario")]
    public Horario Time { get; set; }

    /// <summary>
    /// Status da devolução.
    /// </summary>
    [JsonProperty("status")]
    [JsonConverter(typeof(ReturnStatusConverter))]
    public ReturnStatus Status { get; set; }

    /// <summary>
    /// Motivo
    /// </summary>
    [JsonProperty("motivo")]
    public string motivo { get; set; }
}

/// <summary>
/// Classe que representa os horários da devolução.
/// </summary>
public class Horario
{
    /// <summary>
    /// Horário no qual a devolução foi solicitada no PSP.
    /// </summary>
    [JsonProperty("solicitacao")]
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-ddTHH:mm:ss")]
    public DateTime Request { get; set; }

    /// <summary>
    /// Horário no qual a devolução foi liquidada no PSP.
    /// </summary>
    [JsonProperty("liquidacao")]
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-ddTHH:mm:ss")]
    public DateTime Liquidation { get; set; }
}
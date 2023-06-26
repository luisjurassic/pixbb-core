using System;
using Newtonsoft.Json;
using PixBB.Core.Converters;
using PixBB.Core.Enumerators;

namespace PixBB.Core.Models;

/// <summary>
/// Classe que representa os dados de um pagamento PixPaid.
/// </summary>
public class PixPaid
{
    /// <summary>
    /// Id fim a fim da transação
    /// </summary>
    [JsonProperty("endToEndId")]
    public string EndToEndId { get; set; }

    /// <summary>
    /// Identificador da transação.
    /// <para>
    /// O campo txid, obrigatório, determina o identificador da transação.
    /// O objetivo desse campo é ser um elemento que possibilite ao PSP do
    /// recebedor apresentar ao usuário recebedor a funcionalidade de conciliação de pagamentos.
    /// </para>
    /// <para>
    /// Na pacs.008, é referenciado como TransactionIdentification ou idConciliacaoRecebedor.
    /// </para>
    /// <para>
    /// Em termos de fluxo de funcionamento, o txid é lido pelo aplicativo do PSP do pagador e,
    /// depois de confirmado o pagamento, é enviado para o SPI via pacs.008.
    /// Uma pacs.008 também é enviada ao PSP do recebedor, contendo, além de
    /// todas as informações usuais do pagamento, o txid. Ao perceber um recebimento dotado de txid,
    /// o PSP do recebedor está apto a se comunicar com o usuário recebedor,
    /// informando que um pagamento específico foi liquidado.
    /// </para>
    /// <para>
    /// O txid é criado exclusivamente pelo usuário recebedor e está sob sua responsabilidade.
    /// O txid, no contexto de representação de uma cobrança, é único por CPF/CNPJ do usuário recebedor.
    /// Cabe ao PSP recebedor validar essa regra na API PixPaid.
    /// </para>
    /// </summary>
    [JsonProperty("txid")]
    public string TransactionId { get; set; }

    /// <summary>
    /// Valor do PixPaid.
    /// </summary>
    [JsonProperty("valor")]
    [JsonConverter(typeof(DecimalFormatConverter))]
    public decimal Price { get; set; }

    /// <summary>
    /// Horário em que o PixPaid foi processado no PSP.
    /// </summary>
    [JsonProperty("horario")]
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-ddTHH:mm:ss")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Informação livre do pagador
    /// </summary>
    [JsonProperty("infoPagador")]
    public string InfoPayer { get; set; }

    /// <summary>
    /// Informações sobre o pagador
    /// </summary>
    [JsonProperty("pagador")]
    public Payer Payer { get; set; }

    /// <summary>
    /// Dados da devolução
    /// </summary>
    [JsonProperty("devolucoes")]
    public Devolution[] Devolution { get; set; }
}

/// <summary>
/// Classe que representa um pagador.
/// </summary>
public class Payer
{
    /// <summary>
    /// CNPJ do devedor, para cobranças de pessoa juridica.
    /// </summary>
    [JsonProperty("cnpj", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Cnpj { get; set; }

    /// <summary>
    ///  CPF do devedor, para cobranças de pessoa fisca.
    /// </summary>
    [JsonProperty("cpf", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Cpf { get; set; }

    /// <summary>
    /// Nome do pagador
    /// </summary>
    [JsonProperty("nome")]
    public string Name { get; set; }
}

/// <summary>
/// Classe que representa uma devolução.
/// </summary>
public class Devolution
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
    /// Horário da devolução
    /// </summary>
    [JsonProperty("horario")]
    public TimeDevolution TimeDevolution { get; set; }

    /// <summary>
    /// Status da devolução.
    /// </summary>
    [JsonProperty("status")]
    [JsonConverter(typeof(ReturnStatusConverter))]
    public ReturnStatus status { get; set; }
}

/// <summary>
/// Classe que representa os horários da devolução.
/// </summary>
public class TimeDevolution
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
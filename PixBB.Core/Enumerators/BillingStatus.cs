namespace PixBB.Core.Enumerators;

/// <summary>
/// Enum contendo os status de cobrança
/// </summary>
public enum BillingStatus
{
    /// <summary>
    /// Cobrança ativa
    /// </summary>
    Active = 1,

    /// <summary>
    /// Cobrança concluida
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Removida pelo usuário recebedor
    /// </summary>
    RemovedByRecipientUser = 3,

    /// <summary>
    /// Removida pelo usuário PSP
    /// </summary>
    RemovedByPsp = 4
}
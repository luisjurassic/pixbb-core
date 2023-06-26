using System;
using Newtonsoft.Json;
using PixBB.Core.Models;

namespace PixBB.Core.Exceptions;

/// <summary>
/// Classe de exceção básica da biblioteca.
/// </summary>
public class PixException : Exception
{
    /// <summary>
    /// Erros reportados pelo serviço.
    /// </summary>
    public PixError[] Errors { get; set; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="PixException"/>.
    /// </summary>
    /// <param name="message">Mensagem à ser definida na excessão.</param>
    public PixException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="PixException"/>.
    /// </summary>
    /// <param name="ex">Os dados da execção.</param>
    public PixException(HttpException ex)
        : base(ex.Content)
    {
        try
        {
            Errors = JsonConvert.DeserializeObject<PixError[]>(ex.Content);
        }
        catch
        {
            // ignored
        }
    }
}
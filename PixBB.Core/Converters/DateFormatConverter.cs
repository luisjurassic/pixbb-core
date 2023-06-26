using Newtonsoft.Json.Converters;

namespace PixBB.Core.Converters;

/// <summary>
/// Classe responsável pela conversão da data para um formato específico.
/// </summary>
public class DateFormatConverter : IsoDateTimeConverter
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="DateFormatConverter"/>.
    /// </summary>
    /// <param name="format">Formado de data desejado.</param>
    public DateFormatConverter(string format)
    {
        DateTimeFormat = format;
    }
}
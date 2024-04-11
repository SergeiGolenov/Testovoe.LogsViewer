using System.ComponentModel.DataAnnotations;
using Testovoe.LogsViewer.Shared.ValidationAttributes;

namespace Testovoe.LogsViewer.Shared.Options;

public class AppOptions
{
    [Required(AllowEmptyStrings = false, ErrorMessage = $"Specify required {nameof(FileLog)} parameter")]
    public required string FileLog { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = $"Specify required {nameof(FileOutput)} parameter")]
    public required string FileOutput { get; set; }

    public string? AddressStart { get; set; }
    [Range(0, 32)]
    public int AddressMask { get; set; }

    [DateFormat("dd.MM.yyyy")]
    public required string TimeStart { get; set; }
    [DateFormat("dd.MM.yyyy")]
    public required string TimeEnd { get; set; }

    public DateTime ParsedTimeStart
    {
        get
        {
            try
            {
                return DateTime.Parse(TimeStart);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException($"Specify required {nameof(TimeStart)} parameter");
            }
        }
    }

    public DateTime ParsedTimeEnd
    {
        get
        {
            try
            {
                return DateTime.Parse(TimeEnd);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException($"Specify required {nameof(TimeEnd)} parameter");
            }
        }
    }

    public static bool Validate(AppOptions appOptions)
    {
        if (appOptions.AddressMask != 0 && appOptions.AddressStart == null) return false;
        return true;
    }
}
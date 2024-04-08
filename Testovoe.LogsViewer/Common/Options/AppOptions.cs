using System.ComponentModel.DataAnnotations;
using Testovoe.LogsViewer.Shared.ValidationAttributes;

namespace Testovoe.LogsViewer.Shared.Options;

internal class AppOptions
{
    [Required(AllowEmptyStrings = false)]
    public required string FileLog { get; set; }
    [Required(AllowEmptyStrings = false)]
    public required string FileOutput { get; set; }

    [Range(0, int.MaxValue)]
    public int AddressStart { get; set; } = 0;
    public string? AddressMask { get; set; }

    [Required]
    [DateFormat("dd.MM.yyyy")]
    public required string TimeStart { get; set; }
    [Required]
    [DateFormat("dd.MM.yyyy")]
    public required string TimeEnd { get; set; }

    public DateTime ParsedTimeStart { get => DateTime.Parse(TimeStart); }
    public DateTime ParsedTimeEnd { get => DateTime.Parse(TimeStart); }
}
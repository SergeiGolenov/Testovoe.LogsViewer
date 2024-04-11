using System.ComponentModel.DataAnnotations;

namespace Testovoe.LogsViewer.Shared.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateFormatAttribute(string dateTimeFormat) : ValidationAttribute
{
    private readonly string _dateTimeFormat = dateTimeFormat;
    private string _toValidate = string.Empty;

    public override bool IsValid(object? value)
    {
        try
        {
            _toValidate = (string)value!;
            DateTime.ParseExact(_toValidate, _dateTimeFormat, null);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
        => $"The given date {_toValidate} doesn't match with required {_dateTimeFormat} format";
}
//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;

using EngineerCalc.Models;

namespace EngineerCalc.Infrastructure;

internal sealed class FilePathConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value is string str
            ? new FilePath(str)
            : base.ConvertFrom(context, culture, value);
    }
}

#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using System.Globalization;
namespace System.Primitives.DataTypes
{
    /// <summary>
    /// PercentDataType
    /// </summary>
    public class PercentDataType : DataTypeBase
    {
        public class FormatAttrib
        {
            public Formats Formats;
            public string Format;
        }

        public enum Formats
        {
            Pattern,
        }

        public class ParseAttrib { }

        public PercentDataType()
            : base(Prime.Type, Prime.FormFieldMeta, new DataTypeFormatter(), new DataTypeParser()) { }

        public class DataTypeFormatter : DataTypeFormatterBase<decimal, FormatAttrib, ParseAttrib>
        {
            public DataTypeFormatter()
                : base(Prime.Format, Prime.TryParse) { }
        }

        public class DataTypeParser : DataTypeParserBase<decimal, ParseAttrib>
        {
            public DataTypeParser()
                : base(Prime.TryParse, 0M, "0") { }
        }

        /// <summary>
        /// Prime
        /// </summary>
        public static class Prime
        {
            public static string Format(decimal value, FormatAttrib attrib)
            {
                if (attrib != null)
                    switch (attrib.Formats)
                    {
                        case Formats.Pattern:
                            return value.ToString(attrib.Format, CultureInfo.InvariantCulture);
                        default:
                            throw new InvalidOperationException();
                    }
                return value.ToString("0.00", CultureInfo.InvariantCulture) + "%";
            }

            public static bool TryParse(string text, ParseAttrib attrib, out decimal value)
            {
                if (string.IsNullOrEmpty(text))
                {
                    value = 0M; return false;
                }
                if (text.EndsWith("%"))
                    text = text.Substring(0, text.Length - 1);
                return decimal.TryParse(text, out value);
            }

            public static Type Type
            {
                get { return typeof(decimal); }
            }

            public static DataTypeFormFieldMeta FormFieldMeta
            {
                get
                {
                    return new DataTypeFormFieldMeta()
                    {
                        GetBinderType = (int applicationType) => "Text",
                        GetMaxLength = (int applicationType) => 10,
                        GetSize = (int applicationType, int length) => "10",
                    };
                }
            }
        }
    }
}
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
namespace System
{
    /// <summary>
    /// StringExtensions
    /// </summary>
    public static partial class StringExtensions
    {
        public static string Format<T>(this string text)
        {
            throw new NotImplementedException();
            //return FormatterEx.ValueDelegateFactory<T>.Format(text);
        }

        public static T Parse<T>(this string text) { return Parse<T, T>(text, null); }
        public static T Parse<T>(this string text, Nattrib attrib) { return Parse<T, T>(text, attrib); }
        public static TResult Parse<T, TResult>(this string text) { return Parse<T, TResult>(text, null); }
        public static TResult Parse<T, TResult>(this string text, Nattrib attrib)
        {
            return ParserEx.StringParserDelegateFactory<T, TResult>.Parse(text, default(TResult), attrib);
        }
        public static T Parse<T>(this string text, T defaultValue) { return Parse<T, T>(text, defaultValue, null); }
        public static T Parse<T>(this string text, T defaultValue, Nattrib attrib) { return Parse<T, T>(text, defaultValue, attrib); }
        public static TResult Parse<T, TResult>(this string text, TResult defaultValue) { return Parse<T, TResult>(text, defaultValue, null); }
        public static TResult Parse<T, TResult>(this string text, TResult defaultValue, Nattrib attrib)
        {
            return ParserEx.StringParserDelegateFactory<T, TResult>.Parse(text, defaultValue, attrib);
        }

        public static object Parse<T>(this string text, object defaultValue) { return Parse<T>(text, defaultValue, null); }
        public static object Parse<T>(this string text, object defaultValue, Nattrib attrib)
        {
            return ParserEx.StringParserDelegateFactory<T, object>.Parse2(text, defaultValue, attrib);
        }

        public static string ParseAndFormat<T>(this string text) { return ParseAndFormat<T>(text, null); }
        public static string ParseAndFormat<T>(this string text, Nattrib attribs)
        {
            return null;
        }
    }
}

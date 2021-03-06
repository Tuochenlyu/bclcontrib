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
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
namespace System
{
    public static partial class ParserEx
    {
        #region Registration
        private static Dictionary<Type, object> s_stringParserProviders = null;

        public interface IStringParserBuilder
        {
            IStringParser<TResult> Build<TResult>();
        }

        public interface IStringParser
        {
            object Parse2(string value, object defaultValue, Nattrib attrib);
            bool Validate(string value, Nattrib attrib);
        }

        public interface IStringParser<TResult> : IStringParser
        {
            TResult Parse(string value, TResult defaultValue, Nattrib attrib);
            bool TryParse(string value, Nattrib attrib, out TResult validValue);
        }

        public static void RegisterStringParser<T>(object stringParser) { RegisterStringParser(typeof(T), stringParser); }
        public static void RegisterStringParser(Type key, object stringParser)
        {
            if (s_stringParserProviders == null)
                s_stringParserProviders = new Dictionary<Type, object>();
            s_stringParserProviders.Add(key, stringParser);
        }

        private static bool TryScanForStringParser<TResult>(out Type key, out IStringParser<TResult> stringParser)
        {
            key = null; stringParser = null; return false;
            //throw new NotImplementedException();
        }

        private static IStringParser ScanForStringParser(Type type)
        {
            if (s_stringParserProviders != null)
                foreach (var stringParser2 in s_stringParserProviders)
                    if ((type == stringParser2.Key) || (type.IsSubclassOf(stringParser2.Key)))
                        return (stringParser2.Value as IStringParser);
            return null;
        }
        private static IStringParser<TResult> ScanForStringParser<TResult>(Type type)
        {
            if (s_stringParserProviders != null)
                foreach (var stringParser2 in s_stringParserProviders)
                    if ((type == stringParser2.Key) || (type.IsSubclassOf(stringParser2.Key)))
                        return (stringParser2.Value as IStringParser<TResult>);
            Type key;
            IStringParser<TResult> stringParser;
            if (TryScanForStringParser<TResult>(out key, out stringParser))
            {
                RegisterStringParser(key, stringParser);
                return stringParser;
            }
            return null;
        }
        #endregion

        internal static class StringParserDelegateFactory<T, TResult>
        {
            private static readonly Type s_type = typeof(T);
            private static readonly MethodInfo s_tryParseMethodInfo = s_type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { CoreExInternal.StringType, s_type.MakeByRefType() }, null);
            public static readonly Func<string, TResult, Nattrib, TResult> Parse = CreateParser(s_type);
            public static readonly Func<string, object, Nattrib, object> Parse2 = CreateParser2(s_type);
            public static readonly TryFunc<string, Nattrib, TResult> TryParse = CreateTryParser(s_type);
            public static readonly Func<string, Nattrib, bool> Validate = CreateValidator(s_type);

            static StringParserDelegateFactory() { }

            private static Func<string, TResult, Nattrib, TResult> CreateParser(Type type)
            {
                if (type == CoreExInternal.BoolType)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_Bool", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NBoolType)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_NBool", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.DateTimeType)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_DateTime", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NDateTimeType)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_NDateTime", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.DecimalType)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_Decimal", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NDecimalType)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_NDecimal", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.Int32Type)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_Int32", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NInt32Type)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_NInt32", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.StringType)
                    return (Func<string, TResult, Nattrib, TResult>)Delegate.CreateDelegate(typeof(Func<string, TResult, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("Parser_String", BindingFlags.NonPublic | BindingFlags.Static));
                var parser = ScanForStringParser<TResult>(type);
                if (parser != null)
                    return parser.Parse;
                //EnsureTryParseMethod();
                if (s_tryParseMethodInfo != null)
                    return new Func<string, TResult, Nattrib, TResult>(Parser_Default);
                return null;
            }
            private static Func<string, object, Nattrib, object> CreateParser2(Type type)
            {
                var parser = ScanForStringParser(type);
                if (parser != null)
                    return parser.Parse2;
                //EnsureTryParseMethod();
                if (s_tryParseMethodInfo != null)
                    return new Func<string, object, Nattrib, object>(Parser2_Default);
                return null;
            }

            private static TryFunc<string, Nattrib, TResult> CreateTryParser(Type type)
            {
                if (type == CoreExInternal.BoolType)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_Bool", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NBoolType)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_NBool", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.DateTimeType)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_DateTime", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NDateTimeType)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_NDateTime", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.DecimalType)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_Decimal", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NDecimalType)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_NDecimal", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.Int32Type)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_Int32", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.NInt32Type)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_NInt32", BindingFlags.NonPublic | BindingFlags.Static));
                if (type == CoreExInternal.StringType)
                    return (TryFunc<string, Nattrib, TResult>)Delegate.CreateDelegate(typeof(TryFunc<string, Nattrib, TResult>), typeof(StringParserDelegateFactory<T, TResult>).GetMethod("TryParser_String", BindingFlags.NonPublic | BindingFlags.Static));
                var parser = ScanForStringParser<TResult>(type);
                if (parser != null)
                    return parser.TryParse;
                //EnsureTryParseMethod();
                if (s_tryParseMethodInfo != null)
                    return new TryFunc<string, Nattrib, TResult>(TryParser_Default);
                return null;
            }

            private static Func<string, Nattrib, bool> CreateValidator(Type type)
            {
                if (type == CoreExInternal.BoolType)
                    return new Func<string, Nattrib, bool>(Validator_Bool);
                if (type == CoreExInternal.DateTimeType)
                    return new Func<string, Nattrib, bool>(Validator_DateTime);
                if (type == CoreExInternal.DecimalType)
                    return new Func<string, Nattrib, bool>(Validator_Decimal);
                if (type == CoreExInternal.Int32Type)
                    return new Func<string, Nattrib, bool>(Validator_Int32);
                if (type == CoreExInternal.StringType)
                    return new Func<string, Nattrib, bool>(Validator_String);
                var parser = ScanForStringParser(type);
                if (parser != null)
                    return parser.Validate;
                //EnsureTryParseMethod();
                if (s_tryParseMethodInfo != null)
                    return new Func<string, Nattrib, bool>(Validator_Default);
                return null;
            }

            private static void EnsureTryParseMethod()
            {
                if (s_tryParseMethodInfo == null)
                    throw new InvalidOperationException(string.Format("{0}::TypeParse method required for parsing", typeof(T).ToString()));
            }

            #region Default
            private static TResult Parser_Default(string text, TResult defaultValue, Nattrib attribs)
            {
                if ((text != null) && (text.Length > 0))
                {
                    var args = new object[] { text, default(TResult) };
                    if ((bool)s_tryParseMethodInfo.Invoke(null, BindingFlags.Default, null, args, null))
                        return (TResult)args[1];
                }
                return defaultValue;
            }
            private static object Parser2_Default(string text, object defaultValue, Nattrib attribs)
            {
                if ((text != null) && (text.Length > 0))
                {
                    var args = new object[] { text, default(TResult) };
                    if ((bool)s_tryParseMethodInfo.Invoke(null, BindingFlags.Default, null, args, null))
                        return (TResult)args[1];
                }
                return defaultValue;
            }

            private static bool TryParser_Default(string text, Nattrib attribs, out TResult validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    var args = new object[] { text, default(TResult) };
                    if ((bool)s_tryParseMethodInfo.Invoke(null, BindingFlags.Default, null, args, null))
                    {
                        validValue = (TResult)args[1]; return true;
                    }
                }
                validValue = default(TResult); return false;
            }

            private static bool Validator_Default(string text, Nattrib attribs)
            {
                if ((text != null) && (text.Length > 0))
                {
                    var args = new object[] { text, default(TResult) };
                    if ((bool)s_tryParseMethodInfo.Invoke(null, BindingFlags.Default, null, args, null))
                        return true;
                }
                return false;
            }
            #endregion

            #region Bool
            private static bool Parser_Bool(string text, bool defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    switch (text.ToLowerInvariant())
                    {
                        case "1":
                        case "y":
                        case "true":
                        case "yes":
                        case "on":
                            return true;
                        case "0":
                        case "n":
                        case "false":
                        case "no":
                        case "off":
                            return false;
                    }
                    bool validValue;
                    if (bool.TryParse(text, out validValue))
                        return validValue;
                }
                return defaultValue;
            }
            private static bool? Parser_NBool(string text, bool? defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    switch (text.ToLowerInvariant())
                    {
                        case "1":
                        case "y":
                        case "true":
                        case "yes":
                        case "on":
                            return true;
                        case "0":
                        case "n":
                        case "false":
                        case "no":
                        case "off":
                            return false;
                    }
                    bool validValue;
                    if (bool.TryParse(text, out validValue))
                        return validValue;
                }
                return defaultValue;
            }

            private static bool TryParser_Bool(string text, Nattrib attrib, out bool validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    switch (text.ToLowerInvariant())
                    {
                        case "1":
                        case "y":
                        case "true":
                        case "yes":
                        case "on":
                            validValue = true;
                            return true;
                        case "0":
                        case "n":
                        case "false":
                        case "no":
                        case "off":
                            validValue = false;
                            return true;
                    }
                    bool validValue2;
                    if (bool.TryParse(text, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = default(bool); return false;
            }
            private static bool TryParser_NBool(string text, Nattrib attrib, out bool? validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    switch (text.ToLowerInvariant())
                    {
                        case "1":
                        case "y":
                        case "true":
                        case "yes":
                        case "on":
                            validValue = true;
                            return true;
                        case "0":
                        case "n":
                        case "false":
                        case "no":
                        case "off":
                            validValue = false;
                            return true;
                    }
                    bool validValue2;
                    if (bool.TryParse(text, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = null; return false;
            }

            private static bool Validator_Bool(string text, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    switch (text.ToLowerInvariant())
                    {
                        case "1":
                        case "y":
                        case "true":
                        case "yes":
                        case "on":
                            return true;
                        case "0":
                        case "n":
                        case "false":
                        case "no":
                        case "off":
                            return true;
                    }
                    bool validValue2;
                    return bool.TryParse(text, out validValue2);
                }
                return false;
            }
            #endregion

            #region DateTime
            private static DateTime Parser_DateTime(string text, DateTime defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    DateTime validValue;
                    if (DateTime.TryParse(text, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out validValue))
                        return validValue;
                }
                return defaultValue;
            }
            private static DateTime? Parser_NDateTime(string text, DateTime? defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    DateTime validValue;
                    if (DateTime.TryParse(text, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out validValue))
                        return validValue;
                }
                return defaultValue;
            }

            private static bool TryParser_DateTime(string text, Nattrib attrib, out DateTime validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    DateTime validValue2;
                    if (DateTime.TryParse(text, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = default(DateTime); return false;
            }
            private static bool TryParser_NDateTime(string text, Nattrib attrib, out DateTime? validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    DateTime validValue2;
                    if (DateTime.TryParse(text, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = null; return false;
            }

            private static bool Validator_DateTime(string text, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    DateTime validValue;
                    return DateTime.TryParse(text, out validValue);
                }
                return false;
            }
            #endregion

            #region Decimal
            private static decimal Parser_Decimal(string text, decimal defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    decimal validValue;
                    if (decimal.TryParse(text, NumberStyles.Currency, null, out validValue))
                        return validValue;
                }
                return defaultValue;
            }
            private static decimal? Parser_NDecimal(string text, decimal? defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    decimal validValue;
                    if (decimal.TryParse(text, NumberStyles.Currency, null, out validValue))
                        return validValue;
                }
                return defaultValue;
            }

            private static bool TryParser_Decimal(string text, Nattrib attrib, out decimal validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    decimal validValue2;
                    if (decimal.TryParse(text, NumberStyles.Currency, null, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = default(decimal); return false;
            }
            private static bool TryParser_NDecimal(string text, Nattrib attrib, out decimal? validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    decimal validValue2;
                    if (decimal.TryParse(text, NumberStyles.Currency, null, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = null; return false;
            }

            private static bool Validator_Decimal(string text, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    decimal validValue;
                    return decimal.TryParse(text, NumberStyles.Currency, null, out validValue);
                }
                return false;
            }
            #endregion

            #region Int32
            private static int Parser_Int32(string text, int defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    int validValue;
                    if (int.TryParse(text, out validValue))
                        return validValue;
                }
                return defaultValue;
            }
            private static int? Parser_NInt32(string text, int? defaultValue, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    int validValue;
                    if (int.TryParse(text, out validValue))
                        return validValue;
                }
                return defaultValue;
            }

            private static bool TryParser_Int32(string text, Nattrib attrib, out int validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    int validValue2;
                    if (int.TryParse(text, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = default(int); return false;
            }
            private static bool TryParser_NInt32(string text, Nattrib attrib, out int? validValue)
            {
                if ((text != null) && (text.Length > 0))
                {
                    int validValue2;
                    if (int.TryParse(text, out validValue2))
                    {
                        validValue = validValue2; return true;
                    }
                }
                validValue = null; return false;
            }

            private static bool Validator_Int32(string text, Nattrib attrib)
            {
                if ((text != null) && (text.Length > 0))
                {
                    int validValue;
                    return int.TryParse(text, out validValue);
                }
                return false;
            }
            #endregion

            #region String
            private static string Parser_String(string text, string defaultValue, Nattrib attrib)
            {
                string value2;
                if ((text != null) && ((value2 = text.Trim()).Length > 0))
                    return value2;
                return defaultValue;
            }

            private static bool TryParser_String(string text, Nattrib attrib, out string validValue)
            {
                string value2;
                if ((text != null) && ((value2 = text.Trim()).Length > 0))
                {
                    validValue = value2; return true;
                }
                validValue = default(string); return false;
            }

            private static bool Validator_String(string text, Nattrib attrib)
            {
                return ((text != null) && (text.Trim().Length > 0));
            }
            #endregion
        }
    }
}

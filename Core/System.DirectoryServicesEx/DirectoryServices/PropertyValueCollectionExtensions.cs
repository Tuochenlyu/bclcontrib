﻿#region License
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
namespace System.DirectoryServices
{
    public static class PropertyValueCollectionExtensions
    {
        //[Flags]
        //public enum UacFlags
        //{
        //    PasswordExpired = 0x800000,
        //    Lockout = 0x0010,
        //}

        public static void SetValue(this PropertyValueCollection collection, string value)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (string.IsNullOrEmpty(value))
            {
                if (collection.Count > 0)
                    collection.Clear();
                return;
            }
            collection.Value = value;
        }

        public static void SetValue(this PropertyValueCollection collection, bool value)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            collection.Value = value;
        }

        public static void SetValue(this PropertyValueCollection collection, int value)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            collection.Value = value;
        }

        public static string GetSingleStringValue(this ResultPropertyCollection collection, string propertyName)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            var values = collection[propertyName];
            return ((values != null) && (values.Count > 0) ? (values[0] as string) : string.Empty);
        }
    }
}

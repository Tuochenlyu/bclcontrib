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
using System.Xml.Linq;
using System.IO;
using System.Security;
namespace System.Web.Integrate
{
    /// <summary>
    /// GoogleSiteMapObserver
    /// </summary>
    public class GoogleSiteMapObserver : IObserver<GoogleSiteMapNode>
    {
        private static readonly XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private XElement _siteMap = new XElement(_xmlns + "urlset");
        private TextWriter _textStream;

        public GoogleSiteMapObserver(TextWriter textStream)
        {
            _textStream = textStream;
        }

        public void OnCompleted()
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            doc.Add(_siteMap);
            _textStream.Write(doc.ToString());
        }

        public void OnError(Exception ex)
        {
            throw ex;
        }

        public void OnNext(GoogleSiteMapNode node)
        {
            _siteMap.Add(new XElement(_xmlns + "url",
                new XElement(_xmlns + "loc", SecurityElement.Escape(node.Url)),
                new XElement(_xmlns + "lastmod", node.LastModifyDate),
                new XElement(_xmlns + "changefreq", node.PageDynamism),
                new XElement(_xmlns + "priority", node.PagePriority)
            ));
        }
    }
}
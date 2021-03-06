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
using System.Text;
using System.Patterns.Form;
namespace System.Web.UI.WebControls
{
    /// <summary>
    /// DropDownListEx
    /// </summary>
    public class DropDownListEx : DropDownList, IFormControl
    {
        private string _staticTextSeparator = ", ";
        private FormFieldViewMode _renderViewMode;

        /// <summary>
        /// SingleItemRenderMode
        /// </summary>
        public enum SingleItemRenderMode
        {
            /// <summary>
            /// UseFieldMode
            /// </summary>
            UseFieldMode = 0,
            /// <summary>
            /// RenderHidden
            /// </summary>
            RenderHidden,
            /// <summary>
            /// RenderStaticWithHidden
            /// </summary>
            RenderStaticWithHidden
        }

        public DropDownListEx()
            : base()
        {
            HasHeaderItem = true;
        }

        public bool AutoSelectFirstItem { get; set; }

        public bool AutoSelectIfSingleItem { get; set; }

        public bool HasHeaderItem { get; set; }

        public FormFieldViewMode ViewMode { get; set; }

        #region Option-Groups
        public static ListItem CreateBeginGroupListItem(string text)
        {
            var listItem = new ListItem(text);
            listItem.Attributes["group"] = "begin";
            return listItem;
        }

        public static ListItem CreateEndGroupListItem()
        {
            var listItem = new ListItem();
            listItem.Attributes["group"] = "end";
            return listItem;
        }

        //[Match("match with ListBox RenderContents")]
        protected override void RenderContents(HtmlTextWriter w)
        {
            ListItemCollection itemHash = Items;
            int itemCount = itemHash.Count;
            if (itemCount > 0)
            {
                bool isA = false;
                for (int itemKey = 0; itemKey < itemCount; itemKey++)
                {
                    ListItem listItem = itemHash[itemKey];
                    if (listItem.Enabled)
                        switch (listItem.Attributes["group"])
                        {
                            case "begin":
                                w.WriteBeginTag("optgroup");
                                w.WriteAttribute("label", listItem.Text);
                                w.Write('>');
                                break;
                            case "end":
                                w.WriteEndTag("optgroup");
                                break;
                            default:
                                w.WriteBeginTag("option");
                                if (listItem.Selected)
                                {
                                    if (isA)
                                        VerifyMultiSelect();
                                    isA = true;
                                    w.WriteAttribute("selected", "selected");
                                }
                                w.WriteAttribute("value", listItem.Value, true);
                                if (listItem.Attributes.Count > 0)
                                    listItem.Attributes.Render(w);
                                if (Page != null)
                                    Page.ClientScript.RegisterForEventValidation(UniqueID, listItem.Value);
                                w.Write('>');
                                HttpUtility.HtmlEncode(listItem.Text, w);
                                w.WriteEndTag("option");
                                w.WriteLine();
                                break;
                        }
                }
            }
        }
        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _renderViewMode = ViewMode;
            int itemCountIfListHasSingleItem = (HasHeaderItem ? 2 : 1);
            int firstItemIndex = itemCountIfListHasSingleItem - 1;
            bool setSelectedIndex = ((SelectedIndex == 0) && ((AutoSelectFirstItem) || ((AutoSelectIfSingleItem) && (Items.Count == itemCountIfListHasSingleItem))));
            if (setSelectedIndex)
                SelectedIndex = firstItemIndex;
            SingleItemRenderMode singleItemRenderMode = SingleItemRenderOption;
            if ((singleItemRenderMode != SingleItemRenderMode.UseFieldMode) && (_renderViewMode == FormFieldViewMode.Input) && (Items.Count == itemCountIfListHasSingleItem))
                switch (singleItemRenderMode)
                {
                    case SingleItemRenderMode.RenderHidden:
                        _renderViewMode = FormFieldViewMode.Hidden;
                        break;
                    case SingleItemRenderMode.RenderStaticWithHidden:
                        _renderViewMode = FormFieldViewMode.StaticWithHidden;
                        break;
                }
        }

        protected override void Render(HtmlTextWriter w)
        {
            switch (_renderViewMode)
            {
                case FormFieldViewMode.Static:
                    RenderStaticText(w);
                    break;
                case FormFieldViewMode.StaticWithHidden:
                    RenderStaticText(w);
                    RenderHidden(w);
                    break;
                case FormFieldViewMode.Hidden:
                    RenderHidden(w);
                    break;
                default:
                    base.Render(w);
                    break;
            }
        }

        protected void RenderHidden(HtmlTextWriter w)
        {
            var b = new StringBuilder();
            foreach (ListItem item in Items)
                if (item.Selected)
                    b.Append(item.Value + ",");
            if (b.Length > 0)
                b.Length--;
            w.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            w.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            w.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            w.AddAttribute(HtmlTextWriterAttribute.Value, b.ToString());
            w.RenderBeginTag(HtmlTextWriterTag.Input);
            w.RenderEndTag();
        }

        protected virtual void RenderStaticText(HtmlTextWriter w)
        {
            string staticTextSeparator = StaticTextSeparator;
            var b = new StringBuilder();
            foreach (ListItem item in Items)
                if (item.Selected)
                    b.Append(HttpUtility.HtmlEncode(item.Text) + staticTextSeparator);
            int staticTextSeparatorLength = staticTextSeparator.Length;
            if (b.Length > staticTextSeparatorLength)
                b.Length -= staticTextSeparatorLength;
            w.AddAttribute(HtmlTextWriterAttribute.Class, "static");
            w.RenderBeginTag(HtmlTextWriterTag.Span);
            w.Write(b.ToString());
            w.RenderEndTag();
        }

        public SingleItemRenderMode SingleItemRenderOption { get; set; }

        public string StaticTextSeparator
        {
            get { return _staticTextSeparator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _staticTextSeparator = value;
            }
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Localization;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [PreviewComposite]
    public class TextTagFormat : BaseConfig
    {
        [JsonProperty]
        public bool overrideFontColor = false;

        [Indent]
        [ColorPalette]
        [JsonProperty]
        [ShowIf(nameof(overrideFontColor))]
        public Color fontColor = Color.black;

        public bool overrideBoldStyle = false;

        [JsonProperty]
        [Indent]
        [ShowIf(nameof(overrideBoldStyle))]
        public bool isBold = false;

        public bool overrideItalicStyle = false;

        [JsonProperty]
        [Indent]
        [ShowIf(nameof(overrideItalicStyle))]
        public bool isItalic = false;

        #region Get Text

        public string GetText(string customText)
        {
            string result = customText;

            if (overrideFontColor)
            {
                result = result.ColorTag(fontColor);
            }

            if (overrideBoldStyle)
            {
                if (isBold)
                {
                    result = result.BoldTag();
                }

                if (isItalic)
                {
                    result = result.ItalicTag();
                }
            }

            return result;
        }

        public string GetText(object customText)
        {
            return GetText(customText.ToString());
        }

        #endregion

        #region To String

        public override string ToString()
        {
            var strList = new List<string>();

            if (overrideFontColor)
            {
                strList.Add(fontColor.ToLocalizedString(ColorStringFormat.Name));
            }

            if (overrideBoldStyle)
            {
                if (isBold)
                {
                    strList.Add("Bold");
                }
                else
                {
                    strList.Add("Not Bold");
                }
            }

            if (overrideItalicStyle)
            {
                if (isItalic)
                {
                    strList.Add("Italic");
                }
                else
                {
                    strList.Add("Not Italic");
                }
            }

            if (strList.Count == 0)
            {
                return "No Format Override";
            }

            return strList.ToFormattedString(",");
        }

        #endregion

        #region JSON Serialization

        public bool ShouldSerializefontColor()
        {
            return overrideFontColor == true;
        }

        public bool ShouldSerializeisBold()
        {
            return overrideBoldStyle == true;
        }

        public bool ShouldSerializeisItalic()
        {
            return overrideBoldStyle == true;
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game.Meta.Heuristics
{
    /// <summary>
    /// Easy text formatting in RichText tags
    /// </summary>
    public static class HRichText
    {
        /// <returns>
        /// RichText format of text that will be shown as colored
        /// </returns>
        /// <param name="color"> object of UnityEngine.Color</param>
        /// <param name="text"></param>
        public static string ColorRT(Color color, string text)
        { return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>"; }

        /// <returns>
        /// RichText format of text that will be shown as colored
        /// </returns>
        /// <param name="text"></param>
        /// <param name="color"> object of UnityEngine.Color</param>
        public static string ColorRT(this string text, Color color)
        { return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>"; }

        /// <returns>
        /// RichText format of text that will be shown as colored
        /// </returns>
        /// <param name="text"></param>
        /// <param name="color"> Must be in RRGGBBAA format</param>
        public static string ColorRT(this string text, string color)
        { return "<color=#" + color + ">" + text + "</color>"; }

        /// <returns>
        /// RichText format of text that will be shown as colored
        /// </returns>
        /// <param name="text"></param>
        /// <param name="HEXcolor"> Must be in 0xRRGGBBAA format</param>
        public static string ColorRT(this string text, uint HEXcolor)
        { return "<color=#" + HEXcolor.ToString("x") + ">" + text + "</color>"; }
        
        /// <returns>
        /// RichText format of text that will be shown as bold text
        /// </returns>
        public static string BoldRT(this string text)
        { return "<b>" + text + "</b>"; }
        
        /// <returns>
        /// RichText format of text that will be shown as cursive text
        /// </returns>
        public static string ItalicRT(this string text)
        { return "<i>" + text + "</i>"; }
        
        /// <returns>
        /// Text with no proccesed RichText tags. Put only at the end of other tags implementations
        /// </returns>
        public static string NoParseRT(this string text)
        { return "<noparse>" + text + "</noparse>"; }
        
        /// <returns>
        /// RichText format of text. Text's size will be changed to chosen 
        /// </returns>
        public static string SizeRT(this string text, float value)
        { return "<size=" + value + ">" + text + "</size>"; }
        
        /// <summary>
        /// Remove every <tag>TEXT</tag> RichText construction. If text contain <noparse>TEXT</noparse>, method will not trim RichText in angle brackets
        /// </summary>
        /// <param name="text">Possibly formatted text</param>
        public static string RemoveRichTextTags(string text )
        {
            if (string.IsNullOrEmpty(text)) { return text; } 
            
            Regex noparseBlock = new Regex(@"<noparse>(.*?)</noparse>", RegexOptions.Singleline);
            
            Regex rich = new Regex(@"<[^>]*>");
            
            if (noparseBlock.IsMatch(text))
            {
                var placeholders = new List<string>();
                text = noparseBlock.Replace(text, match =>
                {
                    placeholders.Add(match.Groups[1].Value);
                    
                    return $"{placeholders.Count - 1}";
                });

                
                text = rich.Replace(text, string.Empty);

                for (int i = 0; i < placeholders.Count; i++)
                {
                    text = text.Replace($"{i}", placeholders[i]);
                }
                return text;
            }
            else { return rich.Replace(text, string.Empty); }

        }
        
    }
    
}
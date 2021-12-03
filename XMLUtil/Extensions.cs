using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace XMLUtil
{
    public static class Extensions
    {
        public static string Beautify(this XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = Environment.NewLine,
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        public static T[] GetEnumValues<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type is not a Enum.");
            else
                return (T[])Enum.GetValues(typeof(T));
        }
    }
}

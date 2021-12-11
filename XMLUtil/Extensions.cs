using System;
using System.Xml;
using System.Text;
using System.Runtime.InteropServices;

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

        public static T TryParse<T>(object obj) where T : struct
        {
            T res = default;
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type is not a Enum.");
            foreach (var e in GetEnumValues<T>())
            {
                if (e.ToString() == obj.ToString())
                    res = e;
            }
            return res;
        }

        internal static byte[] ToBytes<T>(this T data) where T : struct
        {
            var size = Marshal.SizeOf(data);
            var bytes = new byte[size];
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
            Marshal.StructureToPtr(data, ptr, true);
            return bytes;
        }

        internal static T ToStruct<T>(this byte[] data) where T : struct
        {
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            return Marshal.PtrToStructure<T>(ptr);
        }

        internal static int SizeOf<T>(this T src) where T : struct => Marshal.SizeOf(src);
    }
}

using System.IO;
using System.Text;

namespace Riivolution_XML_Generator.Classes
{
    /// <summary>
    /// Class for generating the .xml file.
    /// </summary>
    class XML_Generator
    {
        /// <summary>
        /// Generates the xml file.
        /// </summary>
        /// <param name="Game_ID">The Game ID</param>
        /// <param name="RGN">Riivolution Page Name.</param>
        /// <param name="opiton_name">The name of the option</param>
        /// <param name="PID">The name of the Patch ID</param>
        /// <param name="fp">The path to the folder containing the mods.</param>
        /// <param name="fn">The path\name of the file.</param>
        public static void Generate(string Game_ID, string RGN, string opiton_name, string PID, string fp, string fn)
        {
            string[] lines =
            {
                "<wiidisc version=\"1\">",
                $"\t<id game=\"{Game_ID}\" />",
                "\t<options>",
                $"\t\t<section name=\"{RGN}\">",
                $"\t\t\t<option name=\"{opiton_name}\">",
                "\t\t\t\t<choice name=\"Enabled\">",
                $"\t\t\t\t\t<patch id=\"{PID}\" />",
                "\t\t\t\t</choice>",
                "\t\t\t</option>",
                "\t\t</section>",
                "\t</options>",
                $"\t<patch id=\"{PID}\">",
                $"\t<folder external=\"{fp}\" recursive=\"false\" />",
                $"\t\t<folder external=\"{fp}\" disc=\"/\" />",
                "\t</patch>",
                "</wiidisc>"
            };
            if (!fn.EndsWith(".xml"))
            {
                fn += ".xml";
            }
            string fileName = fn;
            File.WriteAllLines(fileName, lines, Encoding.UTF8);
            return;
        }
    }
}
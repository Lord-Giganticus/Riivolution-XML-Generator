using System.IO;

namespace Riivolution_XML_Generator.Classes
{
    /// <summary>
    /// A simple class to check mounted drives.
    /// </summary>
    class Drive_Check
    {
        /// <summary>
        /// Checks all mounted and ready drives for a riivolution folder.
        /// </summary>
        /// <returns>Either a folder path that ends with riivolution or the starting point of the program.</returns>
        public static string Check()
        {
            string starting_point = Directory.GetCurrentDirectory();
            string[] drives = Directory.GetLogicalDrives();
            string folder = "";
            foreach (string d in drives)
            {
                try
                {
                    Directory.SetCurrentDirectory(d);
                    if (Directory.Exists("riivolution"))
                    {
                        folder = d + "riivolution";
                        Directory.SetCurrentDirectory(starting_point);
                        break;
                    }
                } catch
                {
                    continue;
                } 
            }
            if (!folder.EndsWith("riivolution") || string.IsNullOrWhiteSpace(folder))
            {
                folder = starting_point;
            } 
            return folder;
        }
    }
}

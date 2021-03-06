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
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            string folder = "";
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    Directory.SetCurrentDirectory(d.VolumeLabel);
                    if (Directory.Exists("riivolution"))
                    {
                        folder = d.VolumeLabel + "riivolution";
                        Directory.SetCurrentDirectory(starting_point);
                        break;
                    }
                }
            }
            if (!folder.EndsWith("riivolution"))
            {
                folder = starting_point;
            } else
            {
                //pass
            }
            return folder;
        }
    }
}

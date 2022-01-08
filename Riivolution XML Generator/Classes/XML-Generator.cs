using System.IO;
using System.Text;
using System.Windows.Forms;
using XMLUtil;
using System;

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
        public static void Generate(string Game_ID, string Localize, string opiton_name, string PatchID, string FolderPath, string Region, string Section_Name, string Choice_Name)
        {
            if (FolderPath.StartsWith("/") == false)
            {
                FolderPath = "/" + FolderPath;
            } else
            {
                //pass
            }
            string[] lines =
            {
                "<wiidisc version=\"1\">",
                $"\t<id game=\"{Game_ID}\" />",
                "\t<options>",
                $"\t\t<section name=\"{Section_Name}\">",
                $"\t\t\t<option name=\"{opiton_name}\">",
                $"\t\t\t\t<choice name=\"{Choice_Name}\">",
                $"\t\t\t\t\t<patch id=\"{PatchID}\" />",
                "\t\t\t\t</choice>",
                "\t\t\t</option>",
                "\t\t</section>",
                "\t</options>",
                $"\t<patch id=\"{PatchID}\" root=\"{FolderPath}\">",
                "\t<savegame clone=\"false\" external=\"SaveGame/{$__gameid}{$__region}{$__maker}\"/>",
                $"<folder external=\"/{PatchID}/ObjectData\" disc=\"/ObjectData\" recursive=\"true\" create=\"true\" />",
                $"<folder external=\"/{PatchID}/StageData\" disc=\"/StageData\" recursive=\"true\" create=\"true\" />",
                $"<folder external=\"/{PatchID}/LayoutData\" disc=\"/LayoutData\" recursive=\"true\" create=\"true\" />",
                $"<folder external=\"/{PatchID}/AudioRes\" disc=\"/AudioRes\" recursive=\"true\" create=\"true\" />",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/UsEnglish\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/EuFrench\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/EuSpanish\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/EuItalian\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/EuDutch\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/EuGerman\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/UsFrench\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/UsSpanish\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/EuEnglish\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/JpJapanese\"/>",
                $"<folder external=\"/{Localize}/UsEnglish\" create=\"true\" recursive=\"true\" disc=\"/KrKorean\"/>",
                "\t</patch>",
                "</wiidisc>"
            };
            SaveFileDialog saveDialog = new()
            {
                InitialDirectory = Drive_Check.Check(),
                FileName = Game_ID,
                DefaultExt = ".xml",
                Filter = "Riivolution XML file|*.xml",
                FilterIndex = 1,
                CheckPathExists = true,
                Title = "Save the new xml file"
            };
            DialogResult result = saveDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fileName = saveDialog.FileName;
                File.WriteAllLines(fileName, lines, Encoding.UTF8);
            }
            return;
        }

        public static void Generate(in Main main)
        {
            var riiv = new RiivXML();
            riiv.SetNameAndRegions(main.GameTextBox.Text);
            var section = riiv.Options.CreateSection(main.SaveDataComboBox.Text);
            var option = section.CreateOption(main.OptionTextBox.Text);
            var choice = option.CreateChoice(main.ChoiceTextBox.Text);
            choice.Patches.Add(main.PatchTextBox.Text);
            var patch = riiv.CreatePatch(main.PatchTextBox.Text, main.FolderTextBox.Text);
            if (main.SaveDataComboBox.SelectedItem is string str)
            {
                if (str is not "No Custom Save")
                {
                    bool clone = default;
                    if (str is "Clone")
                    {
                        clone = true;
                    } else if (str is "Don't Clone")
                    {
                        clone = false;
                    }
                    patch.CreateSaveGamePatch("SaveGame/{$__gameid}{$__region}{$__maker}", clone);
                }
            }
            if ((bool)main.ObjectDataComboBox.SelectedItem)
            {
                patch.CreateFolderPatch($"{main.PatchTextBox.Text}/ObjectData", "/ObjectData", true, true);
            }
            if ((bool)main.StageDataComboBox.SelectedItem)
            {
                patch.CreateFolderPatch($"{main.PatchTextBox.Text}/StageData", "/StageData", true, true);
            }
            if ((bool)main.LayoutComboBox.SelectedItem)
            {
                patch.CreateFolderPatch($"{main.PatchTextBox.Text}/LayoutData", "/LayoutData", true, true);
            }
            if ((bool)main.AudioResComboBox.SelectedItem)
            {
                patch.CreateFolderPatch($"{main.PatchTextBox.Text}/AudioRes", "/AudioRes", true, true);
            }
            if ((bool)main.CustomMessageComboBox.SelectedItem)
            {
                patch.CreateFolderPatch($"{main.PatchTextBox.Text}/{main.LanguageTextBox.Text}", $"/{main.LanguageTextBox.Text}", true, true);
            }
            SaveFileDialog saveDialog = new()
            {
                InitialDirectory = Drive_Check.Check(),
                FileName = $"{main.GameTextBox.Text}{Extensions.TryParse<RiivXML.Region>(main.RegionComboBox.SelectedItem)}",
                DefaultExt = ".xml",
                Filter = "Riivolution XML file|*.xml",
                FilterIndex = 1,
                CheckPathExists = true,
                Title = "Save the new xml file"
            };
            DialogResult result = saveDialog.ShowDialog();
            if (result is DialogResult.OK)
            {
                var res = riiv.ToString();
                File.WriteAllText(saveDialog.FileName, res);
            }
        }
    }
}

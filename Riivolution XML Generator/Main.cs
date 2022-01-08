using System;
using System.Windows.Forms;
using XMLUtil;
using System.Collections.Generic;
using System.Linq;

namespace Riivolution_XML_Generator
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            CenterToScreen();
            SaveDataComboBox.DataSource = new string[]
            {
                "Clone",
                "Don't Clone",
                "No Custom Save"
            };
            ObjectDataComboBox.DataSource = new bool[]
            {
                false,
                true
            };
            StageDataComboBox.DataSource = new bool[]
            {
                false,
                true
            };
            LayoutComboBox.DataSource = new bool[]
            {
                false,
                true
            };
            AudioResComboBox.DataSource = new bool[]
            {
                false,
                true
            };
            CustomMessageComboBox.DataSource = new bool[]
            {
                false,
                true
            };
            var regions = Extensions.GetEnumValues<RiivXML.Region>().ToList();
            var strings = new List<string>
            {
                "None"
            };
            regions.ForEach(x => strings.Add(x.ToString()));
            RegionComboBox.DataSource = strings;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mbb = MessageBoxButtons.OK;
            MessageBoxIcon mbi = MessageBoxIcon.Warning;
            string warning = "Warning";
            if (GameTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Game ID!", warning, mbb, mbi);
                return;
            }
            if (LanguageTextBox.Text.Trim() == string.Empty)
            {
                //MessageBox.Show("Please enter the Riivolution Page name!",warning,mbb,mbi);
                //return;
            }
            if (OptionTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Option name!", warning, mbb, mbi);
                return;
            }
            if (PatchTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Patch ID!", warning, mbb, mbi);
                return;
            }
            if (FolderTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Folder Path!", warning, mbb, mbi);
                return;
            }
            if (SectionTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Section Name", warning, mbb, mbi);
                return;
            }
            if (ChoiceTextBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Choice Name", warning, mbb, mbi);
                return;
            }


            string gameid = GameTextBox.Text;
            string localize = LanguageTextBox.Text;
            string optionname = OptionTextBox.Text;
            string patchid = PatchTextBox.Text;
            string folderpath = FolderTextBox.Text;
            string region = Extensions.TryParse<RiivXML.Region>(RegionComboBox.SelectedItem).ToString();
            string sectionname = SectionTextBox.Text;
            string choicename = ChoiceTextBox.Text;
            //Classes.XML_Generator.Generate(gameid, localize, optionname, patchid, folderpath, region, sectionname, choicename);
            Classes.XML_Generator.Generate(this);
            MessageBox.Show("Finshed!", "Complete",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

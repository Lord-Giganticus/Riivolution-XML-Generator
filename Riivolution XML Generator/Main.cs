using System;
using System.Windows.Forms;

namespace Riivolution_XML_Generator
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mbb = MessageBoxButtons.OK;
            MessageBoxIcon mbi = MessageBoxIcon.Warning;
            string warning = "Warning";
            if (textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Game ID!", warning, mbb, mbi);
                return;
            }
            if (textBox2.Text.Trim() == string.Empty)
            {
                //MessageBox.Show("Please enter the Riivolution Page name!",warning,mbb,mbi);
                //return;
            }
            if (textBox3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Option name!", warning, mbb, mbi);
                return;
            }
            if (textBox4.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Patch ID!", warning, mbb, mbi);
                return;
            }
            if (textBox5.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Folder Path!", warning, mbb, mbi);
                return;
            }
            if (textBox6.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Game Region", warning, mbb, mbi);
                return;
            }
            if (textBox7.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Section Name", warning, mbb, mbi);
                return;
            }
            if (textBox8.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Choice Name", warning, mbb, mbi);
                return;
            }


            string gameid = textBox1.Text;
            string localize = textBox2.Text;
            string optionname = textBox3.Text;
            string patchid = textBox4.Text;
            string folderpath = textBox5.Text;
            string region = textBox6.Text;
            string sectionname = textBox7.Text;
            string choicename = textBox8.Text;
            string[] comboBox1 = new string[]
            {
                "Yes",
                "No"
            };
            string[] comboBox2 = new string[]
            {
                "Yes",
                "No"
            };
            string[] comboBox3 = new string[]
            {
                "Clone SaveGame",
                "Don't Clone SaveGame",
                "No Custom Save"
            };
            string[] comboBox4 = new string[]
            {
                "Yes",
                "No"
            };
            string[] comboBox5 = new string[]
            {
                "Yes",
                "No"
            };
            string[] comboBox6 = new string[]
            {
                "Yes",
                "No"
            };
            Classes.XML_Generator.Generate(gameid, localize, optionname, patchid, folderpath, region, sectionname, choicename);
            MessageBox.Show("Finshed! Press ok to exit.", "Complete",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

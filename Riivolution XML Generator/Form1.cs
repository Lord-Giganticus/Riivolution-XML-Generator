using System;
using System.Windows.Forms;

namespace Riivolution_XML_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Classes.Open.Url(linkLabel1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mbb = MessageBoxButtons.OK;
            MessageBoxIcon mbi = MessageBoxIcon.Warning;
            string warning = "Warning";
            if (textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Game ID!",warning,mbb,mbi);
                return;
            }
            if (textBox2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Riivolution Page name!",warning,mbb,mbi);
                return;
            } 
            if (textBox3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Option name!",warning,mbb,mbi);
                return;
            } 
            if (textBox4.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Patch ID!",warning,mbb,mbi);
                return;
            }
            if (textBox5.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Folder Path!",warning,mbb,mbi);
                return;
            }

            string gameid = textBox1.Text;
            string rgp = textBox2.Text;
            string on = textBox3.Text;
            string pid = textBox4.Text;
            string fp = textBox5.Text;
            Classes.XML_Generator.Generate(gameid, rgp, on, pid, fp);
            MessageBox.Show("Finshed! Press ok to exit.", "Complete",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Close();
        }
    }
}

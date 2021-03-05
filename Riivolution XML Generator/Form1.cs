using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string waring = "Warning";
            if (textBox1.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Please enter the Game ID!",waring,mbb,mbi);
            }
        }
    }
}

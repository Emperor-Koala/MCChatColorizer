using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCChatColorizer
{
    public partial class Form1 : Form
    {

        string logFile;
        string outFolder;

        public Form1()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.logFile = openFileDialog1.FileName;
                textBox1.Text = logFile;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.ProcessLog(logFile, outFolder);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.outFolder = folderBrowserDialog1.SelectedPath;
                textBox2.Text = outFolder;
                //TODO set folder stuff
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Engine
{
    public partial class error : Form
    {
        private string er;
        public error(string ex)
        {
            InitializeComponent();
            er = ex;
            textBox1.AppendText(ex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Saver.DumpText("ErrorWrite.txt",er);
            Environment.Exit(0);
        }
    }
}

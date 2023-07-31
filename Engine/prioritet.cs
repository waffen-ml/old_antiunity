using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine
{
    public partial class prioritet : Form
    {
        public string prioritet_info = "";

        public prioritet()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            prioritet_info = "upper";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            prioritet_info = "down";
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

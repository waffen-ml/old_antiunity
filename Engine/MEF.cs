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
    public partial class MEF : Form
    {
        public MEF()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                if (File.Exists(textBox1.Text))
                {
                    this.Visible = false;
                    ModelEditor mod = new ModelEditor(textBox1.Text);
                    
                }
                else MessageBox.Show("Ошибка пути!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else MessageBox.Show("Введите что-нибудь!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ModelEditor mod = new ModelEditor("none");
        }
    }
}

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
    partial class rgb_color : Form
    {
        public int r = 0;
        public int g = 0;
        public int b = 0;
        public string type = "";


        public rgb_color(ModelObject ob)
        {
            InitializeComponent();

            textBox1.Text = ob.color.R.ToString();
            textBox2.Text = ob.color.G.ToString();
            textBox3.Text = ob.color.B.ToString();


            
        }

        private bool Check()
        {
            try
            {
                r = Convert.ToInt32(textBox1.Text);
                g = Convert.ToInt32(textBox2.Text);
                b = Convert.ToInt32(textBox3.Text);
            }
            catch { return false; }

            if (r > 255) r = 255;
            if (r < 0) r = 0;
            if (g > 255) g = 255;
            if (g < 0) g = 0;
            if (b > 255) b = 255;
            if (b < 0) b = 0;
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            type = "color";
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                this.Close();

            }
            else MessageBox.Show("Введены неправильные данные!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

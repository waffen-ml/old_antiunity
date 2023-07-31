using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML.System;

namespace Engine
{
    partial class ColliderInfoForm : Form
    {
        public Collider cl = new Collider();

        public ColliderInfoForm(Collider c,int index)
        {
            InitializeComponent();
            cl = c;
            textBox1.Text = cl.x.ToString();
            textBox2.Text = cl.y.ToString();
            textBox3.Text = cl.shape.Size.X.ToString();
            textBox4.Text = cl.shape.Size.Y.ToString();
            textBox5.Text = cl.CollideTag;
            label6.Text = "Индекс:" + index.ToString();

        }

        private bool Check()
        {
            float one = 0f;
            float two = 0f;
            float three = 0f;
            float four = 0f;
            try
            {
                one = float.Parse(textBox1.Text);
                two = float.Parse(textBox2.Text);
                three = float.Parse(textBox3.Text);
                four = float.Parse(textBox4.Text);
            }
            catch { return false; }

            if (three <= 0 || four <= 0) return false;
            else return true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Вы точно хотите удалить коллайдер?", "Уточнение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(r == DialogResult.Yes)
            {
                cl.tag = "delete";
                Close();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Check())
            {
                try
                {
                    cl.x = float.Parse(textBox1.Text);
                    cl.y = float.Parse(textBox2.Text);
                    Vector2f s = cl.shape.Size;
                    s.X = float.Parse(textBox3.Text);
                    s.Y = float.Parse(textBox4.Text);
                    cl.shape.Size = s;
                    cl.CollideTag = textBox5.Text;
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Введены неправильные параметры!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введены неправильные параметры!","Ошибка!", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
    }
}

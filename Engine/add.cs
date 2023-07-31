using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML;
using SFML.System;


namespace Engine
{
    partial class add : Form
    {
        public ModelObject ob = new ModelObject();
        public string type = "";
        public Collider c = new Collider();


        public add()
        {
            InitializeComponent();
        }

        private bool check()
        {
            float c = 1;
            float a = 1;
            float b = 1;
            try
            {
                

                if(comboBox1.Text == "Окружность")
                c = float.Parse(textBox3.Text);
                else
                {
                    a = float.Parse(textBox1.Text);
                    b = float.Parse(textBox2.Text);
                }
                if(c > 0 && b > 0 && c > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (comboBox1.Text == "Коллайдер")
                {
                    Vector2f f = new Vector2f(float.Parse(textBox1.Text), float.Parse(textBox2.Text));
                    type = "collider";
                    c.shape.Size = f;
                    c.x = 350;
                    c.y = 240;


                }
                else if(comboBox1.Text == "Окружность")
                {
                    type = "normal";
                    ob.radius = float.Parse(textBox3.Text);
                    ob.x = 350;
                    ob.y = 240;
                    ob.type = "Circle";

                }
                else if(comboBox1.Text == "Квадрат")
                {
                    type = "normal";
                    ob.width = float.Parse(textBox1.Text);
                    ob.height = float.Parse(textBox2.Text);
                    ob.x = 350;
                    ob.y = 240;


                }
                else MessageBox.Show("Введены неправильные данные!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
            }
            else MessageBox.Show("Введены неправильные данные!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

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
using SFML.Graphics;
using System.IO;

namespace Engine
{
    partial class ObjectInfoForm : Form
    {
        public ModelObject obj;

        private SFML.Graphics.Color FromStringToColor(string cl)
        {
            SFML.Graphics.Color r = new SFML.Graphics.Color();

            if (cl == "White") r = SFML.Graphics.Color.White;
            else if (cl == "Black") r = SFML.Graphics.Color.Black;
            else if (cl == "Yellow") r = SFML.Graphics.Color.Yellow;
            else if (cl == "Blue") r = SFML.Graphics.Color.Blue;
            else if (cl == "Red") r = SFML.Graphics.Color.Red;
            else if (cl == "Magenta") r = SFML.Graphics.Color.Magenta;
            else if (cl == "Green") r = SFML.Graphics.Color.Green;
            else if (cl == "Cyan") r = SFML.Graphics.Color.Cyan;
            else return obj.color;

            return r;

        }

        private string FromColorToString(SFML.Graphics.Color cl)
        {
            if (cl == SFML.Graphics.Color.White) return "White";
            else if (cl == SFML.Graphics.Color.Black) return "Black";
            else if (cl == SFML.Graphics.Color.Yellow) return "Yellow";
            else if (cl == SFML.Graphics.Color.Red) return "Red";
            else if (cl == SFML.Graphics.Color.Cyan) return "Cyan";
            else if (cl == SFML.Graphics.Color.Magenta) return "Magenta";
            else if (cl == SFML.Graphics.Color.Green) return "Green";
            else if (cl == SFML.Graphics.Color.Blue) return "Blue";
            else return "Other";
        }

        public ObjectInfoForm(ModelObject o,int index)
        {
            InitializeComponent();
            obj = o;
            if (obj.type == "Cube") label9.Text = "Тип:Квадрат";
            else label9.Text = "Тип:Окружность";
            textBox1.Text = obj.x.ToString();
            textBox2.Text = obj.y.ToString();
            textBox3.Text = obj.radius.ToString();
            textBox4.Text = obj.texture;
            textBox5.Text = obj.width.ToString();
            textBox6.Text = obj.height.ToString();
            label10.Text = "Индекс:" + index.ToString();

            //combo
            comboBox1.Text = FromColorToString(obj.color);
            //combo 




        }

        private bool Check()
        {
            if (!File.Exists(textBox4.Text)) obj.texture = "none";
            else obj.texture = textBox4.Text;

            if(obj.type == "Circle")
            {
                try
                {
                    float one = 0f;
                    float two = 0f;
                    float three = 0f;
                    one = float.Parse(textBox1.Text);
                    two = float.Parse(textBox2.Text);
                    three = float.Parse(textBox3.Text);
                    if (three > 0) return true;
                    else return false;
                }
                catch { return false; }
            }
            else
            {
                try
                {
                    //1256
                    float one = 0;
                    float two = 0;
                    float three = 0;
                    float four = 0;

                    one = float.Parse(textBox1.Text);
                    two = float.Parse(textBox2.Text);
                    three = float.Parse(textBox5.Text);
                    four = float.Parse(textBox6.Text);

                    if (three > 0 && four > 0) return true;
                    else return false;

                }
                catch { return false; }

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult s = MessageBox.Show("Вы точно хотите это удалить?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(s == DialogResult.Yes)
            {
                obj.tag = "delete";
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                try
                {
                    obj.x = float.Parse(textBox1.Text);
                    obj.y = float.Parse(textBox2.Text);
                    obj.radius = float.Parse(textBox3.Text);
                    obj.width = float.Parse(textBox5.Text);
                    obj.height = float.Parse(textBox6.Text);
                    obj.color = FromStringToColor(comboBox1.Text);
                    if (!File.Exists(textBox4.Text)) obj.texture = "none";
                    else obj.texture = textBox4.Text;
                    if (FromColorToString(obj.color) != "Other") obj.color_type = "color";
                    else obj.color_type = "rgb";
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Введите значения правильно!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else MessageBox.Show("Введите значения правильно!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prioritet prior = new prioritet();
            prior.ShowDialog();
            if (prior.prioritet_info == "") return;
            obj.tag = prior.prioritet_info; 
        }
    }
}

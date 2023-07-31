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
using SFML;



namespace Engine
{
    
    partial class settings : Form
    {
        public reqest req;
        public bool done = false;

        public settings(reqest r)
        {
            InitializeComponent();
            done = false;
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.Black));
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.White));
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.Red));
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.Green));
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.Blue));
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.Magenta));
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.Cyan));
            comboBox1.Items.Add(FromColorToString(SFML.Graphics.Color.Yellow));
            comboBox1.Text = FromColorToString(r.bg);
            req = r;
            if (req.SaveMode) savemode.Text = "SaveMode:Включён";  
            else if(!req.SaveMode)savemode.Text = "SaveMode:Выключен";
            if (r.showcolliders == true) button1.Text = "Коллизии: Видны";
            else button1.Text = "Коллизии: Cпрятаны";
            textBox1.Text = req.speedMultiply.ToString();

        }

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
            else return SFML.Graphics.Color.White;

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
            else return "";
        }

        private void Error()
        {
            MessageBox.Show("Введены неправильные значения!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool Check()
        {
            float m = 0;
            try
            {
                m = float.Parse(textBox1.Text);
            }
            catch{ Error(); return false; }
            if (m <= 1) { Error(); return false; }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Check()) return;
                req.bg = FromStringToColor(comboBox1.Text);
                req.speedMultiply = float.Parse(textBox1.Text);
                done = true;
                this.Close();
            }
            catch { Error(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(req.showcolliders == true)
            {
                req.showcolliders = false;
                button1.Text = "Коллизии: Cпрятаны";
            }
            else
            {
                req.showcolliders = true;
                button1.Text = "Коллизии: Видны";
            }
        }

        private void savemode_Click(object sender, EventArgs e)
        {
            if (!req.SaveMode) req.SaveMode = true;
            else if (req.SaveMode) req.SaveMode = false;

            if(req.SaveMode) savemode.Text = "SaveMode:Включен";
            else if(!req.SaveMode) savemode.Text = "SaveMode:Выключен";
        }

        //(yandex == shit) = true


    }
}

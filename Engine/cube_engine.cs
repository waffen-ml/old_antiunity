using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Threading;


namespace Engine
{
    class CubeEngine
    {

        public RenderWindow win;
        private VideoMode mode = new VideoMode(800, 600);
        private string title = "Engine!";
        public point camera = new point();
        public bool isworking = true;
        public int window_width = 800;
        public int window_height = 600;
        public int pixel_height = 0;
        public int pixel_width;
        public List<RectangleShape> pixels = new List<RectangleShape>();
        public Color bg = new Color(Color.Black);

        public void sleep(int what)
        {
            Thread.Sleep(what);
        }

        public void set_title(string i)
        {
            title = i;
        }

        public void Clear()
        {
            for (int i = 0; i < pixels.Count; i++)
            {
                pixels[i].FillColor = Color.White;
            }
        }

        public void DrawPix(int idx, Color cl)
        {
            pixels[idx].FillColor = cl;
        }
        public void DrawCube(int x, int y, int width, int height, Color cl)
        {
            for (int i = x - Convert.ToInt32(camera.x); i < width - camera.x; i++)
            {
                for (int j = y - Convert.ToInt32(camera.y); j < height - camera.y; j++)
                {
                    try
                    {
                        if (i < (window_width / pixel_width))
                            pixels[i + j * (window_height / pixel_height)].FillColor = cl;
                    }
                    catch
                    {
                        //nothing
                    }
                }


            }
        }


        public void SetWindow(int w, int h)
        {
            window_height = h;
            window_width = w;
            mode = new VideoMode(Convert.ToUInt32(w), Convert.ToUInt32(h));

        }
        public void SetPixels(int w, int h)
        {
            pixel_height = h;
            pixel_width = w;
            int count_w = window_width / w;
            int count_h = window_height / h;

            for (int i = 0; i < count_h * count_w; i++)
            {
                RectangleShape pixel = new RectangleShape();
                pixel.Size = new Vector2f(pixel_width, pixel_height);
                pixels.Add(pixel);
            }

            for (int x = 0; x < count_w; x++)
            {
                for (int y = 0; y < count_h; y++)
                {
                    pixels[x + count_h * y].Position = new Vector2f(pixel_width * x, pixel_height * y);

                }

            }


        }



        private void RenderPixels()
        {
            for (int i = 0; i < pixels.Count; i++)
            {
                if (pixels[i].FillColor == bg) continue;
                win.Draw(pixels[i]);
            }


        }

        public void Start()
        {

            win = new RenderWindow(mode, title, Styles.Close);
            win.SetVerticalSyncEnabled(true);
            win.SetFramerateLimit(60);
            Run();
        }

        public bool Check_Collision(FloatRect f1, FloatRect f2)
        {
            if (f1.Intersects(f2))
            {
                return true;
            }
            return false;
        }
        public void Error(string code)
        {
            isworking = false;
            error k = new error(code);
            k.ShowDialog();
            Environment.Exit(0);

        }
        private void Run()
        {
            win.Closed += OnClose;

            while (win.IsOpen)
            {
                try
                {
                    win.DispatchEvents();
                    win.Clear(bg);
                    RenderPixels();
                    win.Display();
                }
                catch (Exception a)
                {
                    Error(a.ToString());
                }

            }


        }
        private void OnClose(object sender, EventArgs e)
        {
            isworking = false;
            win.Close();
        }

    }

}

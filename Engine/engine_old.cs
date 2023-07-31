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
    class AntiUnity_old
    {
        private View camera = new View();
        public bool camera_on = false;
        public bool autoclear = true;
        public RenderWindow win;
        public point camera_pos = new point();
        public bool isworking = true;
        private WinUtility mode = new WinUtility();
        private Color bg = new Color(Color.Black);
        public List<ModelObject> ObjectsToRender = new List<ModelObject>();

        private void Render()
        {
            foreach(ModelObject a in ObjectsToRender)
            {
                RenderObject(a);
            }
        }

        public void sleep(int what)
        {
            Thread.Sleep(what);
        }

        public void Set_Title(string i)
        {
            mode.title = i;
        }

        public void SetBG(Color c)
        {
            bg = c;
        }

        public void Clear()
        {
            win.Clear(bg);
        }

        public void SetupWindow(uint w, uint h)
        {
            mode.height = h;
            mode.width = w;

        }

        public void UpdateCamera()
        {
            if(camera_on)
            camera.Center = new Vector2f(camera_pos.x, camera_pos.y);
        }

        public void RenderObject(ModelObject ob)
        {
            try
            {
                if (ob.type == "Circle")
                {
                    CircleShape circle = new CircleShape(ob.radius);
                    circle.Position = new Vector2f(ob.x, ob.y);
                    circle.FillColor = ob.color;
                    if (ob.texture != "" && ob.texture != "none") try { circle.Texture = new Texture(ob.texture); } catch { circle.FillColor = Color.Magenta; }
                    win.Draw(circle);

                }
                else if (ob.type == "Cube")
                {
                    RectangleShape cube = new RectangleShape();
                    cube.Size = new Vector2f(ob.width, ob.height);
                    cube.Position = new Vector2f(ob.x, ob.y);
                    cube.FillColor = ob.color;
                    if (ob.texture != "" && ob.texture != "none") try { cube.Texture = new Texture(ob.texture); } catch { cube.FillColor = Color.Magenta; }
                    win.Draw(cube);
                }
                else Error("Unknow object in model!");
            }
            catch (Exception ex)
            {
                Error(ex.ToString());
            }
        }

        public void SetCameraCenter(Vector2f pos)
        {
            if(camera_on)
            camera.Center = pos;
        }

        public void Start()
        {
            win = new RenderWindow(new VideoMode(mode.width,mode.height), mode.title, Styles.Close);
            win.SetVerticalSyncEnabled(true);
            win.SetFramerateLimit(60);
            win.Closed += OnClose;
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

        public void Display()
        {
            if(autoclear)Clear();
            win.Display();
        }

        private void Run()
        {
            while (win.IsOpen)
            {
                try
                {

                    win.DispatchEvents();
                    Render();
                    Display();

                }
                catch (Exception a)
                {
                    Error(a.ToString());
                }

            }


        }

        public void RenderModel(Model mdl)
        {
            try
            {
                if (mdl.objects.Count <= 0) return;
                foreach(ModelObject ob in mdl.objects)
                {
                    if (ob.type == "Circle")
                    {
                        CircleShape circle = new CircleShape(ob.radius);
                        circle.Position = new Vector2f(mdl.position.x + ob.x, mdl.position.y + ob.y);
                        circle.FillColor = ob.color;
                        if (ob.texture != "" && ob.texture != "none") try { circle.Texture = new Texture(ob.texture); } catch { circle.FillColor = Color.Magenta; }
                        win.Draw(circle);
                    }
                    else if (ob.type == "Cube")
                    {
                        RectangleShape cube = new RectangleShape();
                        cube.Size = new Vector2f(ob.width,ob.height);
                        cube.Position = new Vector2f(mdl.position.x + ob.x, mdl.position.y + ob.y);
                        cube.FillColor = ob.color;
                        if (ob.texture != "" && ob.texture != "none") try { cube.Texture = new Texture(ob.texture); } catch { cube.FillColor = Color.Magenta; }
                        win.Draw(cube);
                    }
                    else Error("Unknow object in model!");
                }

            }
            catch (Exception ex)
            {
                Error(ex.ToString());
            }

        }

        private void OnClose(object sender, EventArgs e)
        {
            isworking = false;
            win.Close();
        }

    }

}

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
using SFML.Audio;

namespace Engine
{
    class Engine
    {
        public static RenderWindow win;
        public static bool isworking = false;
        public static bool autoclear = true;
        public static Color bg = new Color(Color.Black);
        public static WinUtility win_mode = new WinUtility();
        private static List<Model> mdl_to_draw = new List<Model>();
        private static List<ModelObject> obj_to_draw = new List<ModelObject>();
        private static View cam = new View();
        private static Font font = new Font("arial.ttf");
        private static bool first_frame = false;
        private static List<WindowText> texts_to_render = new List<WindowText>();
        public static bool can_start = true;
        public static Networking network;
        public static bool Focus = true;
        private static Clock clock = new Clock();
        public static float winTime = 1f;
        private static List<Scene> scenes = new List<Scene>();
        private static string now_scene = "";
        private static string first_scene = "";
        private static bool LogoShow = true;
        private static string EngineName = "ANTIUNITY";
        public static float SecondsToShowLogo = 3.5f;
        public static float TimeValue = 1f;
        public static bool RenderObjectsFirst = true;
        public static float MassValue = 10f;

        //icon
        private static string icon_path = "";
        private static uint bytescount_icon = 32;
        //icon

        public static bool IsKeyPressed(Keyboard.Key key)
        {
            if (!Focus) return false;
            return Keyboard.IsKeyPressed(key);
        }

        private static void LogoShowScene()
        {
            try
            {
                if (!LogoShow) return;
                Timer timer = new Timer();
                //logo
                ModelObject logo = new ModelObject();
                logo.texture = "logo.png";
                logo.width = 200;
                logo.height = 200;
                logo.x = win_mode.width / 2 - logo.width / 2;
                logo.y = win_mode.height / 2 - logo.height / 2 - win_mode.height / 10;
                AddObjectToRender(logo);
                //logo
                RenderText("Made with",Color.Black,2, logo.x - 35, logo.y - 70, "ui");
                RenderText(EngineName, Color.Black, 2f, (logo.x + logo.width / 2) - 2f *17* EngineName.Length * 0.51f, logo.y + logo.width +2, "ui");
                bg = new Color(170,170,170);
                timer.Start();
                while (timer.GetTime() < SecondsToShowLogo)
                {
                    Clear();
                    RenderAll();
                    UpdateTexts();
                    win.Display();
                    win.DispatchEvents();

                }
                Reoload();
            }
            catch (Exception a)
            {
                Error("Logo Error:" +a.ToString());
            }
        }

        public static WindowText GetText(int index)
        {
            try
            {
                return texts_to_render[index];
            }
            catch { return null; }
        }

        public static Collider GetMouseCollider()
        {
            point mspos = new point();
            Vector2i mouse = Mouse.GetPosition(win);
            mspos.x = mouse.X + cam.Center.X - win_mode.width / 2;
            mspos.y = mouse.Y + cam.Center.Y - win_mode.height / 2;
            Collider col = new Collider();
            col.shape.Size = new Vector2f(1, 1);
            col.shape.Position = new Vector2f(mspos.x, mspos.y);
            return col;
        }

        private static void Reoload()
        {
            bg = Color.Black;
            mdl_to_draw.Clear();
            obj_to_draw.Clear();
            texts_to_render.Clear();
            can_clear = true;
            cam.Center = new Vector2f(win_mode.width / 2, win_mode.height / 2);
            RenderObjectsFirst = true;
        }

        public static point GetMousePosition()
        {
            point mspos = new point();
            Vector2i mouse = Mouse.GetPosition(win);
            mspos.x = mouse.X + cam.Center.X - win_mode.width / 2;
            mspos.y = mouse.Y + cam.Center.Y - win_mode.height / 2;
            return mspos;
        }

        public static bool IsLeftButtonClicked()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left)) return true;
            else return false;
        }

        public static bool IsRightButtonClicked()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Right)) return true;
            else return false;
        }

        //scenes
        public static void SetFirstScene(string name)
        {
            if (isworking) Error("Too late!");
            for(int a = 0; a < scenes.Count;a++)
            {
                if(scenes[a].name == name)
                {
                    first_scene = name;
                    now_scene = name;
                    return;
                }
            }
            Error("Invalid first scene!");
        }

        public static void SetSceneWaitTime(string name, int time)
        {
            if (time < 0) Error("Invalid waiting time!");
            for(int a = 0; a < scenes.Count;a++)
            {
                if(scenes[a].name == name)
                {
                    scenes[a].wait = time;
                    return;
                }
            }
            Error("Invalid scene name!");
        }

        public static void LoadScene(string name)
        {
            if (!isworking) Error("Engine is not ready yet!");
            for(int a = 0; a < scenes.Count;a++)
            {
                if(scenes[a].name == name)
                {
                    try
                    {
                        ShutdownScene();
                        Reoload();
                        scenes[a].Start();
                        now_scene = name;
                    }
                    catch(Exception ex) { Error("Invalid scene:" + ex.ToString()); }
                    return;
                    }
            }
            Error("Invalid scene:" + name);
        }

        public static void AddScene(Scene.Void start, Scene.Void update,string name)
        {
            if (name == "") Error("Invalid function name!");
            for(int a = 0; a < scenes.Count;a++)
            {
                if (scenes[a].name == name) Error("Invalid function name!");
            }
            Scene s = new Scene();
            s.Setup(start, update);
            s.name = name;
            scenes.Add(s);
        }

        public static void RemoveScene(string name)
        {
            for(int a = 0; a < scenes.Count;a++)
            {
                if (scenes[a].name == name)
                {
                    if (scenes[a].name == now_scene) ShutdownScene();
                    scenes.RemoveAt(a);
                    return;
                }
            }
        }

        private static void ShutdownScene()
        {
            if (now_scene == "") return;
            for(int a = 0; a < scenes.Count;a++)
            {
                if(scenes[a].name == now_scene)
                {
                    try
                    {
                        scenes[a].Shutdown();
                    }
                    catch { Error("Error with scene! #4905"); }
                    now_scene = "";
                }
            }
        }

        private static void StartFirstScene()
        {
            if (first_scene == "") Error("Invalid first scene! It is nothing now.");
            for(int a = 0; a < scenes.Count;a++)
            {
                if(scenes[a].name == first_scene)
                {
                    try
                    {
                        scenes[a].Start();
                    }
                    catch { Error("ERROR #4033"); }
                    return;
                }
            }
        }

        //scenes

        public static void sleep(double what)
        {
            Thread.Sleep(Convert.ToInt32(what * 1000));
        }

        public static void Quit()
        {
            isworking = false;
            //if (network != null) network.Shutdown();
            ShutdownScene();
            Environment.Exit(0);
        }

        public static void StartNetworking(string addr,int port,string nick, bool isServer)
        {
            network = new Networking();
            //network.Setup(addr, port, nick, isServer);
        }

        public static float winTimeFloat(float p)
        {
            return p * winTime;
        }

        public static int RandomInt(int one, int two)
        {
            Thread.Sleep(3);
            Random rand = new Random(DateTime.Now.Millisecond / 5);
            return rand.Next(one, two + 1);
        }

        public static void UpdateText(int id, string txt)
        {
            try
            {
                texts_to_render[id].UpdateText(txt);
            }
            catch { return; }
        }

        public static int RenderText(string text, Color cl, float scale, float x, float y, string type)
        {
            if (type != "ui" && type != "not_ui") return 0;
            WindowText t = new WindowText();
            t.Setup(text, cl, scale, x, y, font);
            t.type = type;
            t.id = texts_to_render.Count;
            texts_to_render.Add(t);
            return t.id;
        }

        private static void UpdateTextsId()
        {
            for(int a = 0; a < texts_to_render.Count;a++)
            {
                texts_to_render[a].id = a;
            }
        }

        public static void DisableText(int id)
        {
            try
            {
                texts_to_render.RemoveAt(id);
            }
            catch { return; }
            UpdateTextsId();
        }

        public static void MoveCamera(float value,string how)
        {
            Vector2f s = cam.Center;
            if (how == "up") s.Y -= value;
            else if (how == "down") s.Y += value;
            else if (how == "left") s.X -= value;
            else if (how == "right") s.X += value;
            cam.Center = s;
        }

        public static void SetCameraCenter(point p)
        {
            cam.Center = new Vector2f(p.x, p.y);
        }

        public static int get_id(int index,string how)
        {
            try
            {
                if (how == "model") return mdl_to_draw[index].id;
                else if (how == "object") return obj_to_draw[index].id;
                else return -1;
            }
            catch { return -1; }
        }

        public static void UpdateIds(string how)
        {
            if(how == "model")
            {
                for (int a = 0; a < mdl_to_draw.Count; a++) mdl_to_draw[a].id = a;
            }
            else if(how == "object")
            {
                for (int a = 0; a < obj_to_draw.Count; a++) obj_to_draw[a].id = a;
            }
        }

        private static bool can_clear = true;

        public static bool Prioritete(int index,string how)
        {
            if(how == "model")
            {
                try
                {
                    mdl_to_draw.Add(mdl_to_draw[index]);
                    mdl_to_draw.RemoveAt(index);
                    UpdateIds("model");
                }
                catch { return false; }
                return true;
            }
            else if(how == "object")
            {
                try
                {
                    obj_to_draw.Add(obj_to_draw[index]);
                    obj_to_draw.RemoveAt(index);
                    UpdateIds("object");
                }
                catch { return false; }
                return true;
            }
            return false;
        }

        public static int AddModelToRender(Model mdl)
        {
            mdl_to_draw.Add(mdl);
            return mdl_to_draw.Count - 1;
        }

        public static int AddObjectToRender(ModelObject obj)
        {
            obj_to_draw.Add(obj);
            return obj_to_draw.Count - 1;
        }

        public static void Model_RemoveFromRender(Model mdl)
        {
            for(int a = 0; a < mdl_to_draw.Count;a++)
            {
                if(mdl_to_draw[a] == mdl)
                {
                    mdl_to_draw.RemoveAt(a);
                    UpdateIds("model");
                    return;
                }
            }
        }

        public static void Object_RemoveFromRender(ModelObject obj)
        {
            for (int a = 0; a < obj_to_draw.Count; a++)
            {
                if (obj_to_draw[a] == obj)
                {
                    obj_to_draw.RemoveAt(a);
                    UpdateIds("object");
                    return;
                }
            }
        }

        public static void RemoveFromRender(int id, string how)
        {
           if(how == "model")
            {
                try
                {
                    mdl_to_draw.RemoveAt(id);
                    UpdateIds("model");
                }
                catch { }
                return;
            }
           else if(how == "object")
            {
                try
                {
                    obj_to_draw.RemoveAt(id);
                    UpdateIds("object");
                }
                catch { }
                return;
            }
        }

        public static void Manage(string how)
        {
            if (how == "start")
            {
                can_clear = false;
            }
            else if (how == "end") can_clear = true;
            else Error("Invalid manage!");  
        }

        public static void Error(string code)
        {
            isworking = false;
            ShutDownNetwork();
            ShutdownScene();
            error k = new error(code);
            k.ShowDialog();
            Environment.Exit(0);

        }

        public static void SetupWindow(uint width,uint height)
        {
            win_mode.width = width;
            win_mode.height = height;
        }

        public static void SetTitle(string t)
        {
            win_mode.title = t;
        }

        public static void SetIcon(string path,uint bytescount)
        {
            if (bytescount <= 0) bytescount = 32;
            bytescount_icon = bytescount;
            icon_path = path;
        }

        public static void Start()
        {
            if (first_scene == "") Quit();
            try
            {
                win = new RenderWindow(new VideoMode(win_mode.width, win_mode.height), win_mode.title, Styles.Close);
                win.SetVerticalSyncEnabled(true);
                win.SetFramerateLimit(60);
                win.Closed += OnClose;
                cam.Center = new Vector2f(win_mode.width / 2, win_mode.height / 2);
                cam.Size = new Vector2f(win_mode.width, win_mode.height);
                win.GainedFocus += Gfocus;
                win.LostFocus += Lfocus;
                if (icon_path == "") win.SetIcon(32, 32, new Image("logo.png").Pixels);
                else win.SetIcon(bytescount_icon, bytescount_icon, new Image(icon_path).Pixels);
                LogoShowScene();
                StartFirstScene();
            }
            catch(Exception e) { Error("Error:" + e.ToString()); }
            isworking = true;
            Run();

        }

        private static void UpdateTexts()
        {
            for(int a = 0; a < texts_to_render.Count;a++)
            {
                if(texts_to_render[a].type == "not_ui")
                {
                    win.Draw(texts_to_render[a].text);
                }
                else if(texts_to_render[a].type == "ui")
                {
                    texts_to_render[a].UpdateUi(cam,win_mode);
                    win.Draw(texts_to_render[a].text);
                }

            }
        }

        public static void ShutDownNetwork()
        {
            try
            {
                //network.isworking = false;
                //network.Shutdown();
            }
            catch { }
        }

        public static void Clear()
        {
            win.Clear(bg);
        }

        public static void UpdateTime()
        {
            winTime = clock.ElapsedTime.AsSeconds() * TimeValue;
            clock.Restart();
        }

        private static void RenderAll()
        {
            if (RenderObjectsFirst)
            {
                for (int a = 0; a < mdl_to_draw.Count; a++)
                {
                    Renderer.RenderModel(mdl_to_draw[a], win);
                }
                for (int a = 0; a < obj_to_draw.Count; a++)
                {
                    Renderer.RenderObject(obj_to_draw[a], win);
                }
            }
            else
            {
                for (int a = 0; a < obj_to_draw.Count; a++)
                {
                    Renderer.RenderObject(obj_to_draw[a], win);
                }
                for (int a = 0; a < mdl_to_draw.Count; a++)
                {
                    Renderer.RenderModel(mdl_to_draw[a], win);
                }
            }
        }

        private static void Run()
        {
            while (!can_start) ;
            while (win.IsOpen)
            {
                try
                {
                    if(can_clear && autoclear)Clear();
                    win.SetView(cam);
                    UpdateTime();
                    RenderAll();
                    UpdateTexts();
                    win.Display();
                    win.DispatchEvents();
                    if (!first_frame) first_frame = true;
                }
                catch (Exception a)
                {
                    Error(a.ToString());
                }
            }
        }

        private static void OnClose(object sender, EventArgs e)
        {
            isworking = false;
            ShutDownNetwork();
            ShutdownScene();
            win.Close();
            Environment.Exit(0);
        }

        public static void ModelEditorOpen()
        {
            MEF ms = new MEF();
            ms.ShowDialog();
        }

        private static void Gfocus(object sender, EventArgs e)
        {
            Focus = true;
        }

        private static void Lfocus(object sender, EventArgs e)
        {
            Focus = false;
        }

    }
}

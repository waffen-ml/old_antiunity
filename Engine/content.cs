using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using SFML.Audio;

namespace Engine
{
    class point
    {
        public float x = 0;
        public float y = 0;
        public float z = 0;

        public void delete()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public void equal(point e)
        {
            x = e.x;
            y = e.y;
            z = e.z;

        }
    }

    class WinUtility
    {
        public uint width = 0;
        public uint height = 0;
        public string title = "AntiUnity standart window";

    }

    class ModelObject
    {
        public string type = "Cube";
        public float x = 0f;
        public float y = 0f;
        public Color color = new Color(Color.White);
        public string texture = "none";
        public float width;
        public float height;
        public float radius;
        public float scale = 1f;
        public string color_type = "color";
        public string tag = "";
        public int id;

        public void Move(float how_much, string how)
        {
            if (how == "right") x += how_much;
            else if (how == "left") x -= how_much;
            else if (how == "up") y -= how_much;
            else if (how == "down") y += how_much;

        }

        public void Scale(float value)
        {
            if (value == 1)
            {
                radius /= scale;
                height /= scale;
                width /= scale;
                scale = 1;

                return;
            }
            scale = value;
            radius *= value;
            height *= value;
            width *= value;


        }

    }

    class Physics
    {
        public float Mass = 0f;
        private float Now_Mass_Value = 0f;

        public void Stop()
        {
            Now_Mass_Value = 0;
        }

        public float GetGravity()
        {
            if(Now_Mass_Value < Mass*2)
            {
                Now_Mass_Value += Engine.MassValue /1000 * Mass;
            }
            return Now_Mass_Value;
        }
    }

    class Collider
    {
        public float x = 0;
        public float y = 0;
        public RectangleShape shape = new RectangleShape();
        public string tag = "";
        public string CollideTag = "none";

        public void Update(point pos)
        {
            Vector2f s = shape.Position;
            s.X = x + pos.x;
            s.Y = y + pos.y;
            shape.Position = s;
            if (CollideTag == "") CollideTag = "none";
        }
    }

    class WindowText
    {
        public Text text = new Text();
        public int id = 0;
        public string type = "not_ui";

        public float GetTextSize()
        {
            return text.CharacterSize * text.DisplayedString.Length; 
        }

        public void Setup(string txt, Color cl, float scale, float x, float y,Font f)
        {
            text = new Text(txt, f);
            text.Color = cl;
            text.Scale = new Vector2f(scale, scale);
            text.Position = new Vector2f(x, y);

        }

        public void UpdateText(string new_text)
        {
            text.DisplayedString = new_text;
        }
        public void UpdateUi(View cam,WinUtility ut)
        {
            if (type == "not_ui") return;
            Vector2f now = text.Position;
            now.X += cam.Center.X - ut.width /2;
            now.Y += cam.Center.Y - ut.height / 2;
            text.Position = now;

        }

    }

    class Collision
    {
        private static void Error(string code)
        {
            error r = new error(code);
            r.ShowDialog();
            Environment.Exit(0);
        }

        public static bool CollidePart(Model yourmdl, Model mdl, int index)
        {
            for (int b = 0; b < mdl.colliders.Count; b++)
            {
                if (yourmdl.colliders[index].shape.GetGlobalBounds().Intersects(mdl.colliders[b].shape.GetGlobalBounds())) return true;
            }
            return false;

        }

        public static bool Collide(Model yourmdl,Model mdl)
        {
            for (int a = 0; a < yourmdl.colliders.Count; a++)
            {
                for (int b = 0; b < mdl.colliders.Count; b++)
                {
                    if (yourmdl.colliders[a].shape.GetGlobalBounds().Intersects(mdl.colliders[b].shape.GetGlobalBounds())) return true;
                }
            }
            return false;

        }

        public static bool CollideTo(Model yourmdl,Collider col)
        {
            for (int a = 0; a < yourmdl.colliders.Count; a++)
            {
                if (yourmdl.colliders[a].shape.GetGlobalBounds().Intersects(col.shape.GetGlobalBounds())) return true;
            }
            return false;
        }

        public static bool CollidePartTo(Model yourmdl,Collider col, int index)
        {
            try
            {
                if (yourmdl.colliders[index].shape.GetGlobalBounds().Intersects(col.shape.GetGlobalBounds())) return true;
                return false;
            }
            catch { return false; }
        }

        public static bool CollideToTag(Model yourmdl,Model mdl, string tag)
        {
            for (int a = 0; a < yourmdl.colliders.Count; a++)
            {
                for (int b = 0; b < mdl.colliders.Count; b++)
                {
                    if (yourmdl.colliders[a].shape.GetGlobalBounds().Intersects(mdl.colliders[b].shape.GetGlobalBounds()) && mdl.colliders[b].CollideTag == tag) return true;
                }
            }
            return false;
        }

        public static Collider GetCollidedCollider(Model yourmdl,Model mdl)
        {
            for (int a = 0; a < yourmdl.colliders.Count; a++)
            {
                for (int b = 0; b < mdl.colliders.Count; b++)
                {
                    if (yourmdl.colliders[a].shape.GetGlobalBounds().Intersects(mdl.colliders[b].shape.GetGlobalBounds())) return mdl.colliders[b];
                }
            }
            return null;
        }

        public static bool CollidePartToTag(Model yourmdl,Model mdl, string tag, int index)
        {
            try
            {
                for (int b = 0; b < mdl.colliders.Count; b++)
                {
                    if (yourmdl.colliders[index].shape.GetGlobalBounds().Intersects(mdl.colliders[b].shape.GetGlobalBounds()) && mdl.colliders[b].CollideTag == tag) return true;
                }
                return false;
            }
            catch { Error("Invalid collider."); return false; }
        }

    }

    class Timer
    {
        private Clock clock;
        private bool isworking = false;

        public float GetTime()
        {
            if (isworking)
                return clock.ElapsedTime.AsSeconds();
            else return 0f;
        }

        public void Stop()
        {
            if(isworking)
            {
                isworking = false;
                clock = null;
            }
        }

        public void Start()
        {
            if (!isworking)
            {
                clock = new Clock();
                isworking = true;
            }
        }

        public void Restart()
        {
            if (isworking) clock.Restart();
        }
    }

    class Model
    {
        public List<ModelObject> objects = new List<ModelObject>();
        public point position = new point();
        public List<Collider> colliders = new List<Collider>();
        public float scale = 1f;
        public void Move(float how_much, string how)
        {
            if (how == "right") position.x += how_much;
            else if (how == "left") position.x -= how_much;
            else if (how == "up") position.y -= how_much;
            else if (how == "down") position.y += how_much;

        }
        public int id = 0;
        public Physics physics;
        //to move_towards
        private point last_point;
        private float mt_valX = 0;
        private float mt_valY = 0;
        //to move_towards

        private void Error(string code)
        {
            error r = new error(code);
            r.ShowDialog();
            Environment.Exit(0);
        }

        public void GivePhysics(float Mass)
        {
            if (physics != null) Error("Physics of this object is already gained.");
            physics = new Physics();
            physics.Mass = Mass;
        }

        public void Gravity(bool reversed)
        {
            if (physics == null) Error("Invalid physics!");
            if(!reversed)
            {
                position.y += physics.GetGravity();
            }
            else
            {
                position.y -= physics.GetGravity();
            } 
        }

        public void StopGravity()
        {
            if (physics == null) Error("Model physics is null already.");
            physics.Stop();
        }

        public void MoveTowardsGentle(point pos,int time)
        {
            time *= 10;
            if (pos.x == position.x && pos.y == position.y) return;
            if (pos.x - position.x < 1 / time && pos.y - position.y < 1 / time) position.equal(pos);
            float valX = (pos.x - position.x) / time;
            float valY = (pos.y - position.y) / time;
            position.x += valX;
            position.y += valY;
        }

        public void MoveTowards(point pos, int time)
        {
            time *= 10;
            if (pos.x == position.x && pos.y == position.y) { if(last_point!=null)last_point = null; return; }
            if (last_point == null || pos.x != last_point.x || pos.y != last_point.y)
            {
                mt_valX = (pos.x - position.x) / time;
                mt_valY = (pos.y - position.y) / time;
                last_point = new point();
                last_point.equal(pos);
                position.x += mt_valX;
                position.y += mt_valY;
                return;
            }
            else
            {
                position.x += mt_valX;
                position.y += mt_valY;
                return;
            }
        }

        public void Scale(float value)
        {
            if (value == 1)
            {
                for (int a = 0; a < colliders.Count; a++)
                {
                    colliders[a].x /= scale;
                    colliders[a].y /= scale;
                    Vector2f sc = colliders[a].shape.Size;
                    sc.X /= scale;
                    sc.Y /= scale;
                    colliders[a].shape.Size = sc;
                }
                for (int a = 0; a < objects.Count; a++)
                {
                    objects[a].x /= scale;
                    objects[a].y /= scale;
                    objects[a].width /= scale;
                    objects[a].height /= scale;
                    objects[a].radius /= scale;
                    objects[a].scale = 1;
                }
                scale = 1;
                return;
            }

            for (int a = 0; a < colliders.Count; a++)
            {
                colliders[a].x *= value;
                colliders[a].y *= value;
                Vector2f sc = colliders[a].shape.Size;
                sc.X *= value;
                sc.Y *= value;
                colliders[a].shape.Size = sc;
            }
            for (int a = 0; a < objects.Count; a++)
            {
                objects[a].x *= value;
                objects[a].y *= value;
                objects[a].width *= value;
                objects[a].height *= value;
                objects[a].radius *= value;
                objects[a].scale = value;
            }
            scale = value;



        }

        public void UpdateColliders()
        {
            for (int a = 0; a < colliders.Count; a++)
            {
                colliders[a].Update(position);
            }
        }

    }

    class Saver
    {
        private static void Error(string what)
        {
            error r = new error(what);
            r.ShowDialog();
            Environment.Exit(0);
        }

        public static Model OpenModel(string path) {
            Model r = new Model();
            string[] lines = new string[] { };
            int count = 0;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch
            {
                Error("Can't find model's file!");
            }
            if (lines.Length <= 0) Error("Problem with model file");
            if (lines[count] != "/MODEL/") Error("Invalid model!");
            count++;
            try
            {
                r.position.x = float.Parse(lines[count]);
                count++;
                r.position.y = float.Parse(lines[count]);
                count++;
                r.scale = float.Parse(lines[count]);
                count++;
            }
            catch { Error("Invalid model!"); }

            for (int a = count; a < lines.Length; a++)
            {
                if (lines[a] == "/COLLIDER/")
                {
                    a++;
                    Vector2f s = new Vector2f();
                    Vector2f size = new Vector2f();
                    Collider cs = new Collider();
                    try
                    {
                        s.X = float.Parse(lines[a]);
                        a++;
                        s.Y = float.Parse(lines[a]);
                        a++;
                        cs.CollideTag = lines[a];a++;
                        size.X = float.Parse(lines[a]);
                        a++;
                        size.Y = float.Parse(lines[a]);
                        a++;
                        cs.shape.Size = new Vector2f(size.X, size.Y);
                        cs.x = r.position.x + s.X;
                        cs.y = r.position.y + s.Y;
                        if (lines[a] != "/COLLIDER_END/") Error("Invalid model");
                        else
                        {
                            r.colliders.Add(cs);
                            continue;
                        }

                    }
                    catch { Error("Invalid model!"); }

                }
                else if (lines[a] == "/OBJECT/")
                {
                    ModelObject ob = new ModelObject();
                    a++;
                    ob.type = lines[a];
                    a++;
                    ob.texture = lines[a];
                    a++;
                    try
                    {
                        ob.x = float.Parse(lines[a]); a++;
                        ob.y = float.Parse(lines[a]); a++;
                        ob.scale = float.Parse(lines[a]); a++;
                        ob.width = float.Parse(lines[a]); a++;
                        ob.height = float.Parse(lines[a]); a++;
                        ob.radius = float.Parse(lines[a]); a++;
                    }
                    catch { Error("Invalid model!"); }
                    ob.color_type = lines[a]; a++;
                    if (ob.color_type == "color")
                    {
                        if (lines[a] == "White") ob.color = Color.White;
                        else if (lines[a] == "Black") ob.color = Color.Black;
                        else if (lines[a] == "Yellow") ob.color = Color.Yellow;
                        else if (lines[a] == "Red") ob.color = Color.Red;
                        else if (lines[a] == "Cyan") ob.color = Color.Cyan;
                        else if (lines[a] == "Green") ob.color = Color.Green;
                        else if (lines[a] == "Magenta") ob.color = Color.Magenta;
                        else if (lines[a] == "Blue") ob.color = Color.Blue;
                        else Error("Invalid model!");
                        a++;
                    }
                    else if (ob.color_type == "rgb")
                    {
                        int R, G, B;
                        R = 0;
                        G = 0;
                        B = 0;
                        try
                        {
                            R = Convert.ToInt32(lines[a]); a++;
                            G = Convert.ToInt32(lines[a]); a++;
                            B = Convert.ToInt32(lines[a]); a++;
                        }
                        catch { Error("Invalid color!"); }
                        ob.color = new Color(Convert.ToByte(R), Convert.ToByte(G), Convert.ToByte(B));

                    }
                    else Error("Invalid colortype!");

                    if (lines[a] != "/OBJECT_END/") Error("Invalid model!");

                    else
                    {
                        r.objects.Add(ob);
                        continue;
                    }



                }
                else if (lines[a] == "/MODEL_END/") break;

                else Error("Invalid model!");



            }
            return r;
        }

        public static void DumpModel(Model mdl, string path)
        {
            List<string> lines = new List<string>();
            lines.Add("/MODEL/");
            lines.Add(mdl.position.x.ToString());
            lines.Add(mdl.position.y.ToString());
            lines.Add(mdl.scale.ToString());
            for (int a = 0; a < mdl.objects.Count; a++)
            {
                lines.Add("/OBJECT/");
                lines.Add(mdl.objects[a].type);
                lines.Add(mdl.objects[a].texture);
                lines.Add(mdl.objects[a].x.ToString());
                lines.Add(mdl.objects[a].y.ToString());
                lines.Add(mdl.objects[a].scale.ToString());
                lines.Add(mdl.objects[a].width.ToString());
                lines.Add(mdl.objects[a].height.ToString());
                lines.Add(mdl.objects[a].radius.ToString());
                lines.Add(mdl.objects[a].color_type);
                if (mdl.objects[a].color_type == "color")
                {
                    string to_add = "";
                    if (mdl.objects[a].color == Color.White) to_add = "White";
                    else if (mdl.objects[a].color == Color.Black) to_add = "Black";
                    else if (mdl.objects[a].color == Color.Yellow) to_add = "Yellow";
                    else if (mdl.objects[a].color == Color.Red) to_add = "Red";
                    else if (mdl.objects[a].color == Color.Cyan) to_add = "Cyan";
                    else if (mdl.objects[a].color == Color.Blue) to_add = "Blue";
                    else if (mdl.objects[a].color == Color.Magenta) to_add = "Magenta";
                    else if (mdl.objects[a].color == Color.Green) to_add = "Green";
                    else Error("Invalid model!");
                    lines.Add(to_add);
                }
                else if (mdl.objects[a].color_type == "rgb")
                {
                    lines.Add(mdl.objects[a].color.R.ToString());
                    lines.Add(mdl.objects[a].color.G.ToString());
                    lines.Add(mdl.objects[a].color.B.ToString());
                }
                else Error("Invalid color type!");

                lines.Add("/OBJECT_END/");

            }
            foreach (Collider c in mdl.colliders)
            {
                lines.Add("/COLLIDER/");
                lines.Add(c.x.ToString());
                lines.Add(c.y.ToString());
                lines.Add(c.CollideTag);
                lines.Add(c.shape.Size.X.ToString());
                lines.Add(c.shape.Size.Y.ToString());
                lines.Add("/COLLIDER_END/");
            }
            lines.Add("/MODEL_END/");

            try
            {
                File.WriteAllLines(path, lines.ToArray());
            }
            catch
            {
                File.CreateText(path);
                File.WriteAllLines(path, lines.ToArray());
            }



        }

        public static ModelObject OpenObject(string path)
        {
            ModelObject r = new ModelObject();
            string[] lines = new string[] { };
            int a = 0;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch { Error("Can't find object's data!"); }
            if (lines[a] != "/OBJECT/") Error("Invalid object!");
            a++;
            r.type = lines[a]; a++;
            r.texture = lines[a]; a++;
            try
            {
                r.x = float.Parse(lines[a]); a++;
                r.y = float.Parse(lines[a]); a++;
                r.scale = float.Parse(lines[a]); a++;
                r.width = float.Parse(lines[a]); a++;
                r.height = float.Parse(lines[a]); a++;
                r.radius = float.Parse(lines[a]); a++;
            }

            catch { Error("Invalid object!"); }
            r.color_type = lines[a]; a++;
            if (r.color_type == "color")
            {
                if (lines[a] == "White") r.color = Color.White;
                else if (lines[a] == "Black") r.color = Color.Black;
                else if (lines[a] == "Yellow") r.color = Color.Yellow;
                else if (lines[a] == "Red") r.color = Color.Red;
                else if (lines[a] == "Cyan") r.color = Color.Cyan;
                else if (lines[a] == "Green") r.color = Color.Green;
                else if (lines[a] == "Magenta") r.color = Color.Magenta;
                else if (lines[a] == "Blue") r.color = Color.Blue;
                else Error("Invalid object!");
                a++;
            }
            else if (r.color_type == "rgb")
            {
                try
                {
                    int R, G, B = 0;
                    R = Convert.ToInt32(lines[a]); a++;
                    G = Convert.ToInt32(lines[a]); a++;
                    B = Convert.ToInt32(lines[a]); a++;
                    r.color = new Color(Convert.ToByte(R), Convert.ToByte(G), Convert.ToByte(B));
                }
                catch { Error("Invalid object!"); }
            }
            else Error("Invalid color type!");


            if (lines[a] != "/OBJECT_END/") Error("Invalid object!");
            return r;



        }

        public static void DumpObject(ModelObject obj, string path)
        {
            List<string> lines = new List<string>();
            lines.Add("/OBJECT/");
            lines.Add(obj.type);
            lines.Add(obj.texture);
            lines.Add(obj.x.ToString());
            lines.Add(obj.y.ToString());
            lines.Add(obj.scale.ToString());
            lines.Add(obj.width.ToString());
            lines.Add(obj.height.ToString());
            lines.Add(obj.radius.ToString());
            lines.Add(obj.color_type);
            if (obj.color_type == "color")
            {
                string to_add = "";
                if (obj.color == Color.White) to_add = "White";
                else if (obj.color == Color.Black) to_add = "Black";
                else if (obj.color == Color.Yellow) to_add = "Yellow";
                else if (obj.color == Color.Red) to_add = "Red";
                else if (obj.color == Color.Cyan) to_add = "Cyan";
                else if (obj.color == Color.Blue) to_add = "Blue";
                else if (obj.color == Color.Magenta) to_add = "Magenta";
                else if (obj.color == Color.Green) to_add = "Green";
                else Error("Invalid object!");
                lines.Add(to_add);
            }
            else if (obj.color_type == "rgb")
            {
                lines.Add(obj.color.R.ToString());
                lines.Add(obj.color.G.ToString());
                lines.Add(obj.color.B.ToString());
            }
            else Error("Invalid object!");

            lines.Add("/OBJECT_END/");
            try
            {
                File.WriteAllLines(path, lines.ToArray());
            }
            catch
            {
                File.CreateText(path);
                File.WriteAllLines(path, lines.ToArray());
            }


        }

        public static void DumpLines(string toFile, string[] lines)
        {
            try
            {
                if (!File.Exists(toFile))
                {
                    File.CreateText(toFile);
                }
                File.WriteAllLines(toFile, lines);
            }
            catch { Error("There is a problem with file:" + toFile); }

        }

        public static string[] GetLines(string toFile)
        {
            try
            {
                return File.ReadAllLines(toFile);
            }
            catch { Error("There is an error with this path:" + toFile);return null; }
        }

        public static void DumpText(string toFile,string text)
        {
            try
            {
                if (!File.Exists(toFile))
                {
                    File.CreateText(toFile);
                }
                File.WriteAllText(toFile, text);
            }
            catch { Error("There is a problem with file:" + toFile); }

        }

        public static string GetText(string toFile)
        {
            try
            {
                return File.ReadAllText(toFile);
            }
            catch { Error("There is an error with this path:" + toFile); return null; }
        }

    }

    class Renderer
    {
        private static void Error(string code)
        {
            error k = new error(code);
            k.ShowDialog();
            Environment.Exit(0);

        }

        public static void RenderModel(Model mdl, RenderWindow win)
        {
            mdl.UpdateColliders();
            foreach (ModelObject ob in mdl.objects)
            {
                try
                {
                    if (ob.type == "Circle")
                    {
                        CircleShape circle = new CircleShape(ob.radius);
                        circle.Position = new Vector2f(mdl.position.x + ob.x, mdl.position.y + ob.y);
                        if (ob.texture != "" && ob.texture != "none") try { circle.Texture = new Texture(ob.texture); } catch { circle.FillColor = Color.Magenta; }
                        else circle.FillColor = ob.color;
                        win.Draw(circle);

                    }
                    else if (ob.type == "Cube")
                    {
                        RectangleShape cube = new RectangleShape();
                        cube.Size = new Vector2f(ob.width, ob.height);
                        cube.Position = new Vector2f(mdl.position.x + ob.x, mdl.position.y + ob.y);
                        if (ob.texture != "" && ob.texture != "none") try { cube.Texture = new Texture(ob.texture); }
                            catch { cube.FillColor = Color.Magenta; }
                        else cube.FillColor = ob.color;
                        win.Draw(cube);
                    }
                    else Error("Unknow object in model!");
                }
                catch (Exception ex)
                {
                    Error(ex.ToString());
                }
            }

        }

        public static void RenderObject(ModelObject ob,RenderWindow win)
        {
            try
            {
                if (ob.type == "Circle")
                {
                    CircleShape circle = new CircleShape(ob.radius);
                    circle.Position = new Vector2f(ob.x, ob.y);
                    if (ob.texture != "" && ob.texture != "none") try { circle.Texture = new Texture(ob.texture); } catch { circle.FillColor = Color.Magenta; }
                    else circle.FillColor = ob.color;
                    win.Draw(circle);

                }
                else if (ob.type == "Cube")
                {
                    RectangleShape cube = new RectangleShape();
                    cube.Size = new Vector2f(ob.width, ob.height);
                    cube.Position = new Vector2f(ob.x, ob.y);
                    if (ob.texture != "" && ob.texture != "none") try { cube.Texture = new Texture(ob.texture); } catch { cube.FillColor = Color.Magenta; }
                    else cube.FillColor = ob.color;
                    win.Draw(cube);
                }
                else Error("Unknow object in model!");
            }
            catch (Exception ex)
            {
                Error(ex.ToString());
            }
        }


    }

    [Serializable]
    class Message
    {
        public string data = "";
        public string author = "";
        public string info = "";
    }

    class BinaryFormat {

        public byte[] ObjectToBytes(object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms,obj);
                return ms.ToArray();
            }
        }

        public object BytesToObject(byte[] b)
        {
            if (b == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(b, 0, b.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return bf.Deserialize(ms);
            }
        }

    }

    class Scene
    {
        public string name;
        public delegate void Void();
        public int wait = 20;
        private Thread update_thread;
        private bool shutdown = false;
        private Void update_method;
        private Void start_method;

        public void Shutdown()
        {
            shutdown = true;
        }

        public void Update()
        {
            while(!shutdown)
            {
                Thread.Sleep(wait);
                update_method();
            }
        }

        public void Start()
        {
            shutdown = false;
            if(start_method != null)
            start_method();
            if (update_method != null)
            update_thread.Start();
        }

        public void Setup(Void start_meth,Void update_meth)
        {
            start_method = start_meth;
            update_method = update_meth;
            update_thread = new Thread(new ThreadStart(Update));
        }

    }

    class SoundManage
    {
        private Sound sound;
        private SoundBuffer buffer;
        public string name = "Audio";
        public bool looping = false;
        public string path;

        public void Load()
        {
            buffer = new SoundBuffer(path);
            sound = new Sound(buffer);
        }
    
        public void Play()
        {
            if (looping) sound.Loop = true;
            else sound.Loop = false;
            sound.Play();
        }

        public void Stop()
        {
            sound.Stop();
        }

    }

    class SoundManager
    {
        private static List<SoundManage> sounds = new List<SoundManage>();

        private static void Error(string what)
        {
            System.Windows.Forms.MessageBox.Show(what, "Error!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            Environment.Exit(0);
        }

        public static void AddSound(string path, string name, bool looping)
        {
            for(int a = 0; a < sounds.Count;a++)
                if (name == sounds[a].name) Error("This sound name is usless.");
            SoundManage sm = new SoundManage();
            sm.path = path;
            sm.name = name;
            sm.looping = looping;
            sounds.Add(sm);
        }

        public static void StopSound(string name)
        {
            for (int a = 0; a < sounds.Count; a++)
            {
                try
                {
                    if (sounds[a].name == name) sounds[a].Stop();
                    return;
                }
                catch { Error("Sound error."); }
            }
        }

        public static void Load(string name)
        {
            for(int a = 0; a < sounds.Count;a++)
            {
                try
                {
                    if (sounds[a].name == name) sounds[a].Load();
                    return;
                }
                catch { Error("Sound error."); }
            }
        }

        public static void Load()
        {
            for (int a = 0; a < sounds.Count; a++)
            {
                try
                {
                    sounds[a].Load();
                }
                catch { Error("Sound error."); }
            }
        }

        public static void Play(string name)
        {
            for (int a = 0; a < sounds.Count; a++)
            {
                try
                {
                    if (sounds[a].name == name) sounds[a].Play();
                    return;
                }
                catch { Error("Sound error."); }
            }
        }

        public static void RemoveSound(string name)
        {
            for (int a = 0; a < sounds.Count; a++)
            {
                try
                {
                    if (sounds[a].name == name) sounds.RemoveAt(a);
                    return;
                }
                catch { Error("Sound error."); }
            }
        }

        public static void RemoveSound()
        {
            for (int a = 0; a < sounds.Count; a++)
            {
                try
                {
                    sounds.RemoveAt(a);
                }
                catch { Error("Sound error."); }
            }
        }

    }

    class Player
    {
        public point position = new point();
        public Socket socket;
        public string name = "AntiUnityPlayer";
        public List<Message> messages = new List<Message>();
    }

    class Networking {
        
    }



}

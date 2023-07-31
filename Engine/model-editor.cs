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
using System.Windows.Forms;
using System.IO;

namespace Engine
{
    class reqest
    {
        public bool showcolliders = false;
        public Color bg = new Color(Color.White);
        public bool SaveMode = false;
        public float speedMultiply = 2f;
    }

    class button
    {
        public Text t;
        public RectangleShape shape = new RectangleShape();
        public string function;

        public void Setup(Font font, string text)
        {
            t = new Text(text, font);
        }

    }

    class ModelEditor
    {
        private RenderWindow win = new RenderWindow(new VideoMode(700, 500), "Model Editor", Styles.Close);
        private Model mdl = new Model();
        private List<button> buttons = new List<button>();
        private Color bg = new Color(Color.White);
        private bool left_pressed = false;
        private string path = "none";
        private Font font;
        private float speed = 7f;
        private point real = new point();
        private bool showcolliders = false;
        private bool showbuttons = true;
        private Clock EditorTime = new Clock();
        private bool Focus = true;
        private bool SaveMode = false;
        private float speedValue = 340f;
        private float speedMultiply = 2f;
        //buttons utility//
        private bool right_pressed = false;
        private bool open_pressed = false;
        private bool save_pressed = false;
        private bool showc_pressed = false;

        private bool delete_all_b = false;
        private bool c_b = false;
        private bool cub_b = false;
        private bool coll_b = false;
        private bool sh_m_b = false;
        private bool cc_b = false;
        private bool copy_b = false;
        private bool delete_b = false;
        private bool color_b = false;
        private bool savemode_b = false;
        private bool alt_b = false;
        //buttons utility//

        //mouseUtility//
        private float diffX = 0f;
        private float diffY = 0f;
        //mouseUtility//


        public void Error(string code)
        {
            error k = new error(code);
            k.ShowDialog();
            Environment.Exit(0);

        }

        private bool IsMouseInTheWindow()
        {
            float x = Mouse.GetPosition(win).X;
            float y = Mouse.GetPosition(win).Y;
            if (x >= 0 && x <= win.Size.X && y >= 65 && y <= win.Size.Y) return true;
            return false;
        }

        public ModelEditor(string paths)
        {
            if (paths != "none")
            {
                mdl = Saver.OpenModel(paths);
                if (mdl == null) Error("Invalid path!");
            }
            path = paths;
            try
            {
                font = new Font("arial.ttf");
            }
            catch { Error("Invalid font!"); }
            mdl.position.x = 0;
            mdl.position.y = 0;
            Start();

        }

        private void CheckKeysToAdd()
        {
            if (!IsMouseInTheWindow()) return;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                if(!cub_b)
                {
                    ModelObject obj = new ModelObject();
                    obj.color = Color.Black;
                    obj.type = "Cube";
                    obj.width = 75;
                    obj.height = 75;
                    obj.x = Mouse.GetPosition(win).X - obj.width / 2 - mdl.position.x;
                    obj.y = Mouse.GetPosition(win).Y - obj.height / 2 - mdl.position.y;
                    mdl.objects.Add(obj);
                    cub_b = true;
                }
            }
            else cub_b = false;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                if(!c_b)
                {
                    ModelObject obj = new ModelObject();
                    obj.color = Color.Black;
                    obj.radius = 60;
                    obj.type = "Circle";
                    obj.x = Mouse.GetPosition(win).X - obj.radius - mdl.position.x;
                    obj.y = Mouse.GetPosition(win).Y - obj.radius - mdl.position.y;
                    mdl.objects.Add(obj);

                    c_b = true;
                }
            }
            else c_b = false;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                if(!coll_b )
                {
                    Collider r = new Collider();
                    r.shape.Size = new Vector2f(75, 75 );
                    r.x = Mouse.GetPosition(win).X - r.shape.Size.X / 2 - mdl.position.x;
                    r.y = Mouse.GetPosition(win).Y - r.shape.Size.Y / 2 - mdl.position.y;
                    showcolliders = true;
                    mdl.colliders.Add(r);
                    coll_b = true;
                }
            }
            else coll_b = false;
        }

        private void Save()
        {
            if (path == "none")
            {
                save_window s = new save_window();
                s.ShowDialog();
                path = s.path;
                if (path == "none") return;
                point now = new point();
                now.equal(mdl.position);
                mdl.position.equal(real);
                Saver.DumpModel(mdl, path);
                mdl.position.equal(now);
                MessageBox.Show("Сохранено!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                point now = new point();
                now.equal(mdl.position);
                mdl.position.equal(real);
                Saver.DumpModel(mdl, path);
                mdl.position.equal(now);
                MessageBox.Show("Сохранено!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Open()
        {
            save_window s = new save_window();
            s.ShowDialog();
            if (s.path == "none") return;
            if (!File.Exists(s.path))
            {
                MessageBox.Show("Указан неверный путь!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (s.path != path)
            {
                path = s.path;
                mdl = Saver.OpenModel(path);
                real.equal(mdl.position);
                mdl.position.x = 0;
                mdl.position.y = 0;
                MessageBox.Show("Модель успешно открыта!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Вы не можете открыть эту же модель.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RightMouseCheck()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                if (!right_pressed)
                {
                    Vector2i mousePos = Mouse.GetPosition(win);
                    float x_m = mousePos.X;
                    float y_m = mousePos.Y;
                    RectangleShape zone = new RectangleShape();
                    zone.Size = new Vector2f(1, 1);
                    zone.Position = new Vector2f(x_m, y_m);
                    if (showcolliders)
                    {
                        for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                        {
                            if (mdl.colliders[a].shape.GetGlobalBounds().Intersects(zone.GetGlobalBounds()))
                            {
                                Info(a, "collider");
                                return;
                            }

                        }
                    }

                    for (int a = mdl.objects.Count - 1; a >= 0; a--)
                    {
                        if (mdl.objects[a].type == "Cube")
                        {
                            RectangleShape this_cube = new RectangleShape();
                            this_cube.Position = new Vector2f(mdl.objects[a].x + mdl.position.x, mdl.objects[a].y + mdl.position.y);
                            this_cube.Size = new Vector2f(mdl.objects[a].width, mdl.objects[a].height);
                            if (this_cube.GetGlobalBounds().Intersects(zone.GetGlobalBounds()))
                            {
                                Info(a,"object");
                                return;
                            }
                        }
                        else if(mdl.objects[a].type == "Circle")
                        {
                            CircleShape this_circle = new CircleShape(mdl.objects[a].radius);
                            this_circle.Position = new Vector2f(mdl.position.x + mdl.objects[a].x, mdl.position.y + mdl.objects[a].y);
                            if(zone.GetGlobalBounds().Intersects(this_circle.GetGlobalBounds())) {
                                Info(a, "object");
                                return;
                            }

                        }
                    }

                    right_pressed = true;
                }
            }
            else
            {
                right_pressed = false;
            }
        }

        private void MouseFunction(string function)
        {
            if (function == "save") Save();
            else if (function == "open") Open();
            else if (function == "settings") Settings();
            else if (function == "add") Add();

        }

        private void MovingCheck()
        {
            Vector2i mousePos = Mouse.GetPosition(win);
            float x_m = mousePos.X + diffX;
            float y_m = mousePos.Y + diffY;
            if (!SaveMode)
            {
                if (showcolliders)
                {
                    for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                    {
                        if (mdl.colliders[a].tag == "moving")
                        {
                            mdl.colliders[a].x = x_m - mdl.position.x - mdl.colliders[a].shape.Size.X / 2;
                            mdl.colliders[a].y = y_m - mdl.position.y - mdl.colliders[a].shape.Size.Y / 2;
                            return;
                        }
                    }
                }

                for (int a = mdl.objects.Count - 1; a >= 0; a--)
                {
                    if (mdl.objects[a].type == "Cube")
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            mdl.objects[a].x = x_m - mdl.position.x - mdl.objects[a].width / 2;
                            mdl.objects[a].y = y_m - mdl.position.y - mdl.objects[a].height / 2;
                            return;
                        }
                    }
                    else
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            mdl.objects[a].x = x_m - mdl.position.x - mdl.objects[a].radius / 2;
                            mdl.objects[a].y = y_m - mdl.position.y - mdl.objects[a].radius / 2;
                            return;
                        }
                    }
                }
            }
            else
            {
                if (showcolliders)
                {
                    for (int a = 0; a < mdl.colliders.Count; a++)
                    {
                        if (mdl.colliders[a].tag == "moving")
                        {
                            mdl.colliders[a].x = x_m - mdl.position.x - mdl.colliders[a].shape.Size.X / 2;
                            mdl.colliders[a].y = y_m - mdl.position.y - mdl.colliders[a].shape.Size.Y / 2;
                            return;
                        }
                    }
                }

                for (int a = 0; a < mdl.objects.Count; a++)
                {
                    if (mdl.objects[a].type == "Cube")
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            mdl.objects[a].x = x_m - mdl.position.x - mdl.objects[a].width / 2;
                            mdl.objects[a].y = y_m - mdl.position.y - mdl.objects[a].height / 2;
                            return;
                        }
                    }
                    else
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            mdl.objects[a].x = x_m - mdl.position.x - mdl.objects[a].radius / 2;
                            mdl.objects[a].y = y_m - mdl.position.y - mdl.objects[a].radius / 2;
                            return;
                        }
                    }
                }
            }
            //обработка нажатий
            if (!SaveMode)
            {
                if (showcolliders)
                {
                    for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                    {
                        float x = mdl.colliders[a].shape.Position.X;
                        float y = mdl.colliders[a].shape.Position.Y;

                        if (mdl.colliders[a].tag == "moving") return;
                        if (x_m >= x && x_m <= x + mdl.colliders[a].shape.Size.X && y_m >= y && y_m <= y + mdl.colliders[a].shape.Size.Y)
                        {
                            mdl.colliders[a].tag = "moving";
                            diffX = -x_m + mdl.colliders[a].shape.Position.X + mdl.colliders[a].shape.Size.X / 2;
                            diffY = -y_m + mdl.colliders[a].shape.Position.Y + mdl.colliders[a].shape.Size.Y / 2;
                            mdl.colliders.Add(mdl.colliders[a]);
                            mdl.colliders.RemoveAt(a);
                            return;

                        }

                    }
                }


                for (int a = mdl.objects.Count - 1; a >= 0; a--)
                {
                    if (mdl.objects[a].type == "Cube")
                    {
                        float x = mdl.objects[a].x + mdl.position.x;
                        float y = mdl.objects[a].y + mdl.position.y;
                        if (x_m >= x && x_m <= x + mdl.objects[a].width && y_m >= y && y_m <= y + mdl.objects[a].height)
                        {
                            diffX = -x_m + x + mdl.objects[a].width / 2;
                            diffY = -y_m + y + mdl.objects[a].height / 2;
                            mdl.objects[a].tag = "moving";
                            mdl.objects.Add(mdl.objects[a]);
                            mdl.objects.RemoveAt(a);
                            return;
                        }
                    }
                    else
                    {
                        CircleShape shape = new CircleShape(mdl.objects[a].radius);
                        shape.Position = new Vector2f(mdl.objects[a].x + mdl.position.x, mdl.objects[a].y + mdl.position.y);
                        RectangleShape s = new RectangleShape();
                        s.Size = new Vector2f(1, 1);
                        s.Position = new Vector2f(x_m, y_m);
                        if (shape.GetGlobalBounds().Intersects(s.GetGlobalBounds()))
                        {
                            diffX = -x_m + shape.Position.X + mdl.objects[a].radius / 2;
                            diffY = -y_m + shape.Position.Y + mdl.objects[a].radius / 2;
                            mdl.objects[a].tag = "moving";
                            mdl.objects.Add(mdl.objects[a]);
                            mdl.objects.RemoveAt(a);
                            return;
                        }

                    }
                }
            }
            else
            {
                if (showcolliders)
                {
                    for (int a = 0; a < mdl.colliders.Count; a++)
                    {
                        float x = mdl.colliders[a].shape.Position.X;
                        float y = mdl.colliders[a].shape.Position.Y;

                        if (mdl.colliders[a].tag == "moving") return;
                        if (x_m >= x && x_m <= x + mdl.colliders[a].shape.Size.X && y_m >= y && y_m <= y + mdl.colliders[a].shape.Size.Y)
                        {
                            mdl.colliders[a].tag = "moving";
                            diffX = -x_m + mdl.colliders[a].shape.Position.X + mdl.colliders[a].shape.Size.X / 2;
                            diffY = -y_m + mdl.colliders[a].shape.Position.Y + mdl.colliders[a].shape.Size.Y / 2;
                            return;

                        }

                    }
                }


                for (int a = 0;a< mdl.objects.Count;a++)
                {
                    if (mdl.objects[a].type == "Cube")
                    {
                        float x = mdl.objects[a].x + mdl.position.x;
                        float y = mdl.objects[a].y + mdl.position.y;
                        if (x_m >= x && x_m <= x + mdl.objects[a].width && y_m >= y && y_m <= y + mdl.objects[a].height)
                        {
                            diffX = -x_m + x + mdl.objects[a].width / 2;
                            diffY = -y_m + y + mdl.objects[a].height / 2;
                            mdl.objects[a].tag = "moving";
                            return;
                        }
                    }
                    else
                    {
                        CircleShape shape = new CircleShape(mdl.objects[a].radius);
                        shape.Position = new Vector2f(mdl.objects[a].x + mdl.position.x, mdl.objects[a].y + mdl.position.y);
                        RectangleShape s = new RectangleShape();
                        s.Size = new Vector2f(1, 1);
                        s.Position = new Vector2f(x_m, y_m);
                        if (shape.GetGlobalBounds().Intersects(s.GetGlobalBounds()))
                        {
                            diffX = -x_m + shape.Position.X + mdl.objects[a].radius / 2;
                            diffY = -y_m + shape.Position.Y + mdl.objects[a].radius / 2;
                            mdl.objects[a].tag = "moving";
                            return;
                        }

                    }
                }
            }
        }

        private void RenderColliders()
        {
            for(int a = 0; a< mdl.colliders.Count;a++)
            {
                RectangleShape c = new RectangleShape();
                c.FillColor = Color.Red;
                c.Size = mdl.colliders[a].shape.Size;
                mdl.UpdateColliders();
                Vector2f p = new Vector2f(mdl.colliders[a].x + mdl.position.x, mdl.colliders[a].y + mdl.position.y);
                c.Position = p;
                win.Draw(c);
            }
        } 

        private void CheckKeysToScale()
        {
            float value;
            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) value = 4f;
            else value = 2f;


            if (Keyboard.IsKeyPressed(Keyboard.Key.L))
            {
                if (showcolliders)
                {
                    for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                    {
                        if (mdl.colliders[a].tag == "moving")
                        {
                            Vector2f s = mdl.colliders[a].shape.Size;
                            s.X += value;
                            mdl.colliders[a].shape.Size = s;
                            return;
                        }
                    }
                }
                for (int a = mdl.objects.Count - 1; a >= 0; a--)
                {
                    if (mdl.objects[a].tag == "moving")
                    {
                        if (mdl.objects[a].type == "Cube") mdl.objects[a].width += value;
                        else mdl.objects[a].radius += value;
                        return;
                    }
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.J))
            {
                if (showcolliders)
                {
                    for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                    {
                        if (mdl.colliders[a].tag == "moving")
                        {
                            Vector2f s = mdl.colliders[a].shape.Size;
                            if (s.X <= value+1) return;
                            s.X -= value;
                            mdl.colliders[a].shape.Size = s;
                            return;
                        }
                    }
                }
                for (int a = mdl.objects.Count - 1; a >= 0; a--)
                {
                    if (mdl.objects[a].tag == "moving")
                    {
                        if (mdl.objects[a].type == "Cube")
                        {
                            if (mdl.objects[a].width <= value + 1) return;
                            mdl.objects[a].width -= value;
                        }
                        else
                        {
                            if (mdl.objects[a].radius <= value + 1) return;
                            mdl.objects[a].radius -= value;
                        }
                        return;
                    }
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.I))
            {
                if (showcolliders)
                {
                    for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                    {
                        if (mdl.colliders[a].tag == "moving")
                        {
                            Vector2f s = mdl.colliders[a].shape.Size;
                            s.Y += value;
                            mdl.colliders[a].shape.Size = s;
                            return;
                        }
                    }
                }
                for (int a = mdl.objects.Count - 1; a >= 0; a--)
                {
                    if (mdl.objects[a].tag == "moving")
                    {
                        if(mdl.objects[a].type == "Cube") mdl.objects[a].height += value;
                        else mdl.objects[a].radius += value;
                        return;
                    }
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.K))
            {
                if (showcolliders)
                {
                    for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                    {
                        if (mdl.colliders[a].tag == "moving")
                        {
                            Vector2f s = mdl.colliders[a].shape.Size;
                            if (s.Y <= value + 1) return;
                            s.Y -= value;
                            mdl.colliders[a].shape.Size = s;
                            return;
                        }
                    }
                }
                for (int a = mdl.objects.Count - 1; a >= 0; a--)
                {
                    if (mdl.objects[a].tag == "moving")
                    {
                        if (mdl.objects[a].type == "Cube")
                        {
                            if (mdl.objects[a].height <= value + 1) return;
                            mdl.objects[a].height -= value;
                        }
                        else
                        {
                            if (mdl.objects[a].radius <= value + 1) return;
                            mdl.objects[a].radius -= value;
                        }
                        return;
                    }
                }
            }
        }

        private void RgbColor(int index)
        {
            Color r = mdl.objects[index].color;
            bool y = false;
            if (mdl.objects[index].color_type == "color") y = true;
            rgb_color s = new rgb_color(mdl.objects[index]);
            s.ShowDialog();
            if(s.type == "color")
            {
                if(y) mdl.objects[index].color = r;
                else mdl.objects[index].color = Color.Black;
                return;
            }
            mdl.objects[index].color_type = "rgb";
            mdl.objects[index].color = new Color(Convert.ToByte(s.r), Convert.ToByte(s.g), Convert.ToByte(s.b));
        }

        private void CheckKeyToCreateCollider()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.U))
            {
                if(!copy_b)
                {
                    for(int a = mdl.objects.Count - 1; a >= 0; a--)
                    {
                        if(mdl.objects[a].tag == "moving")
                        {
                            ModelObject obj = new ModelObject();
                            obj.x = 20 + mdl.objects[a].x;
                            obj.y = 20 + mdl.objects[a].y;
                            obj.width = mdl.objects[a].width;
                            obj.height = mdl.objects[a].height;
                            obj.color = mdl.objects[a].color;
                            obj.texture = mdl.objects[a].texture;
                            obj.type = mdl.objects[a].type;
                            obj.radius = mdl.objects[a].radius;
                            obj.color_type = mdl.objects[a].color_type;
                            obj.scale = mdl.objects[a].scale;
                            obj.tag = "";
                            mdl.objects.Add(obj);
                            MessageBox.Show("Копия успешно создана.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    if (showcolliders)
                    {
                        for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                        {
                            if(mdl.colliders[a].tag == "moving")
                            {
                                Collider c = new Collider();
                                c.x = 20 + mdl.colliders[a].x;
                                c.y = 20 + mdl.colliders[a].y;
                                c.tag = "";
                                c.shape.Size = mdl.colliders[a].shape.Size;
                                c.shape.Position = mdl.colliders[a].shape.Position;
                                mdl.colliders.Add(c);
                                MessageBox.Show("Копия успешно создана.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;

                            }
                        }
                    }
                    copy_b = true;
                }

            }
            else copy_b = false;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                if(!cc_b)
                {
                    for(int a = mdl.objects.Count - 1; a >= 0; a--)
                    {
                        if(mdl.objects[a].tag == "moving")
                        {
                            Collider c = new Collider();
                            c.x = mdl.objects[a].x;
                            c.y = mdl.objects[a].y;
                            if (mdl.objects[a].type == "Cube")
                                c.shape.Size = new Vector2f(mdl.objects[a].width, mdl.objects[a].height);
                            else c.shape.Size = new Vector2f(mdl.objects[a].radius * 2, mdl.objects[a].radius * 2);

                            mdl.colliders.Add(c);
                            showcolliders = true;
                            MessageBox.Show("Коллайдер успешно создан.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    cc_b = true;
                }

            }
            else cc_b = false;
        }

        private void Change_Object_Place(int index,string method,bool opov)
        {
            if (mdl.objects.Count <= 1) return;
            try
            {
                if (opov)
                {
                    DialogResult r = MessageBox.Show("Вы точно хотите изменить местоположение данного компонента?", "Оповещение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.No) return;
                }
                if (method == "upper")
                {

                    ModelObject s = mdl.objects[index];
                    mdl.objects.RemoveAt(index);
                    mdl.objects.Add(s);

                }
                else if (method == "down")
                {

                    ModelObject first = mdl.objects[0];
                    mdl.objects[0] = mdl.objects[index];
                    for (int a = 1; a < mdl.objects.Count; a++)
                    {
                        ModelObject second = mdl.objects[a];
                        mdl.objects[a] = first;
                        first = second;
                    }
                }
            }
            catch { Error("Here is a problem with model editor."); }
        }

        private void CheckKeys()
        {
            if (Focus)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    mdl.position.x += speed * EditorTime.ElapsedTime.AsSeconds();
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    mdl.position.x -= speed * EditorTime.ElapsedTime.AsSeconds();
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                    mdl.position.y += speed * EditorTime.ElapsedTime.AsSeconds();
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.S) && !Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                {
                    mdl.position.y -= speed * EditorTime.ElapsedTime.AsSeconds();
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) speed = speedValue * speedMultiply;
                else speed = speedValue;

                if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                {
                    if (open_pressed == false)
                    {
                        open_pressed = true;
                        Open();
                    }
                }
                else open_pressed = false;
                if (Keyboard.IsKeyPressed(Keyboard.Key.S) && Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                {
                    if (save_pressed == false)
                    {
                        save_pressed = true;
                        Save();
                    }
                }
                else save_pressed = false;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Tab))
                {
                    mdl.position.x = 0;
                    mdl.position.y = 0;
                }
                CheckKeysToScale();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
                {
                    if (path != "none")
                    {
                        DialogResult result = MessageBox.Show("Вы точно хотите загрузить модель с последнего сохранения?", "Уточнение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes) mdl = Saver.OpenModel(path);
                        return;
                    }
                    MessageBox.Show("Ошибка пути!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.C))
                {
                    Add();
                    return;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.N))
                {
                    if (showcolliders)
                    {
                        for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                        {
                            if (mdl.colliders[a].tag == "moving")
                            {
                                Info(a, "collider");
                                return;
                            }
                        }
                    }
                    for (int a = mdl.objects.Count - 1; a >= 0; a--)
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            Info(a, "object");
                            return;
                        }
                    }
                    return;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.V))
                {
                    if (!showc_pressed)
                    {
                        if (showcolliders) showcolliders = false;
                        else showcolliders = true;
                        showc_pressed = true;
                        return;
                    }
                }
                else showc_pressed = false;

                if (Keyboard.IsKeyPressed(Keyboard.Key.B))
                {
                    if (!sh_m_b)
                    {
                        if (showbuttons) showbuttons = false;
                        else showbuttons = true;
                        sh_m_b = true;
                    }

                }
                else sh_m_b = false;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Delete))
                {
                    if (!delete_b)
                    {
                        for (int a = mdl.objects.Count - 1; a >= 0; a--)
                        {
                            if (mdl.objects[a].tag == "moving")
                            {
                                DialogResult r = MessageBox.Show("Вы точно хотите это удалить?", "Уточнение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (r == DialogResult.Yes) mdl.objects.RemoveAt(a);
                                return;
                            }
                        }
                        if (showcolliders)
                        {
                            for (int a = mdl.colliders.Count - 1; a >= 0; a--)
                            {
                                if (mdl.colliders[a].tag == "moving")
                                {
                                    DialogResult r = MessageBox.Show("Вы точно хотите это удалить?", "Уточнение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (r == DialogResult.Yes) mdl.colliders.RemoveAt(a);
                                    return;
                                }
                            }
                        }

                        delete_b = true;
                    }


                }
                else delete_b = false;

                if (Keyboard.IsKeyPressed(Keyboard.Key.G))
                {
                    if(!savemode_b)
                    {
                        if (SaveMode == true) SaveMode = false;
                        else SaveMode = true;
                        savemode_b = true;
                    }
                }
                else savemode_b = false;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Y))
                {
                    if (!color_b)
                    {
                        for (int a = mdl.objects.Count - 1; a >= 0; a--)
                        {
                            if (mdl.objects[a].tag == "moving")
                            {
                                RgbColor(a);
                                return;
                            }
                        }

                        color_b = true;
                    }
                }
                else color_b = false;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Tilde))
                {
                    if (!delete_all_b)
                    {
                        DialogResult r = MessageBox.Show("Вы точно хотите удалить ВСЁ?", "Оповещение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (r != DialogResult.Yes) return;
                        for (int a = mdl.objects.Count - 1; a >= 0; a--) mdl.objects.RemoveAt(a);
                        for (int a = mdl.colliders.Count - 1; a >= 0; a--) mdl.colliders.RemoveAt(a);
                        delete_all_b = true;
                    }
                }
                else delete_all_b = false;

                BracketsCheck();
                CheckKeysToAdd();
                CheckKeyToCreateCollider();
            }
        }

        private void BracketsCheck()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LBracket))
            {
                if (SaveMode)
                {
                    for (int a = 0; a < mdl.objects.Count; a++)
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            Change_Object_Place(a, "down",true);
                            return;
                        }
                    }
                }
                else
                {
                    for (int a = mdl.objects.Count - 1; a >= 0; a--)
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            Change_Object_Place(a,  "down",true);
                            return;
                        }
                    }
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.RBracket))
            {
                if (SaveMode)
                {
                    for (int a = 0; a < mdl.objects.Count; a++)
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            Change_Object_Place(a, "down",true);
                            return;
                        }
                    }
                }
                else
                {
                    for (int a = mdl.objects.Count - 1; a >= 0; a--)
                    {
                        if (mdl.objects[a].tag == "moving")
                        {
                            Change_Object_Place(a, "upper",true);
                            return;
                        }
                    }
                }
            }
        }

        private void Start()
        {
            win.SetFramerateLimit(60);
            win.SetVerticalSyncEnabled(true);
            win.Closed += OnClose;
            win.GainedFocus += FocusOn;
            win.LostFocus += FocusOff;
            Run();
        }

        private void Settings()
        {
            reqest req = new reqest();
            req.showcolliders = showcolliders;
            req.bg = bg;
            req.SaveMode = SaveMode;
            req.speedMultiply = speedMultiply;
            settings s = new settings(req);
            s.ShowDialog();
            bg = s.req.bg;
            showcolliders = s.req.showcolliders;
            SaveMode = s.req.SaveMode;
            speedMultiply = s.req.speedMultiply;
        }

        private void Info(int index,string how)
        {
            if(how == "object")
            {
                ObjectInfoForm f = new ObjectInfoForm(mdl.objects[index],index);
                f.ShowDialog();
                mdl.objects[index] = f.obj;
                if(mdl.objects[index].tag == "delete")
                {
                    mdl.objects.RemoveAt(index);
                    return;
                }
                else if(mdl.objects[index].tag == "upper" || mdl.objects[index].tag == "down") Change_Object_Place(index, mdl.objects[index].tag,false);

            }
            else if(how == "collider")
            {
                ColliderInfoForm f = new ColliderInfoForm(mdl.colliders[index],index);
                f.ShowDialog();
                mdl.colliders[index] = f.cl;
                if(mdl.colliders[index].tag == "delete")
                {
                    mdl.colliders.RemoveAt(index);
                    return;
                }


            }



        }

        private void Add()
        {
            add a = new add();
            a.ShowDialog();
            if (a.type == "") return;
            if (a.type == "normal")
            {
                a.ob.color = Color.Black;
                mdl.objects.Add(a.ob);
            }
            else if (a.type == "collider") mdl.colliders.Add(a.c);
            else Error("Invalid adding!");

        }

        private void CheckMouse()
        {
            RightMouseCheck();
            
            Vector2i mousePos = Mouse.GetPosition(win);
            
            float x_m = mousePos.X;
            float y_m = mousePos.Y;
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                
                if (left_pressed == false)
                {
                    if (showbuttons)
                    {
                        RectangleShape mouse_collider = new RectangleShape();
                        mouse_collider.Size = new Vector2f(1, 1);
                        mouse_collider.Position = new Vector2f(x_m, y_m);
                        for (int a = 0; a < buttons.Count; a++)
                        {
                            if (buttons[a].shape.GetGlobalBounds().Intersects(mouse_collider.GetGlobalBounds()))
                            {
                                MouseFunction(buttons[a].function);
                                return;
                            }


                        }
                    }
                    left_pressed = true;
                }
                MovingCheck();
            }
            else
            {
                left_pressed = false;
                for(int a = mdl.objects.Count - 1; a >= 0; a--)
                {
                    if(mdl.objects[a].tag == "moving")
                    mdl.objects[a].tag = "";
                }
                for(int a = mdl.colliders.Count - 1; a >= 0; a--)
                {
                    if(mdl.colliders[a].tag == "moving")
                    mdl.colliders[a].tag = "";
                }
                diffX = 0f;
                diffY = 0f;

            }
        }

        private void Run()
        {
            //panel//
            RectangleShape panel = new RectangleShape();
            panel.Position = new Vector2f(0,0);
            panel.Size = new Vector2f(700, 65);
            panel.FillColor = new Color(107,85,85);
            //panel//
            //savebutton//
            button save_button = new button();
            save_button.Setup(font, "Сохранить");
            save_button.shape.Size = new Vector2f(130,45);
            save_button.shape.Position = new Vector2f(10, 10);
            save_button.t.Scale = new Vector2f(0.8f,0.8f);
            save_button.t.Position = new Vector2f(13.5f,15.5f);
            save_button.t.Color = Color.Black;
            save_button.shape.FillColor = new Color(64,148,196);
            save_button.function = "save";
            buttons.Add(save_button);
            //savebutton//
            //openbutton//
            button open_button = new button();
            open_button.Setup(font, "Открыть");
            open_button.shape.Size = new Vector2f(130, 45);
            open_button.shape.Position = new Vector2f(145, 10);
            open_button.t.Scale = new Vector2f(0.8f, 0.8f);
            open_button.t.Position = new Vector2f(158.5f, 15.5f);
            open_button.t.Color = Color.Black;
            open_button.shape.FillColor = new Color(64, 148, 196);
            open_button.function = "open";
            buttons.Add(open_button);
            //openbutton//
            //settingsbutton//
            button settings_button = new button();
            settings_button.Setup(font, "Настройки");
            settings_button.shape.Size = new Vector2f(130, 45);
            settings_button.shape.Position = new Vector2f(280, 10);
            settings_button.t.Scale = new Vector2f(0.8f, 0.8f);
            settings_button.t.Position = new Vector2f(284.5f, 15.5f);
            settings_button.t.Color = Color.Black;
            settings_button.shape.FillColor = new Color(64, 148, 196);
            settings_button.function = "settings";
            buttons.Add(settings_button);
            //settingsbutton//
            //addbutton//
            button add_button = new button();
            add_button.Setup(font, "Создать");
            add_button.shape.Size = new Vector2f(130, 45);
            add_button.shape.Position = new Vector2f(415, 10);
            add_button.t.Scale = new Vector2f(0.8f, 0.8f);
            add_button.t.Position = new Vector2f(431.5f, 15.5f);
            add_button.t.Color = Color.Black;
            add_button.shape.FillColor = new Color(64, 148, 196);
            add_button.function = "add";
            buttons.Add(add_button);
            //addbutton//

            while (win.IsOpen)
            {
                win.DispatchEvents();
                CheckMouse();
                CheckKeys();
                win.Clear(bg);
                Renderer.RenderModel(mdl, win);
                EditorTime.Restart();
                if (showcolliders) RenderColliders();
                if (showbuttons)
                {
                    win.Draw(panel);
                    for (int a = 0; a < buttons.Count; a++)
                    {
                        win.Draw(buttons[a].shape);
                        win.Draw(buttons[a].t);
                    }
                }

                win.Display();
            }
        }

        private void FocusOn(object sender, EventArgs e)
        {
            Focus = true;
        }
        private void FocusOff(object sender, EventArgs e)
        {
            Focus = false;
        }

        private void OnClose(object sender, EventArgs e)
        {
            win.Close();
        }



    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Threading;
using System.Windows.Forms;
using SFML.Audio;

namespace Engine
{
    class Animation
    {
        private int step = 0;
        private string[] frames;
        private Clock clock = new Clock();
        private ModelObject target;
        private float delay = 0f;

        public Animation(ModelObject target, string[] frames, float delay)
        {
            this.frames = frames;
            this.target = target;
            this.delay = delay;
            nextFrame();
        }

        private void nextFrame()
        {
            target.texture = frames[step];
            step = step == frames.Length - 1 ? 0 : step + 1;
        }

        public void update()
        {
            if (clock.ElapsedTime.AsSeconds() < delay) return;
            clock.Restart();
            nextFrame();
        }
    }

    class Game
    {
        private Model player = Saver.OpenModel("car");
        private Model map = Saver.OpenModel("road");
        private float speed = 2f;


        private ModelObject playerSheet;
        private Animation walkXAnim;
        private Animation walkYAnim;
        private Animation currentAnim = null;

        public void Setup()
        {
            Engine.SetupWindow(800,500);
            Engine.SetTitle("Супер игра");
            Engine.SecondsToShowLogo = 0.1f;
            Engine.AddScene(Start, Update, "scene");
            Engine.SetFirstScene("scene");
            Engine.Start();
        }

        private void CheckCollisions(string movementType)
        {
            if (Collision.Collide(player, map))
                player.Move(-speed, movementType);
        }

        private void Movement()
        {
            float velocityX = 0f;
            float velocityY = 0f;
            if(Engine.IsKeyPressed(Keyboard.Key.A))
            {
                velocityX -= speed;
                CheckCollisions("left");
            }
            if (Engine.IsKeyPressed(Keyboard.Key.D))
            {
                velocityX -= speed;
                CheckCollisions("right");
            }
            if (Engine.IsKeyPressed(Keyboard.Key.W))
            {
                velocityY -= speed;
                CheckCollisions("up");
            }
            if (Engine.IsKeyPressed(Keyboard.Key.S))
            {
                velocityY += speed;
                CheckCollisions("down");
            }

            player.Move(velocityX,"right");
            player.Move(velocityY, "down");


        }

        private void Start()
        {
            Engine.bg = new Color(0,191,255);

            walkXAnim = new Animation(null, null, 2f);
            walkYAnim = new Animation(null, null, 2f);

            Engine.AddModelToRender(player);
            Engine.AddModelToRender(map);
        }
        private void Update()
        {
            Movement();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            Game g = new Game();
            g.Setup();
        }
    }

}

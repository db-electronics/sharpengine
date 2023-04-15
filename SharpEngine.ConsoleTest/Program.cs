
using Microsoft.VisualBasic;
using SharpEngine;
using SharpEngine.Drawing;
using SharpEngine.Drawing.Interfaces;
using SharpEngine.Math;
using System.Collections.Concurrent;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SharpEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestEngine game = new();
        }
    }

    class TestEngine : SharpEngine
    {

        ITextElement infoText;

        List<IShapeElement> smileyFace = new();
        ConcurrentBag<IShapeElement> rectangles = new();

        IImageElement mario;
        List<IImageElement> images = new();

        Random rand = new Random();

        float angle = 0f;

        public TestEngine()
            :base(800, 600, 60f, "Test Game")
        {
            
        }

        public override void OnCreate()
        {
            Console.WriteLine("on load");
            BackgroundColour = Color.Black;

            infoText = ElementFactory.CreateTextElement("infoText", "", new Vector2f(10, 10), new Vector2f(15), Color.White);
            infoText.Register();

            var headSize = new Vector2f(200);
            var headPosition = new Vector2f(ScreenSize.X / 2, ScreenSize.Y / 2);

            var headEllipse = ElementFactory.CreateEllipseElement("head", headPosition, headSize, Color.BlueViolet, 3);
            smileyFace.Add(headEllipse);

            var eyeOffset = 40f;
            var eyeSize = new Vector2f(50);
            var eyePositionLeft = headEllipse.Center + new Vector2f(-eyeOffset, -eyeOffset);
            var eyePositionRight = headEllipse.Center + new Vector2f(eyeOffset, -eyeOffset);

            var eyeColour = Color.White;
            smileyFace.Add(ElementFactory.CreateEllipseElement("eyeLeft", eyePositionLeft, eyeSize, eyeColour, 2));
            smileyFace.Add(ElementFactory.CreateEllipseElement("eyeRight", eyePositionRight, eyeSize, eyeColour, 2));

            var pupilColour = Color.Black;
            var pupilSize = eyeSize / 2;
            var pupilPositionLeft = eyePositionLeft;
            var pupilPositionRight = eyePositionRight;
            smileyFace.Add(ElementFactory.CreateEllipseElement("pupilLeft", pupilPositionLeft, pupilSize, pupilColour, 1));
            smileyFace.Add(ElementFactory.CreateEllipseElement("pupilRight", pupilPositionRight, pupilSize, pupilColour, 1));

            smileyFace.ForEach(s => s.Register());

            mario = ElementFactory.CreateImageFromFile("mario", "Assets/mario.png", new Vector2f(100, 100), new Vector2f(86, 110), 1);
            mario.Register();
        }

        public override void OnUpdate(float elapsedTime, int frame)
        {
            //Console.WriteLine($"frame: {frame} - time {elapsedTime}");

            //frameTimeText.Value = $"frame: {frame} - time {elapsedTime}";

            if (frame % 5 == 0)
            {
                // spawn marios
                var newImg = ElementFactory.CreateImageFromFile(
                    Guid.NewGuid().ToString(),
                    "Assets/mario.png",
                    new Vector2f(rand.Next((int)ScreenSize.X), rand.Next((int)ScreenSize.Y)),
                    new Vector2f(86, 110),
                    1);
                newImg.Size *= (float)rand.NextDouble();
                newImg.Register();
                images.Add(newImg);

                infoText.Value = $"mario count: {images.Count}";
            }

            if(frame % 1 == 0)
            {
                int motion = 2;
                foreach (var img in images)
                {
                    var newCenter = new Vector2f(rand.Next(-motion, motion+1), rand.Next(-motion, motion+1));
                    img.Center += newCenter;
                }

                var newSmileyCenter = new Vector2f(rand.Next(-motion, motion + 1), rand.Next(-motion, motion + 1));
                foreach(var item in smileyFace)
                {
                    item.Center += newSmileyCenter;
                }
            }


            if (KeysDown.TryRemove(System.Windows.Forms.Keys.R, out var _))
            {
                mario.RotateFlip(90);
            }

            if (KeysDown.TryRemove(System.Windows.Forms.Keys.E, out var _))
            {
                mario.RotateFlip(-90);
            }

            if (KeysDown.TryRemove(System.Windows.Forms.Keys.X, out var _))
            {
                mario.RotateFlip(0, true);
            }

            if (KeysDown.TryRemove(System.Windows.Forms.Keys.Right, out var _))
            {
                mario.Center += new Vector2f(3, 0);
            }

            if (KeysDown.TryRemove(System.Windows.Forms.Keys.Left, out var _))
            {
                mario.Center += new Vector2f(-3, 0);
            }

            if (KeysDown.TryRemove(System.Windows.Forms.Keys.Up, out var _))
            {
                mario.Center += new Vector2f(0, -3);
            }

            if (KeysDown.TryRemove(System.Windows.Forms.Keys.Down, out var _))
            {
                mario.Center += new Vector2f(0, 3);
            }

            //angle += 0.5f;
            //if(angle >= 360f)
            //{
            //    angle = 0;
            //}
            //mario.RotateFlip(angle);

            //if(frame % 1 == 0)
            //{

            //    var tag = Guid.NewGuid().ToString();
            //    var colour = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));

            //    //rectangles.Add(new RectangleElement(
            //    //    tag,
            //    //    new Vector2f(rand.Next((int)ScreenSize.X), rand.Next((int)ScreenSize.Y)),
            //    //    new Vector2f(rand.Next(20, 200), rand.Next(20, 200)),
            //    //    colour,
            //    //    10));

            //    var rectangle = ElementFactory.CreateRectangle(
            //        tag,
            //        new Vector2f(rand.Next((int)ScreenSize.X), rand.Next((int)ScreenSize.Y)),
            //        new Vector2f(rand.Next(20, 200), rand.Next(20, 200)),
            //        colour,
            //        10);

            //    rectangle.Register();
            //    rectangles.Add(rectangle);

            //    int motion = 5;
            //    int unit = 2;
            //    foreach(var rect in rectangles)
            //    {
            //        //rect.Center += new Vector2f(rand.Next(-motion, motion), rand.Next(-motion, motion));
            //        rect.Size *= new Vector2f((float)rand.NextDouble() * rand.Next(-unit, unit) + 1f, (float)rand.NextDouble() * rand.Next(-unit, unit) + 1f);
            //    }

            //    infoText.Value = $"rectangle count: {rectangles.Count}";
            //}

            //if(frame % 300 == 0)
            //{
            //    for(int i = 0; i < rectangles.Count; i += 2)
            //    {
            //        rectangles.TryTake(out var item);
            //        item?.Dispose();
            //    }
            //}
        }

        public override void OnDestroy()
        {
            Console.WriteLine("game has been destroyed");
        }
    }
}
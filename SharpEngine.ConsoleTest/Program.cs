﻿
using SharpEngine;
using SharpEngine.Drawing;
using SharpEngine.Drawing.Interfaces;
using SharpEngine.Math;
using System.Collections.Concurrent;
using System.Drawing;
using System.Numerics;

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

        ISpriteElement mario;

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

            infoText = new TextElement("infoText", "", new Vector2f(10, 10), new Vector2f(15), Color.White);
            //frameTimeText = new TextElement("frame", "frame time: ", new Vector2f(10, 30), Color.CadetBlue);

            var headSize = new Vector2f(200);
            var headPosition = new Vector2f(ScreenSize.X / 2, ScreenSize.Y / 2);

            var headEllipse = new EllipseElement("head", headPosition, headSize, Color.BlueViolet, 3);
            smileyFace.Add(headEllipse);

            var eyeOffset = 40f;
            var eyeSize = new Vector2f(50);
            var eyePositionLeft = headEllipse.Center + new Vector2f(-eyeOffset, -eyeOffset);
            var eyePositionRight = headEllipse.Center + new Vector2f(eyeOffset, -eyeOffset);

            var eyeColour = Color.White;
            smileyFace.Add(new EllipseElement("eyeLeft", eyePositionLeft, eyeSize, eyeColour, 2));
            smileyFace.Add(new EllipseElement("eyeRight", eyePositionRight, eyeSize, eyeColour, 2));

            var pupilColour = Color.Black;
            var pupilSize = eyeSize / 2;
            var pupilPositionLeft = eyePositionLeft;
            var pupilPositionRight = eyePositionRight;
            smileyFace.Add(new EllipseElement("pupilLeft", pupilPositionLeft, pupilSize, pupilColour, 1));
            smileyFace.Add(new EllipseElement("pupilRight", pupilPositionRight, pupilSize, pupilColour, 1));

            mario = ElementFactory.CreateSpriteFromFile("mario", "Assets/mario.png", new Vector2f(100, 100), 1);
            mario.Size *= 0.1f;
            mario.Register();
        }

        public override void OnUpdate(float elapsedTime, int frame)
        {
            //Console.WriteLine($"frame: {frame} - time {elapsedTime}");

            //frameTimeText.Value = $"frame: {frame} - time {elapsedTime}";

            if (frame % 20 == 0)
            {
                mario.RotateFlip(90);
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
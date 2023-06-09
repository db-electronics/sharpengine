﻿
using SharpEngine.Math;
using SharpEngine.Drawing.Interfaces;
using System.Collections.Concurrent;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing.Text;

namespace SharpEngine
{
    public abstract class SharpEngine
    {
        public Vector2f ScreenSize { get; set; }

        private readonly string _title = "Sharp Engine";
        private readonly Canvas _window = new();

        private readonly float _refreshFrequency;
        private readonly CancellationTokenSource _cts = new();
        private readonly System.Timers.Timer _timer = new();

        private readonly Thread _gameLoop;

        /// <summary>
        /// 
        /// </summary>
        public abstract void OnCreate();
        public abstract void OnUpdate(float elapsedTime, int frameCount);
        public abstract void OnDestroy();

        private readonly object _frameLock = new ();

        /// <summary>
        /// 
        /// </summary>
        public Color BackgroundColour { get; set; } = Color.Black;
        private static ConcurrentDictionary<string, ITextElement> TextElements { get; set; } = new();
        private static ConcurrentDictionary<string, IShapeElement> ShapeElements { get; set; } = new();
        private static ConcurrentDictionary<string, IImageElement> ImageElements { get; set; } = new();
        private static ConcurrentDictionary<string, ImageData> Images { get; set; } = new();

        class ImageData
        {
            public Bitmap Bitmap { get; set; }
            public float Angle { get; set; }
            public bool FlipX { get; set; }
            public bool FlipY { get; set; }

            public ImageData(Bitmap bitmap)
            {
                Bitmap = bitmap;
                Angle = 0f;
                FlipX = false;
                FlipY = false;
            }
        }

        public ConcurrentDictionary<Keys, int> KeysDown { get; set; } = new();

        public SharpEngine(int screenX, int screenY, float refreshFrequency, string title)
        {
            ScreenSize = new Vector2f(screenX, screenY);

            _title = title;
            _refreshFrequency = refreshFrequency;

            // set the refresh frequency
            _timer.Interval = (1 / _refreshFrequency) * 1000;

            _window.Size = new Size(screenX, screenY);
            _window.Text = _title;
            
            // setup callbacks
            _window.Paint += Renderer;
            _window.FormClosing += OnFormClosing;

            _window.KeyDown += OnKeyDown;
            _window.KeyUp += OnKeyUp;

            _gameLoop = new Thread(() => GameLoop(_cts.Token));
            _gameLoop.Start();

            Application.Run(_window);
        }

        private void OnKeyDown(object? sender, KeyEventArgs e) 
        {
            KeysDown.TryAdd(e.KeyCode, e.KeyValue);
        }

        private void OnKeyUp(object? sender, KeyEventArgs e) 
        { 
            KeysDown.TryRemove(e.KeyCode, out var _);
        }

        private void GameLoop(CancellationToken ct)
        {
            float elapsedTime = 0f;
            ConcurrentQueue<float> frameRateQueue = new();

            int frameCount = 0;
            DateTime previousSignalTime = DateTime.Now;
            TimeSpan elapsedTs = TimeSpan.Zero;

            _timer.Elapsed += (s, e) =>
            {
                elapsedTs = e.SignalTime - previousSignalTime;
                elapsedTime = (float)elapsedTs.Ticks / TimeSpan.TicksPerSecond;
                frameRateQueue.Enqueue(1 / elapsedTime);

                _window.BeginInvoke((MethodInvoker)delegate 
                { 
                    _window.Refresh();
                    if(frameCount % _refreshFrequency == 0)
                    {
                        var frameRate = frameRateQueue.Sum() / frameRateQueue.Count;
                        _window.Text = _title + $" {frameRate:0.00} fps";
                        frameRateQueue.Clear();
                        GC.Collect();
                    }
                     
                });
                
                previousSignalTime = e.SignalTime;
                frameCount = unchecked(frameCount + 1);
                OnUpdate(elapsedTime, frameCount);
            };

            OnCreate();

            _timer.Start();
            while (!ct.IsCancellationRequested)
            {
                // loopy mcloopface
            }
        }

        private void Renderer(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            g.Clear(BackgroundColour);

            foreach (var s in ShapeElements.Values.Where(s => s.IsVisible).OrderByDescending(s => s.Priority))
            {
                switch (s.Type)
                {
                    case ShapeType.Rectangle:
                        g.FillRectangle(s.Brush, s.Position.X, s.Position.Y, s.Size.X, s.Size.Y);
                        break;
                    case ShapeType.Ellipse:
                        g.FillEllipse(s.Brush, s.Position.X, s.Position.Y, s.Size.X, s.Size.Y);
                        break;
                    default:
                        break;
                }
                
            }

            foreach(var imgElement in ImageElements.Values.Where(t => t.IsVisible).OrderByDescending(t => t.Priority))
            {
                if(Images.TryGetValue(imgElement.Tag, out var imageData))
                {
                    if(imageData.Angle != imgElement.Angle || imageData.FlipX != imgElement.FlipX || imageData.FlipX != imgElement.FlipX)
                    {
                        RedrawImage(imageData, imgElement.Angle, imgElement.FlipX, imgElement.FlipY);
                    }
                    g.DrawImage(imageData.Bitmap, imgElement.Position.X, imgElement.Position.Y, imgElement.Size.X, imgElement.Size.Y);
                }
            }

            // draw all text elements
            foreach (var t in TextElements.Values.Where(t => t.IsVisible).OrderByDescending(t => t.Priority))
            {
                g.DrawString(t.Value, t.Font, t.Brush, t.Position.X, t.Position.Y);
            }
        }

        public static void RegisterElement<T>(T element, string path = "") where T : IDrawingElement
        {
            switch(typeof(T))
            {
                case Type t when t == typeof(ITextElement):
                    TextElements.TryAdd(element.Tag, (ITextElement)element);
                    break;
                case Type t when t == typeof(IShapeElement):
                    ShapeElements.TryAdd(element.Tag, (IShapeElement)element);
                    break;
                case Type t when t == typeof(IImageElement):
                    ImageElements.TryAdd(element.Tag, (IImageElement)element);
                    Image img = Image.FromFile(path);
                    Images.TryAdd(element.Tag, new ImageData(new Bitmap(img))) ;
                    break;
                default:
                    throw new NotImplementedException($"registering type '{typeof(T)}' is not implemented");
            };
        }

        public static void RegisterElements<T>(IEnumerable<T> elements) where T : IDrawingElement
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(ITextElement):
                    foreach (var element in elements)
                    {
                        TextElements.TryAdd(element.Tag, (ITextElement)element);
                    }
                    break;
                case Type t when t == typeof(IShapeElement):
                    foreach (var element in elements)
                    {
                        ShapeElements.TryAdd(element.Tag, (IShapeElement)element);
                    }
                    break;
                case Type t when t == typeof(IImageElement):
                    foreach (var element in elements)
                    {
                        ImageElements.TryAdd(element.Tag, (IImageElement)element);
                    }
                    break;
                default:
                    throw new NotImplementedException($"registering type '{typeof(T)}' is not implemented");
            };
        }

        public static void DisposeElement<T>(T element) where T : IDrawingElement
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(ITextElement):
                    TextElements.Remove(element.Tag, out var _);
                    break;
                case Type t when t == typeof(IShapeElement):
                    ShapeElements.Remove(element.Tag, out var _);
                    break;
                case Type t when t == typeof(IImageElement):
                    ImageElements.Remove(element.Tag, out var _);
                    break;
                default:
                    throw new NotImplementedException($"Destroying type '{typeof(T)}' is not implemented");
            };
        }

        private void OnFormClosing(object? sender, FormClosingEventArgs e)
        {
            _timer.Stop();
            _cts.Cancel();
            OnDestroy();
        }

        private static void RedrawImage(ImageData imageData, float newAngle, bool flipX, bool flipY)
        {
            float rotateBy = newAngle - imageData.Angle;
            bool mirror = imageData.FlipX ^ flipX;
            if (rotateBy > 360)
                rotateBy -= 360;

            // store new state
            imageData.Angle = newAngle;
            imageData.FlipX = flipX;

            switch (rotateBy)
            {
                case 360f:
                case 0f:
                    if (mirror)
                        imageData.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    else
                        imageData.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
                case -270f:
                case 90f:
                    if (mirror)
                        imageData.Bitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                    else
                        imageData.Bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case -180f:
                case 180f:
                    if (mirror)
                        imageData.Bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                    else
                        imageData.Bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case -90f:
                case 270f:
                    if (mirror)
                        imageData.Bitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
                    else
                        imageData.Bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    if (mirror)
                        imageData.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    //_image = RotateAnyAngle(angle);
                    break;
            }
        }
    }
}
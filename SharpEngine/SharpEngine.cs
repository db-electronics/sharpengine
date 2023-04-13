
using SharpEngine.Math;
using SharpEngine.Drawing.Interfaces;
using System.Collections.Concurrent;
using System.Xml.Linq;
using System.Collections.Generic;

namespace SharpEngine
{
    public abstract class SharpEngine
    {
        public Vector2f ScreenSize { get; set; }

        private readonly string _title = "Sharp Engine";
        private readonly Canvas _window = new ();

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

        /// <summary>
        /// 
        /// </summary>
        public Color BackgroundColour { get; set; } = Color.Black;
        private static ConcurrentDictionary<string, ITextElement> TextElements { get; set; } = new();
        private static ConcurrentDictionary<string, IShapeElement> ShapeElements { get; set; } = new();
        private static ConcurrentDictionary<string, ISpriteElement> SpriteElements { get; set; } = new();

        public SharpEngine(int screenX, int screenY, float refreshFrequency, string title)
        {
            ScreenSize = new Vector2f(screenX, screenY);

            _title = title;
            _refreshFrequency = refreshFrequency;

            // set the refresh frequency
            _timer.Interval = (1 / _refreshFrequency) * 1000;

            _window.Size = new Size((int)ScreenSize.X, (int)ScreenSize.Y);
            _window.Text = _title;
            
            // setup callbacks
            _window.Paint += Renderer;
            _window.FormClosing += OnFormClosing;

            _gameLoop = new Thread(() => GameLoop(_cts.Token));
            _gameLoop.Start();

            Application.Run(_window);
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

            foreach(var s in SpriteElements.Values.Where(t => t.IsVisible).OrderByDescending(t => t.Priority))
            {
                g.DrawImage(s.Bitmap, s.Position.X, s.Position.Y, s.Size.X, s.Size.Y);
            }

            // draw all text elements
            foreach (var t in TextElements.Values.Where(t => t.IsVisible).OrderByDescending(t => t.Priority))
            {
                g.DrawString(t.Value, t.Font, t.Brush, t.Position.X, t.Position.Y);
            }
        }

        public static void RegisterElement<T>(T element) where T : IDrawingElement
        {
            switch(typeof(T))
            {
                case Type t when t == typeof(ITextElement):
                    TextElements.TryAdd(element.Tag, (ITextElement)element);
                    break;
                case Type t when t == typeof(IShapeElement):
                    ShapeElements.TryAdd(element.Tag, (IShapeElement)element);
                    break;
                case Type t when t == typeof(ISpriteElement):
                    SpriteElements.TryAdd(element.Tag, (ISpriteElement)element);
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
                case Type t when t == typeof(ISpriteElement):
                    foreach (var element in elements)
                    {
                        SpriteElements.TryAdd(element.Tag, (ISpriteElement)element);
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
                case Type t when t == typeof(ISpriteElement):
                    SpriteElements.Remove(element.Tag, out var _);
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
    }
}
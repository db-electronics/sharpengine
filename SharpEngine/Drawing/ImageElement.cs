using SharpEngine.Drawing.Interfaces;
using SharpEngine.Math;

namespace SharpEngine.Drawing
{
    public class ImageElement : IImageElement
    {
        public string Tag { get; set; }
        public bool IsVisible { get; set; } = true;
        public int Priority { get; set; }
        public string Path { get; set; }
        public float Angle { get; set; }
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }

        public Vector2f Position { get; set; }

        private Vector2f _center;
        public Vector2f Center
        {
            get { return _center; }
            set { _center = value; Position = value - (_size / 2); }
        }

        private Vector2f _size;
        public Vector2f Size
        {
            get { return _size; }
            set { _size = value; Position = _center - (_size / 2); }
        }

        public ImageElement(string tag, string path, Vector2f center, Vector2f size, int priority)
        {

            if(!File.Exists(path))
                throw new FileNotFoundException(path);

            Tag = tag;
            Path = path;
            Angle = 0f;
            FlipX = false;
            FlipY = false;

            Priority = priority;
            _size = size;
            _center = center;
            Position = center - (_size / 2);
            

            //Bitmap = new Bitmap(_image);
            //Bitmap.MakeTransparent(Color.Transparent);
        }

        public void SwapXY()
        {
            _size.SwapXY();
            // recalculate the center coordinates
            Center = _center;

        }

        public void SetAngle(float angle, bool mirror = false)
        {
            if (angle > 360)
                angle -= 360;

            float diffAngle = System.Math.Abs(angle - Angle);
            
            Angle = angle;
            FlipX = mirror;

            if(diffAngle == 90f || diffAngle == 270)
            {
                SwapXY();
            }
        }

        //private Image RotateAnyAngle(float angle)
        //{
        //    _semaphore.WaitOne();
        //    using (Graphics g = Graphics.FromImage(_original))
        //    {
        //        g.TranslateTransform(_original.Width / 2, _original.Width / 2);
        //        g.RotateTransform(angle);
        //        g.DrawImage(_original, new Point(-_original.Width / 2, -_original.Height / 2));
        //    }
        //    _semaphore.Release();
        //    return _original;
        //}

        public void Register()
        {
            SharpEngine.RegisterElement<IImageElement>(this, this.Path);
        }

        public void Dispose()
        {
            SharpEngine.DisposeElement<IImageElement>(this);
        }
    }
}

using SharpEngine.Drawing.Interfaces;
using SharpEngine.Math;
using System.Drawing;
using System.IO;

namespace SharpEngine.Drawing
{
    public class SpriteElement : ISpriteElement
    {
        public string Tag { get; set; }
        public bool IsVisible { get; set; } = true;
        public int Priority { get; set; }
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
        public Bitmap Bitmap { get; set; }

        private Image _image;
        //private Image _original;
        //private Semaphore _semaphore = new(1,1);

        public SpriteElement(string tag, string path, Vector2f center, int priority)
        {
            _image = Image.FromFile(path);
            Priority = priority;
            _size = new Vector2f(_image.Width, _image.Height);
            _center = center;
            Position = center - (_size / 2);
            Tag = tag;
            
            Bitmap = new Bitmap(_image);
            //Bitmap.MakeTransparent(Color.Transparent);
        }

        public void RotateFlip(float angle, bool mirror = false)
        {
            switch (angle)
            {
                case 0f:
                    if (mirror)
                        _image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    else
                        _image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
                case 90f:
                    if (mirror)
                        _image.RotateFlip(RotateFlipType.Rotate90FlipX);
                    else
                        _image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    SwapXY();
                    break;
                case 180f:
                    if (mirror)
                        _image.RotateFlip(RotateFlipType.Rotate180FlipX);
                    else
                        _image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 270f:
                    if (mirror)
                        _image.RotateFlip(RotateFlipType.Rotate270FlipX);
                    else
                        _image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    SwapXY();
                    break;
                default:
                    if (mirror)
                        _image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    //_image = RotateAnyAngle(angle);
                    break;
            }
            Bitmap = new Bitmap(_image);
            //Bitmap.MakeTransparent(Color.Transparent);
        }

        private void SwapXY()
        {
            (_size.X, _size.Y) = (_size.Y, _size.X);
            Center = _center;

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
            SharpEngine.RegisterElement<ISpriteElement>(this);
        }

        public void Dispose()
        {
            SharpEngine.DisposeElement<ISpriteElement>(this);
        }
    }
}

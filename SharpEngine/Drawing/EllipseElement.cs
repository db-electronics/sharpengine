using SharpEngine.Drawing.Interfaces;
using SharpEngine.Math;

namespace SharpEngine.Drawing
{
    public class EllipseElement : IShapeElement
    {
        public string Tag { get; set; }
        public bool IsVisible { get; set; } = true;
        public ShapeType Type { get; }
        public Vector2f Position { get; set; }

        private Vector2f _center;
        public Vector2f Center
        {
            get { return _center; }
            set { _center = value; Position = value - (Size / 2); }
        }

        private Vector2f _size;
        public Vector2f Size
        {
            get { return _size; }
            set { _size = value; Position = _center - (Size / 2); }
        }

        public int Priority { get; set; }
        public Brush Brush { get; private set; }

        private Color _colour;
        public Color Colour
        {
            get { return _colour; }
            set { _colour = value; Brush = new SolidBrush(value); }
        }

        public EllipseElement(string tag, Vector2f center, Vector2f size, Color colour, int priority = 0)
        {
            Tag = tag;
            Type = ShapeType.Ellipse;
            _center = center;
            Position = center - (size / 2);
            _size = size;
            Colour = colour;
            Brush = new SolidBrush(colour);
            Priority = priority;
        }

        public void Register()
        {
            SharpEngine.RegisterElement<IShapeElement>(this);
        }

        public void Dispose()
        {
            SharpEngine.DisposeElement<IShapeElement>(this);
        }
    }
}

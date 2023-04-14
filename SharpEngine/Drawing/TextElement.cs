using SharpEngine.Drawing.Interfaces;
using SharpEngine.Math;

namespace SharpEngine.Drawing
{
    public class TextElement : ITextElement
    {
        public string Tag { get; set; }
        public bool IsVisible { get; set; } = true;
        public Vector2f Position { get; set; }

        private Vector2f _size;
        public Vector2f Size
        {
            get { return _size; }
            set { _size = value; Font = new Font("Courier New", _size.X, FontStyle.Regular); }
        }

        public int Priority { get; set; }
        public string Value { get; set; }
        public Font Font { get; set; }

        private Color _colour;
        public Color Colour 
        {
            get { return _colour; } 
            set { _colour = value; Brush = new SolidBrush(value); } 
        }

        public Brush Brush { get; private set; }

        public TextElement(string tag, string value, Vector2f position, Vector2f size, Color colour, int priority = 0)
        {
            Tag = tag;
            Value = value;
            Position = position;
            Priority = priority;
            Font = new Font("Courier New", size.X, FontStyle.Regular);
            _colour = colour;
            Brush = new SolidBrush(colour);
            _size = size;
        }

        public void Register()
        {
            SharpEngine.RegisterElement<ITextElement>(this);
        }

        public void Dispose()
        {
            SharpEngine.DisposeElement<ITextElement>(this);
        }
    }
}

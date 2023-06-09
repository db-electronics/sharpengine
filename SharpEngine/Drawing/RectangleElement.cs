﻿using SharpEngine.Drawing.Interfaces;
using SharpEngine.Math;

namespace SharpEngine.Drawing
{
    public class RectangleElement : IShapeElement
    {
        public string Tag { get; set; }
        public bool IsVisible { get; set; } = true;
        public ShapeType Type { get; }
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

        public int Priority { get; set; }
        public Brush Brush { get; private set; }

        private Color _colour;
        public Color Colour
        {
            get { return _colour; }
            set { _colour = value; Brush = new SolidBrush(value); }
        }

        public RectangleElement(string tag, Vector2f center, Vector2f size, Color colour, int priority)
        {
            Tag = tag;
            Type = ShapeType.Rectangle;
            _center = center;
            Position = center - (size / 2);
            _size = size;
            Priority = priority;
            _colour = colour;
            Brush = new SolidBrush(colour);
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

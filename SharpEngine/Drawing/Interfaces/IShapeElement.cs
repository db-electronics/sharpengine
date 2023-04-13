using SharpEngine.Math;

namespace SharpEngine.Drawing.Interfaces
{
    public interface IShapeElement : IDrawingElement
    {
        ShapeType Type { get; }
        Vector2f Center { get; set; }
        Color Colour { get; set; }
        Brush Brush { get; }
    }

    public enum ShapeType { Rectangle, Ellipse }
}

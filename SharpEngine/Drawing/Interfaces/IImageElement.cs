

using SharpEngine.Math;

namespace SharpEngine.Drawing.Interfaces
{
    public interface IImageElement : IDrawingElement
    {
        string Path { get; set; }
        Vector2f Center { get; set; }
        float RotationAngle { get; set; }
        bool FlipX { get; set; }
        bool FlipY { get; set; }
        bool Redraw { get; set; }
    }
}

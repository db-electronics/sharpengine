

using SharpEngine.Math;

namespace SharpEngine.Drawing.Interfaces
{
    public interface IImageElement : IDrawingElement
    {
        string Path { get; set; }
        Vector2f Center { get; set; }
        float CurrentAngle { get; }
        float DifferenceAngle { get; }
        bool FlipX { get; set; }
        bool FlipY { get; set; }
        bool Redraw { get; set; }
        void RotateFlip(float absAngle, bool mirror = false);
        void SwapXY();
    }
}

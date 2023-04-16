

using SharpEngine.Math;

namespace SharpEngine.Drawing.Interfaces
{
    public interface IImageElement : IDrawingElement
    {
        string Path { get; set; }
        Vector2f Center { get; set; }
        float Angle { get; }
        bool FlipX { get; set; }
        bool FlipY { get; set; }
        void SetAngle(float angle, bool mirror = false);
        void SwapXY();
    }
}

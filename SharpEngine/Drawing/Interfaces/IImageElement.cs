

using SharpEngine.Math;

namespace SharpEngine.Drawing.Interfaces
{
    public interface IImageElement : IDrawingElement
    {
        Bitmap Bitmap { get; set; }
        Vector2f Center { get; set; }
        void RotateFlip(float angle, bool mirror = false);
    }
}

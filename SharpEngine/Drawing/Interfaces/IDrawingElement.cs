using SharpEngine.Math;
using System.Numerics;

namespace SharpEngine.Drawing.Interfaces
{
    public interface IDrawingElement : IDisposable
    {
        string Tag { get; set; }
        bool IsVisible { get; set; }
        int Priority { get; set; }
        Vector2f Position { get; }
        Vector2f Size { get; set; }
        void Register();
    }
}

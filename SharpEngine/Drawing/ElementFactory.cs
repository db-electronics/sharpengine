using SharpEngine.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Drawing
{
    public static class ElementFactory
    {
        public static RectangleElement CreateRectangle(string tag, Vector2f center, Vector2f size, Color colour, int priority)
        {
            return new RectangleElement(tag, center, size, colour, priority);
        }

        public static SpriteElement CreateSpriteFromFile(string tag, string path, Vector2f center, int priority)
        {
            return new SpriteElement(tag, path, center, priority);
        }
    }
}

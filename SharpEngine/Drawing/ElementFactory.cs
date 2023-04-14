using SharpEngine.Math;

namespace SharpEngine.Drawing
{
    public static class ElementFactory
    {
        public static TextElement CreateTextElement(string tag, string value, Vector2f position, Vector2f size, Color colour, int priority = 0)
        {
            return new TextElement(tag, value, position, size, colour, priority);
        }

        public static EllipseElement CreateEllipseElement(string tag, Vector2f center, Vector2f size, Color colour, int priority = 0)
        {
            return new EllipseElement(tag, center, size, colour, priority);
        }

        public static RectangleElement CreateRectangle(string tag, Vector2f center, Vector2f size, Color colour, int priority = 0)
        {
            return new RectangleElement(tag, center, size, colour, priority);
        }

        public static ImageElement CreateImageFromFile(string tag, string path, Vector2f center, int priority = 0)
        {
            return new ImageElement(tag, path, center, priority);
        }
    }
}

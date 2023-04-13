namespace SharpEngine.Drawing.Interfaces
{
    public interface ITextElement : IDrawingElement
    {
        string Value { get; set; }
        Font Font { get; set; }
        Color Colour { get; set; }
        Brush Brush { get; }
    }
}

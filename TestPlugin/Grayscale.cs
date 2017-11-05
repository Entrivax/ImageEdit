using ImageEdit;
using ImageEdit.Maths;
using System.Drawing;

namespace TestPlugin
{
    public class Grayscale : IPlugin
    {
        public string Name => "Grayscale";

        public Color Compute(Vector2 position, IImage original)
        {
            var color = original.Get(position.X, position.Y);
            var grayscale = (int)(color.R * 0.299f + color.G * 0.587f + color.B * 0.114);
            return Color.FromArgb(color.A, grayscale, grayscale, grayscale);
        }
    }
}

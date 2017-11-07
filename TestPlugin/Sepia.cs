using System.Drawing;
using ImageEdit;
using ImageEdit.Maths;

namespace TestPlugin
{
    public class Sepia : IPlugin
    {
        public string Name => "Sepia";

        public Color Compute(Vector2 position, IImage original)
        {
            var color = original.Get(position.X, position.Y);
            var colorV = new Vector3(color.R, color.G, color.B) / 255f;
            var r = Clamp(colorV.X * 0.393f + colorV.Y * 0.769f + colorV.Z * 0.189f, 0, 1);
            var g = Clamp(colorV.X * 0.349f + colorV.Y * 0.686f + colorV.Z * 0.168f, 0, 1);
            var b = Clamp(colorV.X * 0.272f + colorV.Y * 0.534f + colorV.Z * 0.131f, 0, 1);
            return Color.FromArgb(color.A, (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        float Clamp(float value, float min, float max)
        {
            return value < min ? min : (value > max ? max : value);
        }
    }
}

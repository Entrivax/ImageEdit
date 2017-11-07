using ImageEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageEdit.Maths;
using System.Drawing;

namespace TestPlugin
{
    public class Bloom : IPlugin
    {
        public string Name => "Bloom";

        public Color Compute(Vector2 position, IImage original)
        {
            var bloom = new Vector4(0);

            var pixelSize = new Vector2(1f / original.Width, 1f / original.Height);

            for(int x = -4; x < 4; x++)
            {
                for(int y = -3; y < 3; y++)
                {
                    var coords = position + new Vector2(x * pixelSize.X, y * pixelSize.Y);
                    var color = original.Get(coords.X, coords.Y);
                    bloom += new Vector4(new Vector3(color.R, color.G, color.B) * 0.0015f, color.A);
                }
            }

            var currentColor = original.Get(position.X, position.Y);

            if (currentColor.R < 0.3f)
                bloom = bloom * bloom * 0.012f;
            else if (currentColor.R < 0.5f)
                bloom = bloom * bloom * 0.009f;
            else
                bloom = bloom * bloom * 0.0075f;

            bloom *= 255f;
            return Color.FromArgb((int)Clamp(currentColor.A + bloom.W, 0, 255), (int)Clamp(currentColor.R + bloom.X, 0, 255), (int)Clamp(currentColor.G + bloom.Y, 0, 255), (int)Clamp(currentColor.B + bloom.Z, 0, 255));
        }
        
        float Clamp(float value, float min, float max)
        {
            return value < min ? min : (value > max ? max : value);
        }
    }
}

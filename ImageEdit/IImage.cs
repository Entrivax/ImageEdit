using System.Drawing;

namespace ImageEdit
{
    public interface IImage
    {
        Color Get(float x, float y);
        int Width { get; }
        int Height { get; }
    }
}

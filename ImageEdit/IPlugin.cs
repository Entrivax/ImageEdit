using ImageEdit.Maths;
using System.Drawing;

namespace ImageEdit
{
    public interface IPlugin
    {
        string Name { get; }
        
        Color Compute(Vector2 position, IImage original);
    }
}

using System.Drawing;
using Microsoft.CodeAnalysis.Text;

namespace Core.Text
{
    public class ColorTextSpan
    {
        public TextSpan Span { get; set; }
        public Color Color { get; set; }
        public Color BackgroundColor { get; set; }
    }
}

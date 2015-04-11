using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

using Core.Workspace;

namespace Core.Text
{
    public class ClassificationHighlighter
    {
        public delegate void EventHandler1();
        public Color DefaultColor { get; protected set; }
        public Color DefaultBackgroundColor { get; protected set; }
        public readonly Dictionary<string, Color> Map;
        public readonly List<ColorTextSpan> Changes;

        public ClassificationHighlighter()
        {            
            Changes = new List<ColorTextSpan>();
            Map = new Dictionary<string, Color>();
            DefaultColor = Color.White;
            DefaultBackgroundColor = Color.FromArgb(32, 32, 32);

            Map.Add(ClassificationTypeNames.Keyword, Color.CornflowerBlue);
            Map.Add(ClassificationTypeNames.ClassName, Color.Turquoise);
            Map.Add(ClassificationTypeNames.DelegateName, Color.Turquoise);
            Map.Add(ClassificationTypeNames.EnumName, Color.LightGoldenrodYellow);
            Map.Add(ClassificationTypeNames.InterfaceName, Color.LightGoldenrodYellow);
            Map.Add(ClassificationTypeNames.StructName, Color.Turquoise);
            Map.Add(ClassificationTypeNames.TypeParameterName, Color.Azure);
            Map.Add(ClassificationTypeNames.Identifier, Color.Turquoise);
            Map.Add(ClassificationTypeNames.Comment, Color.ForestGreen);
            Map.Add(ClassificationTypeNames.Punctuation, DefaultColor);
            Map.Add(ClassificationTypeNames.Operator, DefaultColor);
            Map.Add(ClassificationTypeNames.StringLiteral, Color.YellowGreen);
            Map.Add(ClassificationTypeNames.NumericLiteral, Color.DarkSeaGreen);
            Map.Add(ClassificationTypeNames.ExcludedCode, Color.Tomato);
        }

        // Adapted from: https://roslyn.codeplex.com/SourceControl/latest#Src/Samples/CSharp/ConsoleClassifier/Program.cs
        #region Derivative Work
        public async Task Format(Document document, SourceText text = null)
        {
            if (document == null)
            {
                return;
            }

            if (text == null)
            {
                Debug.WriteLine("Loading text");
                text = SourceText.From(File.OpenRead(document.FilePath));
            }

            document = await Formatter.FormatAsync(document);            

            var cSpans = await Classifier.GetClassifiedSpansAsync(document.WithText(text), TextSpan.FromBounds(0, text.Length));

            var ranges = cSpans.Select(c => new Range(c, text.GetSubText(c.TextSpan).ToString()));

            ranges = FillGaps(text, ranges);

            Changes.Clear();

            foreach (var range in ranges)
            {
                if (Map.Keys.Any(key => key.Equals(range.ClassificationType)))
                {
                    var colorSpan = new ColorTextSpan();
                    colorSpan.Span = range.TextSpan;
                    colorSpan.Color = Map[range.ClassificationType];
                    Changes.Add(colorSpan);
                }
                else
                {
                    if (!string.IsNullOrEmpty(range.ClassificationType))
                    {
                        Debug.WriteLine("Notice: " + range.ClassificationType + " is not mapped to a color");
                    }
                }
            }
        }

        IEnumerable<Range> FillGaps(SourceText text, IEnumerable<Range> ranges)
        {
            const string wsc = null;
            int current = 0;
            Range previous = null;

            foreach (Range range in ranges)
            {
                int start = range.TextSpan.Start;

                if (start > current)
                {
                    yield return new Range(wsc, TextSpan.FromBounds(current, start), text);
                }

                if (previous == null || range.TextSpan != previous.TextSpan)
                {
                    yield return range;
                }

                previous = range;
                current = range.TextSpan.End;
            }

            if (current < text.Length)
            {
                yield return new Range(wsc, TextSpan.FromBounds(current, text.Length), text);
            }

        }
        #endregion
    }
}

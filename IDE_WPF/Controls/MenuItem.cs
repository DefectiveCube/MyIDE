using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;

namespace IDE_WPF.Controls
{
    public class MenuItem : Control
    {
        List<MenuItem> Items;

        string text;
        FormattedText fText;
        Typeface typeface;
        FontFamily family;

        public MenuItem()
        {
            family = new FontFamily("Consolas");
            typeface = new Typeface(family, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        }

        public override void OnDraw()
        {
            base.OnDraw();
        }

        public FormattedText FormattedText
        {
            get { return fText; }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                fText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 16, Brushes.White);
            }
        }
    }
}

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Input;

namespace IDE_WPF.Controls
{
    public class Button : Control
    {
        public Button()
        {
            this.MouseDown += UIElement_MouseDown;
            IsHitTestVisible = true;            
        }

        private void UIElement_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.WriteLine("Mouse Down");
        }

        public string Text { get; set; }

        public override void OnDraw()
        {
            using(var context = Visual.RenderOpen())
            {
                context.DrawRectangle(Brushes.Silver, new Pen(Brushes.Black, 1.0), new Rect(new Size(Width, 20)));
                context.DrawText(new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 16, Brushes.Black), new Point(0,0));
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }
    }
}

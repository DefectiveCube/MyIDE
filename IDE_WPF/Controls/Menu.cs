using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;

namespace IDE_WPF.Controls
{
    public class Menu : Control
    {
        List<MenuItem> Items;

        public Menu()
        {
            
            Height = 20;
            Items = new List<MenuItem>();
        }

        public override void OnDraw()
        {
            var x = 10.0;

            using (var context = Visual.RenderOpen())
            {
                context.DrawRectangle(Brushes.DimGray, null, new Rect(new Size(Width, Height)));

                foreach (var item in Items)
                {
                    context.DrawText(item.FormattedText, new Point(x, 1));

                    x += item.FormattedText.Width + 10;
                }
            }
        }

        public void Add(string text)
        {
            Add(new MenuItem() { Text = text });
        }

        public void Add(MenuItem item)
        {
            Items.Add(item);
        }
    }
}

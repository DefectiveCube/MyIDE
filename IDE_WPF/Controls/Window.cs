using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IDE_WPF.Controls
{
    public class Window : System.Windows.Window
    {
        List<Control> controls;
        VisualCollection visuals;
        DrawingVisual visual;

        Point hp;

        public Window()
        {
            controls = new List<Control>();
            visuals = new VisualCollection(this);
            visual = new DrawingVisual();

            IsHitTestVisible = true;

            Loaded += new RoutedEventHandler(IDEWindow_Loaded);
            SizeChanged += IDEWindow_SizeChanged;

            MouseDown += UIElement_MouseDown;
            AddHandler(UIElement.MouseDownEvent, (RoutedEventHandler)m_Down, true);
        }

        private void m_Down(object sender, RoutedEventArgs e)
        {            
            Debug.WriteLine("Routed Down");
        }

        private void UIElement_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.WriteLine("Mouse Down");

            var p = e.GetPosition(this);
            hp = p;

            List<Control> hits = new List<Control>();

            foreach (var c in controls)
            {
                var result = VisualTreeHelper.HitTest(c.Visual, p);

                if (result != null)
                {
                    hits.Add(c);
                }
            }
        }

        private void IDEWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OnDraw();
        }

        protected void IDEWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OnDraw();
        }
               
        public InputManager KeyBindings { get; set; }
                
        public void Add(Control control)
        {
            controls.Add(control);

            control.OnDraw();

            AddLogicalChild(control);

            visual.Children.Add(control.Visual);
        }

        public void Remove(Control control)
        {
            visual.Children.Remove(control.Visual);
        }

        public void OnDraw()
        {
            visuals.Clear();

            using (var context = visual.RenderOpen())
            {
                context.DrawRectangle(new SolidColorBrush(Color.FromRgb(32, 32, 32)), null, new Rect(new Size(ActualWidth, ActualHeight)));
            }

            foreach(var ctrl in controls)
            {
                ctrl.OnDraw();

                visual.Children.Add(ctrl.Visual);
            }

            visuals.Add(visual);

        }
        
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }
    }
}

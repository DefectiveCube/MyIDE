using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;

namespace IDE_WPF.Controls
{
    [Flags]
    public enum Anchor
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

    public class Control : FrameworkElement
    {
        public static readonly RoutedEvent ClickEvent;
        public static readonly DependencyProperty BackgroundColorProperty;
        public static readonly DependencyProperty PaddingProperty;

        static Control()
        {
            ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Control));

            BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(Control), new FrameworkPropertyMetadata() { AffectsRender = true });
            PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Control), new FrameworkPropertyMetadata() { AffectsRender = true });           
        }

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public Anchor Anchor { get; set; }

        /// <summary>
        /// Absolute Position
        /// </summary>
        /// <returns></returns>
        public Point Position { get; set; }

        private VisualCollection visuals;

        public List<Controls> Controls;

        public DrawingVisual Visual { get; protected set; }

        public InputManager KeyBindings { get; set; }

        public bool AcceptKeyInputs { get; set; }

        public bool AcceptMouseInputs { get; set; }

        public Brush Background { get; set; }

        public Brush Foreground { get; set; }

        public FrameworkElement Target { get; private set; }

        public Control()
        {
            KeyBindings = new InputManager();
            
            KeyDown += KeyBindings.KeyDown;
            KeyUp += KeyBindings.KeyUp;

            SizeChanged += IDEControl_SizeChanged;

            IsHitTestVisible = true;

            visuals = new VisualCollection(this);
            Visual = new DrawingVisual();

            //AddHandler(ClickEvent, (RoutedEventHandler)Mouse_Down, true);
            //AddHandler(UIElement.MouseDownEvent, (RoutedEventHandler)Mouse_Down, true);
        }

        public IEnumerable<Control> HitTest(Point p)
        {
            foreach (var c in Controls)
            {
                if(VisualTreeHelper.HitTest(c.Visual,p) != null)
                {
                    yield return c;
                }
            }

            yield break;
        }

        private void Mouse_Down(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Down");
        }

        private void IDEControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OnDraw();
        }

        public void Add(Control control)
        {
            Visual.Children.Add(control.Visual);
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        public virtual void OnDraw() { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Debug.WriteLine("Rendering");
        }
    }
}

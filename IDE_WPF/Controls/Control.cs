using System;
using System.Collections.Generic;
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
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Control));

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public Anchor Anchor { get; set; }

        private VisualCollection visuals;

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
    }
}

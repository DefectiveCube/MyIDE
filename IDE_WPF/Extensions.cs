using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;


using System.Windows;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace IDE_WPF
{
    public static class Extensions
    {
        public static bool IsDown(this Key key)
        {
            return Keyboard.IsKeyDown(key);
        }

        public static bool IsUp(this Key key)
        {
            return Keyboard.IsKeyUp(key);
        }

        public static bool IsSameColor(this Brush brush, Brush other)
        {
            var color1 = new BrushConverter().ConvertToString(brush);
            var color2 = new BrushConverter().ConvertToString(other);

            var result = color1.Equals(color2);

            return result;
        }

        // TODO: find a better place to put this. Extension methods cannot be placed on static types (Preferred the Keyboard class)
        public static bool IsModifierKeyDown()
        {
            return
                Keyboard.IsKeyDown(Key.LeftCtrl) ||
                Keyboard.IsKeyDown(Key.RightCtrl) ||
                Keyboard.IsKeyDown(Key.LeftShift) ||
                Keyboard.IsKeyDown(Key.RightShift) ||
                Keyboard.IsKeyDown(Key.LeftAlt) ||
                Keyboard.IsKeyDown(Key.RightAlt);
        }

        public static bool IsWindowsKey(this Key key)
        {
            switch (key)
            {
                case Key.LWin:
                case Key.RWin:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsModifier(this Key key)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LWin:
                case Key.RWin:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsCtrl(this Key key)
        {
            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsShift(this Key key)
        {
            switch (key)
            {
                case Key.LeftShift:
                case Key.RightShift:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsDigit(this Key key)
        {
            return key.CompareTo(Key.Help) + key.CompareTo(Key.A) == 0;
        }

        public static bool IsLetter(this Key key)
        {
            return key.CompareTo(Key.D9) + key.CompareTo(Key.LWin) == 0;
        }

        public static bool IsNumPadDigitKey(this Key key)
        {
            return key.CompareTo(Key.Sleep) + key.CompareTo(Key.Multiply) == 0;
        }

        public static bool IsFunctionKey(this Key key)
        {
            return key.CompareTo(Key.Divide) + key.CompareTo(Key.NumLock) == 0;
        }

        public static bool IsSpecialCharacter(this Key key)
        {
            switch (key)
            {
                case Key.OemBackslash:
                case Key.OemCloseBrackets:
                case Key.OemComma:
                case Key.OemPeriod:
                case Key.OemPipe:
                case Key.OemPlus:
                case Key.OemOpenBrackets:
                case Key.OemQuestion:
                case Key.OemQuotes:
                case Key.OemSemicolon:
                case Key.OemTilde:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsArrowKey(this Key key)
        {
            switch (key)
            {
                case Key.Up:
                case Key.Down:
                case Key.Left:
                case Key.Right:
                    return true;
                default:
                    return false;
            }
        }

        public static ModifierKeys ToModifierKey(this Key key)
        {
            switch (key)
            {
                case Key.LeftShift:
                case Key.RightShift:
                    return ModifierKeys.Shift;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return ModifierKeys.Control;
                case Key.LeftAlt:
                case Key.RightAlt:
                    return ModifierKeys.Alt;
                case Key.LWin:
                case Key.RWin:
                    return ModifierKeys.Windows;
                default:
                    return ModifierKeys.None;
            }
        }

        public static bool IsPressed(this ModifierKeys key)
        {
            switch (key)
            {
                case ModifierKeys.None:
                    return !InputManager.IsModifierDown();
                case ModifierKeys.Alt:
                    return InputManager.IsAltDown();
                case ModifierKeys.Shift:
                    return InputManager.IsShiftDown();
                case ModifierKeys.Control:
                    return InputManager.IsCtrlDown();
                case ModifierKeys.Control | ModifierKeys.Shift:
                    return InputManager.IsShiftDown() && InputManager.IsCtrlDown();
                case ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt:
                    return InputManager.IsAltDown() && InputManager.IsCtrlDown() && InputManager.IsShiftDown();
                default:
                    Debug.WriteLine("Could not find value of " + key);
                    return false;
            }
        }

        public static bool IsCurrentState(this ModifierKeys key)
        {
            return false;
        }

        public static IEnumerable<Key> Where(this Key key, Func<Key,bool> func)
        {
            var values = (Key[])Enum.GetValues(typeof(Key));

            return values.Where(func);
        }

        public static Point ToPoint(this LinePosition span)
        {
            return new Point(span.Character + 1, span.Line + 1);
        }

        public static Rect ToRect(this FileLinePositionSpan span, double charHeight, double maxWidth)
        {
            var start = span.StartLinePosition.ToPoint();
            var end = span.EndLinePosition.ToPoint();
            var lines = end.Y - start.Y + 1;
            var size = new Size(maxWidth, lines * charHeight);

            return new Rect(new Point(0, start.Y), size);
        }
    }
}

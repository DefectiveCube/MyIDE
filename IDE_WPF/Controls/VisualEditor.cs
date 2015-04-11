using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Markup;
using System.Windows.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;

using IDE_WPF.Controls;

namespace IDE_WPF
{
    public enum InteractionMode
    {
        Text,
        Object
    }

    public class CodeBox : Control
    {
        //event EventHandler SelectedText;
        //event EventHandler SelectedTextChanged;

        event EventHandler Moved;

        SyntaxTree sourceTree;

        public EditorCursor EditorCursor { get; private set; }

        internal TextStore Data { get; set; }

        public InteractionMode Mode { get; set; }

        public Rect ScrollBox { get; set; }

        //public List<StringBuilder> LinesOfText { get; set; }

        /// <summary>
        /// Return the entire document as a string
        /// </summary>
        /// <returns></returns>
        public string Text
        {
            get { return SourceCode.ToString(); }
            set { SourceTree = CSharpSyntaxTree.ParseText(value, CSharpParseOptions.Default); }
        }

        internal SyntaxTree SourceTree
        {
            get { return sourceTree; }
            set
            {
                sourceTree = value;
                
                Data.Clear();

                // TODO: TextStore should fire an event and pass limits to the Cursor object

                //EditorCursor.Limits.Clear();

                //Renderer.ClearLines();

                Data.Add(SourceCode);

                foreach (var line in SourceCode.Lines)
                {
                    var sb = new StringBuilder();

                    sb.Append(SourceCode.GetSubText(line.Span).ToString().Trim());

                    //Renderer.AddLine(line.ToString());
                    //EditorCursor.Limits.Add(sb.Length);
                }
            }
        }

        internal Document Document { get; set; }

        internal SourceText SourceCode
        {
            get { return SourceTree.GetTextAsync().Result; }
        }

        public CodeBox()
        {
            Focusable = true;
            Margin = new Thickness(0);
            Padding = new Thickness(40, 10, 10, 10);

            Data = new TextStore();
            
            Data.LineAdded += (s, e) => { };
            Data.LineModified += (s, e) => { };
            Data.LineRemoved += (s, e) => { };

            Cursor = Cursors.IBeam;

            KeyBindings.Mode = InteractionMode.Text;

            // TODO: use KeyBindings from App.Config instead of hard-code

            KeyBindings.Add(Move, ModifierKeys.None, k => k.IsArrowKey());
            KeyBindings.Add(Move, ModifierKeys.Shift, k => k.IsArrowKey());
            KeyBindings.Add(Move, Key.PageUp);
            KeyBindings.Add(Move, Key.PageDown);
            KeyBindings.Add(Enter, Key.Enter);
            KeyBindings.Add(Type, ModifierKeys.None, k => k.IsLetter() || k.IsDigit() || k.IsSpecialCharacter() || k == Key.Space);
            KeyBindings.Add(Type, ModifierKeys.Shift, k => k.IsLetter() || k.IsDigit() || k.IsSpecialCharacter() || k == Key.Space);
            KeyBindings.Add(Backspace, Key.Back);
            KeyBindings.Add(SelectAll, ModifierKeys.Control, Key.A);
            KeyBindings.Add(Cut, ModifierKeys.Control, Key.X);
            KeyBindings.Add(Copy, ModifierKeys.Control, Key.C);
            KeyBindings.Add(Paste, ModifierKeys.Control, Key.V);
            KeyBindings.Add(End, Key.End);
            KeyBindings.Add(Delete, Key.Delete);
            
            var c = ConfigurationManager.AppSettings.Get("code_box_font_color");
            var clr = (Color)ColorConverter.ConvertFromString(c);
            
            /*Renderer.Margin = Margin;
            Renderer.Padding = Padding;
            Renderer.DisplayLineNumbers = false;
            Renderer.BackgroundBrush = new SolidColorBrush(Color.FromRgb(32, 32, 32));
            Renderer.ForegroundBrush = Drawer.ConvertToBrush(c);
            //drawer.HighlightBackgroundBrush = new SolidColorBrush(Colors.Coral);
            //drawer.HighlightForegroundBrush = new SolidColorBrush(Colors.White);
            Renderer.FontFamily = new FontFamily("Consolas");
            Renderer.Font = new Typeface(Renderer.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            Renderer.FontSize = 16;
            Renderer.FontWidth = new FormattedText("X", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Renderer.Font, 16, Renderer.ForegroundBrush).Width;

            Renderer.ForegroundBrush.Freeze();
            Renderer.BackgroundBrush.Freeze();

            Renderer.AddLine(string.Empty);*/

            EditorCursor = new EditorCursor();
            EditorCursor.Position.Offset(Margin.Left + Padding.Left, Margin.Top + Padding.Top);
            EditorCursor.PositionChanged += Caret_PositionChanged;

            //GotFocus += new RoutedEventHandler(Editor_GotFocus);
            //LostFocus += new RoutedEventHandler(Editor_LostFocus);
            //Loaded += new RoutedEventHandler(Editor_Loaded);

            MouseDown += UIElement_MouseDown;
            MouseUp += UIElement_MouseUp;
            KeyDown += KeyBindings.KeyDown;
            KeyUp += KeyBindings.KeyUp;

            //this.SelectedText += VisualEditor_SelectedText;
            //this.SelectedTextChanged += VisualEditor_SelectedTextChanged;
        }

        public void Type()
        {
            Input_Typed(KeyBindings.KeysPressed.Last(), InputManager.IsShiftDown());
        }        

        private void Move()
        {
            var line = 0;
            var column = 0;

            if (Key.Left.IsDown())
            {
                column--;
            }

            if (Key.Right.IsDown())
            {
                column++;
            }

            if (Key.Up.IsDown())
            {
                line--;
            }

            if (Key.Down.IsDown())
            {
                line++;
            }

            EditorCursor.RelativeMove(line, column);

            if (Moved != null)
            {
                Moved(this, new EventArgs());
            }
        }

        public void Cut()
        {
            // If there is no selected text, cut entire line
            Clipboard.SetText("", TextDataFormat.Text);
        }

        public void Copy()
        {
            // If there is no selected text, copy entire line
            Clipboard.SetText("", TextDataFormat.Text);
        }

        public void Paste()
        {
            Clipboard.GetText();
        }

        public void Paste(TextDataFormat format)
        {
            Clipboard.GetText(format);
        }

        public void End()
        {
            if (KeyBindings.ModifierKeys.HasFlag(ModifierKeys.Shift))
            {
                // Highlight text
            }

            EditorCursor.MoveToEOL();
        }

        public void SelectAll() { }

        public void Select() { }
        
        private void Input_Typed(Key c, bool isUpper)
        {
            var key = c;
            var text = string.Empty;
            var line = EditorCursor.Line - 1;
            var col = EditorCursor.Column - 1;
            var val = (int)key - 34;

            // TODO: clear text selection highlights

            Debug.WriteLine(key);

            if (key.IsLetter())
            {
                text = isUpper ? key.ToString().ToUpperInvariant() : key.ToString();
            }
            else if (key.IsDigit())
            {
                text = val.ToString();
            }
            else if (key == Key.Space)
            {
                text = " ";
            }
            else if (key.IsSpecialCharacter())
            {
                // Can't handle special characters just yet
            }
            else
            {
                // ???
            }

            Data.Lines.ElementAt(line).Insert(col, text);

            Data.Insert(line, col, text);


            //EditorCursor.Limits[line]++;

            //Renderer.UpdateLine(line, Data.Lines.ElementAt(line).ToString()); //LinesOfText[line].ToString());

            EditorCursor.MoveToNextColumn();
            ///Renderer.Draw();
        }

        private void FrameworkElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Renderer.SizeChanged(e);
            //drawer.Draw();
        }

        private void Caret_PositionChanged(object sender, Point position)
        {
            /*Renderer.CursorPosition = Renderer.GetActualPosition(position);
            Renderer.IsCursorVisible = true;
            Renderer.Stop();
            Renderer.Start();*/
        }

        /// <summary>
        /// Determines which text is highlighted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualEditor_SelectedTextChanged(object sender, EventArgs e)
        {
            /*if(SelectionStart == SelectionEnd)
            {
                return;
            }

            //highlights.Clear();

            // How many lines is the selection?
            var lines = Math.Abs(SelectionEnd.Y - SelectionStart.Y) + 1;
            var IsSingleLine = lines == 1;
          
            var UpperYBound = SelectionEnd.Y > SelectionStart.Y ? SelectionEnd.Y : SelectionStart.Y;
            var LowerYBound = SelectionEnd.Y < SelectionStart.Y ? SelectionEnd.Y : SelectionStart.Y;
            var UpperXBound = SelectionEnd.X > SelectionStart.X ? SelectionEnd.X : SelectionStart.X;
            var LowerXBound = SelectionEnd.X < SelectionStart.X ? SelectionEnd.X : SelectionStart.X;

            // Determine the origin point and the opposite corner point
            var origin = new Point(LowerXBound, LowerYBound);
            var opCorner = new Point(UpperXBound, UpperYBound + FontHeight);

            // TODO: move to drawer class
            //highlights.Add(new Rect(origin, opCorner));
            
            drawer.Draw(displayHighlights: true);

            */
        }

        private void VisualEditor_SelectedText(object sender, EventArgs e)
        {

        }

        private void Editor_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[VisualEditor] Lost Focus");

            e.Handled = true;
        }

        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[VisualEditor] Got Focus");

            e.Handled = true;
        }

#region event handling
       
        private void Space()
        {
            //LinesOfText[EditorCursor.Line - 1].Insert(EditorCursor.Column - 1, ' ');
            Data.Lines.ElementAt(EditorCursor.Line - 1).Insert(EditorCursor.Column - 1, ' ');


            //MoveCursorToColumn(ColumnNumber + 1);
        }

        private void Backspace()
        {
            if (EditorCursor.Line > 1 || EditorCursor.Column > 1)
            {
                if (EditorCursor.Column > 1)
                {
                    EditorCursor.MoveToPreviousColumn();
                }
                else
                {
                    EditorCursor.MoveToPreviousLine();
                    EditorCursor.MoveToEOL();
                }

                Delete();
            }
        }

        private void Enter()
        {
            var sb = new StringBuilder();

            if (!EditorCursor.IsEOL)
            {
                var len = Data.Lines.ElementAt(EditorCursor.Line - 1).Length - EditorCursor.Column;
                //var len = EditorCursor.Limits[EditorCursor.Line - 1] - EditorCursor.Column;
                char[] c = new char[len];

                // Take a substring of the current line
                //LinesOfText[EditorCursor.Line].CopyTo(EditorCursor.Column, c, 0, c.Length);
                Data.Lines.ElementAt(EditorCursor.Line).CopyTo(EditorCursor.Column, c, 0, c.Length);

                // Modify current line length
                //LinesOfText[EditorCursor.Line].Length = EditorCursor.Column - 1;
                Data.Lines.ElementAt(EditorCursor.Line).Length = EditorCursor.Column - 1;

                sb.Append(c);
            }

            //LinesOfText.Insert(EditorCursor.Line, sb);
            Data.Insert(EditorCursor.Line, sb.ToString());

            /*var sb = new StringBuilder();

            if (EditorCursor.ColumnNumber < CurrentLine.Length)
            {
                sb.Append(CurrentLine.ToString().Substring(EditorCursor.ColumnNumber - 1));
                CurrentLine.Length = EditorCursor.ColumnNumber - 1;                
            }

            LinesOfText.Insert(EditorCursor.LineNumber, sb);
            */

            EditorCursor.MoveToNextLine();
            EditorCursor.MoveToBOL();

            //Renderer.Draw();
        }

        private void Delete()
        {
            /*if (EditorCursor.ColumnNumber < CurrentLine.Length)
            {
                CurrentLine.Remove(EditorCursor.ColumnNumber - 1, 1);
            }
            else if(lineNumber != LinesOfText.Count())
            {
                // Append next line to current line
                CurrentLine.Append(LinesOfText.ElementAt(lineNumber).ToString());
                LinesOfText.RemoveAt(lineNumber);
            }*/
        }

        private void UIElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void UIElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Renderer.AnimateMenu();
            e.Handled = true;
        }
        #endregion

        internal void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            //Renderer.CursorPosition = Renderer.GetActualPosition(new Point(1, 1));
            //Renderer.BackgroundSize = new Size(ActualWidth, ActualHeight);
                      
            // For demo purposes
            //Text = "public class TestClass\n{\n//...\n}";

            //Renderer.Start();
        }

        public override void OnDraw()
        {
            var y = 0;

            using (var context = Visual.RenderOpen())
            {
                Debug.WriteLine(string.Format("Drawing {0} line(s)", Data.Lines.Count()));

                foreach (var line in Data.Lines)
                {
                    var fText = new FormattedText(line.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Consolas"), 16, Brushes.White);

                    context.DrawText(fText, new Point(Margin.Left + Padding.Left, Margin.Top + Padding.Top + fText.LineHeight * y++));
                }
            }
        }
    }
}
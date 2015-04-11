using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;

namespace IDE_WPF
{
    public class EditorCursor
    {
        public delegate void PositionChangedHandler(object sender, Point position);

        public event PositionChangedHandler PositionChanged;

        int lineNumber;
        int columnNumber;
        bool changed;

        internal void OnPositionChanged()
        {
            if(changed && PositionChanged != null)
            {
                PositionChanged(this, new Point(Column, Line));
            }

            changed = false;
        }

        public int Line
        {
            get { return lineNumber; }
            private set
            {
                if(lineNumber != value && value > 0 && value <= Limits.Count())
                {
                    changed = true;
                    lineNumber = value;

                    if (columnNumber > Limits[lineNumber - 1])
                    {
                        columnNumber = Limits[lineNumber - 1] + 1;
                    }
                }
            }
        }

        public int Column
        {
            get { return columnNumber; }
            private set
            {
                var limit = Limits[lineNumber - 1] + 1;

                if (columnNumber != value && value > 0 && Limits.Count() > 0 && value <= limit)
                {
                    changed = true;
                    columnNumber = value;
                }
            }
        }

        public int Characters
        {
            get { return Limits.Sum(); }
        }

        public bool IsBOL
        {
            get { return Column == 1; }
        }

        public bool IsBOF
        {
            get { return IsBOL && Line == 1; }
        }

        public bool IsEOL
        {
            get { return false; }
        }

        public bool IsEOF
        {
            get { return false; }
        }

        public int CurrentLineLength
        {
            get
            {
                return Limits[Line - 1];
            }
        }

        public bool Locked { get; set; }

        private List<int> Limits { get; set; }

        public Point Position { get; set; }

        public void Move(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    MoveToPreviousLine();
                    break;
                case Key.Down:
                    MoveToNextLine();
                    break;
                case Key.Left:
                    MoveToPreviousColumn();
                    break;
                case Key.Right:
                    MoveToNextColumn();
                    break;
                default:
                    return;
            }
        }

        public void RelativeMove(int line, int column)
        {
            Line += line;
            Column += column;

            Move();
        }

        public EditorCursor()
        {
            Limits = new List<int>() { 1 };
            Line = 1;
            Column = 1;
            changed = false;
        }

        private void Move()
        {
            Move(Line, Column);
        }

        public void Move(int line, int column)
        {
            Line = line;
            Column = column;

            OnPositionChanged();
        }

        public void MoveToPreviousColumn()
        {
            if (Column > 1)
            {
                Column--;
                Move();
            }
        }

        public void MoveToPreviousLine()
        {
            if (Line > 1)
            {
                Line--;
                Move();
            }
        }

        public void MoveToNextColumn()
        {
            if (Column < Limits[lineNumber - 1] + 1)
            {
                Column++;
                Move();
            }
        }

        public void MoveToNextLine()
        {
            Line++;
            Move();
        }

        public void MoveToLine(int line)
        {
            if (line > 0 && line <= Limits.Count())
            {
                Line = line;
            }

            Move();
        }

        public void MoveToColumn(int column)
        {
            if (column > 0 && column <= CurrentLineLength)
            {
                Column = column;
            }

            Move();
        }

        public void MoveToBOL(int line = 0)
        {
            if(line > 0 && line <= Limits.Count())
            {
                Line = line;
            }

            MoveToColumn(1);
        }

        public void MoveToEOL(int line = 0)
        {
            if (line > 0 && line <= Limits.Count())
            {
                Line = line;
            }

            MoveToColumn(CurrentLineLength);            
        }

        public void MoveToBOF()
        {
            Move(1, 1);
        }

        public void MoveToEOF()
        {
            MoveToEOL(Limits.Count());
        }

        public void Update()
        {
            OnPositionChanged();
        }
    }
}

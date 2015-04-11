using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;

namespace IDE_WPF
{
    public class LineChangedEventArgs : EventArgs
    {
        public int Index;
        public int Length;
    }

    public class TextStore
    {
        public delegate void LineChangedEventHandler(object sender, LineChangedEventArgs e);

        public event LineChangedEventHandler LineAdded;
        public event LineChangedEventHandler LineRemoved;
        public event LineChangedEventHandler LineModified;
        public event EventHandler LinesCleared;

        List<StringBuilder> lines;
        int charTotal = 0;

        public TextStore()
        {
            lines = new List<StringBuilder>();
        }

        public List<int> Limits { get; set; }

        public int Count { get { return lines.Count(); } }

        public int Size { get { return charTotal; } }

        public string Text { get; set; }

        public IEnumerable<StringBuilder> Lines
        {
            get { return lines.AsEnumerable(); }
        }

        public void Add(string text = "")
        {
            Debug.WriteLine(string.Format("Line {0}: {1}", lines.Count(), text));

            lines.Add(new StringBuilder(text));

            FireEvent(LineAdded, lines.Count() - 1, text.Length);
        }

        public void Add(SourceText text)
        {
            foreach (var line in text.Lines)
            {
                var str = text.GetSubText(line.Span).ToString().Trim();

                Add(str);
            }
        }
        
        public void Clear()
        {
            lines.Clear();

            FireEvent(LinesCleared);
        }

        public void Insert(int line, string text = "")
        {
            if(line < 0)
            {
                return;
            }
            
            lines.Insert(line, new StringBuilder(text));

            FireEvent(LineModified, line, text.Length);
        }

        /// <summary>
        /// Insert text at a specific line and column
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="text"></param>
        public void Insert(int line, int column, string text)
        {
            Lines.ElementAt(line).Insert(column, text);

            FireEvent(LineModified, line, Lines.ElementAt(line).Length);
        }

        public void RemoveAt(int index)
        {
            lines.RemoveAt(index);
            
            FireEvent(LineRemoved, index, -1);
        }

        public void Clear(int line)
        {
            Lines.ElementAt(line).Clear();

            FireEvent(LineModified, line, 0);
        }

        private void FireEvent(EventHandler handler)
        {
            if(handler == null)
            {
                return;
            }

            handler(this, new EventArgs());
        }

        private void FireEvent(LineChangedEventHandler handler, int index, int length)
        {
            if(handler == null)
            {
                return;
            }

            var args = new LineChangedEventArgs()
            {
                Index = index,
                Length = length
            };

            handler(this, args);
        }
    }
}

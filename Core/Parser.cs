using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

using Core.Workspace;
using Core.Text;

namespace Core
{
    public class Parser
    {
        public delegate void HighlighterUpdatedHandler(object sender, HighlighterEventArgs e);

        public event EventHandler TreeChanged;
        public event HighlighterUpdatedHandler HighlighterUpdated;

        private ClassificationHighlighter newHighlighter;

        public SourceText Text { get; set; }

        public TextSpan Span { get; set; }

        public bool EnableHighlighting { get; set; }

        public Parser()
        {
            newHighlighter = new ClassificationHighlighter();
            EnableHighlighting = true;
        }

        protected virtual void OnTreeChanged(EventArgs e)
        {
            if (TreeChanged != null)
            {
                TreeChanged(this, e);
            }
        }

        protected virtual void OnHighlighterUpdated(HighlighterEventArgs e)
        {
            if (HighlighterUpdated != null)
            {
                HighlighterUpdated(this, e);
            }
        }

        public void SelectionChanged(object sender, EventArgs e)
        {
            var TextEditor = sender as RichTextBox;

            Span = new TextSpan(TextEditor.SelectionStart, TextEditor.SelectionLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public async void UpdateTree(Document doc)
        {
            if (doc == null)
            {
                Debug.WriteLine("Parser: Failed to update Tree");
                return;
            }

            OnTreeChanged(new EventArgs());

            if (EnableHighlighting)
            {
                await newHighlighter.Format(doc, Text);

                var e = new HighlighterEventArgs();

                if (newHighlighter.Changes.Count > 0)
                {
                    e.Changes = newHighlighter.Changes;

                    OnHighlighterUpdated(e);

                    newHighlighter.Changes.Clear();
                }
            }
        }

        /// <summary>
        /// Normalizes whitespace using predefined rules
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SyntaxNode NormalizeWhitespace(string text)
        {
            return CSharpSyntaxTree.ParseText(text).GetRoot().NormalizeWhitespace();
        }
    }
}
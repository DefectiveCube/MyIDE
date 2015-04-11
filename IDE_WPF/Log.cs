using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;

namespace IDE_WPF
{
    public class Log
    {
        public void Error(string message)
        {
            Message(string.Format("Error: {0}", message));
        }

        public void Debug(string message)
        {
            Message(string.Format("Debug: {0}", message));
        }

        public void Message(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Event(DocumentEventArgs e, DocumentId id)
        {           
            Message(string.Format("Event: Opened/Closed | Document: {0}", id.Id.ToString()));
        }

        public void Event(WorkspaceChangeEventArgs e, Solution oldSolution = null, Solution newSolution = null)
        {
            if (oldSolution != null)
            {
                Message(string.Format("Event: {1} | Solution: {0}", oldSolution.Id.ToString(), e.Kind));
            }
        }

        public void Event(WorkspaceChangeEventArgs e, ProjectId id)
        {
            Message(string.Format("Event: {1} | Project: {0}", id.Id.ToString(), e.Kind));
        }

        public void Event(WorkspaceChangeEventArgs e, DocumentId id)
        {
            Message(string.Format("Event: {1} | Document: {0}", id.Id.ToString(), e.Kind));
        }
    }
}

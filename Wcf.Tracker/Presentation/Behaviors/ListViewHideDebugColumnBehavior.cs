using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wcf.Tracker.Presentation.Behaviors
{
    /// <summary>
    /// Hide debugger column if no debuggers attached.
    /// </summary>
    internal class ListViewHideDebugColumnBehavior : Behavior<ListView>
    {
        private const int DebuggerAttachColumn = 0;

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (!Debugger.IsAttached)
            {
                var gridView = AssociatedObject.View as GridView;
                gridView.Columns.RemoveAt(DebuggerAttachColumn);
            }
        }
    }
}

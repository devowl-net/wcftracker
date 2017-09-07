using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wcf.Tracker.Presentation.Behaviors
{
    /// <summary>
    /// Behavior fill ListView last column width if it have available space.
    /// </summary>
    internal class ListViewLastColumnFillBehavior : Behavior<ListView>
    {
        /// <inheritdoc/>
        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += ListViewOnSizeChanged;
            base.OnAttached();
        }

        private void ListViewOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var listView = AssociatedObject;
            var gridView = listView.View as GridView;
            var availableWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            if (gridView != null)
            {
                availableWidth = gridView.Columns.Aggregate(availableWidth, (current, column) => current - column.ActualWidth);
                if (availableWidth > 0)
                {
                    gridView.Columns[gridView.Columns.Count - 1].Width = availableWidth;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= ListViewOnSizeChanged;
            base.OnDetaching();
        }
    }
}

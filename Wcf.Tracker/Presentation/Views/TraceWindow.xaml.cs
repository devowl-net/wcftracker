using System.Windows;

using Wcf.Tracker.Presentation.ViewModels;

namespace Wcf.Tracker.Presentation.Views
{
    /// <summary>
    /// Interaction logic for TraceWindow.xaml
    /// </summary>
    internal partial class TraceWindow : Window
    {
        /// <summary>
        /// Constructor for <see cref="TraceWindow"/>.
        /// </summary>
        public TraceWindow()
        {
            DataContext = new TraceWindowViewModel();
            InitializeComponent();
        }
    }
}
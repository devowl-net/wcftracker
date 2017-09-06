using Wcf.Tracker.Log;
using Wcf.Tracker.Presentation.Behaviors;
using Wcf.Tracker.Presentation.Views;

namespace Wcf.Tracker.Presentation.ViewModels
{
    /// <summary>
    /// TraceWindow.xaml ViewModel.
    /// </summary>
    internal class TraceWindowViewModel
    {
        /// <summary>
        /// Constructor for <see cref="TraceWindowViewModel"/>.
        /// </summary>
        public TraceWindowViewModel()
        {
            ShowMessageCommand = new DelegateCommand(ShowMessage);
            ClearCommand = new DelegateCommand(Clear);
            ExportCommand = new DelegateCommand(Export);
        }

        /// <summary>
        /// Show message command.
        /// </summary>
        public DelegateCommand ShowMessageCommand { get; private set; }

        /// <summary>
        /// Export command.
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        /// <summary>
        /// Clear command.
        /// </summary>
        public DelegateCommand ClearCommand { get; private set; }

        private void Export(object obj)
        {
        }

        private void Clear(object obj)
        {
            LogTracker.Instance.TraceFrames.Clear();
        }

        private void ShowMessage(object obj)
        {
            // TODO add frame #Id
            var frame = obj as TraceFrame;
            var title = $"{frame.EventTime}  -  {frame.ActionUrl}";
            var messageWindow = new MessageSourceWindow(frame.SourceText, title);
            messageWindow.Show();
        }
    }
}
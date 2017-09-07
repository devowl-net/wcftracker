using System;
using System.IO;
using System.Linq;
using System.Windows;

using Microsoft.Win32;

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
            CopyRowCommand = new DelegateCommand(CopyRow);
        }

        private void CopyRow(object obj)
        {
            var frame = obj as TraceFrame;
            if (frame != null)
            {
                Clipboard.SetText(frame.CallStack);
            }
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
        /// Copy row from context menu.
        /// </summary>
        public DelegateCommand CopyRowCommand { get; private set; }

        /// <summary>
        /// Clear command.
        /// </summary>
        public DelegateCommand ClearCommand { get; private set; }

        private void Export(object obj)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = desktopPath,
                FileName = "TraceLog.log"
            };

            if (saveFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                var fileName = saveFileDialog.FileName;

                using (var fileStream = File.Open(fileName, FileMode.Create))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        foreach (var frame in LogTracker.Instance.TraceFrames.ToArray())
                        {
                            WriteFile(streamWriter, frame.Direction == MessageDirection.Outgoing ? "[Outgoing]" : "[Incoming]");
                            WriteFile(streamWriter, $"[Time] {frame.EventTime.ToString("HH:mm:ss.fff")}");
                            WriteFile(streamWriter, $"[Action URL] {frame.ActionUrl}");
                            if (!string.IsNullOrEmpty(frame.ReplyDuration))
                            {
                                WriteFile(streamWriter, $"[ReplyDuration] {frame.ReplyDuration}");
                            }

                            WriteFile(streamWriter, $"[Size] {frame.Size}");
                            WriteFile(streamWriter, $"[CallStack]{Environment.NewLine}{frame.CallStack}");
                            WriteFile(streamWriter, Environment.NewLine + Environment.NewLine);
                        }
                    }
                }
            }
        }

        private static void WriteFile(TextWriter writer, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                writer.WriteLine(text);
            }
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
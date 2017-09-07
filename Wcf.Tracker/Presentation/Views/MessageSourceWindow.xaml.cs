using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace Wcf.Tracker.Presentation.Views
{
    /// <summary>
    /// Interaction logic for MessageSourceWindow.xaml
    /// </summary>
    // TODO MVVM
    public partial class MessageSourceWindow
    {
        /// <summary>
        /// Constructor for <see cref="MessageSourceWindow"/>.
        /// </summary>
        public MessageSourceWindow(string xml, string title)
        {
            Title = title;
            XmlText = xml;
            DataContext = this;
            InitializeComponent();
            BuildXmlView(xml);
        }

        /// <summary>
        /// Tree XML document.
        /// </summary>
        public XmlDataProvider XmlDataProvider { get; set; }

        /// <summary>
        /// XML source text.
        /// </summary>
        public string XmlText { get; set; }

        private void BuildXmlView(string xml)
        {
            // https://www.codeproject.com/Articles/71069/A-Simple-WPF-XML-Document-Viewer-Control
            var document = new XmlDocument();
            try
            {
                document.LoadXml(xml);
                XmlDataProvider = new XmlDataProvider { Document = document };

                Binding binding = new Binding
                {
                    Source = XmlDataProvider,
                    XPath = "child::node()"
                };
                TreeViewControl.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            }
            catch (XmlException)
            {
                // TODO show error
            }
        }

        private void OnCopyNodePress(object sender, RoutedEventArgs args)
        {
            var xmlText = TreeViewControl.SelectedItem as XmlText;
            if (xmlText != null)
            {
                Clipboard.SetText(xmlText.Value);
            }
        }
    }
}
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Wcf.Demonstration.WcfService;
using Wcf.Tracker;

namespace Wcf.Demonstration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // TODO MVVM
    public partial class MainWindow
    {
        private IService _service;

        private readonly Uri _hostUri = new Uri("net.tcp://127.0.0.1:60000");

        private int _sumValue = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            ValueBefore.Content = _sumValue;
            
            var serviceHost = new ServiceHost(typeof(Service));
            serviceHost.AddServiceEndpoint(typeof(IService), new NetTcpBinding(), _hostUri);
            serviceHost.Open();

            var factory = new ChannelFactory<IService>(new NetTcpBinding()).AttachTracker();
            _service = factory.CreateChannel(new EndpointAddress(_hostUri));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ValueBefore.Content = _sumValue;
            SummResult.Content = _sumValue = _service.Sum(_sumValue, 5);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.IsRepeat)
            {
                return;
            }

            var keyboard = args.KeyboardDevice;
            if ((keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Alt)) != ModifierKeys.None &&
                keyboard.IsKeyDown(Key.L))
            {
                LogService.ShowWindow();
            }
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            LogService.ShowWindow();
        }
    }
}
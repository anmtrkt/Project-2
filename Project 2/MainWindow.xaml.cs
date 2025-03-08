using Project_2.Connector;
using Project_2.HostHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



namespace Project_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataHandler _handler;
        private ipReader _ipReader;
        private portReader _portReader;
        private TcpClientPool _pool;
        private Connector.Connector _connector;
        private bool _sgn;
        private string fileName;
        
        public MainWindow()
        {
            InitializeComponent();

            _handler = new DataHandler();
            _pool = new TcpClientPool(_handler.getThreads());
            _connector = new Connector.Connector(_handler, _pool);
            _ipReader = new ipReader(_handler);
            _portReader = new portReader(_handler);
            _connector.ConnectionStatusChanged += OnConnectionStatusChanged;

        }


        private void openFileDialog(object sender, RoutedEventArgs e)
        {
 
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Choose file"; 
            dialog.DefaultExt = ".txt";  
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();
            openfiledialogButton.Content = dialog.FileName;
            fileName = dialog.FileName;
            _sgn = true;
        }

        private void readStringOrFile()
        {
            if (_sgn)
            {
                _ipReader.ReadFile(fileName);
            }
            else
            {
                _ipReader.ReadLine(ipTxtBox.Text);
            }
        }

        private void ipTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_sgn == true) 
            { 
                openfiledialogButton.Content = "Выбрать файл с айпи";
                _sgn = false; 
            };
        }

        private void StartButton(object sender, RoutedEventArgs e)
        {
            readStringOrFile();
            _portReader.ReadPorts(portTextBox.Text);
            _handler.setTimeout(short.Parse(TimeoutTextButton.Text));
            _handler.setThreads(short.Parse(ThreadsTextButton.Text));
            Task.Run(()=>_connector.FindMeSomething());
        }
        private void OnConnectionStatusChanged(string message)
        {
            Dispatcher.Invoke(new Action(() => { statusLstBox.Items.Add(message); }));

            

        }

        private void TimeoutTextButton_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

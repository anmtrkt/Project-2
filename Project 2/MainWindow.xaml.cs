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
        private Connector.Connector _connector;
        private bool _sgn;

        public MainWindow()
        {
            InitializeComponent();

            _handler = new DataHandler();
            _connector = new Connector.Connector(_handler);
            _ipReader = new ipReader(_handler);
            _portReader = new portReader(_handler);
            ipTxtBox.Text = "100.80.80.90";
            _connector.ConnectionStatusChanged += OnConnectionStatusChanged;

        }


        private void openFileDialog(object sender, RoutedEventArgs e)
        {
 
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Choose file"; 
            dialog.DefaultExt = ".txt";  
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();
            openfiledialogButton.Content = (_ipReader.ReadFile(dialog.FileName));
            _sgn = true;

        }



        private void ipTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _ipReader.ReadLine(ipTxtBox.Text);
            if (_sgn == true) 
            { 
                openfiledialogButton.Content = "Выбрать файл с айпи";
                _sgn = false; 
            };
        }

        private void StartButton(object sender, RoutedEventArgs e)
        {
            _portReader.ReadPorts(portTextBox.Text);
            _handler.setTimeout(short.Parse(TimeoutTextButton.Text));
            _connector.FindMeSomething();
        }
        private void OnConnectionStatusChanged(string message)
        {
            Dispatcher.Invoke(new Action(() => { statusLstBox.Items.Add(message); }));

            

        }

        
    }
}

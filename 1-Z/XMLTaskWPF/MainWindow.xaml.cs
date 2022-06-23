using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace XMLTaskWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IXmlWorker _worker;
        private readonly ILogger _logger;
        public bool IsFileOpened = false;
        private string _xmlFilePath;
        public MainWindow()
        {
            InitializeComponent();
            _logger = LoggerFactory.Create(builder => builder
                                    .SetMinimumLevel(LogLevel.Information))
                                    .CreateLogger<MainWindow>();
            _worker = new XmlDocumentWorker(_logger);
        }
       
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            PrintProducts(_worker.GetAll());
        }
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxDeleteProductName.Text)
                || !string.IsNullOrWhiteSpace(textBoxDeleteProductName.Text))
            {
                _worker.Delete(textBoxDeleteProductName.Text);
                PrintProducts(_worker.GetAll());
            }
        }
        private void PrintProduct(Product Product)
        {
            textBlockXMLFileContent.Text = "=====Product=====" + Environment.NewLine;
            textBlockXMLFileContent.Text += Product?.ToString() ?? "Product not found";

        }
        private void PrintProducts(List<Product> Products)
        {
            textBlockXMLFileContent.Text = "=====Products=====" + Environment.NewLine;
            foreach (var Product in Products)
            {
                textBlockXMLFileContent.Text += Product.ToString();
                textList.Text += Product.ToString();
                Item.Header += Product.ToString();
            }
        }
        private void TextBoxProductName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxProductName.Text))
            {
                PrintProducts(_worker.GetAll());
            }
        }
  

        private void buttonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetParent(AppContext.BaseDirectory)
                .Parent
                .Parent
                .FullName;
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Xml documents (.xml)|*.xml";
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                _xmlFilePath = dialog.FileName;
                textBlockXMLPathFile.Text = _xmlFilePath;
                
                _worker.Load(_xmlFilePath);
                PrintProducts(_worker.GetAll());
            }
        }

        private void buttonFindProductName_Click(object sender, RoutedEventArgs e)
        {
            var Product = _worker.FindBy(textBoxProductName.Text);
            PrintProduct(Product);
        }

        private void TextBlock_Checked(object sender, RoutedEventArgs e)
        {
            textBlockXMLFileContent.Visibility = Visibility.Visible;
            listBoxXMLFileContent.Visibility = Visibility.Collapsed;
            treeViewXMLFileContent.Visibility = Visibility.Collapsed;
        }

        private void TreeView_Checked(object sender, RoutedEventArgs e)
        {
            textBlockXMLFileContent.Visibility = Visibility.Collapsed;
            listBoxXMLFileContent.Visibility = Visibility.Collapsed;
            treeViewXMLFileContent.Visibility = Visibility.Visible;
        }

        private void ListBox_Checked(object sender, RoutedEventArgs e)
        {
            textBlockXMLFileContent.Visibility = Visibility.Collapsed;
            listBoxXMLFileContent.Visibility = Visibility.Visible;
            treeViewXMLFileContent.Visibility = Visibility.Collapsed;
        }
    }
}

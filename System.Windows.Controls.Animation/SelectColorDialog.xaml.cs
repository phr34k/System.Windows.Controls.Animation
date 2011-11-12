using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace System.Windows.Controls.Animation
{
    /// <summary>
    /// Interaction logic for SelectColorDialog.xaml
    /// </summary>
    internal partial class SelectColorDialog : Window
    {
        public SelectColorDialog()
        {
            InitializeComponent();
            Part_ColorListBox.ItemsSource = typeof(Colors).GetProperties();
        }

        private void Part_Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Part_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

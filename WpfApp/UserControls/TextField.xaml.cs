using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for TextField.xaml
    /// </summary>
    public partial class TextField : UserControl
    {
        /// <summary>
        /// Label value.
        /// </summary>
        public string Title { get; set; } = "";

        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                nameof(Text), typeof(string), typeof(TextField),
                new PropertyMetadata(default(string)));
        /// <summary>
        /// TextBox value.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public TextField()
        {
            InitializeComponent();
            this.DataContext = this;
        }

    }
}

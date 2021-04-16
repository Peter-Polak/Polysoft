using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.CustomControls
{

    public class MyButton : Control
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                nameof(Text), typeof(string), typeof(MyButton),
                new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        

        public MyButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(MyButton), new System.Windows.FrameworkPropertyMetadata(typeof(MyButton)));
        }
    }
}

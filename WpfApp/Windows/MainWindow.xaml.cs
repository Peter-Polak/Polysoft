using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
using WindowsInput;
using WindowsInput.Native;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InputSimulator sim;

        public MainWindow()
        {
            InitializeComponent();
            sim = new InputSimulator();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = false;
            //button.IsEnabled = false;
            fillFirsRow();

            //groupBox1.IsEnabled = false;
            //groupBox2.IsEnabled = true;
            //groupBox3.IsEnabled = true;
            button1.IsEnabled = true;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            button2.IsEnabled = false;
            //this.Dispatcher.Invoke(() =>
            //{
            //    insertNewCustomer();
            //});

            //button1.IsEnabled = false;
            insertNewCustomer();

            //groupBox1.IsEnabled = false;
            //groupBox2.IsEnabled = false;
            //groupBox3.IsEnabled = true;

            if(customerNumTextBox.Text.Length > 0) customerNumTextBox.Text = (Int32.Parse(customerNumTextBox.Text) + 1).ToString();
            button2.IsEnabled = true;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            button3.IsEnabled = false;
            fillForm();

            //groupBox1.IsEnabled = true;
            //groupBox2.IsEnabled = false;
            //groupBox3.IsEnabled = false;

            if(invoiceNumTextBox.Text.Length > 0) invoiceNumTextBox.Text = (Int32.Parse(invoiceNumTextBox.Text) + 1).ToString();

            customerTextBox.Clear();

            icoTextBox.Clear();
            dicTextBox.Clear();

            amountTextBox.Clear();
            button3.IsEnabled = true;
            Keyboard.Focus(customerNameTextBox);
        }

        private void fillFirsRow()
        {
            const int delay = 100;

            IntPtr calcWindow = FindWindow(null, "Formulár knihy vyšlých faktúr");
            SetForegroundWindow(calcWindow);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(yearTextBox.Text.Substring(2, 2) + invoiceNumTextBox.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(yearTextBox.Text + invoiceNumTextBox.Text); // Číslo faktúry

            sim.Keyboard.Sleep(delay + 100).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay + 1500).KeyPress(VirtualKeyCode.RIGHT); // Wait for window to open
            sim.Keyboard.Sleep(delay).ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.F7);

            sim.Keyboard.Sleep(delay).TextEntry(customerTextBox.Text); // Názov odberateľa

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        private void insertNewCustomer()
        {
            const int delay = 100;

            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.INSERT);

            if (customerNumTextBox.Text.Length < 4)
            {
                for (int i = 0; i < 4 - customerNumTextBox.Text.Length; i++)
                {
                    customerNumTextBox.Text = customerNumTextBox.Text.Insert(0, "0");
                }
            }

            sim.Keyboard.Sleep(delay + 500);
            //sim.Keyboard.Sleep(delay + 500).TextEntry(customerNumTextBox.Text); // Číslo odberateľa // DOESN'T WORK
            for (int i = 0; i < 4; i++)
            {
                sim.Keyboard.Sleep(delay).TextEntry(customerNumTextBox.Text[i]); // Číslo odberateľa
            }
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).TextEntry(customerTextBox.Text); // Názov odberateľa
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(stateComboBox.Text); // Štát
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            if (icoTextBox.Text.Length > 0) sim.Keyboard.Sleep(delay).TextEntry(icoTextBox.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // IČO

            if (icoTextBox.Text.Length > 0) sim.Keyboard.Sleep(delay).TextEntry(dicTextBox.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // DIČ

            if(dicTextBox.Text.Length > 0)
            {
                sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
                sim.Keyboard.Sleep(delay).TextEntry(icDphTextBox.Text);
            }
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // IČ DPH

            sim.Keyboard.Sleep(delay).TextEntry("3110001"); // ÚČet
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).TextEntry("VFA"); // Druh dokladu
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).TextEntry("00"); // Skupina odberateľov
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        private void fillForm()
        {
            const int delay = 100;

            //IntPtr calcWindow = FindWindow(null, "Formulár knihy vyšlých faktúr");
            //SetForegroundWindow(calcWindow);

            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB); // Extra enter, one enter gets "eaten" after Alt+Tab-ing
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay + 300).KeyPress(VirtualKeyCode.RETURN); // Wait for window to close
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            //sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // WARNING: If Cislo odberatela window was already opened once and closed, this is then NECESSARY

            sim.Keyboard.Sleep(delay).TextEntry(amountTextBox.Text); // Fakturovane eura
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            var dateOfPaymentText = DateTime.Parse(date1Picker.Text).ToString("dd.MM.yyyy");
            //var dueDateText = DateTime.Parse(dateOfPaymentText).AddDays(15).ToString("dd.MM.yyyy");
            var dueDateText = DateTime.Parse(date2Picker.Text).ToString("dd.MM.yyyy");

            sim.Keyboard.Sleep(delay).TextEntry(dateOfPaymentText); // Datum dodania/platby
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(dueDateText); // Datum splatnosti
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry("VYR"); // Druh faktury
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void customerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            customerNameTextBox.Text = (sender as TextBox).Text;
        }

        private void dateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            date2Picker.SelectedDate = DateTime.Parse(date1Picker.Text);
            date2Picker.IsEnabled = false;
        }

        private void dateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            date2Picker.SelectedDate = DateTime.Parse(date1Picker.Text).AddDays(15);
            date2Picker.IsEnabled = true;
        }

        private void date1Picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dateCheckBox.IsChecked ?? false)
            {
                date2Picker.SelectedDate = DateTime.Parse((sender as DatePicker).Text);
            }
            else
            {
                date2Picker.SelectedDate = DateTime.Parse((sender as DatePicker).Text).AddDays(15);
            }
        }

        private void dicTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string icDph = "SK" + (sender as TextBox).Text;
            icDphTextBox.Text = icDph;
        }
    }
}

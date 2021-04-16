using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp.User_Controls;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InputSimulator sim;

        public ObservableCollection<string> States { get; set; }
        public string SelectedState { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            sim = new InputSimulator();
            States = new ObservableCollection<string> { "Slovenská republika", "Česká republika", "Maďarsko", "Rakúsko", "Poľsko", "USA" };
            SelectedState = States[0];
            year.Text = "2021";
            dic.textBox.TextChanged += dic_TextChanged;

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private void fillFirstRowButton_Click(object sender, RoutedEventArgs e)
        {
            fillFirsRow();
        }

        private void insertNewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            //button2.IsEnabled = false;
            //this.Dispatcher.Invoke(() =>
            //{
            //    insertNewCustomer();
            //});

            insertNewCustomer();
        }

        private void fillFormButton_Click(object sender, RoutedEventArgs e)
        {
            fillForm();

            if (customerNumber.Text.Length > 0) customerNumber.textBox.Text = (Int32.Parse(customerNumber.textBox.Text) + 1).ToString();
            if (invoiceNumber.Text.Length > 0) invoiceNumber.textBox.Text = (Int32.Parse(invoiceNumber.textBox.Text) + 1).ToString();

            customerName.textBox.Text = "";

            ico.textBox.Text = "";
            dic.textBox.Text = "";

            amount.textBox.Text = "";
        }

        private void fillFirsRow()
        {
            const int delay = 100;

            IntPtr calcWindow = FindWindow(null, "Formulár knihy vyšlých faktúr");
            SetForegroundWindow(calcWindow);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(year.Text.Substring(2, 2) + invoiceNumber.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(year.Text + invoiceNumber.Text); // Číslo faktúry

            sim.Keyboard.Sleep(delay + 100).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay + 1500).KeyPress(VirtualKeyCode.RIGHT); // Wait for window to open
            sim.Keyboard.Sleep(delay).ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.F7);

            sim.Keyboard.Sleep(delay).TextEntry(customerName.Text); // Názov odberateľa

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        private void insertNewCustomer()
        {
            const int delay = 100;

            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.INSERT);

            if (customerNumber.Text.Length < 4)
            {
                for (int i = 0; i < 4 - customerNumber.Text.Length; i++)
                {
                    customerNumber.Text = customerNumber.Text.Insert(0, "0");
                }
            }

            sim.Keyboard.Sleep(delay + 500);
            //sim.Keyboard.Sleep(delay + 500).TextEntry(customerNumber.Text); // Číslo odberateľa // DOESN'T WORK
            for (int i = 0; i < 4; i++)
            {
                sim.Keyboard.Sleep(delay).TextEntry(customerNumber.Text[i]); // Číslo odberateľa
            }
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).TextEntry(customerName.Text); // Názov odberateľa
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(state.Text); // Štát
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            if (ico.Text.Length > 0) sim.Keyboard.Sleep(delay).TextEntry(ico.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // IČO

            if (ico.Text.Length > 0) sim.Keyboard.Sleep(delay).TextEntry(dic.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // DIČ

            if(dic.Text.Length > 0)
            {
                sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
                sim.Keyboard.Sleep(delay).TextEntry(icDph.Text);
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

            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB); // Extra enter, one enter gets "eaten" after Alt+Tab-ing
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay + 300).KeyPress(VirtualKeyCode.RETURN); // Wait for window to close
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            //sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // WARNING: If Cislo odberatela window was already opened once and closed, this is then NECESSARY

            sim.Keyboard.Sleep(delay).TextEntry(amount.Text); // Fakturovane eura
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            var dateOfPaymentText = DateTime.Parse(date1Picker.Text).ToString("dd.MM.yyyy");
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

        private void dic_TextChanged(object sender, TextChangedEventArgs e)
        {
            string icDphEdited = "SK" + (sender as TextBox).Text;
            icDph.textBox.Text = icDphEdited;
        }
    }
}

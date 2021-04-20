using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
using WpfApp.Base;
using WpfApp.Models;

namespace WpfApp.ViewModel
{
    class InvoiceViewModel : ObservableObject
    {
        #region DLL Imports

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region Private fields

        /// <summary>
        /// Keyboard and mouse simularator.
        /// </summary>
        private InputSimulator _inpSim;

        #endregion

        #region Properties

        /// <summary>
        /// Invoice object that the form is binding to.
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Predefined states to choose from.
        /// </summary>
        public ObservableCollection<string> States { get; set; }

        /// <summary>
        /// Currently selected state.
        /// </summary>
        public string SelectedState { get; set; }

        #endregion

        #region Commands
        
        public ICommand FillFirstRowCommand { get; set; }
        public ICommand FillFormCommand { get; set; }
        public ICommand InsertNewCustomerCommand { get; set; }

        #endregion

        #region Constructor

        public InvoiceViewModel()
        {
            _inpSim = new InputSimulator();
            Invoice = new Invoice();
            States = new ObservableCollection<string>();
            foreach (var state in Properties.Settings.Default.States) States.Add(state);
            SelectedState = States[0];

            FillFirstRowCommand = new RelayCommand(FillFirsRow);
            FillFormCommand = new RelayCommand(FillForm);
            InsertNewCustomerCommand = new RelayCommand(InsertNewCustomer);
        }

        #endregion

        #region Private Methods

        // <summary>
        // Fill the first row of invoice(invoice ID), open customer database window and look up customer.
        // </summary>
        private void FillFirsRow()
        {
            const int delay = 100;

            // Switch window to invoice form (window must be open)
            IntPtr invoiceWindow = FindWindow(null, "Formulár knihy vyšlých faktúr");
            SetForegroundWindow(invoiceWindow);

            // Fill invoice indexes
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Year.ToString().Substring(2, 2) + Invoice.Number);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Year.ToString() + Invoice.Number); // Číslo faktúry
            _inpSim.Keyboard.Sleep(delay + 100).KeyPress(VirtualKeyCode.RETURN);

            // Open customer database window and search window
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay + 1500).KeyPress(VirtualKeyCode.RIGHT); // Wait for window to open
            _inpSim.Keyboard.Sleep(delay).ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.F7);

            // Search for customer name in database of current customers
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Name); // Názov odberateľa
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        /// <summary>
        /// Insert new customer to the database. Must have customer database window open and it must be the last window in focus.
        /// </summary>
        private void InsertNewCustomer()
        {
            const int delay = 100;

            // Switch back to the customer database window
            _inpSim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            // Open window for adding new customers to the database
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB); // Needed only if the customer database window was closed during addition of this invoice
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.INSERT);

            // Format customer ID to have 4 digit (fill with zeros at the beggining until it is 4 digit long)
            if (Invoice.Customer.Number.Length < 4)
            {
                for (int i = 0; i < 4 - Invoice.Customer.Number.Length; i++)
                {
                    Invoice.Customer.Number = Invoice.Customer.Number.Insert(0, "0");
                }
            }

            _inpSim.Keyboard.Sleep(delay + 500); // Wait for window to open

            // Fill customer ID
            //_inpSim.Keyboard.Sleep(delay + 500).TextEntry(customerNumber.Text); // Číslo odberateľa // DOESN'T WORK, have to use the loop to input the ID
            for (int i = 0; i < 4; i++)
            {
                _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Number[i]); // Číslo odberateľa
            }
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill customer name
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Name); // Názov odberateľa
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip fields we don't need to fill
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill state
            _inpSim.Keyboard.Sleep(delay).TextEntry(SelectedState); // Štát
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill IČO if needed
            if (Invoice.Customer.Ico.Length > 0) _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Ico);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // IČO

            // Fill DIČ if needed
            if (Invoice.Customer.Dic.Length > 0) _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Dic);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // DIČ

            // Fill IČ DPH if needed
            if (Invoice.Customer.Dic.Length > 0)
            {
                _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
                _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.IcDph);
            }
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // IČ DPH

            // Fill account constant
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Account); // ÚČet
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill type of document constant
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Type); // Druh dokladu
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill group of customer constant
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Customer.Group); // Skupina odberateľov
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        /// <summary>
        /// Fill the rest of the invoice. Must have customer database window open and it must be the last window in focus.
        /// </summary>
        private void FillForm()
        {
            const int delay = 100;

            // Switch back to the customer database window
            _inpSim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            // Choose customer
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB); // Extra enter, one enter gets "eaten" after Alt+Tab-ing
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip unnecessary fields
            _inpSim.Keyboard.Sleep(delay + 300).KeyPress(VirtualKeyCode.RETURN); // Wait for window to close
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            //_inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // WARNING: If Cislo odberatela window was already opened once and closed, this is then NECESSARY

            // Fill invoice amount
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Amount.ToString()); // Fakturovane eura
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip unnecessary fields
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Format dates
            var dateOfPaymentText = Invoice.DateOfPayment.ToString("dd.MM.yyyy");
            var dueDateText = Invoice.DueDate.ToString("dd.MM.yyyy");

            // Fill dates
            _inpSim.Keyboard.Sleep(delay).TextEntry(dateOfPaymentText); // Datum dodania/platby
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            _inpSim.Keyboard.Sleep(delay).TextEntry(dueDateText); // Datum splatnosti
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip unnecessary fields
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill invoice type
            _inpSim.Keyboard.Sleep(delay).TextEntry(Invoice.Type); // Druh faktury
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip unnecessary fields
            _inpSim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        #endregion
    }
}

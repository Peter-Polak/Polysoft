using System.ComponentModel;

namespace WpfApp.Models
{
    public class Customer : ObservableObject
    {
        #region Private backing fields

        private string _isDph;

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Number { get; set; } = Properties.Settings.Default.CustomerNumber;
        public string Ico { get; set; }
        public string Dic { get; set; }
        public string IcDph
        {
            get { return _isDph; }
            set { _isDph = "SK" + value; }
        }
        public string Account { get; set; } = "3110001";
        public string Type { get; set; } = "VFA";
        public string Group { get; set; } = "00";

        #endregion
    }
}
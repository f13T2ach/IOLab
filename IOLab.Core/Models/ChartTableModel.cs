using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Models
{
    public class ChartTableModel
    {
        private bool _isSelected;
        public int Id { get; set; }

        public string XRow { get; set; }
        public string YRow { get; set; }

        public string PortId { get; set; }

        public MonitorService ChartSevice { get; set; }

        public bool IsSelected {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                }
            }
        }

        /**
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        **/
    }


}

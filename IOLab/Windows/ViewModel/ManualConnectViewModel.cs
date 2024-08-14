using IOLab.Core.Serial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IOLab.Windows.ViewModel
{
    public class ManualConnectViewModel
    {
        private string[] _comIDArray;

        private ObservableCollection<string> _comIDCollection;

        public static bool isConfirmed = false;


        private int _indexOfSelection;
        public int IndexOfSelection
        {
            get { 
                return _indexOfSelection;
            }
            set { 
                _indexOfSelection = value;
                MyDataContainer.selectedItemIndex = value;
            }
        }

        public ObservableCollection<string> ComIDCollection
        {
            get
            {
                return _comIDCollection;
            }
            set
            {
                // 
            }
        }

        public ManualConnectViewModel()
        {
            _comIDCollection = GetComIDCollection();
            MyDataContainer.comArray = _comIDArray;
            _indexOfSelection = -1;
        }

        private ObservableCollection<string> GetComIDCollection()
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();
            Scanner scanner = new Scanner();
            _comIDArray = scanner.ScanAllSerial();
            foreach ( string item in _comIDArray)  {
                collection.Add(item);
            }
            return collection;
        }
    }
}

using IOLab.Core.Models.Records;
using IOLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IOLab.Pages.ViewModel
{
    internal class HomePageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Schema> RecordsSchema { get; set; }
        public ObservableCollection<IOExperiment> IOExperiments { get; set; }

        public Visibility CanShowNoRecordText { get => RecordsSchema.Count==0?Visibility.Visible:Visibility.Hidden; protected set { } }
        public HomePageViewModel() {
            // 读入实验
            RecordsSchema = new ObservableCollection<Schema>();
            foreach(Schema item in Bundles.LocalRecords)
            {
                RecordsSchema.Add(item);
            }

            IOExperiments = new ObservableCollection<IOExperiment>();
            foreach (IOExperiment item in Bundles.LocalExperiments)
            {
                IOExperiments.Add(item);
            }
        }

        // 统计数据

        public string LocalExperimentCount{ get => Bundles.LocalExperiments.Count > 99 ? "99" : Bundles.LocalExperiments.Count.ToString(); set { } }
        public string LocalRecordCount { get => Bundles.LocalRecords.Count > 99 ? "99" : Bundles.LocalRecords.Count.ToString(); set { } }
        public string LocalServicesCount { get => Bundles.LocalMonitorService.Count > 99 ? "99" : Bundles.LocalMonitorService.Count.ToString(); set { } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

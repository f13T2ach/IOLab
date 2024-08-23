/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ManualConnectViewModel.cs is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

using IOLab.Core.Serial;
using System.Collections.ObjectModel;



namespace IOLab.Windows.Dialog.ViewModel
{
    public class ManualConnectViewModel
    {
        private string[] _comIDArray;

        private ObservableCollection<string> _comIDCollection;

        public static bool isConfirmed = false;


        private int _indexOfSelection;
        public int IndexOfSelection
        {
            get
            {
                return _indexOfSelection;
            }
            set
            {
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
            foreach (string item in _comIDArray)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}

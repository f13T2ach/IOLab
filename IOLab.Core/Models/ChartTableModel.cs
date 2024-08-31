/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ChartTableModel.cs is part of the IOLab project.
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

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace IOLab.Core.Models
{
    public class ChartTableModel
    {
        private bool _isSelected;
        public int Id { get; set; }

        public string XRow { get; set; }
        public string YRow { get; set; }

        private string _name;
        public string Name
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private Visibility _freezeIconVisibility = Visibility.Collapsed;
        public Visibility FreezeIconVisibility
        {
            get => _freezeIconVisibility;
            set {
                _freezeIconVisibility = value;
                OnPropertyChanged(nameof(FreezeIconVisibility));
            }
        }

        public string PortId { get; set; }

        private int _xViewField;

        // X轴视野
        public int XViewField
        {
            get => _xViewField;
            set
            {
                int delta = value - _xViewField;
                _xViewField = value;
                XAxes.FirstOrDefault().MaxLimit += delta;
                OnPropertyChanged(nameof(XAxes));

            }
        }
        public MonitorService ChartServicer { get; set; }
        public ObservableCollection<Axis> XAxes { get; set; }
        public ObservableCollection<Axis> YAxes { get; set; }

        private bool _isFrozen = false;
        // TODO 表在冻结状态时的销毁
        // 表是不能被复活的 因为复活可能会对冻结后产生的表冲突
        public bool isFrozen { get => _isFrozen; set
            {
                _isFrozen = value;
                FreezeIconVisibility = isFrozen ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(isFrozen));
            }
        }

        private bool _isPause = false;
        public bool isPause { get => _isPause; set
            {
                _isPause = value;
                // 暂停不会停止监听
                OnPropertyChanged(nameof(isPause));
            }
        }
        private ISeries[] _series;
        public ISeries[] Series
        {
            get => _series;
            set
            {
                _series = value;

                OnPropertyChanged(nameof(Series));
            }
        }

        // 拖动事件
        public bool isDragged = false;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                }
            }
        }

        public ChartTableModel(string yrow, string name, string hex)
        {
            Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double> { 2, 1, 3, 5, 3, 4, 6 },
                    Name = yrow
                }
            };

            XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Name = name,
                    MinLimit = 0,
                    MaxLimit = 20
                }
            };

            _xViewField = 20;

            YAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Name = yrow,
                    MinLimit = 0,
                    MaxLimit = 10
                }
            };

            YRow = yrow;
            Name = name;

        }
        /// <summary>
        /// 添加一个点
        /// </summary>
        /// <param name="point">值</param>
        public void AddDataPoint(double point)
        {
            // Add a new data point to the LineSeries
            if (Series[0] is LineSeries<double> lineSeries&& !isFrozen)
            {
                if (lineSeries.Values is ObservableCollection<double> collection)
                {
                    collection.Add(point);

                    // 追随最新的点
                    if (!isDragged)
                    {
                        AdjustToLatestPoint();
                    }
                    YAxes = new ObservableCollection<Axis>
                    {
                        new Axis
                        {
                            Name = YRow,
                            MinLimit = 0,
                            MaxLimit = collection.Max()
                        }
                    };
                    OnPropertyChanged(nameof(Series));
                }

            }
        }

        /// <summary>
        /// 令表跟随第一个点
        /// </summary>
        public void AdjustToLatestPoint()
        {
            if (Series[0] is LineSeries<double> lineSeries && lineSeries.Values is ObservableCollection<double> collection && collection.Count > 0)
            {
                var latestIndex = collection.Count - 1;
                var latestValue = collection[latestIndex];

                var xAxis = XAxes.FirstOrDefault();
                //var yAxis = YAxes.FirstOrDefault();

                if (xAxis != null)
                {
                    xAxis.MaxLimit = latestIndex + 1;
                    xAxis.MinLimit = Math.Max(latestIndex - XViewField, 0); // Show a window of 10 points before the latest point
                    //yAxis.MaxLimit = latestValue + 1;
                    //yAxis.MinLimit = latestValue - 1;

                    OnPropertyChanged(nameof(XAxes));
                    OnPropertyChanged(nameof(YAxes));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

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

using IOLab.Core.Assets;
using IOLab.Core.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace IOLab.Core.Models
{
    public class ChartTableModel : INotifyPropertyChanged
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
            set
            {
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
        public bool isFrozen
        {
            get => _isFrozen; set
            {
                _isFrozen = value;
                FreezeIconVisibility = isFrozen ? Visibility.Visible : Visibility.Collapsed;
                // 取消占用
                if(InstanceBundle.connectTool != null)
                {
                    // 发送指令
                    InstanceBundle.connectTool._SerialPort.WriteLine(CommandsV1.Disable + PortId);
                }
                OnPropertyChanged(nameof(isFrozen));
            }
        }

        private bool _isPause = false;
        public bool isPause
        {
            get => _isPause; set
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
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public ChartTableModel(string yrow, string name, string hex)
        {
            Series = new ISeries[]
            {
                new LineSeries<float>
                {
                    Values = new ObservableCollection<float>{0},
                    Name = yrow
                }
            };

            XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    //Name = name,
                }
            };

            _xViewField = 20;

            YAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Name = yrow,
                }
            };

            YRow = yrow;
            Name = name;

        }
        /// <summary>
        /// 添加一个点
        /// </summary>
        /// <param name="point">值</param>
        public void AddDataPoint(float point)
        {
            // Add a new data point to the LineSeries
            if (Series[0] is LineSeries<float> lineSeries && !isFrozen)
            {
                if (lineSeries.Values is ObservableCollection<float> collection)
                {
                    collection.Add(point);

                    // 追随最新的点
                    if (!isDragged)
                    {
                        AdjustToLatestPoint();
                    }

                    // 动态调整 Y 轴范围，添加缓冲区
                    float minValue = collection.Min();
                    float maxValue = collection.Max();
                    float buffer = (maxValue - minValue) * 0.1f; // 添加 10% 缓冲区

                    YAxes = new ObservableCollection<Axis>
                    {
                        new Axis
                        {
                            Name = YRow,
                            MinLimit = minValue - buffer, // 调整最小值，留出缓冲空间
                            MaxLimit = maxValue + buffer  // 调整最大值，留出缓冲空间
                        }
                    };


                    OnPropertyChanged(nameof(YAxes));
                    OnPropertyChanged(nameof(Series));
                }
            }
        }


        /// <summary>
        /// 令表跟随第一个点
        /// </summary>
        public void AdjustToLatestPoint()
        {
            if (Series[0] is LineSeries<float> lineSeries && lineSeries.Values is ObservableCollection<float> collection && collection.Count > 0)
            {
                var latestIndex = collection.Count - 1;
                var latestValue = collection[latestIndex];

                var xAxis = XAxes.FirstOrDefault();
                //var yAxis = YAxes.FirstOrDefault();

                if (xAxis != null)
                {
                    xAxis.MaxLimit = latestIndex + 1;
                    xAxis.MinLimit = Math.Max(latestIndex - XViewField, 0); // Show a window of 10 points before the latest point
                    // 全局最大值
                    //yAxis.MaxLimit = 0;
                    //yAxis.MinLimit = 0;

                    OnPropertyChanged(nameof(XAxes));
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

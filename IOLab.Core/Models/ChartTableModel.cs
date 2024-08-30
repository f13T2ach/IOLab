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

namespace IOLab.Core.Models
{
    public class ChartTableModel
    {
        private bool _isSelected;
        public int Id { get; set; }

        public string XRow { get; set; }
        public string YRow { get; set; }

        public string Name { get; set; }

        public string PortId { get; set; }

        public MonitorService ChartServicer { get; set; }
        public ObservableCollection<Axis> XAxes { get; set; }
        public ObservableCollection<Axis> YAxes { get; set; }

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
                    Name = yrow,
                }
            };

            XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Name = name,
                    MinLimit = 0,
                    MaxLimit = 10
                }
            };

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

        public void AddDataPoint(double point)
        {
            // Add a new data point to the LineSeries
            if (Series[0] is LineSeries<double> lineSeries)
            {
                if (lineSeries.Values is ObservableCollection<double> collection)
                {
                    collection.Add(point);
                    if (!isDragged)
                    {
                        AdjustToLatestPoint();
                    }
                    OnPropertyChanged(nameof(Series));
                }

            }
        }

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
                    xAxis.MinLimit = Math.Max(latestIndex - 10, 0); // Show a window of 10 points before the latest point
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

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
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

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

        public MonitorService ChartSevice { get; set; }

        public ISeries[] Series { get; set; }
        public LabelVisual Title { get; set; }

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
                    Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                    Name = yrow,

                }
            };


            Title = new LabelVisual
            {
                Text = name,
                TextSize = 20,
                Paint = new SolidColorPaint
                {
                    Color = SKColor.Parse(hex),
                    SKTypeface = SKFontManager.Default.MatchCharacter('汉')
                },
            };

            YRow = yrow;
            Name = name;
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

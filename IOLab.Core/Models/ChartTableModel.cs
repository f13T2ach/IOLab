using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Microsoft.Windows.Themes;
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
                    Name = yrow
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

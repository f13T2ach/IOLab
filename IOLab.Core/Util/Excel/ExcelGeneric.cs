using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace IOLab.Core.Util.Excel
{
    public class ExcelGeneric
    {
        /// <summary>
        /// 生成包含多个数据集的Excel文件（每个数据集独立Sheet）
        /// </summary>
        /// <param name="datasets">数据集配置列表</param>
        /// <param name="filePath">输出文件路径</param>
        public static void GenerateMultiSheetReport(
            List<DatasetConfig> datasets,
            string filePath)
        {
            ValidateDatasets(datasets);

            ExcelPackage.License.SetNonCommercialPersonal("IOLab");//个人

            using (var package = new ExcelPackage())
            {
                foreach (var config in datasets)
                {
                    // 创建带唯一名称的工作表
                    var worksheet = CreateWorksheet(package, config.SheetName);

                    // 写入数据
                    WriteDataset(worksheet, config);

                    // 创建图表
                    CreateChart(worksheet, config);
                }

                package.SaveAs(new FileInfo(filePath));
            }
        }

        #region 私有方法
        private static void ValidateDatasets(List<DatasetConfig> datasets)
        {
            if (datasets == null || datasets.Count == 0)
                throw new ArgumentException("至少需要提供一个数据集");

            var sheetNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var config in datasets)
            {
                if (config.XData.Count != config.YData.Count)
                    throw new ArgumentException($"数据集 '{config.SheetName}' 的X/Y数据量不一致");

                if (!sheetNames.Add(config.SheetName))
                    throw new ArgumentException($"工作表名称重复: {config.SheetName}");
            }
        }

        private static ExcelWorksheet CreateWorksheet(ExcelPackage package, string sheetName)
        {
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // 设置通用表头样式  
            worksheet.Cells["A1"].Value = "X轴";
            worksheet.Cells["B1"].Value = "Y轴";
            worksheet.Cells["A1:B1"].Style.Font.Bold = true;

            // 修复背景色设置问题，先设置 PatternType  
            worksheet.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            return worksheet;
        }

        private static void WriteDataset(ExcelWorksheet ws, DatasetConfig config)
        {
            // 批量写入数据
            for (int i = 0; i < config.XData.Count; i++)
            {
                int row = i + 2; // 从第2行开始
                ws.Cells[row, 1].Value = config.XData[i];
                ws.Cells[row, 2].Value = config.YData[i];
            }

            // 自动调整列宽
            ws.Column(1).AutoFit();
            ws.Column(2).AutoFit();
        }

        private static void CreateChart(ExcelWorksheet ws, DatasetConfig config)
        {
            var chart = ws.Drawings.AddChart(config.ChartTitle, eChartType.Line);
            chart.SetPosition(1, 0, 3, 0); // 定位到D列
            chart.SetSize(600, 400);

            // 动态数据范围
            int lastRow = config.XData.Count + 1;
            var xRange = ExcelRange.GetAddress(2, 1, lastRow, 1);
            var yRange = ExcelRange.GetAddress(2, 2, lastRow, 2);

            var series = chart.Series.Add(yRange, xRange);
            series.Header = config.YAxisTitle;

            // 图表样式
            chart.Title.Text = config.ChartTitle;
            chart.XAxis.Title.Text = config.XAxisTitle;
            chart.YAxis.Title.Text = config.YAxisTitle;
            chart.Legend.Position = eLegendPosition.Right;
        }
        #endregion
    }

    /// <summary>
    /// 数据集配置类
    /// </summary>
    public class DatasetConfig
    {
        /// <summary>工作表名称（必须唯一）</summary>
        public string SheetName { get; set; }

        /// <summary>X轴数据列表</summary>
        public List<object> XData { get; set; }

        /// <summary>Y轴数据列表</summary>
        public List<object> YData { get; set; }

        /// <summary>X轴标题</summary>
        public string XAxisTitle { get; set; } = "X Axis";

        /// <summary>Y轴标题</summary>
        public string YAxisTitle { get; set; } = "Y Axis";

        /// <summary>图表主标题</summary>
        public string ChartTitle { get; set; } = "Data Analysis";

    }
}
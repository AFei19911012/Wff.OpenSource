using MiniExcelLibs;
using System.Data;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using MiniExcelLibs.OpenXml;

namespace Wff.OpenSource.Views
{
    /// <summary>
    /// Demo_MiniExcel.xaml 的交互逻辑
    /// </summary>
    public partial class Demo_MiniExcel : UserControl
    {
        public Demo_MiniExcel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"samples\xlsx\TestTypeMapping.xlsx";
            var rows1 = MiniExcel.Query<UserAccount>(filename);

            using var stream = File.OpenRead(filename);
            var rows2 = stream.Query<UserAccount>();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            string filename = @"samples\test001.xlsx";
            var rows = MiniExcel.Query(filename).ToList();
            var cell11 = rows[0].A;
            var cell12 = rows[0].B;
            var cell21 = rows[1].A;
            var cell22 = rows[1].B;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            string filename = @"samples\test002.xlsx";
            var rows = MiniExcel.Query(filename, useHeaderRow:true).ToList();
            var cell11 = rows[0].Name;
            var cell12 = rows[0].Value;
            var cell21 = rows[1].Name;
            var cell22 = rows[1].Value;
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            // 默认样式
            var config = new OpenXmlConfiguration()
            {
                TableStyles = TableStyles.Default,
            };

            string filename = $"samples\\test003.xlsx";
            var values = new List<Dictionary<string, object>>()
            {
                new Dictionary<string,object>{{ "Column1", "MiniExcel" }, { "Column2", 1 } },
                new Dictionary<string,object>{{ "Column1", "Github" }, { "Column2", 2 } }
            };
            MiniExcel.SaveAs(filename, values, configuration:config, overwriteFile: true);


            var values2 = Enumerable.Range(1, 1000000).Select((s, index) => new { index, value = Guid.NewGuid() });
            filename = $"samples\\test004.xlsx";
            using var stream = File.Create(filename);
            stream.SaveAs(values2, configuration:config);
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            var filename = $"samples\\test005.xlsx";
            var users = new[] { new { Name = "Jack", Age = 25 }, new { Name = "Mike", Age = 44 } };
            var department = new[] { new { ID = "01", Name = "HR" }, new { ID = "02", Name = "IT" } };
            var sheets = new Dictionary<string, object>
            {
                ["users"] = users,
                ["department"] = department
            };
            MiniExcel.SaveAs(filename, sheets);
        }
    }
}

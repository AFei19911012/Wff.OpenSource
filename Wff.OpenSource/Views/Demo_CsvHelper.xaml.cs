using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wff.OpenSource.Views
{
    /// <summary>
    /// Demo_CsvHelper.xaml 的交互逻辑
    /// </summary>
    public partial class Demo_CsvHelper : UserControl
    {
        public Demo_CsvHelper()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 写一个 csv 文件
            if (true)
            {
                var records = new List<Foo>
                {
                    new Foo { Id = 1, Name = "one" },
                    new Foo { Id = 2, Name = "two" },
                };
                using var writer = new StreamWriter("file.csv");
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(records);
            }

            if (true)
            {
                var records = new List<Foo>
                {
                    new Foo { Id = 1, Name = "one" },
                    new Foo { Id = 2, Name = "two" },
                };

                using var writer = new StreamWriter("file.csv");
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteHeader<Foo>();
                csv.NextRecord();
                foreach (var record in records)
                {
                    csv.WriteRecord(record);
                    csv.NextRecord();
                }
            }

            if (true)
            {
                var records = new List<Foo>
                {
                    new Foo { Id = 3, Name = "three" },
                    new Foo { Id = 4, Name = "four" },
                };

                // Append to the file.
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Don't write the header again.
                    HasHeaderRecord = false,
                };
                using var stream = File.Open("file.csv", FileMode.Append);
                using var writer = new StreamWriter(stream);
                using var csv = new CsvWriter(writer, config);
                csv.WriteRecords(records);
            }

            // 读取 csv 文件
            lb.Items.Clear();
            if (true)
            {
                using var reader = new StreamReader("file.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<Foo>();
                foreach (var record in records)
                {
                    lb.Items.Add($"{record.Id} - {record.Name}");
                }
                lb.Items.Add(Environment.NewLine);
            }

            if (true)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                };
                using var reader = new StreamReader("file.csv");
                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<Foo>();
                foreach (var record in records)
                {
                    lb.Items.Add($"{record.Id} - {record.Name}");
                }
                lb.Items.Add(Environment.NewLine);
            }

            if (true)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                };
                using var reader = new StreamReader("file.csv");
                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<Foo>();
                foreach (var record in records)
                {
                    lb.Items.Add($"{record.Id} - {record.Name}");
                }
                lb.Items.Add(Environment.NewLine);
            }

            if (true)
            {
                using var reader = new StreamReader("file.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Context.RegisterClassMap<FooMap>();
                var records = csv.GetRecords<Foo>();
                foreach (var record in records)
                {
                    lb.Items.Add($"{record.Id} - {record.Name}");
                }
                lb.Items.Add(Environment.NewLine);
            }
        }
    }

    public class Foo
    {
        //[Index(0)]
        [Name("id")]
        public int Id { get; set; }

        //[Index(1)]
        [Name("name")]
        public string Name { get; set; }
    }

    public class FooMap : ClassMap<Foo>
    {
        public FooMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("name");
        }
    }
}
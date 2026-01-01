using SoftCircuits.EasyEncryption;
using SoftCircuits.ExpressionEvaluator;
using SoftCircuits.FixedWidthParser;
using SoftCircuits.OrderedDictionary;
using SoftCircuits.Parsing.Helper;
using SoftCircuits.WinSettings;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Wff.OpenSource.Views
{
    /// <summary>
    /// Demo_SoftCircuits.xaml 的交互逻辑
    /// </summary>
    public partial class Demo_SoftCircuits : UserControl
    {
        public Demo_SoftCircuits()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string txt = "The quick brown fox jumps over the lazy dog.";
            Debug.WriteLine(txt);
            Debug.WriteLine("ParsingHelper helper = new ParsingHelper(txt)");
            ParsingHelper helper = new ParsingHelper(txt);
            Debug.WriteLine($"helper.Peek(): {helper.Peek()}");
            Debug.WriteLine($"helper.Get(): {helper.Get()}");
            Debug.WriteLine($"helper.Get(): {helper.Get()}");

            // 回到起始点
            helper.Reset();

            // 整个文本
            Debug.WriteLine($"helper.Text: {helper.Text}");
            // 下标
            Debug.WriteLine($"helper.Index: {helper.Index}");
            // 是否文末
            Debug.WriteLine($"helper.EndOfText: {helper.EndOfText}");
            // 剩余文本长度
            Debug.WriteLine($"helper.Remaining: {helper.Remaining}");

            // 打印字符
            Debug.WriteLine("");
            Debug.WriteLine("逐个打印字符");
            while (!helper.EndOfText)
            {
                Debug.WriteLine(helper.Peek());
                helper++;
                // helper.Next();
            }

            // 移动位置到下一个不在指定条件里的字符
            txt = "abcdefg123hij";
            Debug.WriteLine("");
            Debug.WriteLine("移动到下一个非数字位置");
            Debug.WriteLine(txt);
            helper = new ParsingHelper(txt);
            helper.Skip('1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
            Debug.WriteLine(helper.Get());

            // 移动位置到不满足条件的字符
            helper.Reset();
            helper.SkipWhile(c => c != '3');
            Debug.WriteLine(helper.Get());

            // 移动位置到指定字符
            helper.Reset();
            helper.SkipTo("123");
            Debug.WriteLine(helper.Get());

            // 分割字符串
            helper = new ParsingHelper("The quick brown fox jumps over the lazy dog.");
            List<string> words = helper.ParseTokens(' ', '.').ToList();
            foreach (var item in words)
            {
                Debug.WriteLine(item);
            }

            // 正则表达式
            helper = new ParsingHelper("Jim Jack Sally Jennifer Bob Gary Jonathan Bill");
            IEnumerable<string> results = helper.ParseTokensRegEx(@"\b[J]\w+");
            foreach (var item in results)
            {
                Debug.WriteLine($"{item}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SoftCircuits.OrderedDictionary.OrderedDictionary<int, string> dictionary = new()
            {
                [101] = "Bob Smith",
                [127] = "Gary Wilson",
                [134] = "Ann Carpenter",
                [187] = "Bill Jackson",
                [214] = "Cheryl Hansen",
            };

            Debug.WriteLine("初始化字典");
            foreach (var item in dictionary)
            {
                Debug.WriteLine($"{item.Key} - {item.Value}");
            }

            Debug.WriteLine("");
            Debug.WriteLine("通过序号访问");
            Debug.WriteLine($"dictionary.ByIndex[3]: {dictionary.ByIndex[3]}");

            // 添加键值对
            dictionary = new();
            dictionary.Add(101, "Bob Smith");
            dictionary.Add(127, "Gary Wilson");
            dictionary.Add(187, "Bill Jackson");
            dictionary.Add(214, "Cheryl Hansen");
            dictionary.Insert(2, 134, "Ann Carpenter");

            // 移除
            dictionary.Remove(134); // Removes 134 - Add Carpenter
            dictionary.RemoveAt(2); // Removes 187 - Bill Jackson

            Debug.WriteLine("dictionary.Remove(134)");
            Debug.WriteLine("dictionary.RemoveAt(2)");

            Debug.WriteLine("");
            Debug.WriteLine("当前字典");
            foreach (var item in dictionary)
            {
                Debug.WriteLine($"{item.Key} - {item.Value}");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ExpressionEvaluator eval = new ExpressionEvaluator();
            Variable v;
            // 数字
            v = eval.Evaluate("2 + 2");        // Returns 4  (Integer)
            v = eval.Evaluate("2 + 3 * 5");    // Returns 17 (Integer)
            v = eval.Evaluate("(2 + 3) * 5");  // Returns 25 (Integer)

            // 字符串
            v = eval.Evaluate("\"2\" & \"2\"");  // Returns 22 (String)
            v = eval.Evaluate("'2' & '2'");      // Returns 22 (String)
            v = eval.Evaluate("\"2\" + \"2\"");  // Returns 4  (Integer)

            // 符号
            eval.EvaluateSymbol += Eval_EvaluateSymbol;
            v = eval.Evaluate("two + two");            // Returns 4  (Integer)
            v = eval.Evaluate("two + three * five");   // Returns 17 (Integer)
            v = eval.Evaluate("(two + three) * five"); // Returns 25 (Integer)

            // 方程
            eval.EvaluateFunction += Eval_EvaluateFunction;
            v = eval.Evaluate("add(2, 2)");               // Returns 4  (Integer)
            v = eval.Evaluate("2 + multiply(3, 5)");      // Returns 17 (Integer)
            v = eval.Evaluate("multiply(add(2, 3), 5)");  // Returns 25 (Integer)
        }

        private void Eval_EvaluateSymbol(object sender, SymbolEventArgs e)
        {
            switch (e.Name.ToUpper())
            {
                case "TWO":
                    e.Result.SetValue(2);
                    break;
                case "THREE":
                    e.Result.SetValue(3);
                    break;
                case "FIVE":
                    e.Result.SetValue(5);
                    break;
                default:
                    e.Status = SymbolStatus.UndefinedSymbol;
                    break;
            }
        }

        private void Eval_EvaluateFunction(object sender, FunctionEventArgs e)
        {
            switch (e.Name.ToUpper())
            {
                case "ADD":
                    if (e.Parameters.Length == 2)
                        e.Result.SetValue(e.Parameters[0] + e.Parameters[1]);
                    else
                        e.Status = FunctionStatus.WrongParameterCount;
                    break;
                case "MULTIPLY":
                    if (e.Parameters.Length == 2)
                        e.Result.SetValue(e.Parameters[0] * e.Parameters[1]);
                    else
                        e.Status = FunctionStatus.WrongParameterCount;
                    break;
                default:
                    e.Status = FunctionStatus.UndefinedFunction;
                    break;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // 定义指定宽度
            FixedWidthField[] PersonFields =
            [
                new FixedWidthField(5),
                new FixedWidthField(10),
                new FixedWidthField(10),
            ];

            string filename = "test.txt";
            // 保存到本地
            using (FixedWidthWriter writer = new(PersonFields, filename))
            {
                writer.Write("1", "Bill", "Smith");
                writer.Write("2", "Karen", "Williams");
                writer.Write("3", "Tom", "Phillips");
                writer.Write("4", "Jack", "Carpenter");
                writer.Write("5", "Julie", "Samson");
            }
            // 从本地读取
            using (FixedWidthReader reader = new(PersonFields, filename))
            {
                while (reader.Read())
                {
                    var v = reader.Values;
                }
            }


            // 生成数据
            List<Product> Products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Description = "Coffee Table", Category = "Furniture", Rating = 4.5 },
                new Product { Id = Guid.NewGuid(), Description = "Spoons", Category = "Utensils", Rating = 4.2 },
                new Product { Id = Guid.NewGuid(), Description = "Carpet", Category = "Flooring", Rating = 4.5 },
                new Product { Id = Guid.NewGuid(), Description = "Knives", Category = "Utensils", Rating = 4.7 },
                new Product { Id = Guid.NewGuid(), Description = "Recliner", Category = "Furniture", Rating = 4.5 },
                new Product { Id = Guid.NewGuid(), Description = "Floor Tiles", Category = "Flooring", Rating = 4.5 },
            };

            filename = "test2.txt";
            // 写数据
            using (FixedWidthWriter<Product> writer = new(filename))
            {
                foreach (var product in Products)
                {
                    writer.Write(product);
                }
            }

            // 读数据
            List<Product> results = [];
            using (FixedWidthReader<Product> reader = new(filename))
            {
                while (reader.Read())
                {
                    results.Add(reader.Item);
                }
            }

            // 转换器
            List<Person> People = new()
            {
                new Person { Id = 1, FirstName = "Bill", LastName = "Smith", BirthDate = new DateTime(1982, 2, 7) },
                new Person { Id = 1, FirstName = "Gary", LastName = "Parker", BirthDate = new DateTime(1989, 8, 2) },
                new Person { Id = 1, FirstName = "Karen", LastName = "Wilson", BirthDate = new DateTime(1978, 6, 24) },
                new Person { Id = 1, FirstName = "Jeff", LastName = "Johnson", BirthDate = new DateTime(1972, 4, 18) },
                new Person { Id = 1, FirstName = "John", LastName = "Carter", BirthDate = new DateTime(1982, 12, 21) },
            };

            filename = "test3.txt";
            // 写数据
            using (FixedWidthWriter<Person> writer = new(filename))
            {
                foreach (var person in People)
                {
                    writer.Write(person);
                }
            }

            // 读数据
            List<Person> results2 = new();
            using (FixedWidthReader<Person> reader = new(filename))
            {
                while (reader.Read())
                {
                    results2.Add(reader.Item);
                }
            }


            filename = "test4.txt";
            // 手动映射
            using (FixedWidthWriter<Person> writer = new(filename))
            {
                writer.MapField(m => m.Id, 20);
                writer.MapField(m => m.FirstName, 12);
                writer.MapField(m => m.LastName, 12);
                writer.MapField(m => m.BirthDate, 12).SetConverterType(typeof(BirthDateConverter));

                foreach (var person in People)
                {
                    writer.Write(person);
                }
            }
        }

        public class Product
        {
            [FixedWidthField(36)]
            public Guid Id { get; set; }
            [FixedWidthField(12)]
            public string Description { get; set; }
            [FixedWidthField(12)]
            public string Category { get; set; }
            [FixedWidthField(10)]
            public double Rating { get; set; }
        }

        public class Person
        {
            [FixedWidthField(8)]
            public int Id { get; set; }
            [FixedWidthField(12)]
            public string FirstName { get; set; }
            [FixedWidthField(12)]
            public string LastName { get; set; }
            [FixedWidthField(12, ConverterType = typeof(BirthDateConverter))]
            public DateTime BirthDate { get; set; }
        }

        public class BirthDateConverter : DataConverter<DateTime>
        {
            private const string Format = "yyyyMMdd";

            public override string ConvertToString(DateTime value) => value.ToString(Format);

            public override bool TryConvertFromString(string s, out DateTime value)
            {
                return DateTime.TryParseExact(s, Format, null, System.Globalization.DateTimeStyles.None, out value);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // 简单加密、解密
            Encryption encrypt = new Encryption("admin", EncryptionAlgorithm.TripleDes);
            string original = "This is my message";
            string cipher = encrypt.Encrypt(original);
            string result = encrypt.DecryptString(cipher);

            // 数据类型
            int originalInt = 55;
            double originalDouble = 123.45;
            string cipherInt = encrypt.Encrypt(originalInt);
            string cipherDouble = encrypt.Encrypt(originalDouble);
            int resultInt = encrypt.DecryptInt32(cipherInt);
            double resultDouble = encrypt.DecryptDouble(cipherDouble);

            // 保存文件
            string filename = "test.data";
            int[] intValues = [123, 88, 902, 27, 16, 4, 478, 54];
            using (EncryptionWriter writer = encrypt.CreateStreamWriter(filename))
            {
                for (int i = 0; i < intValues.Length; i++)
                {
                    writer.Write(intValues[i]);
                }
            }

            // 加载文件
            intValues = new int[8];
            using (EncryptionReader reader = encrypt.CreateStreamReader(filename))
            {
                for (int i = 0; i < intValues.Length; i++)
                {
                    intValues[i] = reader.ReadInt32();
                }
            }

            // 流
            using (MemoryStream stream = new MemoryStream())
            using (EncryptionWriter writer = encrypt.CreateStreamWriter(stream))
            {
                writer.Write("ABC");
                writer.Write(123);
                writer.Write(123.45);
                string s = Encryption.EncodeBytesToString(stream.ToArray());
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // ini 文件读写
            string ini_filename = "test.ini";
            MyIniSettings ini = new MyIniSettings(ini_filename)
            {
                UserName = "Big Wang",
                Password = "admin"
            };
            ini.Save();

            MyIniSettings new_ini = new MyIniSettings(ini_filename);
            new_ini.Load();

            // xml 文件读写
            string xml_filename = "test.xml";
            MyXmlSettings xml = new MyXmlSettings(xml_filename)
            {
                UserName = "Big Wang",
                Password = "admin"
            };
            xml.Save();

            MyXmlSettings new_xml = new MyXmlSettings(xml_filename);
            new_xml.Load();
        }

        public class MyIniSettings : IniSettings
        {
            // Define properties to be saved to file
            public string EmailHost { get; set; }
            public int EmailPort { get; set; }

            // The following properties will be encrypted
            [EncryptedSetting]
            public string UserName { get; set; }
            [EncryptedSetting]
            public string Password { get; set; }

            // The following property will not be saved to file
            // Non-public properties are also not saved to file
            [ExcludedSetting]
            public DateTime Created { get; set; }

            public MyIniSettings(string filename) : base(filename, "Password123")
            {
                // Set initial, default property values
                EmailHost = string.Empty;
                EmailPort = 0;
                UserName = string.Empty;
                Password = string.Empty;

                Created = DateTime.Now;
            }
        }

        public class MyXmlSettings : XmlSettings
        {
            // Define properties to be saved to file
            public string EmailHost { get; set; }
            public int EmailPort { get; set; }

            // The following properties will be encrypted
            [EncryptedSetting]
            public string UserName { get; set; }
            [EncryptedSetting]
            public string Password { get; set; }

            // The following property will not be saved to file
            // Non-public properties are also not saved to file
            [ExcludedSetting]
            public DateTime Created { get; set; }

            public MyXmlSettings(string filename) : base(filename, "Password123")
            {
                // Set initial, default property values
                EmailHost = string.Empty;
                EmailPort = 0;
                UserName = string.Empty;
                Password = string.Empty;

                Created = DateTime.Now;
            }
        }

        public class MyRegistrySettings : RegistrySettings
        {
            // Define properties to be saved to file
            public string EmailHost { get; set; }
            public int EmailPort { get; set; }

            // The following properties will be encrypted
            [EncryptedSetting]
            public string UserName { get; set; }
            [EncryptedSetting]
            public string Password { get; set; }

            // The following property will not be saved to file
            // Non-public properties are also not saved to file
            [ExcludedSetting]
            public DateTime Created { get; set; }

            public MyRegistrySettings(string companyName, string applicationName, RegistrySettingsType settingsType)
                : base(companyName, applicationName, settingsType, "Password123")
            {
                // Set initial, default property values
                EmailHost = string.Empty;
                EmailPort = 0;
                UserName = string.Empty;
                Password = string.Empty;

                Created = DateTime.Now;
            }
        }
    }
}
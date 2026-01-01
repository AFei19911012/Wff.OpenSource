using Dapper;
using Dommel;
using Microsoft.Data.Sqlite;
using System.Windows;
using System.Windows.Controls;

namespace Wff.OpenSource.Views
{
    /// <summary>
    /// Demo_Dommel.xaml 的交互逻辑
    /// </summary>
    public partial class Demo_Dommel : UserControl
    {
        public Demo_Dommel()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // 插入数据
            using var con = new SqliteConnection("Data Source=Dommel.db");
            // 创建表
            con.Execute("CREATE TABLE IF NOT EXISTS Persons (Id INTEGER PRIMARY KEY, Name TEXT)");
            con.Open();
            var user = new Person { Name = "John Doe" };
            var id = con.Insert(user);

            var p = con.Get<Person>(1);

            var products = con.Select<Person>(p => p.Name == "John Doe");

            var ps = con.GetAll<Person>();
            lb.Items.Add("");
            lb.Items.Add("查询数据库");
            foreach (var item in ps)
            {
                lb.Items.Add(item.ToString());
            }

            var peo = await con.GetAsync<Person>(1);
            var peos = await con.SelectAsync<Person>(p => p.Name.Contains("John"));
            lb.Items.Add("");
            lb.Items.Add(peo.ToString());
            foreach (var item in peos)
            {
                lb.Items.Add(item.ToString());
            }
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public override string ToString()
        {
            return $"Id = {Id}, Name = {Name}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NuclearProject
{
    /// <summary>
    /// Логика взаимодействия для TestsWindow.xaml
    /// </summary>
    public partial class TestsWindow : Window
    {
        List<DataLoad.RootObject> data;
        public TestsWindow()
        {
            InitializeComponent();

            data = DataLoad.LoadDataFromJson();
            var lThemes = data.Select(q => q.TestType).Distinct();
            
            //todo если нет вопросов еще
            foreach(string itemThemes in lThemes)
            {
                cmbThemes.Items.Add(itemThemes);
            }
        }

        private void cmbThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btStartTest.IsEnabled = cmbThemes.SelectedValue != null ? true : false;
        }

        private void Button_Click_Questions(object sender, RoutedEventArgs e)
        {
            string nameTheme = cmbThemes.SelectedValue.ToString();
            Questions win = new Questions(nameTheme);
            win.ShowDialog();
        }

        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
                StartModelWindow win = new StartModelWindow("Tests"); //вызываем окно справки
                win.ShowDialog();
        }

        private void Button_Create_Test(object sender, RoutedEventArgs e)
        {
            CreateTestWindow win = new CreateTestWindow(this);
            win.ShowDialog();
        }

        private void Button_Edit_Test(object sender, RoutedEventArgs e)
        {
            EditTestsWindow win = new EditTestsWindow(this);
            win.ShowDialog();
        }
    }
}

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
    /// Логика взаимодействия для EditTestsWindow.xaml
    /// </summary>
    public partial class EditTestsWindow : Window
    {
        List<DataLoad.RootObject> data;
        public EditTestsWindow()
        {
            InitializeComponent();
            data = DataLoad.LoadDataFromJson();

            if (data != null)
            {
                var types = data.Select(question => question.TestType).Distinct();
                var allThemes = data.Select(question => question.Theme).Distinct();
                foreach (var type in types)
                {
                    TestTypes.Items.Add(type);
                }

            }
        }


        private void Next_Action_Click(object sender, RoutedEventArgs e)
        {
            string selectedQuestion = Questions.SelectedValue.ToString();
            int id = data.Where(question => question.Question == selectedQuestion).Select(question => question.ID).Single();

        }

        private void TestTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Themes.Items.Clear();
            Questions.Items.Clear();
            string selectedType = TestTypes.SelectedValue.ToString();
            var themes = data.Where(question => question.TestType == selectedType).Select(question => question.Theme).Distinct();
            foreach (var item in themes)
            {
                Themes.Items.Add(item);
            }
           
        }

        private void Themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Questions.Items.Clear();
            string selectedType = TestTypes.SelectedValue.ToString();
            var theme = Themes.SelectedValue;
            if (theme != null)
            {
                string selectedTheme = theme.ToString();
                var questions = data.Where(question => (question.Theme == selectedTheme) && (question.TestType == selectedType)).Select(question => question.Question).Distinct();
                foreach (var item in questions)
                {
                    Questions.Items.Add(item);
                }
            }
        
        }
    }
}

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
    /// Окно выбора вопроса для редактирования
    /// </summary>
    public partial class EditTestsWindow : Window
    {
        List<DataLoad.RootObject> data;
        TestsWindow testWindow;

        public EditTestsWindow(TestsWindow testWindow)
        {
            //загружаем список вопросов
            InitializeComponent();
            data = DataLoad.LoadDataFromJson();
            this.testWindow = testWindow;
            if (data != null)
            {
                var types = DataLoad.GetQuestionTypes();
                var allThemes = DataLoad.GetQuestionThemes();
                foreach (var type in types)
                {
                    TestTypes.Items.Add(type);
                }

            }
        }
        
        //переход на окно редактирования выбранного вопроса
        private void Next_Action_Click(object sender, RoutedEventArgs e)
        {
            string selectedQuestion = Questions.SelectedValue.ToString();
            var question = DataLoad.GetQuestionByText(selectedQuestion);
            int id = question.ID;
            var win = new EditQuestionWindow(id, this.testWindow);
            win.ShowDialog();

        }

        //удаление выбранного вопроса
        private void Delete_Question_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы действительно хотите удалить данный вопрос?", "Подтвердите удаление", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string selectedQuestionText = Questions.SelectedValue.ToString();
                var selectedQuestion = DataLoad.GetQuestionByText(selectedQuestionText);

                data.Remove(selectedQuestion);
                DataLoad.SaveDataToJson(data);

                var questionTestTypes = DataLoad.GetQuestionTypes();
                var currentItems = testWindow.cmbThemes.Items;
                currentItems.Clear();
                foreach (var type in questionTestTypes)
                {
                    currentItems.Add(type);
                }
                Close();
                MessageBox.Show("Вопрос удален!", "Сообщение");
                return;
            }

            return;
        }

        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            StartModelWindow win = new StartModelWindow("ChangeQuestions"); //вызываем окно справки
            win.ShowDialog();
        }

        //меняем спписок тем в зависимости от выбранного типа теста
        private void TestTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Themes.Items.Clear();
            Questions.Items.Clear();

            string selectedType = TestTypes.SelectedValue.ToString();
            var themes = DataLoad.GetThemesInType(selectedType);
            foreach (var item in themes)
            {
                Themes.Items.Add(item);
            }

        }

        //меняем список вопросов в зависимсоти от выбранной темы
        private void Themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Questions.Items.Clear();

            string selectedType = TestTypes.SelectedValue.ToString();
            var theme = Themes.SelectedValue;

            if (theme != null)
            {
                string selectedTheme = theme.ToString();
                var questions = DataLoad.GetQuestionsInTheme(selectedTheme, selectedType);
                foreach (var item in questions)
                {
                    Questions.Items.Add(item);
                }
            }

        }

        //когда выбраны тип теста, тема и сам вопрос, делаем кнопку редактирования активной
        private void Questions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnNextAction.IsEnabled = Questions.SelectedValue != null ? true : false;
            btnDeleteQuestion.IsEnabled = Questions.SelectedValue != null ? true : false;
        }
    }
}

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
using Newtonsoft.Json;
using System.IO;
namespace NuclearProject
{
    /// <summary>
    /// Логика взаимодействия для CreateTestWindow.xaml
    /// </summary>
    public partial class CreateTestWindow : Window
    {
        List<DataLoad.RootObject> data;
        int id;
        TestsWindow win;

        public CreateTestWindow(TestsWindow win)
        {
            InitializeComponent();
            data = DataLoad.LoadDataFromJson(); //получаем данные из json
            this.win = win; //ссылка на главное окно с тестами

            if (data != null)
            {
                var types = DataLoad.GetQuestionTypes();
                var allThemes = DataLoad.GetQuestionThemes();
                id = DataLoad.GetMaxQuestionId();

                //добаляем темы и типы тестов в списки
                foreach (var type in types)
                {
                    TestTypes.Items.Add(type);
                }
                foreach (var theme in allThemes)
                {
                    Theme.Items.Add(theme);
                }
            }
        }

        private void Save_Tests(object sender, RoutedEventArgs e)
        {
            //получаем данные из полей
            string testType = TestTypes.Text.Trim();
            string questionText = Question.Text.Trim();
            string theme = Theme.Text.Trim();
            string[] answers = Answers.Text.Trim().Split(';').Select(ans => ans.Trim()).ToArray();
            string correctAnswer = CorrectAnswer.Text.Trim();
            string complexity = Complexity.Text;

            if (String.IsNullOrEmpty(testType) || String.IsNullOrEmpty(questionText) || String.IsNullOrEmpty(theme) || String.IsNullOrEmpty(correctAnswer) || String.IsNullOrEmpty(complexity))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка");
                return;
            }

            if (!DataLoad.CheckUniqueQuestion(questionText))
            {
                MessageBox.Show("Вопрос уже существует!", "Ошибка");
                return;
            }

            if (answers.Length > 7 || answers.Length < 2 || answers.Contains(""))
            {
                MessageBox.Show("Введите от 2 до 7 ответов", "Ошибка");
                return;
            }

            if (answers.Distinct().Count() != answers.Length)
            {
                MessageBox.Show("Вы указали повторяющийся возможный ответ!");
                return;
            }

            if (!answers.Contains(correctAnswer))
            {
                MessageBox.Show("Верный ответ не совпадает ни с одним из возможных!", "Ошибка");
                return;
            }

            var answerList = new List<DataLoad.Answer>();
            var questions = new List<DataLoad.RootObject>();
            //увеличиваем id на 1 (автоинкремент)
            int currentId = id;
            currentId++;

            //заполняем список ответов вопроса
            foreach (string answerText in answers)
            {
                int isRight = 0;
                if (answerText == correctAnswer)
                {
                    isRight = 1;
                }
                answerList.Add(new DataLoad.Answer(answerText, isRight));
            }

            //добавляем вопрос и обновляем json
            questions.Add(new DataLoad.RootObject(testType, questionText, theme, currentId, complexity, answerList));
            var updatedJson = data.Concat(questions).ToList();
            DataLoad.SaveDataToJson(updatedJson);

            var questionTestTypes = questions.Select(q => q.TestType).Distinct();
            //добавляем новые типы тестов в выпдающий список окна с тестированием
            var currentItems = win.cmbThemes.Items;
            foreach (var type in questionTestTypes)
            {
                if (!currentItems.Contains(type))
                {
                    currentItems.Add(type);
                }
            }

            Close();
            MessageBox.Show("Вопрос добавлен!", "Сообщение");

        }

        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            StartModelWindow win = new StartModelWindow("CreateQuestions"); //вызываем окно справки
            win.ShowDialog();
        }

        private void TestTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Theme.Items.Clear();
            var selectedType = TestTypes.SelectedValue;
            if (selectedType != null)
            {
                var themes = DataLoad.GetThemesInType(selectedType.ToString());
                foreach (var item in themes)
                {
                    Theme.Items.Add(item);
                }

            }
        }
    }

}

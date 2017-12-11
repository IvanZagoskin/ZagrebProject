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

        public CreateTestWindow()
        {
            InitializeComponent();
            data = DataLoad.LoadDataFromJson();

            if (data != null)
            {
                var types = data.Select(question => question.TestType).Distinct();
                var allThemes = data.Select(question => question.Theme).Distinct();

                id = data.Select(question => question.ID).Max();

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
            string testType = TestTypes.Text.Trim();
            string questionText = Question.Text.Trim();
            string theme = Theme.Text.Trim();
            string[] answers = Answers.Text.Trim().Split(';');
            string correctAnswer = CorrectAnswer.Text.Trim();
            string complexity = Complexity.Text;

            if (String.IsNullOrEmpty(testType) || String.IsNullOrEmpty(questionText) || String.IsNullOrEmpty(theme) || String.IsNullOrEmpty(correctAnswer) || String.IsNullOrEmpty(complexity))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка");
                return;
            }

            if (!CheckUniqueQuestion(questionText))
            {
                MessageBox.Show("Вопрос уже существует!", "Ошибка");
                return;
            }

            if (answers.Length > 7 || answers.Length < 2 || answers.Contains(""))
            {
                MessageBox.Show("Введите от 2 до 7 ответов", "Ошибка");
                return;
            }

            if (!answers.Contains(correctAnswer))
            {
                MessageBox.Show("Верный ответ не совпадает ни с одним из возможных!", "Ошибка");
                return;
            }

            var answerList = new List<DataLoad.Answer>();
            var questions = new List<DataLoad.RootObject>();
            int currentId = id;
            currentId++;


            foreach (string answerText in answers)
            {
                int isRight = 0;
                //если If не был ни разу исключение
                if (answerText == correctAnswer)
                {
                    isRight = 1;
                }
                answerList.Add(new DataLoad.Answer(answerText, isRight));
            }

            //todo foreach all questions
            //todo if empty fields
            questions.Add(new DataLoad.RootObject(testType, questionText, theme, currentId, complexity, answerList));
            var updatedJson = data.Concat(questions).ToList();
            DataLoad.SaveDataToJson(updatedJson);
            Close();
            MessageBox.Show("Вопросы добавлены!", "Сообщение");
            
        }

        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            StartModelWindow win = new StartModelWindow("CreateQuestions"); //вызываем окно справки
            win.ShowDialog();
        }

        private bool CheckUniqueQuestion(string questionText)
        {
            if (data.Where(question => question.Question == questionText).Any())
            {
                return false;
            }

            return true;
        }
    }

}

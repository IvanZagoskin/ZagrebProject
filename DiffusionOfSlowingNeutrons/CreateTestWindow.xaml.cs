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
            string complexity = Complexity.Text.Trim();

            var answerList = new List<DataLoad.Answer>();
            var questions = new List<DataLoad.RootObject>();
            int currentId = id;
            currentId++;

            foreach (string answerText in answers)
            {
                int isRight = 0;
                if(answerText == correctAnswer)
                {
                    isRight = 1;
                }
                answerList.Add(new DataLoad.Answer(answerText, isRight));
            }

            //todo foreach all questions
            //todo if empty fields
            questions.Add(new DataLoad.RootObject(testType, questionText, theme, currentId, complexity, answerList));
            var updatedJson = data.Concat(questions).ToList();
            //todo return true if all OK message
            DataLoad.SaveDataToJson(updatedJson);
            Close();
            MessageBox.Show("Вопросы добавлены!", "Сообщение");
            
        }
    }

}

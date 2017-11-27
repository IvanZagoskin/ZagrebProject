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
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace NuclearProject
{
    /// <summary>
    /// Логика взаимодействия для Questions.xaml
    /// </summary>
    public partial class Questions : Window
    {
        private JArray formedQuestions;
        private const string filename = "database.json";
        private string nameTheme;
        private const int totalNumber = 10;
        private const int easyAmount = 4;
        private const int middleAmount = 4;
        private const int hardAmount = 2;

        public Questions(string nameTheme)
        {
            InitializeComponent();
            //TODO:wrap in file not found try catch
            //вопросы которые были отобраны для текущего теста getQuestionList
            this.nameTheme = nameTheme;
            var JData = JArray.Parse(File.ReadAllText(filename));
            this.formedQuestions = this.GetQuestionList(JData);
            //кладем сформированный список
            this.LoadQuestions(this.formedQuestions);          
        }
        private void LoadQuestions(JArray formedQuestions)
        {
            foreach (var item in formedQuestions)
            {
                var margin = new Thickness(10, 20, 10, 10);
                var answersMargin = new Thickness(10);
                var question = new StackPanel
                {
                    Width = 800
                };

                var questionText = new TextBlock
                {
                    Text = item["Question"].ToString(),
                    Margin = margin,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap
                };
                question.Children.Add(questionText);
                QuestionList.Children.Add(question);
                foreach (var answerItem in item["Answers"])
                {
                    var answer = new RadioButton
                    {
                        Margin = answersMargin,
                        Content = answerItem["Text"].ToString(),
                    };
                    question.Children.Add(answer);
                }

            }
        }
        
        private void SubmitTest(object sender, RoutedEventArgs e)
        {
            var questions = QuestionList.Children.OfType<StackPanel>();
            var selectedAnswers = new Dictionary<string, string>();
            //собирем вопросы и ответы
            foreach (StackPanel question in questions)
            {
                try
                {
                    var questionText = question.Children.OfType<TextBlock>().First().Text;
                    var answer = question.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
                    selectedAnswers.Add(questionText, answer);
                }
                catch (System.NullReferenceException)
                {
                    MessageBox.Show("Необходимо указать ответ на каждый вопрос", "Ошибка");
                    return;
                }

            }
            //проверяем ответы
            this.CheckTestResults(selectedAnswers);
        }

        private void CheckTestResults(Dictionary<string, string> selectedAnswers)
        {
            float points = .0f;
            int percent = 0;
            string themes = "";

            foreach (var item in formedQuestions)
            {
                var selectedAnswer = selectedAnswers.FirstOrDefault(t => t.Key == item["Question"].ToString()).Value;
                foreach (var answerItem in item["Answers"])
                {
                    if ((int)answerItem["isRight"] == 1 && selectedAnswer == answerItem["Text"].ToString())
                    {
                        points += (float)item["Complexity"];
                    }
                    //добавляем темы к списку тем для повторения
                    else if ((int)answerItem["isRight"] == 1 && selectedAnswer != answerItem["Text"].ToString())
                    {
                        if (themes.Contains(item["Theme"].ToString()))
                        {
                            continue;
                        }
                        themes += item["Theme"] + Environment.NewLine;
                    }
                }
            }

            percent = (int)((points / 18) * 100);
            //TODO:make custom window
            MessageBox.Show("Оценка: " + this.GetTotalGrade(percent).ToString() + Environment.NewLine + "Темы для повторения:" + Environment.NewLine + themes, "Ваш результат");

        }
        private JArray GetQuestionList(JArray JData)
        {
            var random = new Random();
            var formedQuestions = new JArray();
            var randomQuestions = JData.OrderBy(q => random.Next()).Where(q => q["TestType"].ToString() == this.nameTheme);
            var easyQuestions = new JArray(randomQuestions.Where(q => (int)q["Complexity"] == 1).Take(easyAmount));
            var middleQuestions = new JArray(randomQuestions.Where(q => (int)q["Complexity"] == 2).Take(middleAmount));
            var hardQuestions = new JArray(randomQuestions.Where(q => (int)q["Complexity"] == 3).Take(hardAmount));
            formedQuestions.Merge(easyQuestions);
            formedQuestions.Merge(middleQuestions);
            formedQuestions.Merge(hardQuestions);

            return formedQuestions;
        }

        private int GetTotalGrade(int percent)
        {
            if (percent >= 0 && percent <= 59)
            {
                return 2;
            }

            else if (percent >= 60 && percent <= 76)
            {
                return 3;
            }

            else if (percent >= 77 && percent <= 94)
            {
                return 4;
            }

            else if (percent >= 95 && percent <= 100)
            {
                return 5;
            }

            return 2;

        }

    }
}

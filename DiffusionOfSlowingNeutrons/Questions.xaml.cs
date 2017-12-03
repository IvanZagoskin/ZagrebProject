using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private readonly Random random = new Random();

        public Questions(string nameTheme)
        {
            InitializeComponent();
            //TODO:wrap in file not found try catch
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

                var answerList = item["Answers"].OrderBy(ans => random.Next());

                foreach (var answerItem in answerList)
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
            double points = 0;
            double percent = 0;
            List<string> themes = new List<string>();

            foreach (var answer in selectedAnswers)
            {
                var currentQuestion = formedQuestions.Where(q => answer.Key == q["Question"].ToString());

                var complexity = (int)currentQuestion.Select(c => c["Complexity"]).FirstOrDefault();
                var theme = currentQuestion.Select(t => t["Theme"]).FirstOrDefault().ToString();
                var allAnswers = currentQuestion.Select(q => q["Answers"]).FirstOrDefault();
                var correctAnswer = allAnswers.Where(ans => (int)ans["isRight"] == 1).Select(ans => ans["Text"]).FirstOrDefault().ToString();
                if (answer.Value == correctAnswer)
                {
                    points += this.GetQuestionValue(complexity);
                }
                else
                {
                    points -= this.GetQuestionValue(complexity);
                    if (themes.Contains(theme))
                    {
                        continue;
                    }

                    themes.Add(theme);
                }
            }

            points = (points < 0) ? 0 : points;
            double res = (points / 22) * 100;
            Console.WriteLine(res);
            percent = Math.Round((points / 22) * 100, 1);
            percent = Math.Round(percent);
            Console.WriteLine(percent);
           
            TestResult window = new TestResult(this.GetTotalGrade((int)percent), (int)percent, themes);
            window.ShowDialog();
        }
        private JArray GetQuestionList(JArray JData)
        {
            var formedQuestions = new JArray();
            var randomQuestions = JData.OrderBy(q => random.Next()).Where(q => q["TestType"].ToString() == this.nameTheme);
            var easyQuestions = new JArray(randomQuestions.Where(q => (int)q["Complexity"] == 1).GroupBy(q => q["Theme"]).Select(q => q.First()).Take(easyAmount));
            var middleQuestions = new JArray(randomQuestions.Where(q => (int)q["Complexity"] == 2).GroupBy(q => q["Theme"]).Select(q => q.First()).Take(middleAmount));
            var hardQuestions = new JArray(randomQuestions.Where(q => (int)q["Complexity"] == 3).GroupBy(q => q["Theme"]).Select(q => q.First()).Take(hardAmount));

            formedQuestions.Merge(easyQuestions);
            formedQuestions.Merge(middleQuestions);
            formedQuestions.Merge(hardQuestions);

            return formedQuestions;
        }

        private int GetTotalGrade(int percent)
        {
            if (percent >= 0 && percent <= 45)
            {
                return 2;
            }

            else if (percent >= 46 && percent <= 63)
            {
                return 3;
            }

            else if (percent >= 64 && percent <= 89)
            {
                return 4;
            }

            else if (percent >= 90 && percent <= 100)
            {
                return 5;
            }

            return 2;

        }

        private int GetQuestionValue(int complexity)
        {
            if (complexity == 3)
            {
                return 1;
            }
            else if (complexity == 2)
            {
                return 2;
            }

            return 3;
        }

        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            StartModelWindow win = new StartModelWindow("Questions"); //вызываем окно справки
            win.ShowDialog();
        }

    }
}

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
        private List<DataLoad.RootObject> formedQuestions;
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
            //получаем вопросы и ответы как лист
            var data = DataLoad.LoadDataFromJson();
            //формируем список вопросов
            this.formedQuestions = GetQuestionList(data);
            //кладем сформированный список
            LoadQuestions(this.formedQuestions);
        }
        //добавляем список вопросов в окно
        private void LoadQuestions(List<DataLoad.RootObject> formedQuestions)
        {
            var margin = new Thickness(10, 20, 10, 10);
            var answersMargin = new Thickness(10);
            foreach (var item in formedQuestions)
            {
                var question = new StackPanel
                {
                    Width = 800
                };

                var questionText = new TextBlock
                {
                    Text = item.Question,
                    Margin = margin,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap
                };
                question.Children.Add(questionText);
                QuestionList.Children.Add(question);

                //наполняем список ответов вопроса в рандомном порядке
                var answerList = item.Answers.OrderBy(ans => random.Next());
                //выводим ответы вопроса на экран
                foreach (var answerItem in answerList)
                {
                    var answer = new RadioButton
                    {
                        Margin = answersMargin,
                        Content = answerItem.Text.ToString(),
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
                    selectedAnswers.Add(questionText, answer);//наполняем вопросами и ответами
                }
                catch (System.NullReferenceException)
                {
                    MessageBox.Show("Необходимо указать ответ на каждый вопрос", "Ошибка");//если не все указаны, просим заполнить все
                    return;
                }

            }
            //проверяем ответы
            CheckTestResults(selectedAnswers);
        }

        private void CheckTestResults(Dictionary<string, string> selectedAnswers)
        {
            double points = 0;
            double percent = 0;
            List<string> themes = new List<string>();

            foreach (var answer in selectedAnswers)
            {
                var currentQuestion = formedQuestions.Where(q => answer.Key == q.Question.ToString());

                var complexity = currentQuestion.Select(c => c.Complexity).FirstOrDefault();
                var theme = currentQuestion.Select(t => t.Theme).FirstOrDefault();
                var allAnswers = currentQuestion.Select(q => q.Answers).FirstOrDefault();
                var correctAnswer = allAnswers.Where(ans => ans.isRight == 1).Select(ans => ans.Text).FirstOrDefault();

                //правильно отвечено + балл, неправильно - балл
                if (answer.Value == correctAnswer)
                {
                    points += GetQuestionValue(complexity);
                }
                else
                {
                    points -= GetQuestionValue(complexity);
                    if (themes.Contains(theme))
                    {
                        continue;
                    }
                    //формируем список тем для повторения
                    themes.Add(theme);
                }
            }

            points = (points < 0) ? 0 : points;
            percent = Math.Round((points / 22) * 100, 1);//вычисляем процент правильных ответов
            percent = Math.Round(percent);
            //передаем проценты, темы в окно результата
            TestResult window = new TestResult(GetTotalGrade((int)percent), (int)percent, themes);
            window.ShowDialog();
        }
   
        private List<DataLoad.RootObject> GetQuestionList(List<DataLoad.RootObject> JData)
        {
            var randomQuestions = JData.OrderBy(q => random.Next()).Where(q => q.TestType.ToString() == this.nameTheme);

            var easyQuestions = randomQuestions.Where(q => q.Complexity == "1").GroupBy(q => q.Theme).Select(q => q.First()).Take(easyAmount).ToList();
            var middleQuestions = randomQuestions.Where(q => q.Complexity == "2").GroupBy(q => q.Theme).Select(q => q.First()).Take(middleAmount).ToList();
            var hardQuestions = randomQuestions.Where(q => q.Complexity == "3").GroupBy(q => q.Theme).Select(q => q.First()).Take(hardAmount).ToList();

            return easyQuestions.Concat(middleQuestions).Concat(hardQuestions).ToList();
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

        private int GetQuestionValue(string complexity)
        {
            if (complexity == "3")
            {
                return 1;
            }
            else if (complexity == "2")
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

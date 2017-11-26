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
        private JArray JData;
        private const string filename = "database.json";
        private int amount;
        public Questions(string nameTheme)
        {
            InitializeComponent();
            //TODO:wrap in file not found try catch
            //string curFile = "database.json";
            //Console.WriteLine(File.Exists(curFile) ? "File exists." : "File does not exist.");
           //вопросы которые были отобраны для текущего теста
            this.JData = JArray.Parse(File.ReadAllText(filename));
            this.amount = JData.Count();
            this.loadQuestions(this.JData);
            //var questions = new List<Label>();
          
        }
        private void loadQuestions(JArray JData)
        {
            foreach (JObject item in JData)
            {
                var margin = new Thickness(10, 20, 10, 10);
                var answersMargin = new Thickness(10);
                var question = new StackPanel();
                //question.Name = '' //TODO:set name
                var questionText = new TextBlock();
                questionText.Text = item["Question"].ToString();
                questionText.Margin = margin;
                question.Children.Add(questionText);
                QuestionList.Children.Add(question);
                foreach (JObject answerItem in item["Answers"])
                {
                    var answer = new RadioButton();
                    answer.Margin = answersMargin;
                    answer.Content = answerItem["Text"].ToString();
                    question.Children.Add(answer);
                }

            }
        }
        
        private void SubmitTest(object sender, RoutedEventArgs e)
        {
            var questions = QuestionList.Children.OfType<StackPanel>();
            var selectedAnswers = new Dictionary<string, string>();
            int total = 0;
            //собирем вопросы и ответы
            foreach (StackPanel question in questions)
            {
                var questionText = question.Children.OfType<TextBlock>().First().Text;
                var answer = question.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Content.ToString();
                selectedAnswers.Add(questionText, answer);
            }
            //проверяем ответы
            foreach (var item in JData)
            {
                var selectedAnswer = selectedAnswers.FirstOrDefault(t => t.Key == item["Question"].ToString()).Value;
                foreach (var answerItem in item["Answers"])
                {
                    if ((int)answerItem["isRight"] == 1 && answerItem["Text"].ToString() == selectedAnswer)
                    {
                        total++;
                    }
                }
            }
            MessageBox.Show("Правильных ответов: " + total + " из " + this.amount, "Ваш результат");

            //TODO:set id on stack panel
            //foreach (JObject item in this.JData)
            //{
            //    if (item["Question"].ToString() == questionText)
            //    {
            //        foreach (JObject answerItem in item["Answers"])
            //        {
            //            if ((int)answerItem["isRight"] == 1)
            //            {
            //                var correctAnswer = answerItem["Text"];
            //                Console.WriteLine(correctAnswer);
            //            }
            //        }
            //    }
            //}



        }
    }
}

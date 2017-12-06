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
    /// Логика взаимодействия для TestResult.xaml
    /// </summary>
    public partial class TestResult : Window
    {
        private int grade;
        private int percent;
        private List<string> themes;
        public TestResult(int grade, int percent, List<string> themes)
        {
            InitializeComponent();
            this.grade = grade;
            this.percent = percent;
            this.themes = themes;
            Grade.Text = grade.ToString();
            Percent.Text = percent.ToString();
            ShowThemesList();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowThemesList()
        {
            //нет тем, все правильно решено, не показываем надпись про список тем
            if(!themes.Any())
            {
                ThemesLabel.Height = 0;
                Themes.Height = 0;
                return;
            }
        
            //выводим темы для повторения
            foreach (var theme in this.themes)
            {
                var themeText = new TextBlock
                {
                    Text = themes.IndexOf(theme) + 1 + ") " + theme,
                    FontSize = 14,
                    Width = 400
                };
                this.Height += 20;
                Themes.Children.Add(themeText);
            }
        }
    }
}

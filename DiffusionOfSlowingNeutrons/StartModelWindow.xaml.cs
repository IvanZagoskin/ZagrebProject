using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace NuclearProject
{
    /// <summary>
    /// Interaction logic for StartModelWindow.xaml
    /// </summary>
    public partial class StartModelWindow : Window
    {
        public StartModelWindow(string fileType)
        {
            InitializeComponent();
            if (fileType == "Reference")
            {
                System.Uri pdf = new System.Uri(String.Format("file:///{0}/REFERENCE.pdf", Directory.GetCurrentDirectory()));
                webHelp.Navigate(pdf);
                this.Title = "Справка";
            } else if(fileType == "Help")
            {
                System.Uri pdf = new System.Uri(String.Format("file:///{0}/HELP.pdf", Directory.GetCurrentDirectory()));
                webHelp.Navigate(pdf);
                this.Title = "Помощь";
            }
                else if (fileType == "Tests")
            {
                System.Uri pdf = new System.Uri(String.Format("file:///{0}/TESTS.pdf", Directory.GetCurrentDirectory()));
                webHelp.Navigate(pdf);
                this.Title = "Помощь";
            }
                else if (fileType == "Questions")
            {
                System.Uri pdf = new System.Uri(String.Format("file:///{0}/QUESTIONS.pdf", Directory.GetCurrentDirectory()));
                webHelp.Navigate(pdf);
                this.Title = "Помощь";
            }
            else if (fileType == "CreateQuestions")
            {
                System.Uri pdf = new System.Uri(String.Format("file:///{0}/ADD_TEST.pdf", Directory.GetCurrentDirectory()));
                webHelp.Navigate(pdf);
                this.Title = "Помощь";
            }
            else if (fileType == "ChangeQuestion")
            {
                System.Uri pdf = new System.Uri(String.Format("file:///{0}/CHANGE_QUESTION.pdf", Directory.GetCurrentDirectory()));
                webHelp.Navigate(pdf);
                this.Title = "Помощь";
            }
        }
    }
}

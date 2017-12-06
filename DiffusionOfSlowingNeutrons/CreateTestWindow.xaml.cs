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
    /// Логика взаимодействия для CreateTestWindow.xaml
    /// </summary>
    public partial class CreateTestWindow : Window
    {
        public CreateTestWindow()
        {
            InitializeComponent();
            var data = DataLoad.LoadDataFromJson();
            var types = data.Select(type => type.TestType).Distinct();
            foreach (var type in types)
            {
                TestTypes.Items.Add(type);
            }
        }

        private void Next_Action_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

﻿using System;
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
    /// Логика взаимодействия для TestsWindow.xaml
    /// </summary>
    public partial class TestsWindow : Window
    {
        List<string> lThemes;

        public TestsWindow()
        {
            InitializeComponent();

            lThemes = new List<string>();
            lThemes.Add("Базовые понятия");
            lThemes.Add("Тепловая динамика");

            foreach(string itemThemes in lThemes)
            {
                cmbThemes.Items.Add(itemThemes);
            }
        }

        private void cmbThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click_Questions(object sender, RoutedEventArgs e)
        {
            string nameTheme = cmbThemes.SelectedValue.ToString();
            Questions win = new Questions(nameTheme);
            win.ShowDialog();
        }
    }
}

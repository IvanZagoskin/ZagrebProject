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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Collections;
using OxyPlot;
using HelixToolkit;
using HelixToolkit.Wpf;
using OxyPlot.Wpf;
using System.IO;

namespace NuclearProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ModellingSession session;
        List<Fuel> lFuels;
        List<Coolant> lCoolants;
        ModelNuclearReactor modelReactor;

        private class Fuel
        {
            private string name;
            private float C, P, V;

            public Fuel(string name, float C, float P, float V)
            {
                this.name = name;
                this.C = C;
                this.P = P;
                this.V = V;

            }

            public string getName() { return this.name; }

            public float getP() { return this.P; }
            public float getC() { return this.C; }
            public float getV() { return this.V; }
            //public float getA() { return this.a; }
            //public float getF() { return this.F; }
            //public float getT() { return this.T; }
        }

        private class Coolant
        {
            private string name;
            private float C, P, V, a, F, T;

            public Coolant(string name, float C, float P, float V, float a, float F, float T)
            {
                this.name = name;
                this.P = P;
                this.C = C;
                this.V = V;
                this.a = a;
                this.F = F;
                this.T = T;
            }

            public string getName() { return this.name; }

            public float getP() { return this.P; }
            public float getC() { return this.C; }
            public float getV() { return this.V; }
            public float getA() { return this.a; }
            public float getF() { return this.F; }
            public float getT() { return this.T; }
        }

        public MainWindow()
        {
            InitializeComponent();

            lFuels = new List<Fuel>();
            lFuels.Add(new Fuel("Оксид урана", 318, 10960, 7.026f));
            lFuels.Add(new Fuel("Металлический уран", 0, 0, 0));
            lFuels.Add(new Fuel("Торий", 0, 0, 0));

            lCoolants = new List<Coolant>();
            lCoolants.Add(new Coolant("Вода", 5670, 620, 180, 1240, 4850, 0.68f));
            lCoolants.Add(new Coolant("Тяжёлая вода", 0, 0, 0, 0, 0, 0));
            lCoolants.Add(new Coolant("Свинец", 0, 0, 0, 0, 0, 0));
            lCoolants.Add(new Coolant("Натрий", 0, 0, 0, 0, 0, 0));

            foreach (Fuel itemFuels in lFuels)
            {
                cmbFuel.Items.Add(itemFuels.getName());
            }

            foreach (Coolant itemCoolants in lCoolants)
            {
                cmbCoolant.Items.Add(itemCoolants.getName());
            }
        }

        private void Button_Click_Ref(object sender, RoutedEventArgs e) //кнопка "Справка"
        {
            StartModelWindow win = new StartModelWindow(); //вызываем окно справки
            win.ShowDialog();
        }

        private void Button_Click_Tests(object sender, RoutedEventArgs e)
        {
            TestsWindow win = new TestsWindow();
            win.ShowDialog();
        }

        private void Button_Click_PlotStart(object sender, RoutedEventArgs e)
        {
            float startPower = 0;
            float coeffA = 0;
            float coeffB = 0;
            float coeffC = 0;

            if ((txtStartPower.Text) != "")
                startPower = float.Parse(txtStartPower.Text.Replace('.', ','));

            if ((txtCoeffA.Text) != "")
                coeffA = float.Parse(txtCoeffA.Text.Replace('.', ','));

            if ((txtCoeffB.Text) != "")
                coeffB = float.Parse(txtCoeffB.Text.Replace('.', ','));

            if ((txtCoeffC.Text) != "")
                coeffC = float.Parse(txtCoeffC.Text.Replace('.', ','));

            string args = startPower
                + " " + coeffA
                + " " + coeffB
                + " " + coeffC;
            System.Diagnostics.Process.Start("Test.exe", args);


        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            try
            {
                float startPower = float.Parse(txtStartPower.Text);
                float coeffA = float.Parse(txtCoeffA.Text.Replace('.',','));
                float coeffB = float.Parse(txtCoeffB.Text.Replace('.', ','));
                float coeffC = float.Parse(txtCoeffC.Text.Replace('.', ','));

                float fuelC = float.Parse(txtFuelC.Text.Replace('.', ','));
                float fuelP = float.Parse(txtFuelP.Text.Replace('.', ','));
                float fuelV = float.Parse(txtFuelV.Text.Replace('.', ','));

                float coolantС = float.Parse(txtCoolantС.Text.Replace('.', ','));
                float coolantP = float.Parse(txtCoolantP.Text.Replace('.', ','));
                float coolantV = float.Parse(txtCoolantV.Text.Replace('.', ','));
                float coolantA = float.Parse(txtCoolantA.Text.Replace('.', ','));
                float coolantF = float.Parse(txtCoolantF.Text.Replace('.', ','));
                float coolantT = float.Parse(txtCoolantT.Text.Replace('.', ','));

                this.modelReactor = new ModelNuclearReactor(fuelC, fuelP, fuelV, coolantС, coolantP, coolantV, coolantA, coolantF, coolantT);

            } catch (System.FormatException exc)
            {
                MessageBox.Show("Введены не все параметры или введены неверно!", "Ошибка");
                Console.WriteLine(exc.ToString() + " : Не все поля заполнены или заполены неверно.");
            }

        }

        private void Change_Coolant(object sender, SelectionChangedEventArgs e)
        {
            object selectedCoolantName = cmbCoolant.SelectedValue;
            Coolant selectedCoolant = lCoolants.Find(item => item.getName() == (string)selectedCoolantName);
            txtCoolantС.Text = selectedCoolant.getC().ToString();
            txtCoolantP.Text = selectedCoolant.getP().ToString();
            txtCoolantV.Text = selectedCoolant.getV().ToString();

            object selectedFuelName = cmbFuel.SelectedValue;
            if (selectedFuelName != null) {
                txtCoolantA.Text = selectedCoolant.getA().ToString();
                txtCoolantF.Text = selectedCoolant.getF().ToString();
                txtCoolantT.Text = selectedCoolant.getT().ToString();
            }
        }

        private void Change_Fuel(object sender, SelectionChangedEventArgs e)
        {
            object selectedFuelName = cmbFuel.SelectedValue;
            Fuel selectedFuel = lFuels.Find(item => item.getName() == (string)selectedFuelName);
            txtFuelC.Text = selectedFuel.getC().ToString();
            txtFuelP.Text = selectedFuel.getP().ToString();
            txtFuelV.Text = selectedFuel.getV().ToString();

            object selectedCoolantName = cmbCoolant.SelectedValue;
            if (selectedCoolantName != null)
            {
                Coolant selectedCoolant = lCoolants.Find(item => item.getName() == (string)selectedCoolantName);
                txtCoolantA.Text = selectedCoolant.getA().ToString();
                txtCoolantF.Text = selectedCoolant.getF().ToString();
                txtCoolantT.Text = selectedCoolant.getT().ToString();
            }
        }

        private void TxtCoefficient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
            {
                if (!char.IsDigit(ch) && ch != '.')
                    e.Handled = true;
            }
        }

        ModelNuclearReactor getModel()
        {
            return modelReactor;
        }
    }
}

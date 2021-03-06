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
using System.Text.RegularExpressions;
using System.Globalization;

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
        List<Reactor> reactors;

        private class Reactor
        {
            private string name;
            private string W;
            private Fuel fuel;
            private Coolant coolant;
            private float alphaFt;
            private float t0;

            public Reactor(string name, string W, Fuel fuel, Coolant coolant, float alphaFt, float t0)
            {
                this.name = name;
                this.W = W;
                this.fuel = fuel;
                this.coolant = coolant;
                this.alphaFt = alphaFt;
                this.t0 = t0;
            }

            public string GetName()
            {
                return name;
            }

            public string GetW()
            {
                return W;
            }

            public Fuel GetFuel()
            {
                return fuel;
            }

            public Coolant GetCoolant()
            {
                return coolant;
            }

            public float GetAlphaFt()
            {
                return alphaFt;
            }

            public float GetT0()
            {
                return t0;
            }
        }

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
            Fuel dioxUr = new Fuel("Диоксид урана", 318, 10960, 7.026f);
            lFuels.Add(dioxUr);
            lFuels.Add(new Fuel("Металлический уран", 113, 18700, 0));
            lFuels.Add(new Fuel("Торий", 200, 11780, 0));

            lCoolants = new List<Coolant>();
            Coolant water = new Coolant("Вода", 5670, 620, 18, 1240, 4850, 0.68f);
            Coolant natrium = new Coolant("Натрий", 1220, 780, 1.11f, 0, 0, 0);
            lCoolants.Add(water);
            lCoolants.Add(new Coolant("Тяжёлая вода", 4208, 682, 0, 0, 0, 0));
            lCoolants.Add(new Coolant("Свинец", 127.5f, 7800, 0, 0, 0, 0));
            lCoolants.Add(natrium);

            reactors = new List<Reactor>();
            
            reactors.Add(new Reactor("РБМК-1000", "3200", dioxUr, water, 6.014f, 0.68f));
            Fuel dioxUr1 = new Fuel("Диоксид урана", 318, 10960, 2.46f);
            reactors.Add(new Reactor("БН-600", "1500", dioxUr1, natrium, 1.049f, 0.11f));

            foreach (Fuel itemFuels in lFuels)
            {
                cmbFuel.Items.Add(itemFuels.getName());
            }

            foreach (Coolant itemCoolants in lCoolants)
            {
                cmbCoolant.Items.Add(itemCoolants.getName());
            }

            foreach (Reactor reactor in reactors)
            {
                cmbReactor.Items.Add(reactor.GetName());
            }
        }

        private void Change_Reactor(object sender, SelectionChangedEventArgs e)
        {
            object selectedReactorName = cmbReactor.SelectedValue;
            Reactor selectedReactor = reactors.Find(item => item.GetName() == (string)selectedReactorName);

            txtInitPower.Text = selectedReactor.GetW();

            cmbFuel.Text = selectedReactor.GetFuel().getName();
            txtFuelC.Text = selectedReactor.GetFuel().getC().ToString();
            txtFuelP.Text = selectedReactor.GetFuel().getP().ToString();
            txtFuelV.Text = selectedReactor.GetFuel().getV().ToString();

            cmbCoolant.Text = selectedReactor.GetCoolant().getName().ToString();
            txtCoolantС.Text = selectedReactor.GetCoolant().getC().ToString();
            txtCoolantP.Text = selectedReactor.GetCoolant().getP().ToString();
            txtCoolantV.Text = selectedReactor.GetCoolant().getV().ToString();

            txtCoolantA.Text = selectedReactor.GetAlphaFt().ToString();
            txtCoolantT.Text = selectedReactor.GetT0().ToString();
            /*object selectedCoolantName = cmbCoolant.SelectedValue;
            if (selectedCoolantName != null)
            {
                Coolant selectedCoolant = lCoolants.Find(item => item.getName() == (string)selectedCoolantName);
                txtCoolantA.Text = selectedCoolant.getA().ToString();
                // txtCoolantF.Text = selectedCoolant.getF().ToString();
                txtCoolantT.Text = selectedCoolant.getT().ToString();
            }

            //Holding a*Ft
            if ((string)selectedCoolantName == "Вода" && (string)selectedFuelName == "Диоксид урана")
            {
                txtCoolantA.Text = "6.014";
            }
            else
            {
                txtCoolantA.Text = "0";
            }*/
        }

        private void Button_Click_Ref(object sender, RoutedEventArgs e) //кнопка "Справка"
        {
            StartModelWindow win = new StartModelWindow("Reference"); //вызываем окно справки
            win.ShowDialog();
        }

        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            StartModelWindow win = new StartModelWindow("Help"); //вызываем окно справки
            win.ShowDialog();
        }

        private void Button_Click_Tests(object sender, RoutedEventArgs e)
        {
            TestsWindow win = new TestsWindow();
            win.ShowDialog();
        }

        private static string TruncateMinuses(string input)
        {
            return Regex.Replace(input, @"-+", "-");
        }

        private void Button_Click_PlotStart(object sender, RoutedEventArgs e)
        {
            double startPower = 0;
            double coeffA = 0;
            double coeffB = 0;
            double coeffC = 0;

            

            if ((txtStartPower.Text) != "")
            {
                txtStartPower.Text = TruncateMinuses(txtStartPower.Text);
                startPower = double.Parse(txtStartPower.Text.Replace('.', ','));
            }
                

            if ((txtCoeffA.Text) != "")
            {
                txtCoeffA.Text = TruncateMinuses(txtCoeffA.Text);
                coeffA = double.Parse(txtCoeffA.Text.Replace('.', ','));
            }
           

            if ((txtCoeffB.Text) != "")
            {
                txtCoeffB.Text = TruncateMinuses(txtCoeffB.Text);
                coeffB = double.Parse(txtCoeffB.Text.Replace('.', ','));
            }


            if ((txtCoeffC.Text) != "")
            {
                txtCoeffC.Text = TruncateMinuses(txtCoeffC.Text);
                coeffC = double.Parse(txtCoeffC.Text.Replace('.', ','));
            }

           if(startPower == 0 && coeffA == 0 && coeffB  == 0 && coeffC == 0)
           {

                MessageBox.Show("Не введены параметры функции!", "Ошибка");

            }
            else
            {
                string args = startPower
                + " " + coeffA
                + " " + coeffB
                + " " + coeffC;

                Console.Write(args);

                System.Diagnostics.Process.Start("NuclearProject.exe", args.Replace(',', '.'));
            }

            


        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            try
            {
                float startPower = float.Parse(txtStartPower.Text) * 1000000.0f;
                float initPower = float.Parse(txtInitPower.Text) * 1000000.0f;
                float coeffA = float.Parse(txtCoeffA.Text.Replace('.',',')) ;
                float coeffB = float.Parse(txtCoeffB.Text.Replace('.', ','));
                float coeffC = float.Parse(txtCoeffC.Text.Replace('.', ','));

                float fuelC = float.Parse(txtFuelC.Text.Replace('.', ',')); //Ct
                float fuelP = float.Parse(txtFuelP.Text.Replace('.', ',')); //pT
                float fuelV = float.Parse(txtFuelV.Text.Replace('.', ',')); //Vt 
                // const1 = Ct*pT*Vt

                float coolantС = float.Parse(txtCoolantС.Text.Replace('.', ',')); //Сж
                float coolantP = float.Parse(txtCoolantP.Text.Replace('.', ',')); //рЖ
                float coolantV = float.Parse(txtCoolantV.Text.Replace('.', ',')); //Vж //const2


                if (startPower + initPower < 0)
                {
                    MessageBox.Show("Вы ввели значение параметра W0 функции отклонения мощности от номинальной больше значения самой номинальной мощности! Попробуйте ввести значение, меньшее по модулю.", "Ошибка");
                    return;
                }

                float coolantAlphaFT = float.Parse(txtCoolantA.Text.Replace('.', ',')) * 1000000.0f; //альфаFT

                float coolantT = float.Parse(txtCoolantT.Text.Replace('.', ',')); //t0

                modelReactor = new ModelNuclearReactor(fuelC, fuelP, fuelV, coolantС, coolantP, coolantV, coolantAlphaFT, coolantT);
                double fuelParams = modelReactor.getPt() * modelReactor.getVt() * modelReactor.getCt();
                double alphaF = modelReactor.getAlphaFT();
                double coolantParams = modelReactor.getP() * modelReactor.getC() * modelReactor.getV();
                float t0 = modelReactor.getT0();
                //int k = 5

                //EnvironmentPreset env = new EnvironmentPreset();
                session = new ModellingSession(fuelParams, alphaF, coolantParams, t0, coeffA, coeffB, coeffC, startPower, initPower); //создаем новую сессию
                this.DataContext = session;
                session.ModelNextNeutron();
                plotAverageTau.InvalidatePlot();
                plotEr.InvalidatePlot();

            }
            catch (System.FormatException exc)
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
               // txtCoolantF.Text = selectedCoolant.getF().ToString();
                txtCoolantT.Text = selectedCoolant.getT().ToString();
            }

            //Holding a*Ft
            if ((string)selectedCoolantName == "Вода" && (string)selectedFuelName == "Диоксид урана")
            {
                txtCoolantA.Text = "6.014";
            }
            else
            {
                txtCoolantA.Text = "0";
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
               // txtCoolantF.Text = selectedCoolant.getF().ToString();
                txtCoolantT.Text = selectedCoolant.getT().ToString();
            }

            //Holding a*Ft
            if ((string)selectedCoolantName == "Вода" && (string)selectedFuelName == "Диоксид урана")
            {
                txtCoolantA.Text = "6.014";
            } else
            {
                txtCoolantA.Text = "0";
            }
        }

        private void TxtCoefficient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
            {
                if (!char.IsDigit(ch)
                    && ch != '.'
                    && ch != '-')
                    e.Handled = true;
            }
        }

        ModelNuclearReactor getModel()
        {
            return modelReactor;
        }
    }
}

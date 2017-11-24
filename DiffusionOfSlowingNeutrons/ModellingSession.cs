using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace NuclearProject
{
    class ModellingSession
    {
        Model model; //экспериментальная модель
        List<DataPoint> deltaTtPoints, deltaT2Points, averageTau, deltaT2; //данные для графиков
        Random rnd;

        public ModellingSession(EnvironmentPreset env, double fuelParams, double alphaF, double coolantParams, float t0, float coeffA, float coeffB, float coeffC, float startPower)
        {
            this.rnd = new Random();
            this.model = new Model(env, fuelParams, alphaF, coolantParams, t0, coeffA, coeffB, coeffC, startPower);
            this.averageTau = new List<DataPoint>();
            this.deltaT2 = new List<DataPoint>();
            this.deltaTtPoints = new List<DataPoint>();
            this.deltaT2Points = new List<DataPoint>();
        }
        
        private double stepE(double E)
        {
            if (E == Model.Et)
                return 0;

            double result;

            if (E / 100.0 > 0.001)
                result = E / 100.0;
            else
                result = E - 0.001;

            if (result < Model.Et)
                result = Model.Et;

            return result;
        }

        public void ModelNextNeutron() //промоделировать судьбу одного нейтрона
        {
            this.averageTau.Clear();
            this.deltaT2.Clear();
            this.deltaTtPoints = model.getDeltaTtPoints();
            this.deltaT2Points = model.getDeltaT2Points();
            //List<DataPoint> deltaTtPoints = model.getDeltaTtPoints();
            //List<DataPoint> deltaT2Points = model.getDeltaT2Points();
            foreach (var point in deltaTtPoints)
            {
                this.averageTau.Add(point);
            }

            foreach (var point in deltaT2Points)
            {
                this.deltaT2.Add(point);
            }
    }


        public List<DataPoint> AverageTau //средний возраст нейтрона от E
        {
            get
            {
                return averageTau;
            }
        }
        public List<DataPoint> DeltaT2
        {
            get
            {
                return deltaT2;
            }
        }
    }
}

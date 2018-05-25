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
        private readonly List<DataPoint> averageTau, deltaT2; //данные для графиков
        private Dictionary<DataPoint, DataPoint> points;

        public ModellingSession(double fuelParams, double alphaF, double coolantParams, double t0, 
            double coeffA, double coeffB, double coeffC, double startPower, double initPower)
        {
            model = new Model(fuelParams, alphaF, coolantParams, t0, coeffA, coeffB, coeffC, startPower, initPower);
            averageTau = new List<DataPoint>();
            deltaT2 = new List<DataPoint>();
        }
        
        public void ModelNextNeutron() 
        {
            points = model.GetPoints();

            averageTau.Clear();
            deltaT2.Clear();

            foreach (var point in points)
            {
                averageTau.Add(point.Key);
                deltaT2.Add(point.Value);
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

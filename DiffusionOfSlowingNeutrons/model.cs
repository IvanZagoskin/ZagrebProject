using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Extreme.Numerics.QuickStart.CSharp;

namespace NuclearProject
{
    
    class Model
    {
        // начальные параметры
        private readonly RungeKutta rungeKutta;


        //конструктор
        public Model(double fuelParams, double alphaF, double coolantParams, double t0, double coeffA, double coeffB, 
            double coeffC, double startPower, double initPower)
        {

            rungeKutta = new RungeKutta(fuelParams, alphaF, coolantParams, t0, coeffA, coeffB, coeffC, startPower, initPower);
    
        }

        public Dictionary<DataPoint, DataPoint> GetPoints()
        {

            return rungeKutta.Solve();
        }
    }
}

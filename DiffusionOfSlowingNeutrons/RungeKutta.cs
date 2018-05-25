using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using Extreme.Mathematics;
using Extreme.Mathematics.Calculus.OrdinaryDifferentialEquations;

namespace Extreme.Numerics.QuickStart.CSharp
{
    using Extreme.Mathematics;
    using Extreme.Mathematics.Calculus.OrdinaryDifferentialEquations;

    class RungeKutta
    {
        private readonly double fuelParams, alphaF, coolantParams;
        private readonly double t0, coeffA, coeffB, coeffC, startPower, initPower;
        private const double finalPoint = 30.0;
        private const double step = 0.5;
        private const double startPoint = 0.0;

        public RungeKutta(double fuelParams, double alphaF, double coolantParams, double t0, double coeffA, double coeffB, 
            double coeffC, double startPower, double initPower)
        {
            this.fuelParams = fuelParams;
            this.alphaF = alphaF;
            this.coolantParams = coolantParams;
            this.coeffA = coeffA;
            this.coeffB = coeffB;
            this.coeffC = coeffC;
            this.t0 = t0;
            this.initPower = initPower;
            this.startPower = startPower;
        }

       public Dictionary<DataPoint, DataPoint> Solve()
        {

            ClassicRungeKuttaIntegrator rk4 = new ClassicRungeKuttaIntegrator();
            var points = new Dictionary<DataPoint, DataPoint>();
            rk4.DifferentialFunction = Lorentz;

            rk4.InitialTime = 0.0;
            rk4.InitialValue = Vector.Create(0.0, 0.0);

            rk4.InitialStepsize = 0.1;

            for (double point = startPoint; point <= 30.0; point += step)
            {
                var y = rk4.Integrate(point);
             
                points.Add(new DataPoint(point, y[0]), new DataPoint(point, y[1]));
            }

            return points;
        }

        /// <summary>
        /// Represents the differential function for the Lorentz attractor.
        /// </summary>
        /// <param name="t">The time value.</param>
        /// <param name="y">The current value.</param>
        /// <param name="dy">On output, the first derivatives.</param>
        /// <returns>A reference to <paramref name="dy"/>.</returns>
        /// <remarks><paramref name="dy"/> may be <see langword="null"/>
        /// on input.</remarks>
        private Vector<double> Lorentz(double t, Vector<double> y, Vector<double> dy)
        {
            if (dy == null)
                dy = Vector.Create<double>(3);

            //BN
            //dy[0] = (1.0 / 8573788.8) * (100000000.0 + 10.0 * t - 1049000.0 * (y[0] - 0.5 * y[1]));

            //dy[1] = (2.0 / 1056276.0) * (1049000.0 * (y[0] - 0.5 * y[1]) - 9602509.091 * y[1]);

            //dy(1) = (1 / 24487577.28) * (100000000 + 10 * t - 6014000.0 * (y(1) - 0.5 * y(2)));

            //dy(2) = (2 / 63277200.0) * (6014000.0 * (y(1) - 0.5 * y(2)) - 93054705.882 * y(2));

            //RBMK
            //dy[0] = (1.0 / 24487577.28) * (100000000.0 + 10.0 * t - 6014000.0 * (y[0] - 0.5 * y[1]));

            //dy[1] = (2.0 / 63277200.0) * (6014000.0 * (y[0] - 0.5 * y[1]) - 93054705.882 * y[1]);

            double c3 = coolantParams / t0;

            dy[0] = (1.0 / fuelParams) * (startPower + coeffA * t +coeffB * t * t + coeffC * t * t * t - alphaF * (y[0] - 0.5 * y[1]));
            dy[1] = (2.0 / coolantParams) * (alphaF * (y[0] - 0.5 * y[1]) - c3 * y[1]);

            return dy;
        }
    }


}

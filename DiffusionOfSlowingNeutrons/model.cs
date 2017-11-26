using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
namespace NuclearProject
{

    public struct ResultPoint
    {
        public Vector3D Position;
        public double Energy;
    }

    class Model
    {
        EnvironmentPreset data; //массовые числа ядер и соответствующие макроконстанты
        private double fuelParams, alphaF, coolantParams;
        private float t0, coeffA, coeffB, coeffC, startPower;
        List<DataPoint> deltaTtPoints;
        List<DataPoint> deltaT2Points;
        public const float finalPoint = 10.0f;
        public const float step = 0.5f;
        public const float startPoint = 0.0f;
        public const double Et = 0.025;
        //конструктор
        public Model(EnvironmentPreset env, double fuelParams, double alphaF, double coolantParams, float t0, float coeffA, float coeffB, float coeffC, float startPower)
        {
            data = env;
            this.fuelParams = fuelParams;
            this.alphaF = alphaF;
            this.coolantParams = coolantParams;
            this.t0 = t0;
            this.coeffA = coeffA;
            this.coeffB = coeffB;
            this.coeffC = coeffC;
            this.startPower = startPower;
            this.deltaTtPoints = new List<DataPoint>();
            this.deltaT2Points = new List<DataPoint>();
        }
        //свойства
        public EnvironmentPreset Data 
        {
            get { return data; }
            set { data = value; }
        }


        public List<DataPoint> getDeltaTtPoints()
        {
            for(float point = startPoint; point <= finalPoint; point += step)
            {
                this.deltaTtPoints.Add(new DataPoint(point, this.getDeltaTt(point)));
            }

            return this.deltaTtPoints;
        }
        public List<DataPoint> getDeltaT2Points()
        {
            for (float point = startPoint; point <= finalPoint; point += step)
            {
                this.deltaT2Points.Add(new DataPoint(point, this.getDeltaT2(point)));
            }

            return this.deltaT2Points;
        }

        private double getFirstEquationResult(double deltaW, double deltaTt, double deltaT2)
        {

            return (1 / this.fuelParams) * (deltaW - (this.alphaF * (deltaTt - deltaT2 / 2)));
        }

        private double getSecondEquationResult(double deltaTt, double deltaT2)
        {
            return (2 / this.coolantParams) * (this.alphaF * (deltaTt - deltaT2 / 2) - (this.coolantParams / this.t0) * deltaT2);
        }

        private double getDeltaW(double t)
        {
            return this.startPower + this.coeffA * t + this.coeffB * Math.Pow(t, 2) + this.coeffC * Math.Pow(t, 3);
        }

        private double getDeltaTt(double k)
        {
            float t0 = this.t0;
            if (k < t0)
            {
                return 0;
            }
            double prevDeltaTt = getDeltaTt(k - t0);
            double prevDeltaT2 = getDeltaT2(k - t0);
            double k1 = getFirstEquationResult(getDeltaW(k - 1), prevDeltaTt, prevDeltaT2);
            double m1 = getSecondEquationResult(prevDeltaTt, prevDeltaT2);
            double k2 = getFirstEquationResult(getDeltaW(k - 1 + t0 / 2), prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double m2 = getSecondEquationResult(prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double k3 = getFirstEquationResult(getDeltaW(k - 1 + t0 / 2), prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double m3 = getSecondEquationResult(prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double k4 = getFirstEquationResult(getDeltaW(k - 1 + t0), prevDeltaTt + k3, prevDeltaT2 + m3);
            double m4 = getSecondEquationResult(prevDeltaTt + k3, prevDeltaT2 + m3);

            return prevDeltaTt + (k1 + 2 * k2 + 2 * k3 + k4) * t0 / 6;
        }

        private double getDeltaT2(double k)
        {
            float t0 = this.t0;
            if (k < t0)
            {
                return 0;
            }
            double prevDeltaTt = getDeltaTt(k - t0);
            double prevDeltaT2 = getDeltaT2(k - t0);
            double k1 = getFirstEquationResult(getDeltaW(k - 1), prevDeltaTt, prevDeltaT2);
            double m1 = getSecondEquationResult(prevDeltaTt, prevDeltaT2);
            double k2 = getFirstEquationResult(getDeltaW(k - 1 + t0 / 2), prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double m2 = getSecondEquationResult(prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double k3 = getFirstEquationResult(getDeltaW(k - 1 + t0 / 2), prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double m3 = getSecondEquationResult(prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double k4 = getFirstEquationResult(getDeltaW(k - 1 + t0), prevDeltaTt + k3, prevDeltaT2 + m3);
            double m4 = getSecondEquationResult(prevDeltaTt + k3, prevDeltaT2 + m3);

            return prevDeltaT2 + (m1 + 2 * m2 + 2 * m3 + m4) * t0 / 6;
        }
    }
}

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
        // начальные параметры
        private double fuelParams, alphaF, coolantParams; 
        private float t0, coeffA, coeffB, coeffC, startPower, initPower;
        List<DataPoint> deltaTtPoints;
        List<DataPoint> deltaT2Points;
        public const double finalPoint = 10.0;
        public const double step = 0.5;
        public const double startPoint = 0.0;
        public const double Et = 0.025;
        private double deltaDeltaT2 = 0;
        private bool isDecrease = false;
        private double maxValue = 0;
        private double multiplier = 0;
        private bool isTimeToIncreaseAccuracy = false;
        Dictionary<double, double> deltaTtValues = new Dictionary<double, double>();
        Dictionary<double, double> deltaT2Values = new Dictionary<double, double>();
        private double deltaT = 0.1;

        //конструктор
        public Model(EnvironmentPreset env, double fuelParams, double alphaF, double coolantParams, float t0, float coeffA, float coeffB, float coeffC,float startPower, float initPower)
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
            this.initPower = initPower;
            this.deltaTtPoints = new List<DataPoint>();
            this.deltaT2Points = new List<DataPoint>();
        }
        //свойства
        public EnvironmentPreset Data
        {
            get { return data; }
            set { data = value; }
        }

        // функция вывода точек первого графика на экран
        public List<DataPoint> getDeltaTtPoints()
        {
            isDecrease = false;
            maxValue = 0;
            multiplier = 0;
            isTimeToIncreaseAccuracy = false;
            deltaTtValues = new Dictionary<double, double>();
            deltaT2Values = new Dictionary<double, double>();
            deltaTtPoints = new List<DataPoint>();
            deltaT2Points = new List<DataPoint>();
            for (double point = startPoint; point <= finalPoint; point += step)
            {
                try
                {
                    this.deltaTtPoints.Add(new DataPoint(point, this.getDeltaTt(point)));
                } catch
                {
                    deltaTtPoints.Add(new DataPoint(point, getDeltaTt(point)));
                }
                
            }

            return this.deltaTtPoints;
        }
        // функция вывода точек второго графика на экран
        public List<DataPoint> getDeltaT2Points()
        {
            isDecrease = false;
            maxValue = 0;
            multiplier = 0;
            isTimeToIncreaseAccuracy = false;
            deltaTtValues = new Dictionary<double, double>();
            deltaT2Values = new Dictionary<double, double>();
            deltaTtPoints = new List<DataPoint>();
            deltaT2Points = new List<DataPoint>();
            getDeltaT2(15);
            deltaT2Values.Add(0, 0);
            for (double point = startPoint; point <= finalPoint; point += step)
            {
                //Console.WriteLine("point = " + point);
                try
                {
                    deltaT2Points.Add(new DataPoint(point, deltaT2Values[point]));
                }
                catch
                {
                    deltaT2Points.Add(new DataPoint(point, getDeltaT2(point)));
                }
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

        // функция расчета точек первого уравнения
        private double getDeltaTt(double k)
        {
            k = round(k, 1);
            if (Math.Abs(getDeltaW(k - deltaT)) > initPower && getDeltaW(k - deltaT) < 0)
            {
                deltaTtValues.Add(k, 0);
                return 0;
            }
            if (deltaTtValues.ContainsKey(k))
            {
                return deltaTtValues[k];
            }
            if (k < deltaT)
            {
                return 0;
            }
            double prevDeltaTt = getDeltaTt(k - deltaT);
            double prevDeltaT2 = getDeltaT2(k - deltaT);
            double k1 = getFirstEquationResult(getDeltaW(k - deltaT), prevDeltaTt, prevDeltaT2);
            double m1 = getSecondEquationResult(prevDeltaTt, prevDeltaT2);
            double k2 = getFirstEquationResult(getDeltaW(k - deltaT / 2), prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double m2 = getSecondEquationResult(prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double k3 = getFirstEquationResult(getDeltaW(k - deltaT / 2), prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double m3 = getSecondEquationResult(prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double k4 = getFirstEquationResult(getDeltaW(k), prevDeltaTt + k3, prevDeltaT2 + m3);
            double m4 = getSecondEquationResult(prevDeltaTt + k3, prevDeltaT2 + m3);

            double currDeltaTt = prevDeltaTt + (k1 + 2 * k2 + 2 * k3 + k4) * deltaT / 6;
            deltaTtValues.Add(k, currDeltaTt);
            return currDeltaTt;
        }

        // функция расчета точек второго уравнения
        private double getDeltaT2(double k)
        {
            k = round(k, 1);
            if (Math.Abs(getDeltaW(k - deltaT)) > initPower && getDeltaW(k - deltaT) < 0)
            {
                deltaT2Values.Add(k, 0);
                return 0;
            }
            if (deltaT2Values.ContainsKey(k))
            {
                return deltaT2Values[k];
            }
            if (isTimeToIncreaseAccuracy)
            {
                return getAccurateValue(k);
            }
            if (k < deltaT)
            {
                return 0;
            }
            double prevDeltaTt = getDeltaTt(k - deltaT);
            double prevDeltaT2 = getDeltaT2(k - deltaT);
            double k1 = getFirstEquationResult(getDeltaW(k - deltaT), prevDeltaTt, prevDeltaT2);
            double m1 = getSecondEquationResult(prevDeltaTt, prevDeltaT2);
            double k2 = getFirstEquationResult(getDeltaW(k - deltaT / 2), prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double m2 = getSecondEquationResult(prevDeltaTt + k1 / 2, prevDeltaT2 + m1 / 2);
            double k3 = getFirstEquationResult(getDeltaW(k - deltaT / 2), prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double m3 = getSecondEquationResult(prevDeltaTt + k2 / 2, prevDeltaT2 + m2 / 2);
            double k4 = getFirstEquationResult(getDeltaW(k), prevDeltaTt + k3, prevDeltaT2 + m3);
            double m4 = getSecondEquationResult(prevDeltaTt + k3, prevDeltaT2 + m3);

            double currDeltaT2 = prevDeltaT2 + (m1 + 2 * m2 + 2 * m3 + m4) * deltaT / 6;

            if (deltaDeltaT2 == 0)
            {
                deltaDeltaT2 = Math.Abs(currDeltaT2);
                deltaT2Values.Add(k, currDeltaT2);
                return currDeltaT2;
            }

            double currDeltaDeltaT2 = currDeltaT2 - prevDeltaT2;
            if (Math.Abs(currDeltaDeltaT2) < deltaDeltaT2)
            {
                isDecrease = true;
                deltaT2Values.Add(k, currDeltaT2);
                return currDeltaT2;
            }
            else
            {
                if (isDecrease)
                {
                    if (prevDeltaT2 > 0)
                    {
                        maxValue = Math.Ceiling(prevDeltaT2);
                    } else
                    {
                        maxValue = Math.Round(prevDeltaT2);
                    }
                    multiplier = getMultiplier(k - deltaT, prevDeltaT2, maxValue);
                    isTimeToIncreaseAccuracy = true;
                    try
                    {
                        if (!deltaT2Values.ContainsKey(k))
                    {
                        deltaT2Values.Add(k, getAccurateValue(k));
                    }
                } catch (Exception e) { }
                return getAccurateValue(k);
                }
            }

            if (!deltaT2Values.ContainsKey(k))
            {
                deltaT2Values.Add(k, currDeltaT2);
            }
            return currDeltaT2;
        }

        // функция получения множителя для getAccurateValue 
        private double getMultiplier(double k, double funcValue, double maxFuncValue)
        {
            return (maxFuncValue - funcValue) / Math.Exp(-k);
        }

        // функция получения точного значения второго уравнения
        private double getAccurateValue(double k)
        {
            if (!deltaT2Values.ContainsKey(k))
            {
                deltaT2Values.Add(k, maxValue - multiplier * Math.Exp(-k));
            }
            return maxValue - multiplier * Math.Exp(-k);
        }

        public static double round(double value, int places)
        {
            return (double)Decimal.Round((decimal)value, 1);
        }
    }
}

using System;
namespace QAnalytics.Models
{
    public class LinReg
    {
        public double M;
        public double B;
        public double R;

        public double[] XValues;
        public double[] YValues;

        public double XMean;
        public double YMean;

        public double XStdDev;
        public double YStdDev;

        public LinReg(double[] xvals, double[] yvals)
        {
            this.XValues = xvals;
            this.YValues = yvals;
        }

        public LinReg Calc(){
            this.XMean = CalcMean(this.XValues);
            this.YMean = CalcMean(this.YValues);

            this.XStdDev = CalcStdDev(this.XValues, this.XMean);
            this.YStdDev = CalcStdDev(this.YValues, this.YMean);

            this.R = CalcR(this.XValues, this.YValues, this.XMean, this.YMean, this.XStdDev, this.YStdDev);

            this.M = this.R * (this.YStdDev / this.XStdDev);

            this.B = this.YMean - (this.M * this.XMean);

            return this;
        }

        public static double CalcMean(double[] vals){
            double sum = 0;
            foreach(double n in vals){
                sum += n;
            }
            return sum / (double)vals.Length;
        }

        public static double CalcStdDev(double[] vals, double mean){
            double sum = 0;

            foreach(double n in vals){
                sum += Math.Pow(mean - n, 2);
            }

            return Math.Sqrt(sum / (double) vals.Length);
        }

        public static double CalcR(double[] xvals, double[] yvals, double xmean, double ymean, double xstd, double ystd){
            if (xvals.Length != yvals.Length) throw new Exception("X and Y arrays must be same length!");
            double sum = 0;

            for (int i = 0; i < xvals.Length; i++){
                sum += (xvals[i] - xmean) * (yvals[i] - ymean);
            }
            sum /= (xstd * ystd);
            return sum / (xvals.Length - 1);
        }
    }
}

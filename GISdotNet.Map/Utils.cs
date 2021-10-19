using System;


namespace GISdotNet.Map
{
    class Utils
    {
        public static double GetVectorLength(int a, int b)
        {
            return (double)Math.Sqrt(Math.Pow(Math.Abs(a), 2) + Math.Pow(Math.Abs(b), 2));
        }

        public static double CalculateVectorAngle(double x, double y) 
        {
            double tan;
            double angle = 0;
            switch (x > 0)
            {
                case true when y > 0:
                    tan = y / x;
                    angle = Math.Atan(tan);
                    break;
                case true when y < 0:
                    tan = x / y;
                    angle = (-(Math.Atan(tan))) + 4.71;
                    break;
                case true when y == 0:                   
                    angle = 0;
                    break;
                case false when y == 0:                    
                    angle = 3.14;
                    break;
                case false when y > 0:
                    tan = x / y;
                    angle = (-(Math.Atan(tan))) + 1.57;
                    break;
                case false when y < 0:
                    tan = y / x;
                    angle = Math.Atan(tan) + 3.14;
                    break;
            }

            return angle;
        }

    }
}

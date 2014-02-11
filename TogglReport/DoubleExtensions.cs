//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TogglReport
//{
//    public static class DoubleExtensions
//    {
//        public static double RoundI(this double number, double roundingInterval)
//        {
//            if (roundingInterval == 0) { return 0; }

//            double intv = Math.Abs(roundingInterval);
//            double modulo = number % intv;
//            if ((intv - modulo) == modulo)
//            {
//                var temp = (number - modulo).ToString("#.##################");
//                if (temp.Length != 0 && temp[temp.Length - 1] % 2 == 0) modulo *= -1;
//            }
//            else if ((intv - modulo) < modulo)
//                modulo = (intv - modulo);
//            else
//                modulo *= -1;

//            return number + modulo;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using static XamalTiler.Create_Image;
using static XamalTiler.Colour_Class;

namespace XamalTiler
{
	public partial class Game1
	{

        #region Formula Variables

        internal static class Hits
        {
            internal static bool _maxHitsCounted = false;

            static List<Location> _locations = new List<Location>();

            internal struct Location
            {
                internal int _x, _y;

                internal Location(int x, int y)
                {
                    _x = x; _y = y;
                }
            }


            internal static void ClearList()
            {
                _locations = new List<Location>();
            }

            internal static void Add_Location(int x, int y)
            {
                _locations.Add(new Location(x, y));
            }


            internal static void Add_FieldData_To_PixelData()
            {
                int x, y, index, hit, cscount = _currentColorSet._colorSpreadCount - 1;
                long hits;

                Ensure_Spread_Compliance();

                foreach (var local in _locations)
                {
                    x = local._x;
                    y = local._y;

                    hits = ++_field[x, y];

                    if (hits > _maxHits) _maxHits = hits;
                    if (hits > cscount) hits = cscount;

                    hit = _adjustSpreadVertices[(int)hits];

                    index = y * _stride + x * 4;

                    _pixelData[index] = _colorSpread[hit].R;
                    _pixelData[index + 1] = _colorSpread[hit].G;
                    _pixelData[index + 2] = _colorSpread[hit].B;
                }

                _maxHitsCounted = true;
            }
        }


        public class IconTrigValues
        {
            public int _sectors;

            public double[] c;
            public double[] s;

            public IconTrigValues(int sectors)
            {
                if (sectors < 3) sectors = 3;

                _sectors = sectors;

                c = new double[sectors];
                s = new double[sectors];

                for (int i = 0; i < sectors; i++)
                {
                    c[i] = Math.Cos(2 * Math.PI * i / sectors);
                    s[i] = Math.Sin(2 * Math.PI * i / sectors);
                }
            }
        }

        public class Position
        {
            public double x, y;

            public Position(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public class SquareValues : Position
        {
            #region Variable Declaration

            public double alpha, beta, gamma, lambda, ma, omega;
            public int trig;

            #endregion

            public SquareValues(SquareValues toCloneValues) :
                base(toCloneValues.x, toCloneValues.y)
            {
                Populate_Greek(toCloneValues);
            }

            /// <summary>
            /// Instantiate copy of new Values set to Position x, y
            /// </summary>
            /// <param name="position"></param>
            /// <param name="toCloneValues"></param>
            public SquareValues(Position position, SquareValues toCloneValues) :
                base(position.x, position.y)
            {
                Populate_Greek(toCloneValues);
            }

            /// <summary>
            /// Instantiate new SquareValues 
            /// </summary>
            /// <param name="Position.x"></param>
            /// <param name="Position.y"></param>
            /// <param name="aplha"></param>
            /// <param name="beta"></param>
            /// <param name="gamma"></param>
            /// <param name="lamda"></param>
            /// <param name="ma"></param>
            /// <param name="omega"></param>
            public SquareValues(
                double x, double y, double aplha, double beta, double gamma,
                double lamda, double ma, double omega, int trig = 17) : base(x, y)
            {
                this.alpha = aplha;
                this.beta = beta;
                this.gamma = gamma;
                this.lambda = lamda;
                this.ma = ma;
                this.omega = omega;
                this.trig = trig;
            }


            private void Populate_Greek(SquareValues values)
            {
                this.alpha = values.alpha;
                this.beta = values.beta;
                this.gamma = values.gamma;
                this.lambda = values.lambda;
                this.ma = values.ma;
                this.omega = values.omega;
                this.trig = values.trig;
            }

        }

        public class HexagonValues
        {
            #region Variable Declaration

            public bool sk11Lock, sk12Lock, sk21Lock, sel1Lock, sel2Lock;

            public double k11, k12, k21, k22, el11, el12, el21, el22, el31, el32;
            public double em11, em12, em21, em22, em31, em32;
            public double en11, en12, en21, en22, en31, en32;
            public double enh11, enh12, enh21, enh22, enh31, enh32;

            public double a11, a12, a21, a22, a31, a32;
            public double ah11, ah12, ah21, ah22, ah31, ah32;

            #endregion


            public HexagonValues(double beta, double gamma,
                double sk11 = 1.0, double sk12 = 0.0, double sk21 = 0.5,
                double sel11 = 1.0, double sel21 = 0.0)
            {
                this.a11 = beta; this.a12 = gamma;

                this.k11 = sk11; this.k12 = sk12;
                this.k21 = sk21;
                this.el11 = sel11; this.el21 = sel21;

                Calculate_Values();
            }

            public HexagonValues(HexagonValues toCopyHex)
            {
                this.a11 = toCopyHex.a11; this.a12 = toCopyHex.a12;

                this.k11 = toCopyHex.k11; this.k12 = toCopyHex.k12;
                this.k21 = toCopyHex.k21;
                this.el11 = toCopyHex.el11; this.el21 = toCopyHex.el21;

                Calculate_Values();
            }


            public void Change_Beta(double value)
            {
                this.a11 = value;

                Calculate_Values();
            }
            public void Change_Gamma(double value)
            {
                this.a12 = value;

                Calculate_Values();
            }

            public void Change_Sk11(double value)
            {
                this.k11 = value;

                Calculate_Values();
            }
            public void Change_Sk12(double value)
            {
                this.k12 = value;

                Calculate_Values();
            }
            public void Change_Sk21(double value)
            {
                this.k21 = value;

                Calculate_Values();
            }
            public void Change_Sel11(double value)
            {
                this.el11 = value;

                Calculate_Values();
            }
            public void Change_Sel21(double value)
            {
                this.el21 = value;

                Calculate_Values();
            }


            private void Calculate_Values()
            {
                this.k22 = sq3 / 2; this.el22 = 2 / sq3;

                this.el12 = -1 / sq3; this.el22 = 2 / sq3;
                this.el31 = -(el11 + el21); el32 = -(el12 + el22);

                this.em11 = 2 * el11 + el21; this.em12 = 2 * el12 + el22;
                this.em21 = 2 * el21 + el31; this.em22 = 2 * el22 + el32;
                this.em31 = 2 * el31 + el11; this.em32 = 2 * el32 + el12;

                this.en11 = 3 * el11 + 2 * el21; this.en12 = 3 * el12 + 2 * el22;
                this.en21 = 3 * el21 + 2 * el31; this.en22 = 3 * el22 + 2 * el32;
                this.en31 = 3 * el31 + 2 * el11; this.en32 = 3 * el32 + 2 * el12;

                this.enh11 = 3 * el11 + el21; this.enh12 = 3 * el12 + el22;
                this.enh21 = 3 * el21 + el31; this.enh22 = 3 * el22 + el32;
                this.enh31 = 3 * el31 + el11; this.enh32 = 3 * el32 + el12;

                SetVectorThree(a11, a12);
            }

            public void SetVectorThree(SquareValues hexagonSqr)
            {
                SetVectorThree(hexagonSqr.beta, hexagonSqr.gamma);
            }
            public void SetVectorThree(double beta, double gamma)
            {
                a11 = beta; a12 = gamma;
                a21 = (-a11 - sq3 * a12) / 2; a22 = (sq3 * a11 - a12) / 2;
                a31 = -a11 - a21; a32 = -a12 - a22;

                ah11 = a11; ah12 = -a12;
                ah21 = (-ah11 - sq3 * ah12) / 2; ah22 = (sq3 * ah11 - ah12) / 2;
                ah31 = -ah11 - ah21; ah32 = -ah12 - ah22;
            }
        }

        public class HyperDegreeValues
        {
            public bool doDeltaNP;

            public bool dLock, npdLock, sLock;
            public int degree, nPDegree;

            public double scale;

            /// <summary>
            /// Instantiate hyperDegreeValues
            /// </summary>
            /// <param name="degree"></param>
            /// <param name="nPDegree"></param>
            /// <param name="scale"></param>
            /// <param name="doDeltaNP">Use Degree Values</param>
            public HyperDegreeValues(int degree, int nPDegree, double scale = 1.0, bool doDeltaNP = false)
            {
                this.degree = degree;
                this.nPDegree = nPDegree;
                this.scale = scale;
                this.doDeltaNP = doDeltaNP;
            }

            /// <summary>
            /// Instantiate Cloned hyperDegreeValues
            /// </summary>
            /// <param name="hyperDegreeValues"></param>
            public HyperDegreeValues(HyperDegreeValues hyperDegreeValues)
            {
                this.degree = hyperDegreeValues.degree;
                this.nPDegree = hyperDegreeValues.nPDegree;
                this.scale = hyperDegreeValues.scale;
                this.doDeltaNP = hyperDegreeValues.doDeltaNP;
            }
        }

        #endregion


    }
}
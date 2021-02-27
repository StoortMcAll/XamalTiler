using System;
using System.Collections.Generic;

using static XamalTiler.Game1;
using static XamalTiler.Create_Image;

namespace XamalTiler
{
	internal static class Iterate_Class
	{

        internal static void IterateTask()
        {
            while (_iterateState != IterateState.StopIterate)
            {
				switch (_iterateState)
				{
					case IterateState.Iterating:
                        Hits._maxHitsCounted = false;

						switch (_quiltType)
						{
							case QuiltType.Square:
                                Do_SquareTask();
                                break;
							case QuiltType.Hexagon:
                                Do_HexagonTask();
                                break;
							case QuiltType.Icon:
                                Do_IconTask();
								break;
							default:
								break;
						}
						
						break;
					case IterateState.PollingData:
                        Poll_FieldData();

                        if (_iterateState != IterateState.StoppingIterate)
                            _iterateState = IterateState.Iterating;

                        if (_iterCounter > 1000)
                            if (_maxHits > _iterCounter / 2)
                                _fatalValues = true;
                       
                        break;
                    case IterateState.StoppingIterate:
                        if (!_isDataPolled) Poll_FieldData();

                     //   _isDataPolled = false;

                      //  Create_Image.Set_Texture_PixelData();

                        _iterateState = IterateState.StopIterate;
                        break;
					default:
						break;
				}
			}
        }

        internal static void Do_SquareTask()
        {
            double xnew, ynew, x = _square.x; double y = _square.y;
            
            int wid = _fieldWidth, hgt = _fieldHeight;

            do
            {
                double p2x = p2 * x; double p2y = p2 * y;
                double sx = Math.Sin(p2x); double sy = Math.Sin(p2y);

                xnew = (_square.lambda + _square.alpha * Math.Cos(p2y)) * sx
                       - _square.omega * sy
                       + _square.beta * Math.Sin(2 * p2x)
                       + _square.gamma * Math.Sin(3 * p2x) * Math.Cos(2 * p2y)
                       + _square.ma * x;

                ynew = (_square.lambda + _square.alpha * Math.Cos(p2x)) * sy
                       + _square.omega * sx
                       + _square.beta * Math.Sin(2 * p2y)
                       + _square.gamma * Math.Sin(3 * p2y) * Math.Cos(2 * p2x)
                       + _square.ma * y;

                xnew = (xnew - (int)xnew) + 1;
                xnew -= (int)xnew;
                ynew = (ynew - (int)ynew) + 1;
                ynew -= (int)ynew;
                
                x = xnew; y = ynew;

                Hits.Add_Location((int)(x * wid), (int)(y * hgt));

                _iterations++;

            } while (_iterateState == IterateState.Iterating);

            _square.x = x; _square.y = y;

            _iterCounter += _iterations;
          
            _iterations = 0;
        }


        internal static void Do_IconTask()
        {
            double xnew, ynew, x = _square.x;
            double y = _square.y;
            
            int wid = _fieldWidth, hgt = _fieldHeight;

            do
            {
                xnew = _square.alpha * x + _square.beta * y + _square.ma;
                ynew = _square.gamma * x + _square.lambda * y + _square.omega;

                int serpoint = _rand.Next(_square.trig);
                x = xnew; y = ynew;

                xnew = _itrigValues.c[serpoint] * x - _itrigValues.s[serpoint] * y;
                ynew = _itrigValues.s[serpoint] * x - _itrigValues.c[serpoint] * y;

                x = (xnew - (int)xnew) + 1;
                x -= (int)x;
                y = (ynew - (int)ynew) + 1;
                y -= (int)y;

                Hits.Add_Location((int)(x * wid), (int)(y * hgt));

                x = xnew; y = ynew;

                _iterations++;

            } while (_iterateState == IterateState.Iterating);

            _square.x = x; _square.y = y;

            _iterCounter += _iterations;

            _iterations = 0;
        }


        internal static void Do_HexagonTask()
        {
            double bx, by, xnew = _square.x, ynew = _square.y;

            int wid = _fieldWidth, hgt = _fieldHeight;

            do
            {
                double s11 = Math.Sin(p2 * (_hexValues.el11 * xnew + _hexValues.el12 * ynew));
                double s12 = Math.Sin(p2 * (_hexValues.el21 * xnew + _hexValues.el22 * ynew));
                double s13 = Math.Sin(p2 * (_hexValues.el31 * xnew + _hexValues.el32 * ynew));
                double s21 = Math.Sin(p2 * (_hexValues.em11 * xnew + _hexValues.em12 * ynew));
                double s22 = Math.Sin(p2 * (_hexValues.em21 * xnew + _hexValues.em22 * ynew));
                double s23 = Math.Sin(p2 * (_hexValues.em31 * xnew + _hexValues.em32 * ynew));
                double s31 = Math.Sin(p2 * (_hexValues.en11 * xnew + _hexValues.en12 * ynew));
                double s32 = Math.Sin(p2 * (_hexValues.en21 * xnew + _hexValues.en22 * ynew));
                double s33 = Math.Sin(p2 * (_hexValues.en31 * xnew + _hexValues.en32 * ynew));
                double s3h1 = Math.Sin(p2 * (_hexValues.enh11 * xnew + _hexValues.enh12 * ynew));
                double s3h2 = Math.Sin(p2 * (_hexValues.enh21 * xnew + _hexValues.enh22 * ynew));
                double s3h3 = Math.Sin(p2 * (_hexValues.enh31 * xnew + _hexValues.enh32 * ynew));

                double sx = (_hexValues.el11 * s11 + _hexValues.el21 * s12 + _hexValues.el31 * s13);
                double sy = (_hexValues.el12 * s11 + _hexValues.el22 * s12 + _hexValues.el32 * s13);

                xnew = _square.ma * xnew + _square.lambda * sx - _square.omega * sy;
                ynew = _square.ma * ynew + _square.lambda * sy + _square.omega * sx;
                xnew += _square.alpha * (_hexValues.em11 * s21 + _hexValues.em21 * s22 + _hexValues.em31 * s23);
                ynew += _square.alpha * (_hexValues.em12 * s21 + _hexValues.em22 * s22 + _hexValues.em32 * s23);
                xnew += _hexValues.a11 * s31 + _hexValues.a21 * s32 + _hexValues.a31 * s33;
                ynew += _hexValues.a12 * s31 + _hexValues.a22 * s32 + _hexValues.a32 * s33;
                xnew += _hexValues.ah11 * s3h1 + _hexValues.ah21 * s3h2 + _hexValues.ah31 * s3h3;
                ynew += _hexValues.ah12 * s3h1 + _hexValues.ah22 * s3h2 + _hexValues.ah32 * s3h3;

                by = 2 * ynew / sq3; bx = xnew - by / 2;

                bx = (bx - (int)bx) + 1;
                bx -= (int)bx;
                by = (by - (int)by) + 1;
                by -= (int)by;

                xnew = bx * _hexValues.k11 + by * _hexValues.k21;
                ynew = bx * _hexValues.k12 + by * _hexValues.k22;

                Hits.Add_Location((int)(bx * wid), (int)(by * hgt));

                _iterations++;

            } while (_iterateState == IterateState.Iterating) ;

            _square.x = xnew; _square.y = ynew;

            _iterCounter += _iterations;

            _iterations = 0;
        }

    }
}
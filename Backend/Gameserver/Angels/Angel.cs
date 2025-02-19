namespace Gameserver.Angel
{
    public class Angle
    {
        private readonly int _numerator;
        private readonly int _denominator;

        public Angle(int numerator, int denominator = 1)
        {
            if (denominator < 0)
            {
                _numerator = -numerator;
                _denominator = -denominator;
            }
            else if (denominator == 0)
            {
                throw new ArgumentException("Zero denominator");
            }
            else
            {
                _numerator = numerator;
                _denominator = denominator;
            }

            var gcd = GCD(_numerator, _denominator);
            _numerator /= gcd;
            _denominator /= gcd;
        }

        private static int GCD(int a, int b)
        {
            b = Math.Abs(b);
            a = Math.Abs(a);
            while (a != 0 && b != 0)
            {
                if (a >= b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a | b;
        }

        private Angle Round(int k = 360)
        {
            if (k == 0)
            {
                throw new DivideByZeroException();
            }

            return new Angle(_numerator % (k * _denominator), _denominator);
        }

        public static Angle operator %(Angle A, int b) => A.Round(b);

        public override string ToString() => Math.Round(1d * _numerator / _denominator, 5).ToString().Replace(",", ".")
                                            + $" deg ({_numerator} / {_denominator})";

        public override bool Equals(object? obj) => obj is Angle angle &&
                                                    _numerator == angle._numerator &&
                                                    _denominator == angle._denominator;

        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

        public static Angle operator +(Angle A, Angle B)
        {
            var numerator = A._numerator * B._denominator + B._numerator * A._denominator;
            var denominator = A._denominator * B._denominator;
            return new Angle(numerator, denominator) % 360;
        }
    }
}

using Gameserver.Angel;


namespace Tests
{
    public class TAngle
    {
        [Fact]
        public void TestNotZeroAngle()
        {
            //Arrange, Act, Assert
            Assert.Equal("Zero denominator", Assert.Throws<ArgumentException>(() => new Angle(1, 0)).Message);
        }

        [Fact]
        public void TestToString()
        {
            //Arrange
            var A = new Angle(22, 7);

            //Act
            var output = A.ToString();

            //Assert
            Assert.Equal("3.14286 deg (22 / 7)", output);
        }

        [Fact]
        public void TestEqualsTrue()
        {
            //Arrange
            var A = new Angle(-123, 123);
            var B = new Angle(123, -123);

            //Act & Assert
            Assert.Equal(A, B);
        }

        [Fact]
        public void TestEqualsFalse()
        {
            //Arrange
            var A = new Angle(123, 23);
            var B = new Angle(23, 123);
            var b = 1;

            //Act & Assert
            Assert.NotEqual(A, B);
            Assert.False(A.Equals(b));
        }

        [Fact]
        public void TestGetHashCodeTrue()
        {
            //Arrange
            var A = new Angle(523545324, -11111111);
            var B = new Angle(-523545324, 11111111);

            //Act & Assert
            Assert.Equal(A.GetHashCode(), B.GetHashCode());
        }

        [Fact]
        public void TestGetHashCodeFalse()
        {
            //Arrange
            var A = new Angle(1231223, 12313311);
            var B = new Angle(12313311, 1231223);

            //Act & Assert
            Assert.NotEqual(A.GetHashCode(), B.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(TestSumData))]
        public void TestSum(Angle A, Angle B, Angle C)
        {
            //Arrange & Act & Assert
            Assert.Equal(C, A + B);
        }

        public static IEnumerable<object[]> TestSumData => new List<object[]>
        {
            //Objects for testing
            new object[] { new Angle(360), new Angle(90), new Angle(90) },
            new object[] { new Angle(1, 3), new Angle(1, 7), new Angle(10, 21) },
            new object[] { new Angle(1, 3), new Angle(-20, 60), new Angle(0, 1) },
            new object[] { new Angle(9999, 3333), new Angle(2425, 25), new Angle(100) },
        };

        [Theory]
        [MemberData(nameof(TestModData))]
        public void TestMod(Angle A, int k, Angle B)
        {
            //Arrange & Act & Assert
            Assert.Equal(B, A % k);
        }

        public static IEnumerable<object[]> TestModData => new List<object[]>
        {
            //Objects for testing
            new object[] { new Angle(127, 3), 5, new Angle(7, 3) },
            new object[] { new Angle(-370), 360, new Angle(-10) },
            new object[] {new Angle(127, -3), 5, new Angle(-7, 3) },
            new object[] {new Angle(0), 3, new Angle(0) },
        };

        [Fact]
        public void TestRoundModZero()
        {
            //Arrange & Act & Assert
            Assert.Throws<DivideByZeroException>(() => new Angle(124, 7) % 0);
        }
    }
}

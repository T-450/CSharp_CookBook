namespace EnumerationRecordsTests
{
    using System.Reflection;
    using Xunit;

    public class HamburgerTests
    {
        [Fact]
        public void DifferentInstancesShouldNotBeEqual()
        {
            var cheeseburger = Hamburger.Cheeseburger;
            var bigMac = Hamburger.BigMac;

            Assert.NotEqual(bigMac, cheeseburger);
        }

        [Fact]
        public void SameInstancesShouldBeEqual()
        {
            var cheeseburger1 = Hamburger.Cheeseburger;
            var cheeseburger2 = CreateCopy(cheeseburger1);

            Assert.Equal(cheeseburger1, cheeseburger2);
        }

        [Fact]
        public void GetAll_ShouldReturnAllInstances()
        {
            var hamburgers = Hamburger.GetAll().ToHashSet();

            Assert.Equal(hamburgers.Count, 3);

            Assert.True(hamburgers.Any(b => b == Hamburger.BigMac));
            Assert.True(hamburgers.Any(b => b == Hamburger.BigTasty));
            Assert.True(hamburgers.Any(b => b == Hamburger.Cheeseburger));
        }

        [Fact]
        public void AbsoluteDifference_ShouldReturnCorrectDifference()
        {
            var cheeseburger = Hamburger.Cheeseburger;
            var bigTasty = Hamburger.BigTasty;

            int result = Hamburger.AbsoluteDifference(cheeseburger, bigTasty);

            Assert.Equal(result, 2);
        }

        [Fact]
        public void FromValue_ShouldReturnCorrectInstance()
        {
            var result = Hamburger.FromValue(Hamburger.Cheeseburger.Value);

            Assert.Equal(result, Hamburger.Cheeseburger);
        }

        [Fact]
        public void FromValue_ShouldThrowIfNoMatchingItemFound()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => Hamburger.FromValue(1000));

            Assert.Equal<string>(exception.Message, "'1000' is not a valid value in Hamburger");
        }

        [Fact]
        public void FromName_ShouldReturnCorrectInstance()
        {
            var result = Hamburger.FromDisplayName(Hamburger.Cheeseburger.DisplayName);

            Assert.Equal(result, Hamburger.Cheeseburger);
        }

        [Fact]
        public void FromName_ShouldThrowIfNoMatchingItemFound()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => Hamburger.FromDisplayName("Egg"));

            Assert.Equal(exception.Message, "'Egg' is not a valid display name in Hamburger");
        }

        [Fact]
        public void DifferentImplementationsWillNotClash()
        {
            var hamburgers = Hamburger.GetAll().ToList();
            var sausages = Sausage.GetAll().ToList();

            Assert.Equal(hamburgers.Count, 3);
            Assert.Equal(sausages.Count, 2);

        }

        [Fact]
        public void CompareTo_ShouldReturn0ForSameInstances()
        {
            var hamburger1 = Hamburger.Cheeseburger;
            var hamburger2 = CreateCopy(hamburger1);

            int result = hamburger1.CompareTo(hamburger2);

            Assert.Equal(result, 0);
        }

        [Fact]
        public void CompareTo_ShouldReturnMinus1ForItemWhenValueIsLessThanTheComparedValue()
        {
            var hamburger1 = Hamburger.Cheeseburger;
            var hamburger2 = Hamburger.BigTasty;

            int result = hamburger1.CompareTo(hamburger2);

            Assert.Equal(result, -1);
        }

        [Fact]
        public void CompareTo_ShouldReturn1ForItemWhenValueIsGreaterThanTheComparedValue()
        {
            var hamburger1 = Hamburger.BigTasty;
            var hamburger2 = Hamburger.Cheeseburger;

            int result = hamburger1.CompareTo(hamburger2);

            Assert.Equal(result, 1);
        }

        private static Hamburger CreateCopy(Hamburger hamburger)
        {
            return (Hamburger)typeof(Hamburger).GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new[] { typeof(int), typeof(string) }, null)
                !.Invoke(new object[] { hamburger.Value, hamburger.DisplayName });
        }
        // TODO Add test that ensures that code throws if duplicate names.
    }
}

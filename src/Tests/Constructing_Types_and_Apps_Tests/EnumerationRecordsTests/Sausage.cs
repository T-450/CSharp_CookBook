namespace EnumerationRecordsTests
{
    using EnumerationRecords;

    internal record Sausage : Enumeration<Sausage>
    {
        public static Sausage HotDog = new Sausage(1, "Hot Dog");
        public static Sausage Pølse = new Sausage(2, "Pølse");

        private Sausage(int value, string displayName) : base(value, displayName) { }
    }
}

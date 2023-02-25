namespace EnumerationRecords
{
    /************************************************************************
     * Source: https://josef.codes/enumeration-class-in-c-sharp-using-records
     *************************************************************************/
    using System.Reflection;

    /// <summary>
    ///     By using a record, we get the equality checks for free since records implements the IEquatable
    ///     <T>
    ///         interface automatically.
    ///         Records doesn't implement the IComparable interface, so it needs to be implemented manually.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract record Enumeration<T> : IComparable<T> where T : Enumeration<T>
    {
        private static readonly Lazy<Dictionary<int, T>> AllItems;
        private static readonly Lazy<Dictionary<string, T>> AllItemsByName;

        #region Ctor
        /// <summary>
        ///     Cache all instances in two dictionaries, one using Value as key and the other one where the DisplayName would be the key.
        ///     This will allow for really fast lookups, close to O(1).
        ///     If we don't wrap the initialization of our dictionaries in a Lazy class, we will get runtime errors.
        ///     This is because the static constructor will run before the constructor of our actual implementation.
        ///     When that happens, we will not get any values from the reflection call, everything will be null.
        /// </summary>
        /// <exception cref="Exception"></exception>
        static Enumeration()
        {
            AllItems = new Lazy<Dictionary<int, T>>(() =>
            {
                return typeof(T)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Where(x => x.FieldType == typeof(T))
                    .Select(x => x.GetValue(null))
                    .Cast<T>()
                    .ToDictionary(x => x.Value, x => x);
            });
            AllItemsByName = new Lazy<Dictionary<string, T>>(() =>
            {
                var items = new Dictionary<string, T>(AllItems.Value.Count);
                foreach (var item in AllItems.Value)
                {
                    if (!items.TryAdd(item.Value.DisplayName, item.Value))
                    {
                        throw new Exception(
                            string.Format(@"DisplayName needs to be unique. '{0}' already exists", item.Value.DisplayName));
                    }
                }
                return items;
            });
        }
        #endregion

        protected Enumeration(int value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public int Value { get; }
        public string DisplayName { get; }

        /// <inheritdoc />
        public int CompareTo(T? other) => Value.CompareTo(other!.Value);

        /// <inheritdoc />
        public override string ToString() => DisplayName;

        /// <summary>
        ///     Returns an Enumerable<T> of <T>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetAll() => AllItems.Value.Values;

        /// <summary>
        ///     Returns the absolute value of a specified Enumeration<T> instances.
        /// </summary>
        /// <param name="firstValue"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static int AbsoluteDifference(Enumeration<T> firstValue, Enumeration<T> secondValue)
            => Math.Abs(firstValue.Value - secondValue.Value);

        /// <summary>
        ///     Gets an item by value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T FromValue(int value)
        {
            if (AllItems.Value.TryGetValue(value, out var matchingItem))
            {
                return matchingItem;
            }
            throw new InvalidOperationException(string.Format(@"'{0}' is not a valid value in {1}", value, typeof(T).Name));
        }

        /// <summary>
        ///     Gets an item by displayName
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T FromDisplayName(string displayName)
        {
            if (AllItemsByName.Value.TryGetValue(displayName, out var matchingItem))
            {
                return matchingItem;
            }
            throw new InvalidOperationException(
                string.Format("'{0}' is not a valid display name in {1}", displayName, typeof(T).Name));
        }
    }
}

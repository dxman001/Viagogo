namespace Viagogo.Common
{
    internal class ContainsComparator : IEqualityComparer<string>
    {
        public bool Equals(string x, string y) =>
            x.Contains(y, StringComparison.OrdinalIgnoreCase) ||
            y.Contains(x, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode(string obj) =>
            obj[0].GetHashCode();

    }
}

using NullGuard;

namespace Json.Comparer
{
    public interface IValueConverter
    {
        string Convert(string value);
    }
}
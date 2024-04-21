
namespace SuperTorus.Application.Extensions
{
    public static class RandomExtensions
    {
        public static double GetRandomValue(this Random _random, double minValue, double maxValue)
        {
            return _random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}

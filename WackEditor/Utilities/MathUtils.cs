namespace WackEditor.Utilities
{
    public static class MathUtils
    {
        public static float epsilon => 0.0000001f;

        public static bool IsTheSameAs(this float value, float other)
        {
            return Math.Abs(value - other) < epsilon;
        }
        public static bool? IsTheSameAs(this float? value, float? other)
        {
            if (!value.HasValue || other.HasValue) return false;
            return Math.Abs(value.Value - other.Value) < epsilon;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WackEditor.Utilities
{
    public static class MathUtils
    {
        public static float Epsilon => 0.0000001f;

        public static bool IsTheSameAs(this float value, float other)
        {
            return Math.Abs(value - other) < Epsilon;
        } 
        public static bool? IsTheSameAs(this float? value, float? other)
        {
            if (!value.HasValue || other.HasValue) return false;
            return Math.Abs(value.Value - other.Value) < Epsilon;
        }
    }
}

﻿namespace System.Collections.Generic
{
    public static class IEnumirableExtentions
    {
        public static void ForEach<T>(this IEnumerable<T> enumirable, Action<T> action)
        {
            foreach (var item in enumirable)
            {
                action(item);
            }
        }
    }
}

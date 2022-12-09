using System.Collections.Generic;

namespace RaptorUtils.Collections {
    public static class CollectionExt {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;
    }
}
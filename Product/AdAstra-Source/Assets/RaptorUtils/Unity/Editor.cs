using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaptorUtils.Unity.Editor {
    [Serializable]
    public class InspectorSetOnlyArray<T> : IEnumerable<T> {
        [SerializeField] private T[] content;
        public T this[int index] => content[index];
        public int Length => content.Length;
        public IEnumerator<T> GetEnumerator() {
            foreach (var item in content) {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
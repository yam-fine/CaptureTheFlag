using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{

    [Serializable]
    public class SerializableStack<T> : IEnumerable<T>
    {
        [SerializeField] private List<T> data = new List<T>();

        public int Count
        {
            get { return data.Count; }
        }

        public void Push(T item)
        {
            data.Add(item);
        }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException("Stack is empty");

            T item = data[Count - 1];
            data.RemoveAt(Count - 1);
            return item;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("Stack is empty");

            return data[Count - 1];
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(T item)
        {
            return data.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = Count - 1; i >= 0; i--)
                yield return data[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
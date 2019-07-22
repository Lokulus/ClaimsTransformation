using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ClaimsTransformation.Engine
{
    public class CappedDictionary<TKey, TValue>
    {
        public CappedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.Keys = new Queue(capacity);
            this.Store = new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity, comparer);
            this.Capacity = capacity;
        }

        public Queue Keys { get; private set; }

        public ConcurrentDictionary<TKey, TValue> Store { get; private set; }

        public int Capacity { get; private set; }

        public void Add(TKey key, TValue value)
        {
            this.Store[key] = value;
            if (this.Keys.Count >= this.Capacity)
            {
                this.Store.TryRemove(this.Keys.Dequeue());
            }
            this.Keys.Enqueue(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var result = this.Store.TryGetValue(key, out value);
            if (result && this.Keys.Count >= this.Capacity)
            {
                //When a cache hit occurs, push the key to the back of queue.
                this.Keys.Remove(key);
                this.Keys.Enqueue(key);
            }
            return result;
        }

        public class Queue
        {
            public Queue(int capacity)
            {
                this.Values = new List<TKey>(capacity);
            }

            public List<TKey> Values { get; private set; }

            public void Enqueue(TKey value)
            {
                this.Values.Add(value);
            }

            public void Remove(TKey value)
            {
                this.Values.Remove(value);
            }

            public TKey Dequeue()
            {
                var value = this.Values[0];
                this.Values.RemoveAt(0);
                return value;
            }

            public int Count
            {
                get
                {
                    return this.Values.Count;
                }
            }
        }
    }
}
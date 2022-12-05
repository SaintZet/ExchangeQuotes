using ExchangeQuotes.Math.Abstractions;
using System.Collections.Concurrent;

namespace ExchangeQuotes.Math.Statistic;

public class MedianCalculator : IStatisticThreadSafeCalculator
{
    private readonly Heap<int> _minHeap;
    private readonly Heap<int> _maxHeap;

    public MedianCalculator()
    {
        _minHeap = new Heap<int>(HeapType.Min, 1000000000);
        _maxHeap = new Heap<int>(HeapType.Max, 1000000000);
    }

    public enum HeapType
    {
        Max,
        Min,
    }

    public void AddNumberToSequence(int number)
    {
        lock (_minHeap) lock (_maxHeap)
            {
                _minHeap.Push(number);
                _maxHeap.Push(_minHeap.Pop());

                if (_minHeap.Count < _maxHeap.Count)
                {
                    _minHeap.Push(_maxHeap.Pop());
                }
            }
    }

    public double GetCurrentResult()
    {
        lock (_minHeap) lock (_maxHeap)
            {
                if (_minHeap.Count == 0 && _maxHeap.Count == 0)
                {
                    return 0;
                }

                if (_minHeap.Count == _maxHeap.Count)
                {
                    return ((double)_minHeap.Peek() + _maxHeap.Peek()) / 2;
                }
                else if (_minHeap.Count > _maxHeap.Count)
                {
                    return _minHeap.Peek();
                }
                else
                {
                    return _maxHeap.Peek();
                }
            }
    }

    public class Heap<KeyT> where KeyT : IComparable
    {
        private readonly HeapType _type;
        private int _count;
        private KeyT[] _heap;

        public Heap(HeapType heapType, int initalCapacity)
        {
            if (initalCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException("Capacity should be greater than 0.");
            }

            _heap = new KeyT[initalCapacity + 1];

            _type = heapType;
        }

        public int Count => _count;

        public bool Any() => _count != 0;

        public void Push(KeyT key)
        {
            if (_count + 1 >= _heap.Length)
            {
                // Double the length of the array
                var newHeap = new KeyT[_count * 2 + 1];
                _heap.CopyTo(newHeap, 0);

                _heap = newHeap;
            }

            _heap[++_count] = key;

            Swim(_count);
        }

        public KeyT Pop()
        {
            KeyT max = _heap[1];

            Exchange(1, _count--);

            Sink(1);

            _heap[_count + 1] = default(KeyT)!;

            return max;
        }

        public KeyT Peek() => _heap[1];

        // Swim to the top (larger than one or both) of its children's
        private void Swim(int k)
        {
            // parent of node at k is at k / 2;
            while (k > 1 && LessOrMore(k / 2, k))
            {
                Exchange(k, k / 2);
                k = k / 2;
            }
        }

        private void Sink(int k)
        {
            while (2 * k <= _count)
            {
                int j = 2 * k;
                if (j < _count && LessOrMore(j, j + 1))
                {
                    j++;
                }

                if (!LessOrMore(k, j))
                {
                    break;
                }

                Exchange(k, j);
                k = j;
            }
        }

        private bool LessOrMore(int i, int j)
        {
            if (_type == HeapType.Max)
            {
                return _heap[i].CompareTo(_heap[j]) < 0;    // Less
            }
            else
            {
                return _heap[i].CompareTo(_heap[j]) > 0;    // More
            }
        }

        private void Exchange(int i, int j)
        {
            KeyT? temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }
    }
}
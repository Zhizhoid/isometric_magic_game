using System;

public class Heap<T> where T : IHeapItem
{
    public int Count
    {
        get
        {
            return itemCount;
        }
    }

    private T[] items;
    private int itemCount;

    public delegate int compareDel(T a, T b);
    private compareDel compare;

    public Heap(int maxSize, compareDel _compare)
    {
        items = new T[maxSize];
        itemCount = 0;
        compare = _compare;
    }

    public void Add(T item)
    {
        item.HeapIndex = itemCount;
        items[itemCount] = item;
        sortUp(item);
        itemCount++;
    }

    public T Pop()
    {
        if(itemCount == 0)
        {
            throw new Exception("Can't pop if the heap is empty");
        }

        T res = items[0];

        itemCount--;
        items[0] = items[itemCount];
        if (itemCount != 0) {
            items[0].HeapIndex = 0;
            sortDown(items[0]);
        }


        return res;
    }

    public void Update(T item)
    {
        if(!Contains(item))
        {
            return;
        }
        sortUp(item);
        sortDown(item);
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public T Peek()
    {
        if (itemCount == 0)
        {
            throw new Exception("Can't peek if the heap is empty");
        }

        return items[0];
    }

    private void sortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while(true)
        {
            T parentItem = items[parentIndex];

            if(compare(item, parentItem) > 0)
            {
                swap(item, parentItem);
                parentIndex = (item.HeapIndex - 1) / 2;
            }
            else
            {
                break;
            }
        }
    }

    private void sortDown(T item)
    {
        int lsonIndex = 2 * item.HeapIndex + 1;

        while(true)
        {
            T lsonItem;

            if (lsonIndex < itemCount)
            {
                lsonItem = items[lsonIndex];
            }
            else
            {
                break;
            }

            T swapItem = lsonItem;
            int swapIndex = lsonIndex;

            if (lsonIndex + 1 < itemCount)
            {
                T rsonItem = items[lsonIndex + 1];
                if (compare(swapItem, rsonItem) < 0)
                {
                    swapItem = rsonItem;
                    swapIndex++;
                }
            }

            if (compare(item, swapItem) < 0)
            {
                swap(item, swapItem);
                lsonIndex = 2 * item.HeapIndex + 1;
            }
            else
            {
                break;
            }
        }
    }

    private void swap(T a, T b)
    {
        items[a.HeapIndex] = b;
        items[b.HeapIndex] = a;

        int prevAHeapIndex = a.HeapIndex;
        a.HeapIndex = b.HeapIndex;
        b.HeapIndex = prevAHeapIndex;
    }
}

public interface IHeapItem
{
    public int HeapIndex
    {
        get;
        set;
    }
}

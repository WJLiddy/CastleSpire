using System;

// An InsertionQueue designed such that Insertion = O(1), Dequeing = O(n), and
// Memory usage is O(1) per insertion that does not get freed until clear() is called.

//!!!! I should test against a fast heap implementaion online !!!!
public class InsertionQueue<T> where T : IComparable
{
    private T[] List;
    private int FirstElementInList = 0;
    private int LastElementInList = 0;
    
    public InsertionQueue(int maxElements)
    {
        // Pointer to first element
        FirstElementInList = 0;
        // Pointer to last element
        LastElementInList = -1;
        List = new T[maxElements];
    }

    //TODO: Circular array-itize and expand. Do not crash!
    public void Enqueue(T toQueue)
    {
        List[LastElementInList+1] = toQueue;
        LastElementInList++;
    }

    public T Dequeue()
    {
        Sort();
        if (FirstElementInList > LastElementInList)
            return default(T);
        T toReturn = List[FirstElementInList];
        FirstElementInList++;
        return toReturn;
    }

    public int Count()
    {
        return 1 + (LastElementInList - FirstElementInList);
    }

    public void Clear()
    {
        // Pointer to first element
        FirstElementInList = 0;
        // Pointer to last element
        LastElementInList = -1;
    }

    private void Sort()
    {
        for (int counter = FirstElementInList; counter < LastElementInList; counter++)
        {
            int index = counter + 1;
            while (index > FirstElementInList)
            {
                if (List[index - 1].CompareTo(List[index]) > 0)
                {
                    T temp = List[index - 1];
                    List[index - 1] = List[index];
                    List[index] = temp;
                }
                index--;
            }
        }
    }
}
 
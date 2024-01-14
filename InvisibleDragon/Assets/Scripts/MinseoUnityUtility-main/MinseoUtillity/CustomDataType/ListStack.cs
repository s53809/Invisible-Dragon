using System;
using System.Collections.Generic;

namespace CustomDataType
{
    public class ListStack<T>
    {
        public List<T> StackList { get; set; }
        public T Top { get { return StackList[Count - 1]; } }
        public Int32 Count { get { return StackList.Count; } }
        public void Push(T item)
        {
            StackList.Add(item);
        }
        public T Pop()
        {
            if (StackList.Count == 0) throw new Exception("ListStack is Empty!");

            T item = StackList[Count - 1];
            StackList.RemoveAt(Count - 1);
            return item;
        }
    }
}
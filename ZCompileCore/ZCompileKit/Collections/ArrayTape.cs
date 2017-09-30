using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileKit.Collections
{
    public class ArrayTape<T>
    {
        protected T[] array;
        protected int i;
        public int Index
        {
            get
            {
                return i;
            }
        }
        T NullT;
        int length;

        public ArrayTape(T[] array,T nullt)
        {
            this.array = array;
            NullT = nullt;
            i = 0;
            length = array.Length;
        }

        public bool MoveNext()
        {
            if (i < length)
            {
                i++;
                return true;
            }
            return false;
        }

        public T Current
        {
            get
            {
                if(i<length)
                {
                    return array[i];
                }
                else
                {
                    return NullT;
                }
            }
        }

        public T First
        {
            get
            {
               return array[0];
            }
        }

        public T Next
        {
            get
            {
                if (i < length-1)
                {
                    return array[i+1];
                }
                else
                {
                    return NullT;
                }
            }
        }

        public T Pre
        {
            get
            {
                if (i>1)
                {
                    return array[i - 1];
                }
                else
                {
                    return NullT;
                }
            }
        }

        public bool IsEnd
        {
            get
            {
                return i >= length;
            }
        }
    }
}

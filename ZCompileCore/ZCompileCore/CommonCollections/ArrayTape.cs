using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.CommonCollections
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
        //T NullT;
        int length;

        //public ArrayTape(T[] array)
        //    : this(array, default(T))
        //{
          
        //}

        public ArrayTape(IEnumerable<T> array)
        {
            this.array = array.ToArray();
            //NullT = nullt;
            i = 0;
            length = this.array.Length;
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

        public int Count
        {
            get
            {
                return length;
            }
        }

        public T[] GetArray()
        {
            return array;
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
                    throw new CCException("Tape为空");
                }
            }
        }

        public bool HasCurrent
        {
            get
            {
                if (i < length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasNext
        {
            get
            {
                if (i < length - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasPre
        {
            get
            {
                if (i ==0)
                {
                    return false;
                }
                else
                {
                    return true;
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
                    //return NullT;
                    throw new CCException("Tape为空");
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
                    //return NullT;
                    throw new CCException("Tape为空");
                }
            }
        }

        //public bool HasCurrent
        //{
        //    get
        //    {
        //        return i >= length;
        //    }
        //}
    }
}

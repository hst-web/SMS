using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZT.Utillity
{
    public class CollectionUtils
    {
        public static bool IsEmpty(ICollection coll)
        {
            return coll == null || coll.Count <= 0;
        }

        public static bool IsNotEmpty(ICollection coll)
        {
            return !IsEmpty(coll);
        }
    }

    public class Safes
    {
        public static List<T> of<T>(List<T> source) where T : class
        {
            if (CollectionUtils.IsEmpty(source))
            {
                return new List<T>();
            }
            return source;
        }

        public static T of<T>(T source) where T : class, new()
        {
            if (source == null)
            {
                return new T();
            }

            return source;
        }
    }
}

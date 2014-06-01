﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace TypeLess.Helpers
{
    public static class ListSync
    {

        /// <summary>
        /// Makes sure that list1 equals list2, for matching items an update func can be supplied
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="withList"></param>
        public static void SyncList<T>(List<T> list1, List<T> withList, Action<T, T> onUpdate = null, Action<T> onRemove = null, Action<T> onAdd = null) where T : IEquatable<T>
        {

            //if an item already exist in the old list then call updateFunc with the old and the new item
            if (onUpdate != null)
            {

                foreach (var x in list1)
                {
                    var matchingItem = withList.Where(y => y.Equals(x)).FirstOrDefault();
                    if (matchingItem != null)
                    {
                        onUpdate(x, matchingItem);
                    }
                }

            }

            //if oldList does not contain an item in new list then remove it from old list
            if (onRemove != null)
            {

                foreach (var x in list1)
                {
                    if (!withList.Contains(x))
                        onRemove(x);
                }

            }
            list1.RemoveAll(x => !withList.Contains(x));

            //if an item in new list does not exist in old list then add it

            foreach (var x in withList)
            {
                if (!list1.Contains(x))
                {
                    if (onAdd != null)
                    {
                        onAdd(x);
                    }
                    list1.Add(x);
                }
            }

        }

    }
}

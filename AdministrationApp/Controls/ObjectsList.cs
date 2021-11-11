using System;
using System.Collections.Generic;
using System.Linq;
using Xwt;
namespace AdministrationApp.Controls
{
    public abstract class ObjectsList<T>:ListBox
    {
        protected List<T> objects = new List<T>();
        protected ObjectsList()
        {
            GridLinesVisible = true;
        }
        public void AddObject(T obj,string displayName)
        {
            Items.Add(obj, displayName);
        }
        public void RemoveObject(Func<T,bool> comparator)
        {
            var areaToRemove = Items.FirstOrDefault(s => comparator((T)s));
            Items.Remove(areaToRemove);
        }
        public void EditObject(T obj, string displayName, Func<T, bool> comparator)
        {
            RemoveObject(comparator);
            Items.Insert(0, obj, displayName);
            SelectRow(0);
        }
        public T GetSelectedObject()
        {
            if (SelectedRow == -1) return default(T);
            return (T)Items[SelectedRow];
        }


    }
}

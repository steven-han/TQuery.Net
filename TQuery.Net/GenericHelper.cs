﻿/**  功能：泛型排序类
 *   作者：Steven_Han 
 *   MSN： Steven_Han@live.com
**/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TQuery.Net
{
    public class GenericHelper
    {
        /// <summary>
        /// 自定义绑定列表类
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        public class IListSort<T> : BindingList<T>
        {
            private bool isSorted;
            private PropertyDescriptor sortProperty;
            private ListSortDirection sortDirection;

            /// <summary>
            /// 构造函数
            /// </summary>
            public IListSort()
                : base()
            {
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="list">IList类型的列表对象</param>
            public IListSort(IList<T> list)
                : base(list)
            {
            }

            /// <summary>
            /// 自定义排序操作
            /// </summary>
            /// <param name="property"></param>
            /// <param name="direction"></param>
            protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
            {
                List<T> items = this.Items as List<T>;

                if (items != null)
                {
                    ObjectPropertyCompare<T> pc = new ObjectPropertyCompare<T>(property, direction);
                    items.Sort(pc);
                    isSorted = true;
                }
                else
                {
                    isSorted = false;
                }

                sortProperty = property;
                sortDirection = direction;

                this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }

            /// <summary>
            /// 获取一个值，指示列表是否已排序
            /// </summary>
            protected override bool IsSortedCore
            {
                get
                {
                    return isSorted;
                }
            }

            /// <summary>
            /// 获取一个值，指示列表是否支持排序
            /// </summary>
            protected override bool SupportsSortingCore
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// 获取一个只，指定类别排序方向
            /// </summary>
            protected override ListSortDirection SortDirectionCore
            {
                get
                {
                    return sortDirection;
                }
            }

            /// <summary>
            /// 获取排序属性说明符
            /// </summary>
            protected override PropertyDescriptor SortPropertyCore
            {
                get
                {
                    return sortProperty;
                }
            }

            /// <summary>
            /// 移除默认实现的排序
            /// </summary>
            protected override void RemoveSortCore()
            {
                isSorted = false;
                this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }


        private class ObjectPropertyCompare<T> : IComparer<T>
        {
            private PropertyDescriptor property;
            private ListSortDirection direction;

            // 构造函数
            public ObjectPropertyCompare(PropertyDescriptor property, ListSortDirection direction)
            {
                this.property = property;
                this.direction = direction;
            }

            // 实现IComparer中方法
            public int Compare(T x, T y)
            {
                object xValue = x.GetType().GetProperty(property.Name).GetValue(x, null);
                object yValue = y.GetType().GetProperty(property.Name).GetValue(y, null);

                int returnValue;

                if (xValue is IComparable)
                {
                    returnValue = ((IComparable)xValue).CompareTo(yValue);
                }
                else if (xValue.Equals(yValue))
                {
                    returnValue = 0;
                }
                else
                {
                    returnValue = xValue.ToString().CompareTo(yValue.ToString());
                }

                if (direction == ListSortDirection.Ascending)
                {
                    return returnValue;
                }
                else
                {
                    return returnValue * -1;
                }
            }
        }
    }
}

using System;
using System.Collections;

namespace RunLogger.Utils
{
    internal class EnumeratorHook : IEnumerable
    {
        internal IEnumerator enumerator;
        internal Action prefixAction = () => { };
        internal Action<object> preItemAction = item => { };
        internal Func<object, object> itemAction = item => item;
        internal Action<object> postItemAction = item => { };
        internal Action postfixAction = () => { };

        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        internal IEnumerator GetEnumerator()
        {
            prefixAction();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                preItemAction(item);
                yield return itemAction(item);
                postItemAction(item);
            }
            postfixAction();
        }
    }
}
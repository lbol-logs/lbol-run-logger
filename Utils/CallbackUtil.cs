using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RunLogger.Utils
{
    public static class CallbackUtil
    {
        public delegate void Callback();
        private static Callback callback;

        public static void AddCallback(Callback _callback)
        {
            callback += _callback;
        }

        public static void Execute()
        {
            BepinexPlugin.log.LogDebug("Execute");
            callback();
        }
    }
}

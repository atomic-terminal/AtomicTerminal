using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools {
    public class MyUtility
    {
        public static int RandValue(int maxValue)
        {
            return UnityEngine.Random.Range(0, maxValue);
        }
    }
}


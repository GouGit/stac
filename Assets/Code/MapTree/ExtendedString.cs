﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendedString
{
    public static Vector3 ToVector3(this string str)
    {
        if(str.StartsWith("(") && str.EndsWith(")"))
            str = str.Substring(1, str.Length - 2);

        string[] sArray = str.Split(',');

        return new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2])
        );
    }

    public static int ToInt(this string str)
    {
        return int.Parse(str);
    }

    public static float ToFloat(this string str)
    {
        return float.Parse(str);
    }
}
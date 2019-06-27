using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static RaycastHit GetNearestHit(RaycastHit[] _hits, Vector3 _pos)
    {
        int nearest = 0;
        float dist = float.MaxValue;

        for (int i = 0; i < _hits.Length; i++)
        {
            if (Vector3.Distance(_pos, _hits[i].point) < dist)
            {
                dist = Vector3.Distance(_pos, _hits[i].point);
                nearest = i;
            }
        }

        return _hits[nearest];
    }

    public static Vector3 AbsVector(Vector3 _v)
    {
        Vector3 v = _v;

        v.x = Mathf.Abs(v.x);
        v.y = Mathf.Abs(v.y);
        v.z = Mathf.Abs(v.z);

        return v;
    }

    public static Vector3 RoundVector(Vector3 _v, int _decimals)
    {
        Vector3 v = _v;
        float d = Mathf.Pow(10, _decimals);

        v.x = ((int)(v.x * d)) / d;
        v.y = ((int)(v.y * d)) / d;
        v.z = ((int)(v.z * d)) / d;

        return v;
    }

    public static Vector3 DivVector(Vector3 _v1, Vector3 _v2)
    {
        Vector3 v = Vector3.zero;
        
        v.x = _v1.x == _v2.x || _v2.x == 0 ? 1 : _v1.x / _v2.x;
        v.y = _v1.y == _v2.y || _v2.y == 0 ? 1 : _v1.y / _v2.y;
        v.z = _v1.z == _v2.z || _v2.z == 0 ? 1 : _v1.z / _v2.z;

        return v;
    }

    public static Vector3 MultVector(Vector3 _v1, Vector3 _v2)
    {
        Vector3 v = Vector3.zero;

        v.x = _v1.x * _v2.x;
        v.y = _v1.y * _v2.y;
        v.z = _v1.z * _v2.z;

        return v;
    }

    public static Vector3 ClampVector01(Vector3 _v)
    {
        Vector3 v = _v;

        v.x = Mathf.Clamp01(v.x);
        v.y = Mathf.Clamp01(v.y);
        v.z = Mathf.Clamp01(v.z);

        return v;
    }
}
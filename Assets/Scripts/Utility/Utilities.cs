using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utilities
{

    #region Inversion

    public static float ReturnPositive(float v){
        float tmp=0;
        if (v < 0){
            return -v;
        }
        else{
            return v;
        }
        // float pv = tmp;
        // return pv;
    }
    
    public static Vector3 ReturnPositive(Vector3 v){
        float tmpx=0; float tmpy=0; float tmpz=0;
        if (v.x < 0){
            tmpx = -v.x;
        }
        else{
            tmpx = v.x;
        }
        if (v.y < 0){
            tmpy = -v.y;
        }
        else{
            tmpy = v.y;
        }
        if (v.z < 0){
            tmpz = -v.z;
        }
        else{
            tmpz = v.z;
        }

        Vector3 pv = new Vector3(tmpx, tmpy, tmpz);
        return pv;
    }

    #endregion
    
    #region MinMax Vector
    //using a list
    public static Vector3 ReturnMax(List<Vector3> v){
        
        float[] tmpx = new float[v.Count];
        float[] tmpy = new float[v.Count];
        float[] tmpz = new float[v.Count];

        // Vector3[] a = new Vector3[v.Count]; // not used
        // a = v.ToArray();
        
        for (int i = 0; i < v.Count; i++){
            tmpx[i] = v[i].x;
            tmpy[i] = v[i].y;
            tmpz[i] = v[i].z;
        }
        
        float hx = tmpx.Max();
        float hy = tmpy.Max();
        float hz = tmpz.Max();

        Vector3 vm = new Vector3(hx,hy,hz);
        return vm;
    }
    public static Vector3 ReturnMin(List<Vector3> v){
        
        float[] tmpx = new float[v.Count];
        float[] tmpy = new float[v.Count];
        float[] tmpz = new float[v.Count];

        for (int i = 0; i < v.Count; i++){
            tmpx[i] = v[i].x;
            tmpy[i] = v[i].y;
            tmpz[i] = v[i].z;
        }
        
        float hx = tmpx.Min();
        float hy = tmpy.Min();
        float hz = tmpz.Min();

        Vector3 vm = new Vector3(hx,hy,hz);
        return vm;
    }
    #endregion

    #region MinMax Float

    public static float ReturnMax(float[] v){
        
        float[] tmpx = new float[v.Length];

        for (int i = 0; i < v.Length; i++){
            tmpx[i] = v[i];
        }
        float h = tmpx.Max();

        return h;
    }
    public static float ReturnMin(float[] v){
        
        float[] tmpx = new float[v.Length];

        for (int i = 0; i < v.Length; i++){
            tmpx[i] = v[i];
        }
        float h = tmpx.Min();

        return h;
    }

    #endregion

    #region Vector Division

    public static Vector3 DivideVector(Vector3 a, Vector3 b){
        float ax = a.x; float ay = a.y; float az = a.z;
        float bx = b.x; float by = b.y; float bz = b.z;

        float dx = ax / bx;
        float dy = ay / by;
        float dz = az / bz;

        Vector3 d = new Vector3(dz, dy, dz);
        
        return d;
    }

    #endregion

    #region Value Limiters

    public static float SetUpperLimit(float input, float limit){
        if (input >= limit){
            return limit;
        }
        else{
            return input; 
        }
        
    }
    public static float SetLowerLimit(float input, float limit){
        if (input <= limit){
            return limit;
        }
        else{
            return input; 
        }
    }

    public static float SetUpperLowerLimit(float input, float upper, float lower){
        float i = input;
        float o = 0;

        if (i >= upper){
            o = upper;
        }
        if (i <= lower){
            o = lower;
        }

        if (i < upper && i > lower){
            o = i;
        }
        return o;
    }
    public static float SetLimitsPositiveNegative(float input, float limit){
        if (limit >= 0){
            if (input >= limit){
                return limit;
            }

            if (input <= -limit){
                return -limit;
            }
            else{
                return input; 
            }
        }
        else{
            if (input >= -limit){
                return -limit;
            }

            if (input <= limit){
                return limit;
            }
            else{
                return input; 
            }
        }
    }
    public static Vector3 SetUpperLimit(Vector3 input, Vector3 limit){
        float x = 0; float y = 0; float z = 0;
        if (input.x >= limit.x){
            x = limit.x;
        }
        if (input.y >= limit.y){
            y = limit.y;
        }
        if (input.z >= limit.z){
            z = limit.z;
        }
        Vector3 r = new Vector3(x, y, z);
        return r;
    }
    public static Vector3 SetLowerLimit(Vector3 input, Vector3 limit){
        float x = 0; float y = 0; float z = 0;
        if (input.x <= limit.x){
            x = limit.x;
        }
        if (input.y <= limit.y){
            y = limit.y;
        }
        if (input.z <= limit.z){
            z = limit.z;
        }
        Vector3 r = new Vector3(x, y, z);
        return r;
    }

    #endregion
}

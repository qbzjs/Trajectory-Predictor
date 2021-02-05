using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSC_Math
{

    public static int[] IntArray_from_ElementRepeatNumbers(int number_of_elements, int number_of_repeats)
    {
        int[] output_array = new int[number_of_elements * number_of_repeats];
        for (int i = 0; i < number_of_elements; i++)
        {
            for (int i2 = 0; i2 < number_of_repeats; i2++)
            {
                output_array[(i * number_of_repeats) + i2] = i;
            }
        }
        return output_array;
    }
    public static Int16[] Int16Array_from_ElementRepeatNumbers(int number_of_elements, int number_of_repeats)
    {
        Int16[] output_array = new Int16[number_of_elements * number_of_repeats];
        for (Int16 i = 0; i < number_of_elements; i++)
        {
            for (int i2 = 0; i2 < number_of_repeats; i2++)
            {
                output_array[(i * number_of_repeats) + i2] = i;
            }
        }
        return output_array;
    }
    public static byte[] byteArray_from_ElementRepeatNumbers(int number_of_elements, int number_of_repeats)
    {
        byte[] output_array = new byte[number_of_elements * number_of_repeats];
        for (byte i = 0; i < number_of_elements; i++)
        {
            for (int i2 = 0; i2 < number_of_repeats; i2++)
            {
                output_array[(i * number_of_repeats) + i2] = i;
            }
        }
        return output_array;
    }


    public static int[] RandPerm_intArray(int[] input_array)
    {
        List<int> temp_trialOrder = new List<int>(input_array);
        int[] output_array = new int[input_array.Length];
        for (int i = 0; i < input_array.Length; i++)
        {
            int i2;
            i2 = UnityEngine.Random.Range(0, temp_trialOrder.Count);
            output_array[i] = temp_trialOrder[i2];
            temp_trialOrder.Remove(temp_trialOrder[i2]);
            //Debug.Log(temp_trialOrder.Count);
        }
        return output_array;
    }
    public static Int16[] RandPerm_Int16Array(Int16[] input_array)
    {
        List<Int16> temp_trialOrder = new List<Int16>(input_array);
        Int16[] output_array = new Int16[input_array.Length];
        for (int i = 0; i < input_array.Length; i++)
        {
            int i2;
            i2 = UnityEngine.Random.Range(0, temp_trialOrder.Count);
            output_array[i] = temp_trialOrder[i2];
            temp_trialOrder.Remove(temp_trialOrder[i2]);
            //Debug.Log(temp_trialOrder.Count);
        }
        return output_array;
    }
    public static byte[] RandPerm_byteArray(byte[] input_array)
    {
        List<byte> temp_trialOrder = new List<byte>(input_array);
        byte[] output_array = new byte[input_array.Length];
        for (int i = 0; i < input_array.Length; i++)
        {
            int i2;
            i2 = UnityEngine.Random.Range(0, temp_trialOrder.Count);
            output_array[i] = temp_trialOrder[i2];
            temp_trialOrder.Remove(temp_trialOrder[i2]);
            //Debug.Log(temp_trialOrder.Count);
        }
        return output_array;
    }







    public static List<int> Randperm_intList_from_GameObjectListElementClones(List<GameObject> _objectList, int _cloneNumber)
    {
        //Debug.Log(_objectList.Count);
        //Debug.Log(_cloneNumber);
        return RandPerm_List(RandpermList_from_ElementClones(_objectList, _cloneNumber));
    }

    public static List<int> RandpermList_from_ElementClones(List<GameObject> _objectList, int _cloneNumber)
    {
        List<int> _output = new List<int>();
        for (int i = 0; i < _objectList.Count; i++)
        {
            for (int i2 = 0; i2 < _cloneNumber; i2++)
                _output.Add(i);
        }
        return _output;
    }

    public static List<int> RandPerm_List(List<int> _input)
    {
        int[] _w_array = new int[_input.Count];
        List<int> _output = new List<int>();
        _w_array = RandPerm_intArray(IntArray_from_ElementRepeatNumbers(_input.Count, 1));
        for (int t1 = 0; t1 < _w_array.Length; t1++)
            _output.Add(_input[_w_array[t1]]);
        //Debug.Log("_output");
        //for (int t1 = 0; t1 < _w_array.Length; t1++)
        //    Debug.Log(_output[t1]);
        return _output;
    }

    public static int RemoveFirst_intListElement(List<int> _input)
    {
        if (_input.Count > 0)
        {
            int _output = _input[0];
            _input.RemoveAt(0);
            return _output;
        }
        throw new System.Exception("Empty list");
    }

}



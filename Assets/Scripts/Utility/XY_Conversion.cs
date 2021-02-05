using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XY_Conversion {

    public Vector2 ConvertToXY(float LR, float FR, float LF)
    {
        Vector2 xy = Vector2.zero;

        //float x = ((0f * 0.5774f) - (1f * LR)) + ((0.8660f * 0.5774f) - (0.5f * FR)) + (-(0.8660f * 0.5774f) - (0.5f * LF));
        //float y = ((-1f * 0.5774f) - (0f * LR)) + ((0.5f * 0.5774f) + (0.8660f * FR)) + ((0.5f * 0.5774f) - (0.8660f * LF));
        float x = (2f/3f) * ( -(1f * LR) - (0.5f * FR) - (0.5f * LF) );
        float y = (2f/3f) * ( (0.8660f * FR) - (0.8660f * LF) );
        //float x = -((2f / 3f) * LR) - ((1f / 3f) * FR) - ((1f / 3f) * LF);
        //float y = ((1.1547f / 2f) * FR) - ((1.1547f / 2f) * LF);

        xy = new Vector2(x, y);

        return xy;
    }

    public Vector2 ConvertToXY_LR(float LR)
    {
        Vector2 xy = Vector2.zero;

        float x = (0f * 0.5774f) - (1f * LR);
        float y = (-1f * 0.5774f) - (0f * LR);

        xy = new Vector2(x, y);

        return xy;
    }

    public Vector2 ConvertToXY_FR(float FR)
    {
        Vector2 xy = Vector2.zero;

        float x = (0.8660f * 0.5774f) - (0.5f * FR);
        float y = (0.5f * 0.5774f) + (0.8660f * FR);

        xy = new Vector2(x, y);

        return xy;
    }

    public Vector2 ConvertToXY_LF(float LF)
    {
        Vector2 xy = Vector2.zero;

        float x = -(0.8660f * 0.5774f) - (0.5f * LF);
        float y = (0.5f * 0.5774f) - (0.8660f * LF);

        xy = new Vector2(x, y);

        return xy;
    }
}

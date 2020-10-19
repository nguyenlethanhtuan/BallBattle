using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Color getColorCode(int color){
        switch(color){
            case Team.RED: return Color.red;
            case Team.BLUE: return  Color.blue;
            case Team.WHITE: return Color.white;
            case Team.BLACK: return Color.black;
            default: return Color.black;
        }
    }

    public static Color setAlpha(Color color, float alpha){
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Vector3 scaleVector3X(Vector3 v3, float scaleRate){
        return new Vector3(scaleRate, v3.y, v3.z);
    }
}

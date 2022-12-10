using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath
{
    /// <summary>
    /// Rotates a Vector2 by a given angle.
    /// ANGLE IN RADIANS!
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 RotateVector2(Vector2 vector, float angle) {
        float cosA = Mathf.Cos(angle);
        float sinA = Mathf.Sin(angle);

        Vector2 newX = new Vector2(cosA, sinA);
        Vector2 newY = new Vector2(-sinA, cosA);


        return vector.x * newX + vector.y * newY;
    }

    public static float SquareOf(float f) {
        return f * f;
    }
}

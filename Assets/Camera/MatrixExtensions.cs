using UnityEngine;

/// <summary>
/// UNITY MATRICES ARE COLUMN ORIENTED
/// left    x axis = column 0
/// up      y axis = column 1
/// forward z axis = column 2
/// translation    = column 3
/// </summary>

// some code from https://forum.unity.com/threads/how-to-assign-matrix4x4-to-transform.121966/

public static class MatrixExtensions
{

    // ------------------------
    // FROM UNITY FORUM VERSION 1
    // ------------------------

    public static void FromMatrix(this Transform transform, Matrix4x4 matrix)
    {
        transform.localScale = matrix.ExtractScale();
        transform.rotation = matrix.ExtractRotation();
        transform.position = matrix.ExtractPosition();
    }

    public static Quaternion ExtractRotation(this Matrix4x4 matrix)
    {
        Vector3 forward;        // column 2 = fwd axis 
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;        // column 1 = up axis
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    public static Vector3 ExtractPosition(this Matrix4x4 matrix)
    {
        Vector3 position;        //column 3
        position.x = matrix.m03; //row 0 col 3
        position.y = matrix.m13; //row 1 col 3
        position.z = matrix.m23; //row 2 col 3
        return position;
    }

    public static Vector3 ExtractScale(this Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude; // x axis = column 0
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude; // y axis = column 1
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude; // z axis = column 2
        return scale;
    }

    // ------------------------
    // FROM UNITY FORUM VERSION 2
    // ------------------------

    /// <summary>
    /// returns position - matrix passed by reference to improve performance; no changes will be made to it.
    /// <summary>
    public static Vector3 ExtractTranslationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 position;        //column 3
        position.x = matrix.m03; //row 0 col 3
        position.y = matrix.m13; //row 1 col 3
        position.z = matrix.m23; //row 2 col 3
        return position;
    }

    /// <summary>
    /// returns rotation quaternion - matrix passed by reference to improve performance; no changes will be made to it.
    /// <summary>
    public static Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 forward;        // column 2 = fwd axis 
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;        // column 1 = up axis
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    /// <summary>
    /// returns x axis vector (alain)
    /// <summary>
    public static Vector3 ExtractXAxisFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 column;
        column = new Vector3(matrix.m00, matrix.m10, matrix.m20);
        return column.normalized;
    }

    /// <summary>
    /// returns x axis vector (alain)
    /// <summary>
    public static Vector3 ExtractYAxisFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 column;
        column = new Vector3(matrix.m01, matrix.m11, matrix.m21);
        return column.normalized;
    }

    /// <summary>
    /// returns x axis vector (alain)
    /// <summary>
    public static Vector3 ExtractZAxisFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 column;
        column = new Vector3(matrix.m02, matrix.m12, matrix.m22);
        return column.normalized;
    }

    /// <summary>
    /// returns scale - matrix passed by reference to improve performance; no changes will be made to it.
    /// <summary>
    public static Vector3 ExtractScaleFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude; // x axis = column 0
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude; // y axis = column 1
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude; // z axis = column 2

        // extra code from forum post to handle negative scale
        // seems bogus -> scale.x ? could be scale.y !!! or scale.z 
        //if (Vector3.Cross(matrix.GetColumn(0), matrix.GetColumn(1)).normalized != (Vector3)matrix.GetColumn(2).normalized)
        //{
        //    scale.x *= -1;
        //}

        // handle negative scale - MY guess solution
        //if (Vector3.Dot(matrix.GetColumn(0), Vector3.left) < 0)     scale.x *= -1;
        //if (Vector3.Dot(matrix.GetColumn(1), Vector3.up) < 0)       scale.y *= -1;
        //if (Vector3.Dot(matrix.GetColumn(2), Vector3.forward) < 0)  scale.z *= -1;

        return scale;
    }


    /// <summary>
    /// Extract position, rotation and scale from TRS matrix.
    /// </summary>
    public static void DecomposeMatrix(ref Matrix4x4 matrix, out Vector3 localPosition, out Quaternion localRotation, out Vector3 localScale)
    {
        localPosition = ExtractTranslationFromMatrix(ref matrix);
        localRotation = ExtractRotationFromMatrix(ref matrix);
        localScale = ExtractScaleFromMatrix(ref matrix);
    }

    /// <summary>
    /// Set transform component from TRS matrix.
    /// </summary>
    public static void TransformFromMatrix(Transform transform, ref Matrix4x4 matrix)
    {
        transform.localPosition = ExtractTranslationFromMatrix(ref matrix);
        transform.localRotation = ExtractRotationFromMatrix(ref matrix);
        transform.localScale = ExtractScaleFromMatrix(ref matrix);
    }

    //public static void TransformFromMatrix(Transform transform, ref Matrix4x4 matrix)
    //{
    //    transform.position = ExtractTranslationFromMatrix(ref matrix);
    //    transform.rotation = ExtractRotationFromMatrix(ref matrix);
    //    transform.localScale = ExtractScaleFromMatrix(ref matrix);
    //}

    /// <summary>
    /// Identity quaternion.
    /// </summary>
    /// <remarks>
    /// <para>It is faster to access this variation than <c>Quaternion.identity</c>.</para>
    /// </remarks>
    public static readonly Quaternion IdentityQuaternion = Quaternion.identity;

    /// <summary>
    /// Identity matrix.
    /// </summary>
    /// <remarks>
    /// <para>It is faster to access this variation than <c>Matrix4x4.identity</c>.</para>
    /// </remarks>
    public static readonly Matrix4x4 IdentityMatrix = Matrix4x4.identity;


    // ------------------------
    // ADDED BY ME
    // ------------------------

    /// <summary>
    /// returns 4x4 ranslation matrix
    /// <summary>
    public static Matrix4x4 TranslationToMatrix(Vector3 offset)
    {
        Matrix4x4 matrix = IdentityMatrix;
        matrix.m03 = offset.x; //row 0 col 3
        matrix.m13 = offset.y; //row 1 col 3
        matrix.m23 = offset.z; //row 2 col 3
        return matrix;
    }

    /// <summary>
    /// returns 4x4 scale matrix
    /// <summary>
    public static Matrix4x4 ScaleToMatrix(Vector3 scale)
    {
        Matrix4x4 matrix = IdentityMatrix;
        matrix.m00 = scale.x; //row 0 col 0
        matrix.m11 = scale.y; //row 1 col 1
        matrix.m22 = scale.z; //row 2 col 2
        return matrix;
    }

    /// <summary>
    /// returns 4x4 rotation matrix
    /// <summary>
    public static Matrix4x4 QuaternionToMatrix(Quaternion q)
    {
        return Matrix4x4.Rotate(q);
    }



    // var matrix = Matrix4x4.TRS(new Vector3(1, 2, 3), Quaternion.Euler(10, 20, 30), new Vector3(12, 23, 34));

    /*
    // from https://github.com/lordofduct/space.../master/SpacepuppyBase/Utils/TransformUtil.cs
    //public static Quaternion MatrixToRotation(Matrix4x4 m)
    //{
    //    // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
    //    Quaternion q = new Quaternion();
    //    q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
    //    q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
    //    q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
    //    q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
    //    q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
    //    q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
    //    q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
    //    return q;
    //}
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOrbit : MonoBehaviour
{
    [Range(-89.99f,89.99f)]
    public float pitch;
    [Range(-180f, 180f)]
    public float azimuth;
    public float distance = 10;

    public GameObject target;
    private GameObject origin;

    private Quaternion q_pitch;
    private Quaternion q_azimuth;
    private Quaternion q_rot;

    // Start is called before the first frame update
    void Start()
    {
        origin = new GameObject();
        origin.name = gameObject.name + " LookAt";
    }

    // Update is called once per frame
    void Update()
    {
        q_pitch = Quaternion.AngleAxis(pitch, Vector3.left);
        q_azimuth = Quaternion.AngleAxis(azimuth, Vector3.up);
        q_rot = q_pitch * q_azimuth;

        #region transform local, not working

        // None of code below works
        // transformations are always in object local space
        // rotation operates as a camera pan/tilt, not orbit around 0,0,0

        // transform.SetPositionAndRotation(distance * Vector3.back, q_pitch * q_azimuth);
        // transform.position = distance * Vector3.back;
        // transform.rotation = q_pitch * q_azimuth;

        // regular matrix multiplication does not work, matrix transformations are not stacked
        //var matrix = Matrix4x4.TRS(distance * Vector3.back, MatrixExtensions.IdentityQuaternion, Vector3.one);
        //matrix *= Matrix4x4.TRS(Vector3.zero, q_pitch * q_azimuth, Vector3.one);
        //MatrixExtensions.TransformFromMatrix(transform, ref matrix);

        #endregion

        #region orbit y not using up vector

        // hack1 orbit does not works, because y rotation happens in xz plane, not around vector up
        // var matrix = MatrixExtensions.QuaternionToMatrix(q_rot);
        // var fwd = MatrixExtensions.ExtractZAxisFromMatrix(ref matrix);
        // transform.rotation = q_rot; 
        // transform.position = -distance * fwd;
        // transform.rotation = Quaternion.LookRotation(fwd, Vector3.up); // this is no better

        #endregion

        // vert/horiz orbit working correctly
        var matrix1 = MatrixExtensions.QuaternionToMatrix(q_pitch);
        var matrix2 = MatrixExtensions.QuaternionToMatrix(q_azimuth);
        transform.position = distance * Vector3.back;
        transform.position = matrix1.MultiplyPoint(transform.position);
        transform.position = matrix2.MultiplyPoint(transform.position);

        // added track object
        if (target != null)
        {
            transform.position += target.transform.position;
            transform.LookAt(target.transform);
        }
        else {
            // just in case the target is removed
            transform.LookAt(origin.transform);
        }
    }

}

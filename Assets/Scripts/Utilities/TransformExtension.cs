using UnityEngine;

public static class TransformExtension {

    // Rotate a direction Vector3 by Quaternion
    public static Vector3 RotateBy (this Vector3 vec, Quaternion rotation) {
        return rotation * vec;
    }

    // Utility to convert local rotation to global
    public static Quaternion TransformRotation (this Transform trans, Quaternion rotation) {
        return trans.rotation * rotation;
    }

    public static Quaternion TransformRotation (this Transform trans, Vector3 rotation) {
        Quaternion rot = Quaternion.Euler (rotation);
        return trans.rotation * rot;
    }

    public static Quaternion TransformRotation (this Transform trans, float x, float y, float z) {
        Quaternion rot = Quaternion.Euler (x, y, z);
        return trans.rotation * rot;
    }
}

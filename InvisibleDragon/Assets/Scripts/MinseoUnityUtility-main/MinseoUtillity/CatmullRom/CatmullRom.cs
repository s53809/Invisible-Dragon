using System;
using UnityEngine;

namespace Spline
{
    public static class CatmullRom
    {
        private static Matrix4x4 mMatrix = new Matrix4x4(
            new Vector4(0, -0.5f, 1, -0.5f),
            new Vector4(1, 0, -2.5f, 1.5f),
            new Vector4(0, 0.5f, 2, -1.5f),
            new Vector4(0, 0, -0.5f, 0.5f)
            );
        public static Vector3 Calculate(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Single t)
        {
            Matrix4x4 tMatrix = new Matrix4x4(
                new Vector4(1, 0, 0, 0),
                new Vector4(t, 0, 0, 0),
                new Vector4((Single)Math.Pow(t, 2), 0, 0, 0),
                new Vector4((Single)Math.Pow(t, 3), 0, 0, 0));

            Matrix4x4 pMatrix = new Matrix4x4(
                    new Vector4(p0.x, p1.x, p2.x, p3.x),
                    new Vector4(p0.y, p1.y, p2.y, p3.y),
                    new Vector4(p0.z, p1.z, p2.z, p3.z),
                    new Vector4(0, 0, 0, 0));

            Matrix4x4 Output = tMatrix * mMatrix * pMatrix;

            return new Vector3(Output.m00, Output.m01, Output.m02);
        }
    }

}
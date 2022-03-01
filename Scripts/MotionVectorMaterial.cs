using Gist2.Extensions.ComponentExt;
using Gist2.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectVelocity {

    public class MotionVectorMaterial : System.IDisposable, IValue<Material> {

        public const string PATH = "ObjectVelocity/Hidden_MotionVectorCapture";

        public static readonly int P_MotionVectorCapture0 = Shader.PropertyToID("_MotionVectorCapture0");

        protected Material mat;

        public MotionVectorMaterial() {
            mat = new Material(Resources.Load<Shader>(PATH));
        }

        #region interface

        #region properties
        public float Power { get; set; } = 1f;
        #endregion

        #region IDisposable
        public void Dispose() {
            mat.Destroy();
        }
        #endregion

        #region IValue
        public Material Value {
            get {
                var p0 = new Vector4(Power, 0f, 0f, 0f);

                mat.SetVector(P_MotionVectorCapture0, p0);
                return mat;
            }
        }
        #endregion

        #region static
        public static implicit operator Material(MotionVectorMaterial mvm)
            => mvm != null ? mvm.Value : null;
        #endregion

        #endregion
    }
}

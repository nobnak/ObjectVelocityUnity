using Gist2.Extensions.ComponentExt;
using Gist2.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectVelocity {

    public class VelocityVisualizerMaterial : System.IDisposable, IValue<Material> {

        public const string PATH = "ObjectVelocity/Hidden_VelocityVisualizer";

        public static readonly int P_ObjectVelocity0 = Shader.PropertyToID("_VelocityVisualizer0");

        protected Material mat;
        protected Vector4 p0;

        public VelocityVisualizerMaterial() {
            mat = new Material(Resources.Load<Shader>(PATH));
        }

        #region interface

        #region IDisposable
        public void Dispose() {
            mat.Destroy();
            mat = null;
        }
        #endregion

        #region IValue
        public Material Value { get => ApplyProperties().mat; }
        #endregion

        #region static
        public static implicit operator Material(VelocityVisualizerMaterial vvm)
            => (vvm != null) ? vvm.Value : null;
        #endregion

        public VelocityVisualizerMaterial ApplyProperties() {
            mat.SetVector(P_ObjectVelocity0, p0);
            return this;
        }
        public float Power { get => p0.x; set => p0.x = value; }
        #endregion
    }
}

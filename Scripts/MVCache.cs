using AffineDecomposition;
using AffineDecomposition.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ObjectVelocity {

    public class MVCache : MonoBehaviour {

        public Link link = new Link();

        protected int currFrame;
        protected Affine apast;

        #region unity
        private void OnEnable() {
            currFrame = -1;
            apast = (Affine)GetModelView(TargetCamera);
        }
        #endregion

        #region interface
        public Affine GetPast() {
            if (currFrame != Time.frameCount) {
                currFrame = Time.frameCount;
                var t = Mathf.Clamp01(Time.deltaTime * VelocityCam.GlobalTuner.targetFPS);
                var acurr = GetCurrent();
                apast = Affine.Lerp(apast, acurr, t);
            }
            return apast;
        }
        public Affine GetCurrent() => (Affine)GetModelView(TargetCamera);
        public float4x4 GetModelView(Camera c) 
            => math.mul(FlipZ(c.worldToCameraMatrix), (float4x4)transform.localToWorldMatrix);

        #region static
        public static float4x4 FlipZ(float4x4 m) => math.mul(float4x4.Scale(1f, 1f, -1f), m);
        #endregion

        public Camera TargetCamera { get => link.targetCam ?? Camera.main; }
        #endregion

        #region classes
        [System.Serializable]
        public class Link {
            public Camera targetCam;
        }
        #endregion
    }
}

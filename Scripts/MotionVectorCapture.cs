using Gist2.Extensions.SizeExt;
using Gist2.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectVelocity {

    [ExecuteAlways]
    public class MotionVectorCapture : MonoBehaviour {

        public TextureChangedEvents events = new TextureChangedEvents();
        public Tuner tuner = new Tuner();

        protected MotionVectorMaterial mat;
        protected RenderTextureWrapper output;

        #region unity
        private void OnEnable() {
            mat = new MotionVectorMaterial();
            output = new RenderTextureWrapper() {
                Generator = size => {
                    var tex = new RenderTexture(size.x, size.y, 0, RenderTextureFormat.ARGBHalf);
                    tex.wrapMode = TextureWrapMode.Clamp;
                    tex.filterMode = FilterMode.Bilinear;
                    tex.hideFlags = HideFlags.DontSave;
                    events.OnCreateTex.Invoke(tex);
                    return tex;
                },
            };
        }
        private void OnDisable() {
            output?.Dispose();
            output = null;
            mat?.Dispose();
            mat = null;
        }
        private void OnPreRender() {
            var c = Camera.current;
            c.depthTextureMode |= DepthTextureMode.MotionVectors | DepthTextureMode.Depth;
        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            output.Size = source.Size();
            mat.Power = tuner.power;

            Graphics.Blit(source, output, mat);
            Graphics.Blit(source, destination);

            events.OnUpdateTex.Invoke(output);
        }
        #endregion

        #region classes
        [System.Serializable]
        public class Tuner {
            [Range(0f, 10f)]
            public float power = 1f;
        }
        #endregion
    }
}

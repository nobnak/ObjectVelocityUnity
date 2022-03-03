using Gist2.Extensions.SizeExt;
using Gist2.Wrappers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectVelocity {

    [ExecuteAlways]
    public class VelocityVisualizerInvoker : MonoBehaviour {

        public static readonly int P_0 = Shader.PropertyToID("_VelocityVisualizer0");

        public TextureChangedEvents events = new TextureChangedEvents();
        public Tuner tuner = new Tuner();

        protected VelocityVisualizerMaterial mat;
        protected RenderTextureWrapper output;

        #region unity
        private void OnEnable() {
            mat = new VelocityVisualizerMaterial();
            output = new RenderTextureWrapper() {
                Generator = size => {
                    var tex = new RenderTexture(size.x, size.y, 24, RenderTextureFormat.ARGB32);
                    tex.filterMode = FilterMode.Bilinear;
                    tex.wrapMode = TextureWrapMode.Clamp;
                    tex.hideFlags = HideFlags.DontSave;
                    events.OnCreateTex.Invoke(tex);
                    return tex;
                },
            };
        }
        private void OnDisable() {
            mat.Dispose();
            mat = null;
        }
        private void Update() {
            mat.Power = tuner.power;
        }
        #endregion

        #region interface
        public void ListenOnUpdateVelocityTex(Texture tex) {
            if (mat == null || tex == null) return;

            output.Size = tex.Size();
            Graphics.Blit(tex, output, mat);
            events.OnUpdateTex.Invoke(output);
        }
        #endregion

        #region classes
        [System.Serializable]
        public class Tuner {
            [Range(0f, 1f)]
            public float power = 1f;
        }
        #endregion
    }
}

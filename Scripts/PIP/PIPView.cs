using Gist2.Extensions.SizeExt;
using LLGraphicsUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PIP {

    [ExecuteAlways]
    public class PIPView : MonoBehaviour {

        public Tuner tuner = new Tuner();
        public List<PIPTexture> textures = new List<PIPTexture>();

        protected GLMaterial mat;

        #region unity
        private void OnEnable() {
            mat = new GLMaterial();
        }
        private void OnDisable() {
            mat?.Dispose();
            mat = null;
        }
        private void OnRenderObject() {
            var c = Camera.current;
            if (c != Camera.main || (c.cullingMask & (1 << gameObject.layer)) == 0) return;

            if (mat == null) return;

            var prop = new GLProperty() {
                Cull = GLProperty.CullEnum.None,
                ZTestMode = GLProperty.ZTestEnum.ALWAYS,
                ZWriteMode = false,
                SrcBlend = BlendMode.One,
                DstBlend = BlendMode.Zero,
                VertexColor = GLProperty.KW_VERTEX_COLOR.NO_VERTEX_COLOR,
            };
            mat.LoadProperty(prop);

            var screenSize = c.Size();
            var texShare = tuner.texShare;
            var texHeight = Mathf.RoundToInt(texShare * screenSize.y);
            var texGap = tuner.texGap;
            var texOffset = (float)texGap;

            using (new GLMatrixScope()) {
                GL.LoadIdentity();
                GL.LoadPixelMatrix();

                foreach (Texture tex in textures) {
                    if (tex != null) {
                        var texSize = tex.Size();
                        var texAspect = texSize.Aspect();
                        var rect = new Rect(texOffset, texGap + texHeight, texAspect * texHeight, -texHeight);
                        texOffset += rect.width + texGap;

                        Graphics.DrawTexture(rect, tex, mat);
                    }
                }
            }
        }
        #endregion

        #region classes
        [System.Serializable]
        public class Tuner {
            [Range(0f, 1f)]
            public float texShare = 0.2f;
            public int texGap = 10;
        }
        #endregion
    }
}

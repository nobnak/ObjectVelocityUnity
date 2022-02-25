using Gist2.Extensions.LODExt;
using Gist2.Extensions.SizeExt;
using Gist2.Wrappers;
using UnityEngine;

namespace ObjectVelocity {

    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class VelocityCam : MonoBehaviour {

        public Link link = new Link();
        public Tuner tuner = new Tuner();
        public Events events = new Events();

        CameraWrapper manualCam;
        RenderTextureWrapper output;

        #region Unity
        void OnEnable() {
            output = new RenderTextureWrapper() {
                Generator = size => {
                    var tex = new RenderTexture(size.x, size.y, 24, RenderTextureFormat.ARGBFloat);
                    tex.filterMode = FilterMode.Bilinear;
                    tex.wrapMode = TextureWrapMode.Clamp;
                    events.OnUpdateVelocityTex.Invoke(tex);
                    return tex;
                }
            };
            manualCam = new CameraWrapper() {
                Updator = c => {
                    if (c == null) {
                        var go = new GameObject("Manual Camera");
                        go.hideFlags = HideFlags.HideAndDontSave;
                        c = go.AddComponent<Camera>();
                        c.enabled = false;
                    }
                    c.CopyFrom(link.refCam);
                    c.cullingMask = tuner.mask;
                    c.targetTexture = output;
                    return c;
                }
            };
        }
        void Update() {
            output.Size = link.refCam.Size().LOD(tuner.lod);
            manualCam.Value.RenderWithShader(link.replacementShader, null);
        }
        void OnDestroy() {
            if (output != null) {
                output.Dispose();
                output = null;
            }
        }
        #endregion

        #region classes
        [System.Serializable]
        public class TextureEvent : UnityEngine.Events.UnityEvent<Texture> {}

        [System.Serializable]
        public class Tuner {
            public LayerMask mask = -1;
            public int lod = 0;
        }

        [System.Serializable]
        public class Link {
            public Camera refCam;
            public Shader replacementShader;
        }

        [System.Serializable]
        public class Events {
            public TextureEvent OnUpdateVelocityTex = new TextureEvent();
        }
        #endregion
    }
}

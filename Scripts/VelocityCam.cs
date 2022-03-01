using Gist2.Extensions.LODExt;
using Gist2.Extensions.SizeExt;
using Gist2.Wrappers;
using UnityEngine;

namespace ObjectVelocity {

    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class VelocityCam : MonoBehaviour {

        public const string TAG_REPLACEMENT = "Velocity";

        public Link link = new Link();
        public TextureChangedEvents events = new TextureChangedEvents();
        public Tuner tuner = new Tuner();

        CameraWrapper manualCam;
        RenderTextureWrapper output;

        #region Unity
        void OnEnable() {
            output = new RenderTextureWrapper() {
                Generator = size => {
                    var tex = new RenderTexture(size.x, size.y, 24, RenderTextureFormat.ARGBFloat);
                    tex.filterMode = FilterMode.Bilinear;
                    tex.wrapMode = TextureWrapMode.Clamp;
                    events.OnCreateTex.Invoke(tex);
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

            GlobalTuner = tuner;
        }
        void Update() {
            var p0 = new Vector4(tuner.power, 1f / Time.deltaTime, 0f, 0f);
            p0 = new Vector4(tuner.power, tuner.targetFPS, 0f, 0f);

            output.Size = link.refCam.Size().LOD(tuner.lod);

            Shader.SetGlobalVector(ObjectVelocity.P_0, p0);
            manualCam.Value.RenderWithShader(link.replacementShader, TAG_REPLACEMENT);

            events.OnUpdateTex?.Invoke(output);
        }
        void OnDestroy() {
            if (output != null) {
                events.OnUpdateTex.Invoke(null);
                output.Dispose();
                output = null;
            }
        }
        #endregion

        #region interface

        #region static
        public static Tuner GlobalTuner { get; set; }
        #endregion

        #endregion

        #region classes
        [System.Serializable]
        public class Tuner {
            public LayerMask mask = -1;
            public int lod = 0;
            [Range(0f, 10f)]
            public float power = 1f;
            [Range(1f, 30f)]
            public float targetFPS = 5f;
        }

        [System.Serializable]
        public class Link {
            public Camera refCam;
            public Shader replacementShader;
        }
        #endregion
    }
}

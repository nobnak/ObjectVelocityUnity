using UnityEngine;

namespace ObjectVelocity {

    [ExecuteAlways]
	public class ObjectVelocity : MonoBehaviour {

		public static readonly int P_PrevMV = Shader.PropertyToID("_PrevMV");

		public int layer = -1;

		Renderer rend;
		MaterialPropertyBlock block;
		Matrix4x4 prevModelViewMatrix;

		#region Unity
		protected virtual void OnEnable() {
			rend = GetComponent<Renderer>();

			block = new MaterialPropertyBlock();
			prevModelViewMatrix = ModelView;

			if (layer >= 0) rend.gameObject.layer = layer;

		}
		protected virtual void Update () {
			rend.GetPropertyBlock(block);
			block.SetMatrix(P_PrevMV, prevModelViewMatrix);
			rend.SetPropertyBlock(block);

			prevModelViewMatrix = ModelView;
    	}
        private void OnDisable() {
			block?.Clear();
        }
        #endregion

        #region member
        protected Matrix4x4 ModelView => Camera.main.worldToCameraMatrix * transform.localToWorldMatrix;
        #endregion
    }
}

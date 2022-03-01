using Gist2.Math.Primitives;
using UnityEngine;

namespace ObjectVelocity {

    [ExecuteAlways]
	public class ObjectVelocity : MonoBehaviour {

		public static readonly int P_PrevMV = Shader.PropertyToID("_PrevMV");
		public static readonly int P_0 = Shader.PropertyToID("_ObjectVelocity0");

		public int layer = -1;

		Renderer rend;
		MaterialPropertyBlock block;

		TRSMatrix smoothedLastCameraMatrix;
		TRSMatrix smoothedLastParentMatrix;
		TRSMatrix smoothedLastObjectMatrix;

		#region Unity
		protected virtual void OnEnable() {
			rend = GetComponent<Renderer>();

			block = new MaterialPropertyBlock();

			smoothedLastCameraMatrix = Camera.main.transform;
			smoothedLastObjectMatrix = transform;
			smoothedLastParentMatrix = (transform.parent != null) ? transform.parent : TRSMatrix.Identity;

			if (layer >= 0) rend.gameObject.layer = layer;

		}
		protected virtual void Update () {
			var t = Mathf.Clamp01(Time.deltaTime * VelocityCam.GlobalTuner.targetFPS);
			smoothedLastObjectMatrix = TRSMatrix.Lerp(smoothedLastObjectMatrix, transform, t);
			smoothedLastCameraMatrix = TRSMatrix.Lerp(smoothedLastCameraMatrix, Camera.main.transform, t);
			if (transform.parent != null)
				smoothedLastParentMatrix = TRSMatrix.Lerp(smoothedLastParentMatrix, transform.parent, t);
			var prev = GetWorldToCameraMatrix(smoothedLastCameraMatrix)
				* smoothedLastParentMatrix * smoothedLastObjectMatrix;

			rend.GetPropertyBlock(block);
			block.SetMatrix(P_PrevMV, prev);
			rend.SetPropertyBlock(block);
    	}
        private void OnDisable() {
			block?.Clear();
        }
		#endregion

		#region interface

		#region static
		public static Matrix4x4 GetWorldToCameraMatrix(TRSMatrix trscam)
			=> Matrix4x4.Inverse(Matrix4x4.TRS(
				trscam.position,
				trscam.rotation,
				new Vector3(1f, 1f, -1f)
				));
        #endregion

        public Matrix4x4 ModelView => Camera.main.worldToCameraMatrix * transform.localToWorldMatrix;
        #endregion
    }
}

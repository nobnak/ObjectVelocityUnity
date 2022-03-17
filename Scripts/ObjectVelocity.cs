using AffineDecomposition;
using AffineDecomposition.Model;
using Unity.Mathematics;
using UnityEngine;

namespace ObjectVelocity {

    [ExecuteAlways]
	public class ObjectVelocity : MonoBehaviour {

		public static readonly int P_PrevMV = Shader.PropertyToID("_PrevMV");
		public static readonly int P_0 = Shader.PropertyToID("_ObjectVelocity0");

		public int layer = -1;

		Renderer rend;
		MaterialPropertyBlock block;

		MVCache view;
		TRS model;

		#region Unity
		protected virtual void OnEnable() {
			rend = GetComponent<Renderer>();
			view = GetComponentInParent<MVCache>();
			if (view == null) view = transform.parent.gameObject.AddComponent<MVCache>();
			
			block = new MaterialPropertyBlock();

			model = transform;

			if (layer >= 0) rend.gameObject.layer = layer;
		}
		protected virtual void Update () {
			var t = math.clamp(Time.deltaTime * VelocityCam.GlobalTuner.targetFPS, 0f, 1f);
			model = TRS.Lerp(model, transform, t);

			var mv = math.mul((float4x4)view.GetPast(), model);

			rend.GetPropertyBlock(block);
			block.SetMatrix(P_PrevMV, mv);
			rend.SetPropertyBlock(block);
    	}
        private void OnDisable() {
			block?.Clear();
        }
		#endregion

		#region interface
		#endregion
    }
}

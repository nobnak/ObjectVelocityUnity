using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectVelocity {

    [ExecuteAlways]
    public class PIP : MonoBehaviour {

        #region unity
        private void OnRenderObject() {
            var c = Camera.current;
            if (c != Camera.main || (c.cullingMask & (1 << gameObject.layer)) == 0) return;


        }
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PIP {

    public class PIPTexture : MonoBehaviour {

        public Texture tex = null;

        #region interface
        public void SetTexture(Texture tex) => this.tex = tex;
        #endregion

        #region static
        public static implicit operator Texture (PIPTexture pip) => (pip != null) ? pip.tex : null;
        #endregion
    }
}

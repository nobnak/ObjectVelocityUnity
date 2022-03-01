using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectVelocity {

    [System.Serializable]
    public class TextureEvent : UnityEngine.Events.UnityEvent<Texture> { }

    [System.Serializable]
    public class TextureChangedEvents {
        public TextureEvent OnCreateTex = new TextureEvent();
        public TextureEvent OnUpdateTex = new TextureEvent();
    }
}
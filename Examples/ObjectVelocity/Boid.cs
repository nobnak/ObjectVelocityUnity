using UnityEngine;
using System.Collections;

namespace ObjectVelocity {

    public class Boid : MonoBehaviour {
        public const float TWO_PI = 2f * Mathf.PI;

        public GameObject fab;
        public int count = 10;
        public float rotationSpeed = 1f;
        public float freq = 0.1f;

    	void Start () {
            for (var i = 0; i < count; i++) {
                var inst = Instantiate (fab);
                inst.transform.SetParent (transform, false);
                inst.transform.localPosition = Random.insideUnitSphere;
                inst.transform.localRotation = Random.rotationUniform;
                inst.SetActive (true);
            }
    	}
    	
    	void Update () {
            var t = Time.timeSinceLevelLoad * freq;
            var speed = TWO_PI * rotationSpeed;

            transform.localRotation = Quaternion.Euler (
                speed * Noise (t, 0f), speed * Noise (t, 100f), speed * Noise (t, 2000f));
    	}
        float Noise(float x, float y) {
            return 2f * Mathf.PerlinNoise(x, y) - 1f;
        }
    }
}

using UnityEngine;

namespace Game.UI
{
    public class ParticleDelay : MonoBehaviour
    {
        public float delayTime = 1.0f;
	
        // Use this for initialization
        void Start ()
        {
            gameObject.SetActive(false);
            Invoke ("DelayFunc", delayTime);
        }

        public void DelayFunc ()
        {
            gameObject.SetActive(true);
        }
    }
}
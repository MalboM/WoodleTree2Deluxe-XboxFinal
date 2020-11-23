using UnityEngine;

namespace DynamicShadowProjector.Sample {
	public class RotateOtherVerse : MonoBehaviour {

		public float m_rotateSpeed = 90.0f;

		void Update()
		{
            transform.Rotate(Vector3.right * Time.deltaTime * m_rotateSpeed);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class BoxSound : MonoBehaviour {

	AudioSource sound;
	public float maxVolume;

	float curSpeed;
	Rigidbody rb;

	void Start () {
		rb = this.gameObject.GetComponent<Rigidbody> ();
		sound = this.gameObject.GetComponent<AudioSource> ();
	}

	void Update () {
		curSpeed = Vector3.Magnitude (rb.velocity);
		if (curSpeed != 0f) {
			if (!sound.isPlaying)
				sound.Play ();
			sound.volume = Mathf.Lerp (sound.volume, maxVolume, Mathf.Abs (curSpeed) / 5f);
		} else {
			if (sound.isPlaying) {
				if (sound.volume > 0.01f)
					sound.volume = Mathf.Lerp (sound.volume, 0f, Time.deltaTime * 5f);
				else
					sound.Stop ();
			}
		}
	}
}

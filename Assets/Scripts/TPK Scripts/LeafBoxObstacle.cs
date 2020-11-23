using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBoxObstacle : MonoBehaviour {

	public int boxType;
	GameObject theBox;
	GameObject thePFX;

    AudioSource sound;
    public AudioClip[] correctSounds;
    public AudioClip wrongSound;

	void Start () {
        sound = this.gameObject.GetComponent<AudioSource>();
		theBox = this.transform.Find ("LeafBox").gameObject;
		thePFX = this.transform.Find ("Particles").gameObject;
	}

	public void DestroyBox(){
        StartCoroutine("DelayDeactivate");
		thePFX.SetActive (true);
        sound.clip = correctSounds[Random.Range(0, correctSounds.Length)];
        sound.pitch = Random.Range(0.8f, 1.2f);
        sound.Stop();
        sound.Play();
	}

    public void Wrong() {
        if (sound.clip != wrongSound)
            sound.clip = wrongSound;
        sound.Stop();
        sound.Play();
    }

    IEnumerator DelayDeactivate()
    {
        for (float f = 0f; f < 0.4f; f += Time.deltaTime * Time.timeScale)
        {
            while (DataManager.isSuspended)
                yield return null;

            yield return null;
        }
        theBox.SetActive(false);
        for (float f = 0f; f < 2f; f += Time.deltaTime * Time.timeScale)
        {
            while (DataManager.isSuspended)
                yield return null;

            yield return null;
        }
        Destroy(this.transform.parent.gameObject);
    }
}

using UnityEngine;
using System.Collections;
using ICode;

public class EnemyDieCommon : MonoBehaviour {

    Animator animator;
    public bool  die;
    public ICodeBehaviour Icodescript;
    public GameObject dieParticles;
    private ParticleSystem.EmissionModule dieParticleComponent;

	public GameObject berryPrefab;
	GameObject berry1;
	GameObject berry2;
	GameObject berry3;
	Rigidbody rb1;
	Rigidbody rb2;
	Rigidbody rb3;

	public AudioClip berryRelease;
	AudioSource sound;
    public AudioClip dieSound;

    void Awake() {
		animator = GetComponent<Animator>();

        if (this.gameObject.GetComponent<AudioSource>() == null)
            this.gameObject.AddComponent<AudioSource>();
        sound = this.gameObject.GetComponent<AudioSource>();

        if (berry1 == null && berryPrefab != null) {
			int rand = Random.Range (1, 4);
			berry1 = Instantiate (berryPrefab, null);
			rb1 = berry1.transform.GetChild(0).GetComponent<Rigidbody> ();
			berry1.SetActive (false);
			if (rand > 1) {
				berry2 = Instantiate (berryPrefab, null);
				rb2 = berry2.transform.GetChild(0).GetComponent<Rigidbody> ();
				berry2.SetActive (false);
			}
			if (rand > 2) {
				berry3 = Instantiate (berryPrefab, null);
				rb3 = berry3.transform.GetChild(0).GetComponent<Rigidbody> ();
				berry3.SetActive (false);
			}
		}
    }

	public void Die(){
		this.GetComponent<Collider>().enabled = false;
		animator.SetBool("Die", true);
        sound.clip = dieSound;
        sound.Play();
        StartCoroutine(Killed());
	}

    IEnumerator Killed() {
        dieParticles.GetComponent<ParticleSystem>().Play();
		yield return new WaitForSeconds(1.5f);
		if (berryPrefab != null) {
			berry1.SetActive (true);
			berry1.transform.position = this.transform.position;
			rb1.velocity = new Vector3 (Random.Range (-5f, 5f), 0f, Random.Range (-5f, 5f));
			rb1.AddForce (Vector3.up * rb1.mass * 5f, ForceMode.Impulse);
			if (berry2 != null) {
				berry2.SetActive (true);
				berry2.transform.position = this.transform.position;
				rb2.velocity = new Vector3 (Random.Range (-5f, 5f), 0f, Random.Range (-5f, 5f));
				rb2.AddForce (Vector3.up * rb2.mass * 5f, ForceMode.Impulse);
			}
			if (berry3 != null) {
				berry3.SetActive (true);
				berry3.transform.position = this.transform.position;
				rb3.velocity = new Vector3 (Random.Range (-5f, 5f), 0f, Random.Range (-5f, 5f));
				rb3.AddForce (Vector3.up * rb3.mass * 5f, ForceMode.Impulse);
			}
		}
		sound.enabled = true;
        sound.Stop();
		sound.clip = berryRelease;
		sound.loop = false;
		sound.PlayDelayed (0f);
		dieParticles.GetComponent<ParticleSystem>().Stop();
		gameObject.SetActive(false);
    }
}



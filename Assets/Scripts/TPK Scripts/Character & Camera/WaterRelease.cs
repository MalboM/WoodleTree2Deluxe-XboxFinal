using UnityEngine;
using System.Collections;

public class WaterRelease : MonoBehaviour {
    
    public GameObject splashParticles;
    [HideInInspector] public ParticleSystem.EmissionModule splashEM;

    [HideInInspector] public MeshRenderer mesh;
    private RaycastHit groundHit;
    public LayerMask whatIsCollidable;

    public AudioClip splashNoise;
    private AudioSource sound;

    [HideInInspector] public Rigidbody rb;

	EnemyHP currentEnemy;
    LeafBoxObstacle lbo;

    void Start(){
        splashEM = splashParticles.GetComponent<ParticleSystem>().emission;
        splashEM.enabled = false;
        groundHit = new RaycastHit();
        mesh = this.GetComponent<MeshRenderer>();
        sound = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.gameObject.GetComponent<NPCCage>() != null)
            {
                other.gameObject.GetComponent<NPCCage>().CageHit();
            }
            else
            {
                if (other.gameObject.name != "LeafBox" && other.gameObject.layer == 15)
                {
                    currentEnemy = other.gameObject.GetComponent<EnemyHP>();
                    if (currentEnemy != null && currentEnemy.isDark && !currentEnemy.invincible)
                    {
                        currentEnemy.health -= 100;
                        if (currentEnemy.health <= 0 && !currentEnemy.infiniteHealth)
                        {
                            currentEnemy.Death();
                        }
                        else
                        {
                            currentEnemy.BeenHit(true, -1);
                        }
                    }
                //    BurstDrop();
                }
                else
                {
                    if (other.gameObject.name != "LeafBox" && Physics.Raycast(this.transform.position, -Vector3.up, out groundHit, 2f, whatIsCollidable) && groundHit.normal.y >= 0.75f)
                    { }
                    else
                    {
                        lbo = other.gameObject.GetComponentInParent<LeafBoxObstacle>();
                        if (lbo != null)
                        {
                         //   BurstDrop();
                            if (lbo.boxType == 5)
                                lbo.DestroyBox();
                            else
                            {
                                lbo.Wrong();
                                PlayerManager.GetMainPlayer().PlayBlockStar();
                            }
                        }
                    }
                }
            }
            BurstDrop();
        }
    }

    void BurstDrop()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        mesh.enabled = false;
        splashEM.enabled = true;
        splashParticles.GetComponent<ParticleSystem>().Play();
        sound.clip = splashNoise;
        sound.PlayDelayed(0);
        StartCoroutine(Hush());
    }

    IEnumerator Hush(){
        yield return new WaitForSeconds(1f);
        mesh.enabled = true;
        splashEM.enabled = false;
        this.gameObject.SetActive(false);
    }
}

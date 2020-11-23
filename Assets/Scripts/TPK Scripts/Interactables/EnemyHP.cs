using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ICode;
using UnityEngine.AI;

public class EnemyHP : MonoBehaviour {

    public bool debugTest;

    public int health;
    int initHealth;
	public bool infiniteHealth;
    [SerializeField] private int invincibilityFrames;
    [HideInInspector] public bool invincible;
    [HideInInspector] public EnemyBarrier enemyBarrier;

    public bool damageOnly;
    public bool instantKill;

    public int damageAmount = 1;

    public bool isDark;

    public Animator anim;
    bool hasCheckedParam;
    bool hasParam;

	Rigidbody rb;
    BoxCollider boxCol;
    SphereCollider sphereCol;
    TPC chara;
    TPC currentChara;

    public Renderer[] renderers;
    List<Color> origRimCol = new List<Color>();
    List<float> origRimInt = new List<float>();

    public Collider[] collidersToIgnore;

    int f = 1;
    bool reverse = false;
    float iterator = 0f;
    int mCount = 0;
    Color endCol = new Color(1f, 0f, 0f, 0f);
    
    public ICodeBehaviour icodescript;

    public int berriesToSpawn = 1;
    public bool spawnBigBerries = false;

    public AudioClip berryRelease;
    AudioSource sound;
    public AudioClip dieSound;
    public bool useBigOuchSounds;

    private ParticleSystem.EmissionModule dieParticleComponent;
    NavMeshAgent nma;

    public bool dontFreezeEnemy;
    bool freezeEnemy;
    Vector3 fzPos;

    public GameObject parentToDisable;

    bool wasKilled;

    [HideInInspector] public bool hitByShield;

    void Start()
    {
        initHealth = health;

        if (anim == null && this.gameObject.GetComponent<Animator>() != null)
            anim = this.gameObject.GetComponent<Animator>();
        if (anim != null)
        {
            foreach (AnimatorControllerParameter pa in anim.parameters)
            {
                if (pa.name == "Hurt")
                    hasParam = true;
            }
        }

        if (this.gameObject.GetComponent<Rigidbody>())
            rb = this.gameObject.GetComponent<Rigidbody>();

        if (this.gameObject.GetComponent<BoxCollider>() != null)
            boxCol = this.gameObject.GetComponent<BoxCollider>();
        if (this.gameObject.GetComponent<SphereCollider>() != null)
            sphereCol = this.gameObject.GetComponent<SphereCollider>();

        if (renderers.Length == 0)
            renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                if (m.HasProperty("_RimColor") && m.HasProperty("_RimIntensityF"))
                {
                    origRimCol.Add(m.GetColor("_RimColor"));
                    origRimInt.Add(m.GetFloat("_RimIntensityF"));
                }
            }
        }

        if (this.GetComponent<ICodeBehaviour>() != null)
            icodescript = this.GetComponent<ICodeBehaviour>();

        if (!damageOnly)
        {
            if (this.transform.parent.parent != null && this.transform.parent.parent.GetComponent<IActivablePrefab>() != null)
                sound = this.transform.parent.parent.gameObject.AddComponent<AudioSource>();
            else
                sound = this.transform.parent.gameObject.AddComponent<AudioSource>();
            sound.playOnAwake = false;
        }

        if (infiniteHealth || damageOnly)
            endCol = new Color(1f, 1f, 1f, 0f);

        if (collidersToIgnore.Length > 0)
        {
            foreach (Collider c in collidersToIgnore)
            {
                if (sphereCol != null)
                    Physics.IgnoreCollision(sphereCol, c, true);
                if (boxCol != null)
                    Physics.IgnoreCollision(boxCol, c, true);
            }
        }

        if (infiniteHealth && instantKill && damageOnly)
            this.enabled = false;

    }

    private void LateUpdate()
    {
        if (!dontFreezeEnemy)
        {
            if (chara == null)
                chara = PlayerManager.GetMainPlayer();

            if (chara != null)
            {
                if (chara.inCutscene && !freezeEnemy)
                {
                    freezeEnemy = true;
                    fzPos = this.transform.position;
                }
                if (!chara.inCutscene && freezeEnemy)
                    freezeEnemy = false;
            }

            if (freezeEnemy)
            {
                this.transform.position = fzPos;
                if (icodescript != null && icodescript.gameObject != this.gameObject)
                    icodescript.transform.position = fzPos;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Foliage")
            TreeShakeManager.ShakeTree(other.gameObject, other.gameObject.GetComponent<MeshRenderer>());
        
        if (other.gameObject.name.Contains("Shield"))
        {
            hitByShield = true;
            if (icodescript != null)
                SendFlying(true);
            else
            {
                PlayerManager.GetMainPlayer().BounceCharacter(false, this.gameObject, false, false);
                StartCoroutine("WaitForShield");
            }
        }
    }

    //
    public void Death()
    {
        if (enemyBarrier)
            enemyBarrier.EnemyKilled();

        if (anim != null && hasParam)
        {
            anim.ResetTrigger("Hurt");
            anim.SetTrigger("Hurt");
        }
        if(useBigOuchSounds)
            EnemySoundManager.PlayBigOuchSound();
        else
            EnemySoundManager.PlayOuchSound();
        StopCoroutine("HitFlash");
        StartCoroutine("HitFlash");

        if (icodescript != null)
        {
            icodescript.enabled = false;
        }
        this.GetComponent<Collider>().enabled = false;
        if(anim)
            anim.SetBool("Die", true);

        if(icodescript == null)
            GameObject.FindWithTag("FXPool").GetComponent<BerryPFX>().PlayEffect(3, this.transform.position, this.gameObject, Vector3.zero, false);
        else 
            GameObject.FindWithTag("FXPool").GetComponent<BerryPFX>().PlayEffect(3, this.transform.position, icodescript.gameObject, Vector3.zero, true);

        if (rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (sound != null && dieSound != null)
        {
            sound.clip = dieSound;
            sound.Play();
        }
        StartCoroutine(Killed());

        // counter for trophies
        if (isDark)
        {
            //
            int darkKilledEnemies;
            if (!PlayerPrefs.HasKey("DarkEnemiesKilledCount"))
            {
                PlayerPrefs.SetInt("DarkEnemiesKilledCount", 0);
                darkKilledEnemies = 0;
            }
            else
                darkKilledEnemies = PlayerPrefs.GetInt("DarkEnemiesKilledCount");
            //
            if (darkKilledEnemies < 100)
            {
                darkKilledEnemies++;
                PlayerPrefs.SetInt("DarkEnemiesKilledCount", darkKilledEnemies);

                //
                if (PlayerPrefs.HasKey("DarkEnemiesAchiev"))
                {
                    if (PlayerPrefs.GetInt("DarkEnemiesAchiev") == 0 && darkKilledEnemies >= 100)
                        SetDarkAchievs();
                }
                else
                    PlayerPrefs.SetInt("DarkEnemiesAchiev", 0);
            }else
                SetDarkAchievs();
        }
        else
        {
            //
            int normalKilledEnemies;
            if (!PlayerPrefs.HasKey("NormalEnemiesKilledCount"))
            {
                PlayerPrefs.SetInt("NormalEnemiesKilledCount", 0);
                normalKilledEnemies = 0;
            }
            else
                normalKilledEnemies = PlayerPrefs.GetInt("NormalEnemiesKilledCount");
            //
            if (normalKilledEnemies < 100)
            {
                normalKilledEnemies++;
                PlayerPrefs.SetInt("NormalEnemiesKilledCount", normalKilledEnemies);
                
                //
                if (PlayerPrefs.HasKey("NormalEnemiesAchiev"))
                {
                    if (PlayerPrefs.GetInt("NormalEnemiesAchiev") == 0 && normalKilledEnemies >= 100)
                        SetNormalAchievs();
                }
                else
                    PlayerPrefs.SetInt("NormalEnemiesAchiev", 0);
            }else
                SetNormalAchievs();
        }
    }

    //
    void SetNormalAchievs()
    {
        //
#if UNITY_PS4
        //
        // check trophy :  normal enemies
        PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.DEFEAT_100_NATURAL_ENEMIES);
        PlayerPrefs.SetInt("NormalEnemiesAchiev", 1);
#endif


#if UNITY_XBOXONE
        //
        // check trophy : items >= 3 and items = all 
        //
        XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_WARRIOR);
#endif
    }

    //
    void SetDarkAchievs()
    {
        //
#if UNITY_PS4
        //
        // check trophy :  dark enemies
        PS4Manager.ps4TrophyManager.UnlockTrophy((int)PS4_TROPHIES.ANNIHILIATE_100_DARK_ENEMIES);
        PlayerPrefs.SetInt("DarkEnemiesAchiev", 1);

#endif

#if UNITY_XBOXONE
        //
        // check trophy : items >= 3 and items = all 
        //
        XONEAchievements.SubmitAchievement((int)XONEACHIEVS.WOODLE_AVENGER);
#endif
    }

    public void BeenHit(bool wasWindballOrDrop, int charaID) {
        if (anim != null && hasParam)
        {
            anim.ResetTrigger("Hurt");
            anim.SetTrigger("Hurt");
        }
        StopCoroutine("SendFlyingCoro");
        currentChara = PlayerManager.GetPlayer(charaID);
        SendFlying(wasWindballOrDrop);
        if (!infiniteHealth)
        {
            if (useBigOuchSounds)
                EnemySoundManager.PlayBigOuchSound();
            else
                EnemySoundManager.PlayOuchSound();
        }
        StopCoroutine("HitFlash");
        StartCoroutine("HitFlash");
    }

    public void SendFlying(bool wasWindballOrDrop)
    {
        StartCoroutine("SendFlyingCoro", wasWindballOrDrop);
    }

    IEnumerator WaitForShield()
    {
        yield return new WaitForSeconds(1f);
        hitByShield = false;
    }

    public IEnumerator SendFlyingCoro(bool wasWindballOrDrop)
    {
        if (currentChara == null)
            currentChara = PlayerManager.GetMainPlayer();
        Vector3 atkDir = (currentChara.transform.forward) * 10f;
        if (!wasWindballOrDrop)
        {
            if (rb)
                rb.isKinematic = true;
            if (this.gameObject.name != "DarkEnemyGiga2")
            {
                Vector3 newPos = Vector3.zero;
                float extension = 1f;
                if (boxCol != null)
                {
                    extension += (2f - boxCol.size.z);
                    newPos = currentChara.transform.position + (currentChara.transform.forward * extension * (boxCol.size.z));
                }
                if (sphereCol != null)
                {
                    extension += (2f - (sphereCol.radius * 2f));
                    newPos = currentChara.transform.position + (currentChara.transform.forward * extension * (sphereCol.radius * 2f));
                }

                if (sphereCol == null && boxCol == null)
                    newPos = currentChara.transform.position + (currentChara.transform.forward);
                for (int atkd = 0; (atkd < 40 && Vector3.Angle((this.transform.position - currentChara.transform.position).normalized, currentChara.transform.forward) < 90f); atkd++)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, newPos, 0.2f);

                    //        if (chara.input.GetButtonDown("Attack"))
                    //            invincible = false;

                    yield return null;
                }
            }
            else
                yield return new WaitForSeconds(0.6f);
        }
        if (rb)
        {
            rb.isKinematic = false;
            rb.AddForce(atkDir, ForceMode.Impulse);
        }
    //    invincible = true;
        for (int inv = 0; inv < invincibilityFrames; inv++)
        {
            if (rb)
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime);
            yield return null;
        }
        if (rb)
            rb.isKinematic = true;

        hitByShield = false;
    //    invincible = false;
    }

    IEnumerator HitFlash()
    {
        f = 1;
        reverse = false;
        iterator = 0f;
        mCount = 0;
        while(f >= 0)
        {
            iterator = (float)f / 20f;

            mCount = 0;
            foreach (Renderer r in renderers)
            {
                foreach (Material m in r.materials)
                {
                    if (m.HasProperty("_RimColor") && m.HasProperty("_RimIntensityF"))
                    {
                        m.SetColor("_RimColor", Color.Lerp(origRimCol[mCount], endCol, iterator));
                    //    if (m.HasProperty("_RimIntensityF"))
                            m.SetFloat("_RimIntensityF", Mathf.Lerp(origRimInt[mCount], 10f, iterator));
                        mCount++;
                    }
                }
            }

            if (f == 20)
                reverse = true;
            if (!reverse)
                f++;
            else
                f--;
            yield return null;
        }
    }

    IEnumerator Killed()
    {
        wasKilled = true;

        Vector3 curScale = this.transform.localScale;
        if (icodescript)
            curScale = icodescript.gameObject.transform.localScale;
        for(float k = 0f; k <= 1f; k += (1f/60f))
        {
            if(icodescript)
                icodescript.gameObject.transform.localScale = Vector3.Lerp(curScale, Vector3.one * 0.001f, k);
            else
                this.transform.localScale = Vector3.Lerp(curScale, Vector3.one * 0.001f, k);
            yield return null;
        }

        if (spawnBigBerries)
        {
            int total = Random.Range(2, 4);
            for (int k = 0; k < total; k++)
                BerrySpawnManager.SpawnABigBerry(this.transform.position);
        }
        else {
            if (berriesToSpawn > 0)
            {
                for (int k = 0; k < berriesToSpawn; k++)
                    BerrySpawnManager.SpawnABerry(this.transform.position);
            }
        }

        if (sound != null)
        {
            sound.enabled = true;
            sound.clip = berryRelease;
            sound.loop = false;
            sound.Stop();
            sound.PlayDelayed(0f);
        }

        foreach (Renderer r in renderers)
            r.enabled = false;

        yield return new WaitForSeconds(1f);


        if (parentToDisable == null)
        {
            if (this.transform.parent.parent != null && this.transform.parent.parent.GetComponent<IActivablePrefab>())
                Destroy(this.transform.parent.parent.gameObject);
            else
                Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Destroy(parentToDisable.gameObject);
        //    parentToDisable.SetActive(false);
        }
        
    }

    public void Reset()
    {
        if (wasKilled)
        {
            wasKilled = false;
            this.GetComponent<Collider>().enabled = true;
            anim.SetBool("Die", false);

            if (rb)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            health = initHealth;

            if (icodescript != null)
                icodescript.enabled = true;
        }
    }
}
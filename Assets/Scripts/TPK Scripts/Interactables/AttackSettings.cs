using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSettings : MonoBehaviour {
    public bool testsetsetsetset;

    public int attackAmount;
    int addedAttack = 0;
    public bool activeAttack;
    public int enemyLayer;

    [HideInInspector] public bool isWindball;
    private EnemyHP currentEnemy;

	[HideInInspector] public TPC tpc;

    List<Coroutine> coroutes = new List<Coroutine>();

    List<Collider> hitColliders = new List<Collider>();
    [HideInInspector] public Collider thisCollider;

    RaycastHit raycast;

    private void Start()
    {
        raycast = new RaycastHit();
    }

    private void Update()
    {
        if (!isWindball)
        {
            if (!activeAttack && !tpc.isHoldingLeaf && hitColliders.Count != 0)
            {
                foreach (Collider c in hitColliders)
                {
                    if (c != null)
                        Physics.IgnoreCollision(thisCollider, c, false);
                }

                hitColliders.Clear();

                thisCollider.enabled = false;
            }
            if (tpc != null && thisCollider != null)
            {
                if ((activeAttack || tpc.isHoldingLeaf) && !thisCollider.enabled)
                    thisCollider.enabled = true;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (thisCollider == null)
            thisCollider = this.GetComponent<Collider>();
        
        if (isWindball && activeAttack)
        {
            if (Physics.Linecast(this.transform.position - (this.GetComponent<Rigidbody>().velocity.normalized*2f), this.transform.position + (this.GetComponent<Rigidbody>().velocity.normalized*5f), out raycast, tpc.whatIsGround) 
                && raycast.collider == other && raycast.normal.y < tpc.groundTolerance && !raycast.collider.gameObject.name.Contains("Button"))
            {
                StopWindball();
                activeAttack = false;
            }
        }

        if (activeAttack)
        {
            if (other.gameObject.name == "Foliage")
                TreeShakeManager.ShakeTree(other.gameObject, other.gameObject.GetComponent<MeshRenderer>());

            if (other.gameObject.layer == enemyLayer)
            {
                Physics.IgnoreCollision(thisCollider, other, false);
                hitColliders.Add(other);

                currentEnemy = other.gameObject.GetComponent<EnemyHP>();
                //    Physics.IgnoreCollision(tpc.gameObject.GetComponent<Collider>(), other, true);

                //    StopCoroutine("InvinceDelay");
                //    coroutes.Add(StartCoroutine("InvinceDelay", other));

                if (this.gameObject.name == "Stone Attack")
                    StartCoroutine(StoneBounce());

                if (currentEnemy != null && !currentEnemy.invincible && !currentEnemy.damageOnly)
                {
                    tpc.HitAnEnemy(other.gameObject.transform.position, isWindball, currentEnemy.infiniteHealth);

                    if (!isWindball && tpc.pID == 0)
                    {
                        if (tpc.elfPower)
                            addedAttack = 1;
                        else
                            addedAttack = 0;
                    }

                    currentEnemy.health -= (attackAmount + addedAttack);
                    if (currentEnemy.health <= 0 && !currentEnemy.infiniteHealth)
                    {
                        currentEnemy.Death();
                    }
                    else
                    {
                        currentEnemy.BeenHit(isWindball, tpc.pID);
                    }
                }
            }

            if (other.gameObject.layer == 0 && !other.isTrigger)
            {
                if(other.gameObject.GetComponentInParent<EnemyBarrier>())
                {
                    if (isWindball)
                        StopWindball();
                    else
                        tpc.HitPushBack(tpc.transform.position - other.gameObject.transform.position);
                }
            }
        }
    }

    void StopWindball()
    {
        tpc.berryPFX.PlayEffect(3, this.transform.position, this.gameObject, Vector3.zero, false);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        foreach (Renderer r in this.gameObject.GetComponentsInChildren<Renderer>())
            r.enabled = false;
    }

    IEnumerator StoneBounce()
    {
        yield return null;
        tpc.gameObject.transform.position += (Vector3.up * 0.15f);
        tpc.BounceCharacter(false, null, false, false);
    }

    /*
    IEnumerator InvinceDelay(Collider other) {
        yield return null;
        if (this.gameObject.name == "Stone Attack")
        {
            tpc.gameObject.transform.position += (Vector3.up * 0.15f);
            tpc.BounceCharacter(false, null, false, false);
        }
        yield return new WaitForSeconds(0.8f);
        if(other != null)
            Physics.IgnoreCollision(tpc.gameObject.GetComponent<Collider>(), other, false);

        coroutes.RemoveAt(0);
    }*/
}

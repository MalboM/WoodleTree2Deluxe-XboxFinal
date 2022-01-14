using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryCollect : MonoBehaviour {
    
    public BerryManagerTrigger.BerryType berryType;
    public int amount;
    int initAmount;

    [HideInInspector] public bool collected;

    public int id;
    TPC tpc;

    private void Start()
    {
        if (amount <= 0)
            amount = 1;
        initAmount = amount;

        tpc = PlayerManager.GetMainPlayer();

        if(berryType == BerryManagerTrigger.BerryType.blue)
        {
            //IF IN ANY CASE THE PREF HAS VALUE THE BERRY SHUOLDN'T BE ACTIVE
            if (PlayerPrefs.GetString(this.gameObject.scene.name + "BlueBerry" + id) != "" &&
                PlayerPrefs.GetString(this.gameObject.scene.name + "BlueBerry" + id) != "0" &&
                PlayerPrefs.GetString(this.gameObject.scene.name + "BlueBerry" + id) != null)
                {
                    transform.root.gameObject.SetActive(false);
                }
        }
        

        /*
        if (tpc.ps.enableDebugTab && berryType == BerryManagerTrigger.BerryType.blue)
        {
            collected = true;
            if (berryType == BerryManagerTrigger.BerryType.blue)
            {
                string temp1 = PlayerPrefs.GetString(this.gameObject.scene.name + "BlueBerry");
                string temp2 = temp1.Remove(id, 1);
                string temp3 = temp2.Insert(id, "1");
                PlayerPrefs.SetString(this.gameObject.scene.name + "BlueBerry", temp3);
            }
            StartCoroutine("MultiCollect", tpc.initialParent.GetComponent<BerryManagerTrigger>());
        }*/
    }

    void OnEnable ()
    {
        this.GetComponent<SphereCollider>().enabled = true;

        if(initAmount != 0)
            amount = initAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //    if(other.transform.parent.gameObject.GetComponent<BerryManagerTrigger>())
            //        other.transform.parent.gameObject.GetComponent<BerryManagerTrigger>().CollectBerry(this.transform.parent.gameObject, berryType);
            //    else
            collected = true;


            if (berryType == BerryManagerTrigger.BerryType.blue)
            {
                Debug.LogError("YAN: BerryCollect in scene " + this.gameObject.scene.name +". Collect BB ID: " + id.ToString());

                string temp1 = PlayerPrefs.GetString(this.gameObject.scene.name + "BlueBerry" + id);

                Debug.LogError("YAN: BB Pref orig value: " + temp1);

                temp1 = "1";

                Debug.LogError("YAN: BB Pref new value: " + temp1);

                PlayerPrefs.SetString(this.gameObject.scene.name + "BlueBerry" + id, temp1);

                //string temp2 = temp1.Remove(id, 1);
                //string temp3 = temp2.Insert(id, "1");


#if UNITY_XBOXONE && !UNITY_EDITOR

                DataManager.xOneEventsManager.SaveBlueBerry
                    (
                        (this.gameObject.scene.name + "BlueBerry"+ id), temp1
                    );
#endif

            }

            StartCoroutine("MultiCollect", other.gameObject.GetComponent<TPC>().initialParent.GetComponent<BerryManagerTrigger>());
        }
    }

    IEnumerator MultiCollect(BerryManagerTrigger bmt)
    {
        while (tpc.inCutscene || tpc.ps.sS.inStart || Time.timeSinceLevelLoad < 2f)
            yield return null;

        bmt.CollectBerry(this.transform.parent.gameObject, berryType, initAmount, (initAmount == amount), initAmount);
        amount--;
        yield return new WaitForSeconds(0.2f);
        if(amount != 0)
            StartCoroutine("MultiCollect", bmt);
    }

    /*
        int indx = Random.Range(0, heh.Length);
        char charac = heh[indx];
        string heh2 = heh.Remove(indx, 1);
        string heh3 = heh2.Insert(indx, "X");
    */
}

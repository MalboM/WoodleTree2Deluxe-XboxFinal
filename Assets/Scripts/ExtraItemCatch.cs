using UnityEngine;
using System.Collections;

public class ExtraItemCatch : MonoBehaviour {

    public GameObject animatorobject;
    Animator animator;

    public string nameobj;
    
    public ActivateItemsExtra scriptcollectables;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")

        {
        //    if (PlayerPrefs.GetInt("FirstCollectable", 0) == 0)
        //    {
        //        PlayerPrefs.SetInt("FirstCollectable", 1);
                ItemPromptManager.DisplayPrompt(18, -1, null, null, null, false, false);
            //    }

            this.gameObject.GetComponent<Collider>().enabled = false;
            animator = this.GetComponent<Animator>();
            animator.SetBool("Activate", true);
            if (GameObject.Find("Collectables") != null && GameObject.Find("Collectables").GetComponent<ActivateItemsExtra>() != null)
            {
                scriptcollectables = GameObject.Find("Collectables").GetComponent<ActivateItemsExtra>();

                if (nameobj == "marmellade1")
                {
                    scriptcollectables.marmellade1obj.SetActive(true);
                    PlayerPrefs.SetInt("marmellade1", 1);
                }

                if (nameobj == "marmellade2")
                {
                    scriptcollectables.marmellade2obj.SetActive(true);
                    PlayerPrefs.SetInt("marmellade2", 1);
                }


                if (nameobj == "marmellade3")
                {
                    scriptcollectables.marmellade3obj.SetActive(true);
                    PlayerPrefs.SetInt("marmellade3", 1);
                }
                if (nameobj == "plantjar")
                {
                    scriptcollectables.plantjarobj.SetActive(true);
                    PlayerPrefs.SetInt("plantjar", 1);
                }

                if (nameobj == "plantjar2")
                {
                    scriptcollectables.plantjar2obj.SetActive(true);
                    PlayerPrefs.SetInt("plantjar2", 1);
                }

                if (nameobj == "book1")
                {
                    scriptcollectables.book1obj.SetActive(true);
                    PlayerPrefs.SetInt("book1", 1);
                }

                if (nameobj == "book2")
                {
                    scriptcollectables.book2obj.SetActive(true);
                    PlayerPrefs.SetInt("book2", 1);

                }
                if (nameobj == "book3")
                {
                    scriptcollectables.book3obj.SetActive(true);
                    PlayerPrefs.SetInt("book3", 1);

                }
                if (nameobj == "paint")
                {
                    scriptcollectables.paint1obj.SetActive(true);
                    PlayerPrefs.SetInt("paint", 1);

                }
                if (nameobj == "paint2")
                {
                    scriptcollectables.paint2obj.SetActive(true);
                    PlayerPrefs.SetInt("paint2", 1);

                }
                if (nameobj == "gameboy")
                {
                    scriptcollectables.gameboyobj.SetActive(true);
                    PlayerPrefs.SetInt("gameboy", 1);

                }
                if (nameobj == "bell")
                {
                    scriptcollectables.bellobj.SetActive(true);
                    PlayerPrefs.SetInt("bell", 1);

                }
                if (nameobj == "heater")
                {
                    scriptcollectables.heaterobj.SetActive(true);
                    PlayerPrefs.SetInt("heater", 1);

                }
                if (nameobj == "globe")
                {
                    scriptcollectables.globjeobj.SetActive(true);
                    PlayerPrefs.SetInt("globe", 1);

                }
                if (nameobj == "cupbear")
                {
                    scriptcollectables.cupbearobj.SetActive(true);
                    PlayerPrefs.SetInt("cupbear", 1);

                }
                if (nameobj == "compass")
                {
                    scriptcollectables.compassobj.SetActive(true);
                    PlayerPrefs.SetInt("compass", 1);

                }
                if (nameobj == "carpet")
                {
                    scriptcollectables.carpetobj.SetActive(true);
                    PlayerPrefs.SetInt("carpet", 1);

                }

                if (nameobj == "candle")
                {
                    scriptcollectables.candleobj.SetActive(true);
                    PlayerPrefs.SetInt("candle", 1);

                }

                if (nameobj == "statue1")
                {
                    scriptcollectables.statue1obj.SetActive(true);
                    PlayerPrefs.SetInt("statue1", 1);

                }

                if (nameobj == "statue2")
                {
                    scriptcollectables.statue2obj.SetActive(true);
                    PlayerPrefs.SetInt("statue2", 1);

                }

                if (nameobj == "statue3")
                {
                    scriptcollectables.statue3obj.SetActive(true);
                    PlayerPrefs.SetInt("statue3", 1);

                }

                if (nameobj == "mask1")
                {
                    scriptcollectables.mask1obj.SetActive(true);
                    PlayerPrefs.SetInt("mask1", 1);

                }

                if (nameobj == "mask2")
                {
                    scriptcollectables.mask2obj.SetActive(true);
                    PlayerPrefs.SetInt("mask2", 1);

                }

                if (nameobj == "mask3")
                {
                    scriptcollectables.mask3obj.SetActive(true);
                    PlayerPrefs.SetInt("mask3", 1);

                }

                if (nameobj == "map")
                {
                    scriptcollectables.mapobj.SetActive(true);
                    PlayerPrefs.SetInt("map", 1);

                }

                if (nameobj == "jukebox")
                {
                    scriptcollectables.jukeboxobj.SetActive(true);
                    PlayerPrefs.SetInt("jukebox", 1);

                }
                if (nameobj == "inbox")
                {
                    scriptcollectables.inboxobj.SetActive(true);
                    PlayerPrefs.SetInt("inbox", 1);

                }
                if (!PlayerPrefs.HasKey("CollectablesTotal"))
                    PlayerPrefs.SetInt("CollectablesTotal", 0);
                PlayerPrefs.SetInt("CollectablesTotal", PlayerPrefs.GetInt("CollectablesTotal", 0) + 1);
                other.gameObject.GetComponent<TPC>().itemText.text = PlayerPrefs.GetInt("CollectablesTotal").ToString();
            //    PlayerPrefs.Save();

                StartCoroutine(WaitToDeact());
            }
        }

    }

    IEnumerator WaitToDeact()
    {
        yield return new WaitForSeconds(2f);
        this.transform.parent.gameObject.SetActive(false);
    }
}

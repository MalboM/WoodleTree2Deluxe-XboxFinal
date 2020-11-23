using UnityEngine;
using System.Collections;

public class ActivateAtDistanceStayExternalWorld : MonoBehaviour {

    public GameObject nature;
    
    public GameObject itemsflowerbooster;
    public GameObject itemsberries;
    public GameObject itemshouses;


    public GameObject enemieswarriors;
    public GameObject enemieswoodtiny;

    public GameObject enemiesworm;

    public GameObject enemiesbushhorned;

    public GameObject enemiesleaf;
    public GameObject enemiesmushroom;
    public GameObject enemieswoodleseed;


    public GameObject blackmug;

    void Start()
    {
        if (nature != null)
            nature.SetActive(false);

        if (enemieswarriors != null)
            enemieswarriors.SetActive(false);

        if (enemieswoodtiny != null)
            enemieswoodtiny.SetActive(false);

        if (itemsflowerbooster != null)
            itemsflowerbooster.SetActive(false);

        if (itemsberries != null)
            itemsberries.SetActive(false);



         if (enemiesworm != null)
             enemiesworm.SetActive(false);

        if (enemiesbushhorned != null)
            enemiesbushhorned.SetActive(false);

        if (enemiesleaf != null)
            enemiesleaf.SetActive(false);


        if (enemiesmushroom != null)
            enemiesmushroom.SetActive(false);

        if (enemieswoodleseed != null)
            enemieswoodleseed.SetActive(false);


        if (itemshouses != null)
            itemshouses.SetActive(false);


        if (blackmug != null)
            blackmug.SetActive(false);
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if(nature != null)
                nature.SetActive(true);

            if (enemieswarriors != null)
                enemieswarriors.SetActive(true);

            if (enemieswoodtiny != null)
                enemieswoodtiny.SetActive(true);


            if (itemsflowerbooster != null)
                itemsflowerbooster.SetActive(true);


            if (itemsberries != null)
                itemsberries.SetActive(true);


             if (enemiesworm != null)
                 enemiesworm.SetActive(true);

            if (enemiesbushhorned != null)
                enemiesbushhorned.SetActive(true);


            if (enemiesleaf != null)
                enemiesleaf.SetActive(true);


            if (enemiesmushroom != null)
                enemiesmushroom.SetActive(true);


            if (enemieswoodleseed != null)
                enemieswoodleseed.SetActive(true);


            if (itemshouses != null)
                itemshouses.SetActive(true);


            if (blackmug != null)
                blackmug.SetActive(true);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.name == "Woodle Character")
        {
            if (nature != null)
                nature.SetActive(false);

            if (enemieswarriors != null)
                enemieswarriors.SetActive(false);

            if (enemieswoodtiny != null)
                enemieswoodtiny.SetActive(false);

            if (itemsflowerbooster != null)
                itemsflowerbooster.SetActive(false);

            if (itemsberries != null)
                itemsberries.SetActive(false);



            if (enemiesworm != null)
                 enemiesworm.SetActive(false);


            if (enemiesbushhorned != null)
                enemiesbushhorned.SetActive(false);

            if (enemiesleaf != null)
                enemiesleaf.SetActive(false);


            if (enemiesmushroom != null)
                enemiesmushroom.SetActive(false);


            if (enemieswoodleseed != null)
                enemieswoodleseed.SetActive(false);


            if (itemshouses != null)
                itemshouses.SetActive(false);


            if (blackmug != null)
                blackmug.SetActive(false);


        }


    }



}

using UnityEngine;
using System.Collections;

public class ActivateItemsExtra : MonoBehaviour
{


    public bool marmellade1;
    public bool marmellade2;
    public bool marmellade3;
    public bool plantjar;
    public bool plantjar2;
    public bool book1;
    public bool book2;
    public bool book3;
    public bool paint;
    public bool paint2;
    public bool gameboy;
    public bool bell;
    public bool heater;
    public bool globe;
    public bool cupbear;
    public bool compass;
    public bool carpet;
    public bool candle;
    public bool statue1;
    public bool statue2;
    public bool statue3;
    public bool mask1;
    public bool mask2;
    public bool mask3;
    public bool map;
    public bool jukebox;
    public bool inbox;

    public GameObject marmellade1obj;
    public GameObject marmellade2obj;
    public GameObject marmellade3obj;
    public GameObject plantjarobj;
    public GameObject plantjar2obj;
    public GameObject book1obj;
    public GameObject book2obj;
    public GameObject book3obj;
    public GameObject paint1obj;
    public GameObject paint2obj;
    public GameObject gameboyobj;
    public GameObject bellobj;
    public GameObject heaterobj;
    public GameObject globjeobj;
    public GameObject cupbearobj;
    public GameObject compassobj;
    public GameObject carpetobj;
    public GameObject candleobj;
    public GameObject statue1obj;
    public GameObject statue2obj;
    public GameObject statue3obj;
    public GameObject mask1obj;
    public GameObject mask2obj;
    public GameObject mask3obj;
    public GameObject mapobj;
    public GameObject jukeboxobj;
    public GameObject inboxobj;

    void Start()
    {

        if (PlayerPrefs.GetInt("marmellade1", 0) == 1)
        {
            marmellade1obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("marmellade2", 0) == 1)
        {
            marmellade2obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("marmellade3", 0) == 1)
        {
            marmellade3obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("plantjar", 0) == 1)
        {
            plantjarobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("plantjar2", 0) == 1)
        {
            plantjar2obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("book1", 0) == 1)
        {
            book1obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("book2", 0) == 1)
        {
            book2obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("book3", 0) == 1)
        {
            book3obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("paint", 0) == 1)
        {
            paint2obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("paint2", 0) == 1)
        {
            paint2obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("gameboy", 0) == 1)
        {
            gameboyobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("bell", 0) == 1)
        {
            bellobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("heater", 0) == 1)
        {
            heaterobj.SetActive(true);
        }


        if (PlayerPrefs.GetInt("globe", 0) == 1)
        {
            globjeobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("cupbear", 0) == 1)
        {
            cupbearobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("compass", 0) == 1)
        {
            compassobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("carpet", 0) == 1)
        {
            carpetobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("candle", 0) == 1)
        {
            candleobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("statue1", 0) == 1)
        {
            statue1obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("statue2", 0) == 1)
        {
            statue2obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("statue3", 0) == 1)
        {
            statue3obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("mask1", 0) == 1)
        {
            mask1obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("mask2", 0) == 1)
        {
            mask2obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("mask3", 0) == 1)
        {
            mask3obj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("map", 0) == 1)
        {
            mapobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("jukebox", 0) == 1)
        {
            jukeboxobj.SetActive(true);
        }

        if (PlayerPrefs.GetInt("inbox", 0) == 1)
        {
            inboxobj.SetActive(true);
        }
    }

    public void DeactivateAllInHouse()
    {
        marmellade1obj.SetActive(false);
        marmellade2obj.SetActive(false);
        marmellade3obj.SetActive(false);
        plantjarobj.SetActive(false);
        plantjar2obj.SetActive(false);
        book1obj.SetActive(false);
        book2obj.SetActive(false);
        book3obj.SetActive(false);
        paint1obj.SetActive(false);
        paint2obj.SetActive(false);
        gameboyobj.SetActive(false);
        bellobj.SetActive(false);
        heaterobj.SetActive(false);
        globjeobj.SetActive(false);
        cupbearobj.SetActive(false);
        compassobj.SetActive(false);
        carpetobj.SetActive(false);
        candleobj.SetActive(false);
        statue1obj.SetActive(false);
        statue2obj.SetActive(false);
        statue3obj.SetActive(false);
        mask1obj.SetActive(false);
        mask2obj.SetActive(false);
        mask3obj.SetActive(false);
        mapobj.SetActive(false);
        jukeboxobj.SetActive(false);
        inboxobj.SetActive(false);
    }
}

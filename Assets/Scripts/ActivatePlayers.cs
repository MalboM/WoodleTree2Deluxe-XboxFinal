using UnityEngine;
using System.Collections;
//using Steamworks;

public class ActivatePlayers : MonoBehaviour {
    
    private GameObject[] charas = new GameObject[3];
    bool firstInactive;

    public GameObject fox;
    public GameObject beaver;
    public GameObject bush;

    ThirdPersonController fTPC;
    ThirdPersonController bvTPC;
    ThirdPersonController bsTPC;

    GameObject c2;
    GameObject c3;
    GameObject c4;

    private PlayerInputs pI;

    int fIX;
    int bvIX;
    int bsIX;

    void Start () {
        charas = GameObject.FindGameObjectsWithTag("Multiplayer");
        pI = GameObject.FindWithTag("Handler").GetComponent<PlayerInputs>();
        int cnt = 0;
        while (cnt < 3) {
            if (charas[cnt].gameObject == fox)
                fTPC = charas[cnt].gameObject.GetComponent<ThirdPersonController>();
            if (charas[cnt].gameObject == beaver)
                bvTPC = charas[cnt].gameObject.GetComponent<ThirdPersonController>();
            if (charas[cnt].gameObject == bush)
                bsTPC = charas[cnt].gameObject.GetComponent<ThirdPersonController>();
            cnt++;
        }
        if (fTPC.playerIndex == 2)
            charas[0] = fox;
        if (fTPC.playerIndex == 3)
            charas[1] = fox;
        if (fTPC.playerIndex == 4)
            charas[2] = fox;

        if (bvTPC.playerIndex == 2)
            charas[0] = beaver;
        if (bvTPC.playerIndex == 3)
            charas[1] = beaver;
        if (bvTPC.playerIndex == 4)
            charas[2] = beaver;

        if (bsTPC.playerIndex == 2)
            charas[0] = bush;
        if (bsTPC.playerIndex == 3)
            charas[1] = bush;
        if (bsTPC.playerIndex == 4)
            charas[2] = bush;
    }
	
    void Update(){
        if (firstInactive){
            if (!charas[0].activeInHierarchy && pI.startPress2)
            {
                Debug.Log("X2");
                charas[0].gameObject.SetActive(true);
                charas[0].gameObject.GetComponent<ThirdPersonController>().MadeActive();
               // SteamUserStats.SetAchievement("Friend request!");
            }
            if(!charas[1].activeInHierarchy && pI.startPress3)
            {
                Debug.Log("X3");
                charas[1].gameObject.SetActive(true);
                charas[1].gameObject.GetComponent<ThirdPersonController>().MadeActive();
              //  SteamUserStats.SetAchievement("Friend request!");
            }
            if (!charas[2].activeInHierarchy && pI.startPress4)
            {
                Debug.Log("X4");
                charas[2].gameObject.SetActive(true);
                charas[2].gameObject.GetComponent<ThirdPersonController>().MadeActive();
               // SteamUserStats.SetAchievement("Friend request!");
            }
        }
    }
    
	void LateUpdate () {
        if (!firstInactive){
            int i = 0;
            while(i < 3){
                charas[i].gameObject.SetActive(false);
                i++;
            }
            firstInactive = true;
        }
	}
}

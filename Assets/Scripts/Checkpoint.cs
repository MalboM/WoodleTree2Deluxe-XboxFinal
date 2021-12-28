using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public int checkpointID;

    //bool activatedcheckpoint;
    public GameObject checkpointposition;

    float positionx;
    float positiony;
    float positionz;

	GameObject character;
    
    PauseScreen ps;
    Animator animator;

    //public TPC scriptcamera;


    void Start(){
		if (checkpointID == 0)
			ActivateCheckpoint ();

        if (character == null)
            character = PlayerManager.GetMainPlayer().gameObject;

        animator = GetComponent<Animator>();

        //    activatedcheckpoint = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == character)
        {
         //   if (!activatedcheckpoint)
                EnterCheckpoint();
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == character)
        {
            if (activatedcheckpoint)
                activatedcheckpoint = false;
        }
    }*/

    void EnterCheckpoint(){
    //    activatedcheckpoint = true;
        if (PlayerPrefs.GetInt("FirstCheckpoint", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstCheckpoint", 1);
            GameObject.FindWithTag("Pause").transform.Find("Event Text").gameObject.GetComponent<TextTriggerMain>().SetText(2);
        }

        if (ps == null)
            ps = GameObject.FindWithTag("Pause").GetComponent<PauseScreen>();
        if (ps != null)
        {
            ps.checkpointAnim.ResetTrigger("checkpoint");
            ps.checkpointAnim.SetTrigger("checkpoint");
        }
        animator.SetBool("Activated", true);

        PlayerPrefs.SetInt("LastCheckpoint", checkpointID);
        ActivateCheckpoint();
    }

	public void ActivateCheckpoint()
    {
        positionx = checkpointposition.transform.position.x;
		positiony = this.transform.position.y + 1f;
		positionz = checkpointposition.transform.position.z;

		PlayerPrefs.SetInt ("Checkpoint" + checkpointID.ToString (), 1);
		PlayerPrefs.SetInt ("Checkpoint" + checkpointID.ToString () + "Scene", this.gameObject.scene.buildIndex);
	//	PlayerPrefs.SetFloat ("Checkpoint" + checkpointID.ToString () + "X", positionx);
	//	PlayerPrefs.SetFloat ("Checkpoint" + checkpointID.ToString () + "Y", positiony);
	//	PlayerPrefs.SetFloat ("Checkpoint" + checkpointID.ToString () + "Z", positionz);
            
        PlayerPrefs.Save();
	}
}

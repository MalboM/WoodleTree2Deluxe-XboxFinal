using UnityEngine;
using System.Collections;

public class SetUpExampleHigh: MonoBehaviour {
	 
	public GameObject AudioSource;
	public HighPass sceneHighPass;

	void Update () {

	if (Input.GetKeyDown ("1")) 
		{
			sceneHighPass.cutoff = 10;
		}

	if (Input.GetKeyDown ("2")) 
		{
			sceneHighPass.cutoff = 300;
		}

	if (Input.GetKeyDown ("3")) 
		{
			sceneHighPass.cutoff = 600;
		}

	if (Input.GetKeyDown ("4")) 
		{
			sceneHighPass.cutoff = 3000;
		}

	if (Input.GetKeyDown ("5")) 
		{
			sceneHighPass.cutoff = 10000;
		}

		if (Input.GetKeyDown ("6")) 
		{
			sceneHighPass.cutoff = 20000;
		}
	
	}

	void OnGUI ()	
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(Screen.width / 800.0f, Screen.height / 700.0f, 1));

		sceneHighPass.cutoff = (int)(GUI.VerticalSlider (new Rect (394, 200, 40, 110),(float)sceneHighPass.cutoff, 10, 20000));
		GUI.Box (new Rect (375, 160, 50, 24), sceneHighPass.cutoff.ToString ("F0"));
	}	
}
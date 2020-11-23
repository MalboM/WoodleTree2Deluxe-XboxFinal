using UnityEngine;
using System.Collections;

public class SetUpExampleLow : MonoBehaviour {
	 
	public GameObject AudioSource;
	public LowPass sceneLowPass;

	void Update () {

	if (Input.GetKeyDown ("1")) 
		{
			sceneLowPass.cutoff = 300;
		}

	if (Input.GetKeyDown ("2")) 
		{
			sceneLowPass.cutoff = 600;
		}

	if (Input.GetKeyDown ("3")) 
		{
			sceneLowPass.cutoff = 900;
		}

	if (Input.GetKeyDown ("4")) 
		{
			sceneLowPass.cutoff = 2000;
		}

	if (Input.GetKeyDown ("5")) 
		{
			sceneLowPass.cutoff = 6000;
		}

		if (Input.GetKeyDown ("6")) 
		{
			sceneLowPass.cutoff = 20000;
		}
	
	}

	void OnGUI ()	
	{
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3(Screen.width / 800.0f, Screen.height / 700.0f, 1));

		sceneLowPass.cutoff = (int)(GUI.VerticalSlider (new Rect (394, 200, 40, 110),(float)sceneLowPass.cutoff, 100, 20000));
		GUI.Box (new Rect (375, 160, 50, 24), sceneLowPass.cutoff.ToString ("F0"));
	}	
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateAtDistanceNew : MonoBehaviour {

	public float fadeDelay = 0.0f; 
	public float fadeTime = 1f; 
	public bool fadeInOnStart = false; 
	public bool fadeOutOnStart = false;
	private bool logInitialFadeSequence = true; 
	public bool test;

	GameObject chara;
	public GameObject obj;
	public float distance;
	float dist;
	bool fading;

	private Color[] colors; 

	void Start ()
	{
		if (distance == 0f)
			distance = 10f;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.name == "Woodle Character")
                chara = g;
        }
        //	FadeOut ();
    }

	void Update() {	
		if(obj != null){
			dist = Vector3.Distance(chara.transform.position, obj.transform.position);
			if (!fading) {
				if (obj.activeInHierarchy == true && dist >= distance) {
					FadeOut ();
				}
				if (obj.activeInHierarchy == false && dist < distance) {
					FadeIn ();
				}
			}
		}
	}

	float MaxAlpha()
	{
		float maxAlpha = 0.0f; 
		Renderer[] rendererObjects = GetComponentsInChildren<Renderer>(); 
		foreach (Renderer item in rendererObjects){
			maxAlpha = Mathf.Max (maxAlpha, item.material.color.a); 
		}
		return maxAlpha; 
	}

	IEnumerator FadeSequence (float fadingOutTime)
	{
		// log fading direction, then precalculate fading speed as a multiplier
		bool fadingOut = (fadingOutTime < 0.0f);
		float fadingOutSpeed = 1.0f / fadingOutTime; 

		// grab all child objects
		Renderer[] rendererObjects = GetComponentsInChildren<Renderer>(); 
		if (colors == null){
			//create a cache of colors if necessary
			int q = 0;
			for (int i = 0; i < rendererObjects.Length; i++){
				foreach (Material m in rendererObjects[i].materials)
					q++;
			}
			colors = new Color[q]; 

			// store the original colours for all child objects
			q = 0;
			for (int i = 0; i < rendererObjects.Length; i++){
				foreach (Material m in rendererObjects[i].materials) {
					if (m.HasProperty("_Color")) 
						colors [q] = m.color;
					q++;
				}
			}
		}
		for (int i = 0; i < rendererObjects.Length; i++){
			foreach (Material m in rendererObjects[i].materials) {
				/*	m.SetFloat ("_Mode", 2f);
				m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				m.SetInt("_ZWrite", 0);
				m.DisableKeyword("_ALPHATEST_ON");
				m.DisableKeyword("_ALPHABLEND_ON");
				m.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				m.renderQueue = 3000;*/
				m.shader = Shader.Find ("Legacy Shaders/Transparent/Bumped Diffuse");
			}
		}

		// make all objects visible
		for (int i = 0; i < rendererObjects.Length; i++){
			rendererObjects[i].enabled = true;
		}

		// get current max alpha
		float alphaValue = MaxAlpha();

		// This is a special case for objects that are set to fade in on start. 
		// it will treat them as alpha 0, despite them not being so. 
		if (logInitialFadeSequence && !fadingOut){
			alphaValue = 0.0f; 
			logInitialFadeSequence = false; 
		}

		// iterate to change alpha value 
		while ( (alphaValue >= (0.0f) && fadingOut) || (alphaValue <= (1.0f) && !fadingOut)) {
			alphaValue += Time.deltaTime * fadingOutSpeed;
			int q = 0;
			for (int i = 0; i < rendererObjects.Length; i++){
				foreach (Material m in rendererObjects[i].materials) {
					if (m.HasProperty ("_Color")) {
						Color newColor = (colors != null ? colors [q] : m.color);
						newColor.a = Mathf.Min (newColor.a, alphaValue); 
						newColor.a = Mathf.Clamp (newColor.a, 0.0f, 1.0f);
						m.SetColor ("_Color", newColor);
					}
					q++;
				}
			}
			yield return null; 
		}

		// turn objects off after fading out
		if(obj != null){
			if (fadingOut)
				obj.SetActive (false);
			else {
				for (int i = 0; i < rendererObjects.Length; i++) {
					foreach (Material m in rendererObjects[i].materials) {
						m.shader = Shader.Find ("Standard");
						/*	m.SetFloat ("_Mode", 0f);
						m.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
						m.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
						m.SetInt ("_ZWrite", 1);
						m.DisableKeyword ("_ALPHATEST_ON");
						m.DisableKeyword ("_ALPHABLEND_ON");
						m.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
						m.renderQueue = -1;*/
					}
				}
			}
		}
		fading = false;
	}


	void FadeIn ()
	{
		fading = true;
		obj.SetActive (true);
		FadeIn (fadeTime); 
	}

	void FadeOut ()
	{
		fading = true;
		FadeOut (fadeTime); 		
	}

	void FadeIn (float newFadeTime)
	{
		StopAllCoroutines(); 
		StartCoroutine("FadeSequence", newFadeTime); 
	}

	void FadeOut (float newFadeTime)
	{
		StopAllCoroutines(); 
		StartCoroutine("FadeSequence", -newFadeTime); 
	}
}

using UnityEngine;
using System.Collections;

public class ActivateAtDistanceNew2 : MonoBehaviour{

    GameObject chara;
    public GameObject obj;
    public float distance;
	public float timePerCheck;
    float dist;
	float checker;
	public float fadeDelay = 0.0f; 
	public float fadeTime = 1f; 
	bool fadeInOnStart = false; 
	bool fadeOutOnStart = false;
	private bool logInitialFadeSequence = true; 
	private Color[] colors; 
	float[] renderObjsMode = new float[512];
	bool fading;
	Renderer[] rendererObjects;

    void Start() {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.name == "Woodle Character")
                chara = g;
        }

		if (distance == 0f)
			distance = 10f;
		if (timePerCheck == 0f)
			timePerCheck = 2f;
		checker = timePerCheck + 1f;

	/*	rendererObjects = GetComponentsInChildren<Renderer> ();
		if (colors == null) {
			//create a cache of colors if necessary
			int q = 0;
			for (int i = 0; i < rendererObjects.Length; i++) {
				foreach (Material m in rendererObjects[i].materials) {
					q++;
				}
			}
			colors = new Color[q]; 

			// store the original colours for all child objects
			q = 0;
			int mo = 0;
			for (int i = 0; i < rendererObjects.Length; i++) {
				foreach (Material m in rendererObjects[i].materials) {
					if (m.HasProperty ("_Color"))
						colors [q] = m.color;
					if (m.shader == Shader.Find ("Toony Colors Pro 2/Standard PBS")) {
						renderObjsMode [mo] = m.GetFloat ("_Mode");
						mo++;
					}
					q++;
				}
			}
        }*/
        StartCoroutine(CheckNow());
    }

	float MaxAlpha()
	{
		float maxAlpha = 0.0f; 
		Renderer[] rendererObjects = GetComponentsInChildren<Renderer>(); 
		foreach (Renderer item in rendererObjects){
			if(item.material.HasProperty("_Color"))
				maxAlpha = Mathf.Max (maxAlpha, item.material.color.a); 
		}
		return maxAlpha; 
	}

    IEnumerator CheckNow() {
        yield return new WaitForSeconds(timePerCheck);
        if(chara != null && obj != null)
        {
            dist = Vector3.Distance(chara.transform.position, obj.transform.position);
            if (!fading)
            {
                if (obj.activeInHierarchy == true && dist >= distance)
                {
                    //    FadeOut();
                    obj.SetActive(false);
                }
                if (obj.activeInHierarchy == false && dist < distance)
                {
                    //    FadeIn();
                    obj.SetActive(true);
                }
            }
        }
        StartCoroutine(CheckNow());
    }

	IEnumerator FadeSequence (float fadingOutTime)
	{
		// log fading direction, then precalculate fading speed as a multiplier
		bool fadingOut = (fadingOutTime < 0.0f);
		float fadingOutSpeed = 1.0f / fadingOutTime; 

		if (GetComponentsInChildren<Renderer> () != null) {
			// grab all child objects

			if(fadingOutTime < 0f){
				for (int i = 0; i < rendererObjects.Length; i++) {
					foreach (Material m in rendererObjects[i].materials) {
						if (m.shader == Shader.Find ("Toony Colors Pro 2/Standard PBS")) {
							m.SetFloat ("_Mode", 3f);
							m.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
							m.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
							m.SetInt ("_ZWrite", 0);
							m.DisableKeyword ("_ALPHATEST_ON");
							m.DisableKeyword ("_ALPHABLEND_ON");
							m.EnableKeyword ("_ALPHAPREMULTIPLY_ON");
							m.renderQueue = 3000;
						}
					}
				}
			}

			// make all objects visible
			for (int i = 0; i < rendererObjects.Length; i++)
				rendererObjects [i].enabled = true;

			// get current max alpha
			float alphaValue = -1f;
			for (int aaa = 0; ((aaa < rendererObjects.Length) && (alphaValue < 0f)); aaa++) {
				if (rendererObjects [aaa].material.shader == Shader.Find ("Toony Colors Pro 2/Standard PBS")) {
					alphaValue = MaxAlpha ();
				}
			}
			if (alphaValue < 0f)
				alphaValue = 0f;

			// This is a special case for objects that are set to fade in on start. 
			// it will treat them as alpha 0, despite them not being so. 
			if (logInitialFadeSequence && !fadingOut) {
				alphaValue = 0.0f; 
				logInitialFadeSequence = false; 
			}

			// iterate to change alpha value 
			while ((alphaValue >= (0.0f) && fadingOut) || (alphaValue <= (1.0f) && !fadingOut)) {
				alphaValue += Time.deltaTime * fadingOutSpeed;
				int q = 0;
				for (int i = 0; i < rendererObjects.Length; i++) {
					foreach (Material m in rendererObjects[i].materials) {
						if (m.HasProperty ("_Color")) {
							Color newColor = ((colors != null && q < colors.Length) ? colors [q] : m.color);
							newColor.a = Mathf.Min (newColor.a, alphaValue); 
							newColor.a = Mathf.Clamp (newColor.a, 0.0f, 1.0f);
							m.SetColor ("_Color", newColor);
						}
						q++;
					}
				}
										yield return null; 
			}

			if(obj != null){
				if (fadingOut) {
					obj.SetActive (false);
				}
				else {
					int mod = 0;
					for (int i = 0; i < rendererObjects.Length; i++) {
						foreach (Material m in rendererObjects[i].materials) {
							if (m.shader == Shader.Find ("Toony Colors Pro 2/Standard PBS")) {
								if (renderObjsMode [mod] == 0f) {
									m.SetFloat ("_Mode", 0f);
									m.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
									m.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
									m.SetInt ("_ZWrite", 1);
									m.DisableKeyword ("_ALPHATEST_ON");
									m.DisableKeyword ("_ALPHABLEND_ON");
									m.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
									m.renderQueue = -1;
								}
								if (renderObjsMode [mod] == 1f) {
									m.SetFloat ("_Mode", 1f);
									m.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
									m.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
									m.SetInt ("_ZWrite", 1);
									m.EnableKeyword ("_ALPHATEST_ON");
									m.DisableKeyword ("_ALPHABLEND_ON");
									m.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
									m.renderQueue = 2450;
								}
								if (renderObjsMode [mod] == 2f) {
									m.SetFloat ("_Mode", 2f);
									m.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
									m.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
									m.SetInt ("_ZWrite", 0);
									m.DisableKeyword ("_ALPHATEST_ON");
									m.EnableKeyword ("_ALPHABLEND_ON");
									m.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
									m.renderQueue = 3000;
								}
								mod++;
							}
						}
					}
				}
			}
		} else {
			if (fadingOut)
				obj.SetActive 								(false);
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

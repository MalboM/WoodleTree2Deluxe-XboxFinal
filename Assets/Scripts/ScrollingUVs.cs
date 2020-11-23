using UnityEngine;
using System.Collections;

public class ScrollingUVs : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";

    Vector2 uvOffset = Vector2.zero;
	Renderer r;

    public bool projMode;
    public Projector projector;
    public bool randomRate;

	void OnEnable(){
        if(!projMode)
    		r = this.GetComponent<Renderer> ();
	}

    void Update()
    {
        if(!randomRate)
            uvOffset += (uvAnimationRate * Time.deltaTime);
        else
            uvOffset += (new Vector2(Random.Range(0f,1f), Random.Range(0f, 1f)) * Time.deltaTime);
        if (r != null && r.enabled)
        {
            if(textureName != "5f4d61696e546578" || r.materials[materialIndex].HasProperty("5f4d61696e546578"))
                r.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
        if (projMode)
        {
            projector.material.SetTextureOffset(textureName, uvOffset);
        }
    }
}
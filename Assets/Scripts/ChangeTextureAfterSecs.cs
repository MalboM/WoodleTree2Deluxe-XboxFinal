using UnityEngine;
using System.Collections;

public class ChangeTextureAfterSecs : MonoBehaviour {


    public Texture texture1;
    public Texture texture2;

    public bool changedtexture;

    // Use this for initialization
    void Awake()
    {
        StartCoroutine(changetexture1());

    }


    
    IEnumerator changetexture1()
    {

        while (true)
        {
            yield return new WaitForSeconds(2.5f);

            this.GetComponent<Renderer>().materials[1].mainTexture = texture2;


            yield return new WaitForSeconds(1.0f);
            this.GetComponent<Renderer>().materials[1].mainTexture = texture1;

           
        }
    }


}


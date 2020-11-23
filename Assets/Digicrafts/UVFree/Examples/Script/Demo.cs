using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Demo : MonoBehaviour {

	public GameObject obj;

	public Slider topSlider;
	public Slider bottomSlider;
	public Dropdown topDropdown;
	public Dropdown bottomDropdown;
	public Dropdown modelDropdown;

	public ColorPicker topColorPicker;
	public ColorPicker bottomColorPicker;

	public Button topColorButton;
	public Button bottomColorButton;

	private Color topColor;
	private Color bottomColor;

	private List<Mesh> mesh;


	// Use this for initialization
	void Start () {
	
		topSlider.value=obj.GetComponent<MeshRenderer>().material.GetFloat("_PowerTop");
		bottomSlider.value=obj.GetComponent<MeshRenderer>().material.GetFloat("_PowerBottom");

		mesh = new List<Mesh>();

		GameObject o = (GameObject)Resources.Load("stone");
		mesh.Add(o.GetComponentInChildren<MeshFilter>().sharedMesh);
		o = (GameObject)Resources.Load("lion");
		mesh.Add(o.GetComponentInChildren<MeshFilter>().sharedMesh);
		o = (GameObject)Resources.Load("cube");
		mesh.Add(o.GetComponentInChildren<MeshFilter>().sharedMesh);
		o = (GameObject)Resources.Load("teaport");
		mesh.Add(o.GetComponentInChildren<MeshFilter>().sharedMesh);



		topColorPicker.gameObject.SetActive(false);
		bottomColorPicker.gameObject.SetActive(false);

		topColor = new Color(1,1,1);
		bottomColor = new Color(1,1,1);

		topColorPicker.CurrentColor=topColor;
		bottomColorPicker.CurrentColor=bottomColor;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeStrength(Slider target){

		if(target.name=="TopSlider"){
			obj.GetComponent<MeshRenderer>().material.SetFloat("_PowerTop",target.value);
		} else if(target.name=="BottomSlider"){
			obj.GetComponent<MeshRenderer>().material.SetFloat("_PowerBottom",target.value);
		}

	}

	public void changeTopDirection(){

//		obj.GetComponent<MeshRenderer>().material.DisableKeyword("_UVFREE_UP_TOP");
//		obj.GetComponent<MeshRenderer>().material.DisableKeyword("_UVFREE_UP_FRONT");
//		obj.GetComponent<MeshRenderer>().material.DisableKeyword("_UVFREE_UP_LEFT");

		int index = topDropdown.value;

//		if(index==0){
//
//		} else if(index==1)
//			obj.GetComponent<MeshRenderer>().material.EnableKeyword("_UVFREE_UP_TOP");
//		else if(index==2)
//			obj.GetComponent<MeshRenderer>().material.EnableKeyword("_UVFREE_UP_FRONT");
//		else if(index==3)
//			obj.GetComponent<MeshRenderer>().material.EnableKeyword("_UVFREE_UP_LEFT");

		obj.GetComponent<MeshRenderer>().material.SetFloat("_Top",index);
	}

	public void changeBottomDirection(){
//
//		obj.GetComponent<MeshRenderer>().material.DisableKeyword("_UVFREE_DOWN_BOTTOM");
//		obj.GetComponent<MeshRenderer>().material.DisableKeyword("_UVFREE_DOWN_BACK");
//		obj.GetComponent<MeshRenderer>().material.DisableKeyword("_UVFREE_DOWN_RIGHT");

		int index = bottomDropdown.value;

//		if(index==0){
//
//		} else if(index==1)
//			obj.GetComponent<MeshRenderer>().material.EnableKeyword("_UVFREE_DOWN_BOTTOM");
//		else if(index==2)
//			obj.GetComponent<MeshRenderer>().material.EnableKeyword("_UVFREE_DOWN_BACK");
//		else if(index==3)
//			obj.GetComponent<MeshRenderer>().material.EnableKeyword("_UVFREE_DOWN_RIGHT");
//
		obj.GetComponent<MeshRenderer>().material.SetFloat("_Bottom",index);
	}

	public void changeModel(){

		int idx = modelDropdown.value;
		obj.GetComponent<MeshFilter>().mesh= mesh[idx];

	}


	public void toggleTopColor() {				
		bottomColorPicker.gameObject.SetActive(false);
		topColorPicker.gameObject.SetActive(!topColorPicker.gameObject.activeSelf);


	}

	public void toggleBottomColor() {
		topColorPicker.gameObject.SetActive(false);
		bottomColorPicker.gameObject.SetActive(!bottomColorPicker.gameObject.activeSelf);
	}

	public void setTopColor(Color c) {

		topColor.r=c.r;
		topColor.g=c.g;
		topColor.b=c.b;

		obj.GetComponent<MeshRenderer>().material.SetColor("_ColorTop",topColor);

		ColorBlock cb = topColorButton.colors;
		cb.normalColor=cb.highlightedColor=cb.pressedColor=topColor;
		topColorButton.colors=cb;
	}

	public void setBottomColor(Color c) {

		bottomColor.r=c.r;
		bottomColor.g=c.g;
		bottomColor.b=c.b;

		obj.GetComponent<MeshRenderer>().material.SetColor("_ColorBottom",bottomColor);

		ColorBlock cb = bottomColorButton.colors;
		cb.normalColor=cb.highlightedColor=cb.pressedColor=bottomColor;
		bottomColorButton.colors=cb;
	}


}

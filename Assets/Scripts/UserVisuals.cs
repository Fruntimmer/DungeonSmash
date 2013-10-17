using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserVisuals{
	private static UserVisuals instance;
	private UserVisuals() {}

	public static UserVisuals Instance
	{	
		get 
		{
			if (instance == null)
			{
				instance = new UserVisuals();
			}
			return instance;
	  }
	}

	public Color dragColor = new Color(0.74f,1,0.65f,0.43f);
	public Color pieceColor = new Color(0.67f,1,0.60f,0.7f);
	private GameObject powerIndicator;
	private GameObject powerIndicatorLine;
	private List<GameObject> indicatorList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void CreatePowerIndicator(Vector3 pos){
		powerIndicator = (GameObject)GameObject.Instantiate(Resources.Load("PowerIndicatorPrefab"), new Vector3(pos.x, 0.11f, pos.z), Quaternion.identity);
		powerIndicatorLine = GameObject.Find("PowerIndicator");
	}

	public void UpdatePowerIndicator(Vector3 pos, float dist){
		Vector3 posFixed = new Vector3(pos.x, 0.11f, pos.z);
		powerIndicator.transform.LookAt(posFixed);
		powerIndicator.transform.localScale = new Vector3(2-((dist/5)*1), 1, dist);
		powerIndicatorLine.renderer.material.color = new Color((100+((dist/5)*155))/255, 0,0,.8f);
	}
	public void DestroyPowerIndicator(){
		GameObject.Destroy(powerIndicator);
	}

	public void CreateSelection(Vector3 pos){
		CreateIndicator(pos+(Vector3.up*0.01f), 5, dragColor);
		CreateIndicator(pos+(Vector3.up*0.02f), .75f, pieceColor);
	}
	public void CreateIndicator(Vector3 pos, float radius, Color col){
		GameObject indicator = (GameObject)GameObject.Instantiate(Resources.Load("RangeIndicatorPrefab"), pos, Quaternion.identity);
		indicator.transform.localScale = new Vector3(radius*2, 1, radius*2);
		indicator.renderer.material.color = col;
		indicatorList.Add(indicator);

	}
	public void DestroyIndicator(){
		foreach(GameObject obj in indicatorList){
			GameObject.Destroy(obj);
		}
	}
}

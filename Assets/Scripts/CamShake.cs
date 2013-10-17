using UnityEngine;
using System.Collections;

public class CamShake : MonoBehaviour {
	float intensity = 0;
	bool shakeActive = false;
	float sDrag = 1;
	float sTimer = 0.4f;
	float sATimer;
	float max = 0.3f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(shakeActive){
			Shake();
		}
	}
	void Shake(){
		transform.position += new Vector3(Random.Range(-max,max)*intensity*sDrag, 0, Random.Range(-max,max)*intensity*sDrag);
		sDrag *=0.8f;
		sATimer -= Time.deltaTime;
		if(sATimer <= 0){
			shakeActive = false;
			sDrag = 1;
		}
	}
	void ActivateShake(float intensity){
		this.intensity = intensity;
		shakeActive = true;
		sATimer = sTimer;
	}
}

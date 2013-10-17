using UnityEngine;
using System.Collections;

public class CamMovementScript : MonoBehaviour {
	Vector3 prevPos;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		transform.position += moveDirection * Time.deltaTime *25;
	}
	void RightClickMove(Vector3 moveDir){
		transform.position += moveDir * Time.deltaTime *25;
	}
}

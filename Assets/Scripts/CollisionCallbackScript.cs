using UnityEngine;
using System.Collections;

public class CollisionCallbackScript : MonoBehaviour {

	public CollisionDelegate collisionDelegate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		collisionDelegate(collision);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour {

	public string letter;

	private bool approached;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Approach()
	{
		approached = true;
		ChangeColour(Color.black);
	}

	public void Grab(GameObject hand)
	{
		if (approached)
		{
			Grabbed(hand);
		}
		approached = false;
	}

	public void Leave()
	{
		ChangeColour(Color.white);
		approached = false;
	}

	private void Grabbed(GameObject hand)
	{
		ChangeColour(Color.red);
		var parent = transform.parent.gameObject;
		var parentRigidbody = parent.GetComponent<Rigidbody>();
		Debug.Log("letters parent rigidbody, sleeping? " + parentRigidbody.IsSleeping());
		var handRigidbody = hand.GetComponent<Rigidbody>();
		
		// parent.GetComponent<SpringJoint>().connectedBody = hand.GetComponent<Rigidbody>();
		// gameObject.GetComponent<SpringJoint>().connectedBody = handRigidbody;

		// turns out even directly adding force to parent object didn't work:
		// parentRigidbody.AddForce(new Vector3(0f, 0f, 0.1f));

		// ...but suddenly by deleting all Rigidbody components in all parent's children, then things start moving...
		// ...but quite unstable
		parent.GetComponent<SpringJoint>().connectedBody = handRigidbody;

		// the colliders cannot be set to Triggers (i think this is Physics.OverlapSphere not picking them up)

		//parentRigidbody.AddForce(new Vector3(0f, 0f, 0.1f));
	}

	private void ChangeColour(Color color)
	{
		Material material = gameObject.GetComponent<Renderer>().material;
		material.color = color;
	}

}

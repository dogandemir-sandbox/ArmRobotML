using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public bool isAttached = false;
	public ArmAgent armAgent;
	Vector3 startPos;
	Rigidbody rb;


	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		startPos = transform.position;
		rb = GetComponent<Rigidbody>();
		
	}
	public void Reset(){
		transform.position = startPos;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
	void FixedUpdate()
	{
		if (isAttached){
			transform.position = armAgent.handleSensorArr[1].transform.position;
		}
		
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("BallEndPlatform")){
			armAgent.BallTouchedEndPlatform();
			return;
		}

		if (other.gameObject.CompareTag("Platform")){
			armAgent.BallTouchedPlatform();
			return;
		}
	}


	public void Attach(){
		isAttached = true;
		rb.isKinematic = true;
	}

	public void Detach(){
		isAttached = false;
		rb.isKinematic = false;
	}


}

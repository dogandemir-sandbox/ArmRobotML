using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAngle : MonoBehaviour {

	public Transform target;

	void Update () {
		 // Check if the gaze is looking at the front side of the object
		// Vector3 forward = transform.forward;
		// Vector3 toOther = (target.position - transform.position).normalized;
		
		// if(Vector3.Dot(forward, toOther) < 0.95f){
		// 	Debug.Log("Not facing the object");
		// 	return ;
		// }
		
		// Debug.Log("Facing the object");
		// return ;

		// print( Vector3.Distance(  transform.position, target.position ) );

		print( transform.up  );

		// Vector3 offset = target.position - transform.position;
		// float sqrLen = offset.sqrMagnitude;
		// print(sqrLen);

		// print( Vector3.Distance(transform.position, target.position));

		// print( transform.TransformDirection(target.localPosition) );
		
	}
}

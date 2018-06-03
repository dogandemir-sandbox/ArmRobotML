using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSensor : MonoBehaviour {

	public ArmAgent armAgent;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Ball")){
			armAgent.HandleSensorTouched();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Ball")){
			armAgent.HandleSensorUntouched();
		}
	}
}

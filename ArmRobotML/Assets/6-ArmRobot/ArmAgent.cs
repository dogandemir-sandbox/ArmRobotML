using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAgent : Agent {

	public Transform Arm;
	public Transform Hand;
	public Transform[] FingerArr;
	public Transform[] handleSensorArr;
	public Transform endPlatform;
	public Transform platform;
	public Ball ball;
	public int handleSensorCounter=0;
	float fingerRotZ = 0f;
	float handRotZ = 0f;
	float armRotZ = 0f;
	float bodyRotY = 0f;
	float prevDistSensorToBall = float.MaxValue;
	float prevDistBallToEnd = float.MaxValue;
	float prevDistSens0ToSens2 = float.MaxValue;
	RayPerceptionV2 midHandleRayPer;
	string[] detectableObjects;
	bool isAttachable = false;

	public override void InitializeAgent(){
		midHandleRayPer = handleSensorArr[1].GetComponent<RayPerceptionV2>();
		detectableObjects = new string[] { "BallEndPlatform", "Ball" };
	}

	public override void CollectObservations(){
		
		AddVectorObs( transform.rotation.eulerAngles.y / 360f);
		AddVectorObs( armRotZ / 90f );
		AddVectorObs( handRotZ / 90f );
		AddVectorObs ( fingerRotZ / 60f );
		AddVectorObs( ball.isAttached ? 1 : 0 );
		AddVectorObs ( isAttachable ? 1 : 0);
		
		AddVectorObs( (handleSensorArr[0].position - ball.transform.position) / 10f );
		AddVectorObs( (handleSensorArr[1].position - ball.transform.position) / 10f );
		AddVectorObs( (handleSensorArr[2].position - ball.transform.position) / 10f );

		AddVectorObs( (handleSensorArr[0].position - handleSensorArr[2].position) / 10f );
		// AddVectorObs( (handleSensorArr[2].position - handleSensorArr[1].position) / 10f );


		AddVectorObs( (ball.transform.position - endPlatform.position) / 10f );

        AddVectorObs((handleSensorArr[0].position - platform.position) / 10f);
		AddVectorObs((handleSensorArr[1].position - platform.position) / 10f);
		AddVectorObs((handleSensorArr[2].position - platform.position) / 10f);

		AddVectorObs((handleSensorArr[0].position - endPlatform.position) / 10f);
		AddVectorObs((handleSensorArr[1].position - endPlatform.position) / 10f);
		AddVectorObs((handleSensorArr[2].position - endPlatform.position) / 10f);

		AddVectorObs((ball.transform.position - platform.position) / 10f);

	}

	void MoveAgent( float action ){

		if ( action == 0 ){
			transform.Rotate(Vector3.up, -1f );
			
		}else if (action == 1){
			transform.Rotate(Vector3.up, 1f );
		}

		if ( action == 2 ){
			armRotZ -= 1f;
			armRotZ = Mathf.Clamp(armRotZ,-90f,90f);
			Arm.localRotation = Quaternion.Euler( new Vector3(0,0, armRotZ) );

		}else if (action == 3){
			armRotZ += 1f;
			armRotZ = Mathf.Clamp(armRotZ,-90f,90f);
			Arm.localRotation = Quaternion.Euler( new Vector3(0,0, armRotZ) );
		}

		if ( action == 4 ){
			handRotZ -= 1f;
			handRotZ = Mathf.Clamp(handRotZ,-90f,90f);
			Hand.localRotation = Quaternion.Euler( new Vector3(0,0, handRotZ) );

		}else if (action == 5){
			handRotZ += 1f;
			handRotZ = Mathf.Clamp(handRotZ,-90f,90f);
			Hand.localRotation = Quaternion.Euler( new Vector3(0,0, handRotZ) );
		}

		if ( action == 6 ){ // close fingers

			fingerRotZ -= 1f;
			fingerRotZ = Mathf.Clamp(fingerRotZ,-60f,0f);
			
			FingerArr[0].localRotation = Quaternion.Euler( new Vector3(0,0, fingerRotZ) );
			FingerArr[1].localRotation = Quaternion.Euler( new Vector3(0,0, fingerRotZ) );
			FingerArr[2].localRotation = Quaternion.Euler( new Vector3(0,180f, fingerRotZ) );
			FingerArr[3].localRotation = Quaternion.Euler( new Vector3(0,180f, fingerRotZ) );

		}else if (action == 7){

			fingerRotZ += 1f;
			fingerRotZ = Mathf.Clamp(fingerRotZ,-60f,0f);
			FingerArr[0].localRotation = Quaternion.Euler( new Vector3(0,0, fingerRotZ) );
			FingerArr[1].localRotation = Quaternion.Euler( new Vector3(0,0, fingerRotZ) );
			FingerArr[2].localRotation = Quaternion.Euler( new Vector3(0,180f, fingerRotZ) );
			FingerArr[3].localRotation = Quaternion.Euler( new Vector3(0,180f, fingerRotZ) );
		}
	}

	public override void AgentAction(float[] vectorAction, string textAction){

		AddReward(-0.05f);
		MoveAgent( vectorAction[0] );
		
		if (!ball.isAttached){
			float dist0 = Vector3.Distance( handleSensorArr[1].position , ball.transform.position );
			if ( dist0 < prevDistSensorToBall){
				AddReward(0.1f);
				prevDistSensorToBall = dist0;
			}
			
		}
		
		else {
			float dist1 = Vector3.Distance( handleSensorArr[1].position , endPlatform.position );
			if ( dist1 < prevDistBallToEnd){
				AddReward(0.1f);
				prevDistBallToEnd = dist1;
			}
			
		}
		

		float distBallToSensor0 = Vector3.Distance( ball.transform.position, handleSensorArr[0].position );
		float distBallToSensor1 = Vector3.Distance( ball.transform.position, handleSensorArr[1].position );
		float distBallToSensor2 = Vector3.Distance( ball.transform.position, handleSensorArr[2].position );
		if (distBallToSensor0 < 0.5f && distBallToSensor1 < 0.5f && distBallToSensor2 < 0.5f ){
			if (!isAttachable){
				AddReward(0.1f);
			}
			isAttachable = true;
			
		}else{
			if (isAttachable){
				AddReward(-0.1f);
			}
			isAttachable = false;
		}

		if (isAttachable && !ball.isAttached){
			float dist2 = Vector3.Distance( handleSensorArr[0].position , handleSensorArr[2].position );
			if ( dist2 < prevDistSens0ToSens2){
				AddReward(0.1f);
				prevDistSens0ToSens2 = dist2;
			}
			
		}

		
	}

	public override void AgentReset(){
		
		ball.isAttached = false;
		isAttachable = false;

		bodyRotY = 0f;
		armRotZ = 0f;
		handRotZ = 0f;
		fingerRotZ = 0f;

		transform.rotation = Quaternion.identity;
		Arm.rotation = Quaternion.identity;
		Hand.rotation = Quaternion.identity;

		FingerArr[0].rotation = Quaternion.Euler(Vector3.zero);
		FingerArr[1].rotation = Quaternion.Euler(Vector3.zero);
		FingerArr[2].rotation = Quaternion.Euler(new Vector3(0,180f,0));
		FingerArr[3].rotation = Quaternion.Euler(new Vector3(0,180f,0));

		ball.Detach();
		ball.Reset();

		prevDistBallToEnd = float.MaxValue;
		prevDistSens0ToSens2 = float.MaxValue;
		prevDistSensorToBall = float.MaxValue;

	}

	public void HandleSensorTouched(){
		handleSensorCounter++;
		CheckHandleSensors();
	}

	public void HandleSensorUntouched(){
		handleSensorCounter--;
		CheckHandleSensors();
	}

	public void CheckHandleSensors(){

		AddReward(0.05f * handleSensorCounter);

		if (ball.isAttached && handleSensorCounter<3){
			ball.Detach();
			return;
		}

		if(handleSensorCounter == 3 && !ball.isAttached){
			ball.Attach();
			SetReward( 1f );
			return;
		}
	}

	public void BallTouchedEndPlatform(){
		Done();
		SetReward( 1f );
	}

	public void BallTouchedPlatform(){
		Done();
		SetReward( -1f );
	}
}

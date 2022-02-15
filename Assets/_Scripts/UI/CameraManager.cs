using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton < CameraManager > {
	public Camera cam;
	public Transform tracker;
	public Transform target;

	public float trackSpeed;
	public float followSpeed;

	public Vector3[] trackerOffsets;
	public int offsetUsed;
	public Vector3 targetOffset;


	private Vector3 trackerSmooth;
	private Vector3 followSmooth;

	void Update ( ) {
		tracker.position = Vector3.SmoothDamp ( tracker.position, target.position + targetOffset, ref trackerSmooth, trackSpeed );
		transform.position = Vector3.SmoothDamp ( transform.position, tracker.position + trackerOffsets [ offsetUsed ], ref followSmooth, followSpeed );
		transform.LookAt ( tracker.position, Vector3.up );
	}

	public void TargetTransform ( Transform trans, int newOffset = 1 ) {
		target = trans;
		offsetUsed = newOffset;
	}

	public void RestoreDefault ( ) {
		target = Player.main.transform;
		offsetUsed = 0;
	}
}
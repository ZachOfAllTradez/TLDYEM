using UnityEngine;
using System.Collections;

public class TrackerOffset : MonoBehaviour 
{
	public Vector3 offset = Vector3.zero;
	public float lerpSpeed = 0.1f;

	private Transform originalParentTransform;
	private Transform cameraTransform;
	private Vector3 lastPosition;
	private Quaternion lastRotation;

	// Use this for initialization
	void Start () 
	{
		originalParentTransform = transform.parent;
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		// Anchor Unity object to Oculus camera
		transform.parent = cameraTransform;
		transform.localPosition = originalParentTransform.position - cameraTransform.position;
		lastPosition = transform.localPosition;

		lastRotation = transform.rotation;
	}

	void FixedUpdate () 
	{
		// Smooth position while applying offset
		transform.localPosition = Vector3.Lerp(lastPosition, (originalParentTransform.position + offset) - cameraTransform.position, lerpSpeed);
		lastPosition = transform.localPosition;

		// Smooth rotation
		transform.rotation = Quaternion.Lerp(lastRotation, originalParentTransform.rotation, lerpSpeed);
		lastRotation = transform.rotation;
	}

	public void TrackerLost()
	{

	}

	public void TrackerFound()
	{

	}



}

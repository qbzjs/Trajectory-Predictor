using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MotionDataFormat
{
	[Space(5)]
	public string tag;
	[Header("Position / Rotation")]
	[Space(5)]
	public Vector3 position;
	public Vector3 rotation;
	// public float elapsedTime;
	// public string timeStamp;

	[Header("Speed / Acceleration")]
	[Space(5)]
	public float speed;
	public Vector3 velocity;
	public Vector3 acceleration;
	public float accelerationStrength;
	public Vector3 direction;

	[Header("Angular")]
	[Space(5)]
	public float angularSpeed;
	public Vector3 angularVelocity;
	public Vector3 angularAcceleration;
	public float angularAccelerationStrength;
	public Vector3 angularAxis;

	public MotionDataFormat(
		string tag, Vector3 position, Vector3 rotation, 
		float speed, Vector3 velocity, Vector3 acceleration, float accelerationStrength, Vector3 direction,
		float angularSpeed, Vector3 angularVelocity, Vector3 angularAcceleration, float angularAccelerationStrength, Vector3 angularAxis)
	{
		this.tag = tag;
		this.position = position;
		this.rotation = rotation;

		this.speed = speed;
		this.velocity = velocity;
		this.acceleration = acceleration;
		this.accelerationStrength = accelerationStrength;
		this.direction = direction;

		this.angularSpeed = angularSpeed;
		this.angularVelocity = angularVelocity;
		this.angularAcceleration = angularAcceleration;
		this.angularAccelerationStrength = angularAccelerationStrength;
		this.angularAxis = angularAxis;
	}
}

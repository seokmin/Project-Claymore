using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	[SerializeField]
	private float _moveSpeed = 0.1f;

	[SerializeField]
	private Rigidbody2D _rigidBody = null;


	public void FixedUpdate()
	{
		if (false == isLocalPlayer)
			return;

		handlePlayerMove();
		//if (Input.GetKeyDown(KeyCode.A))
			handlePlayerRotation();
	}
	
	private void handlePlayerRotation()
	{
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var playerToMouse = mousePos - transform.position;
		var angleInRadius = Mathf.Atan2(playerToMouse.y, playerToMouse.x);
		var angleInDegree = (180 / Mathf.PI) * angleInRadius;
		_rigidBody.MoveRotation(angleInDegree);

	}

	private void handlePlayerMove()
	{
		var playerDirection = Vector2.zero;

		if (Input.GetKey(KeyCode.W))
			playerDirection += Vector2.up;
		if (Input.GetKey(KeyCode.S))
			playerDirection += Vector2.down;
		if (Input.GetKey(KeyCode.D))
			playerDirection += Vector2.right;
		if (Input.GetKey(KeyCode.A))
			playerDirection += Vector2.left;

		var moveVector = playerDirection.normalized;

		_rigidBody.MovePosition(_rigidBody.position + moveVector * _moveSpeed);
	}

	public override void OnStartLocalPlayer()
	{
		var camera = GameObject.Find("TopdownCamera");
		camera.GetComponent<CameraFollow>().Target = transform;
	}
}
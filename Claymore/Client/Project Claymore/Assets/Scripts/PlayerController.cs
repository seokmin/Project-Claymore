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

	[SerializeField]
	public Transform _bulletPoint = null;

	[SerializeField]
	public GameObject _bullet_Rifle = null;

	public void FixedUpdate()
	{
		if (false == isLocalPlayer)
			return;

		handlePlayerMove();
		handlePlayerRotation();
		handleShooting();
	}
	
	private void handleShooting()
	{
		if(Input.GetMouseButton(0))
		{
			tryShoot(WeaponType.kRifle);
		}
	}

	public enum WeaponType
	{
		kNone	= 0,
		kRifle	= 1,
	}

	private float _lastTimeShoot = 0.0f;

	private void tryShoot(WeaponType weapon)
	{
		switch(weapon)
		{
			case WeaponType.kRifle:
				{
					if (Time.time - _lastTimeShoot > 0.15f)
					{
						_lastTimeShoot = Time.time;
						CmdFire(weapon, Camera.main.ScreenToWorldPoint(Input.mousePosition));
					}
				}
				break;
		}
	}

	[Command]
	private void CmdFire(WeaponType weapon, Vector3 destination)
	{
		var bullet = (GameObject)Instantiate(
			_bullet_Rifle, _bulletPoint.position, _bulletPoint.rotation);

		var forwardVector = destination - bullet.transform.position;
		forwardVector.z = 0;
		forwardVector.Normalize();
		forwardVector *= 100;

		bullet.GetComponent<Rigidbody2D>().velocity = forwardVector;

		NetworkServer.Spawn(bullet);

		Destroy(bullet, 1.0f);
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
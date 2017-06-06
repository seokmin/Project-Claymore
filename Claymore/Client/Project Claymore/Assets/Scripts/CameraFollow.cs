using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform Target { get; set; }

	[SerializeField]
	private Vector2 _offset = Vector2.zero;

	[SerializeField]
	private Vector3 _defaultPlayerPositionInScreen = new Vector3(0.5f, 0.5f, 0.0f);

	public void LateUpdate()
	{
		if (Target == null)
			return;
		var mousePosition = Vector3.Min(Vector3.one, Camera.main.ScreenToViewportPoint(Input.mousePosition));
		var worldDistance = Camera.main.ViewportToWorldPoint(mousePosition) - Camera.main.ViewportToWorldPoint(_defaultPlayerPositionInScreen);

		worldDistance.x *= (1.0f - _offset.x);
		worldDistance.y *= (1.0f - _offset.y);

		var cameraPosition = Target.position + worldDistance;
		cameraPosition.z = -99;

		transform.localPosition = cameraPosition;
	}
}
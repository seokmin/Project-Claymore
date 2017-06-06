using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {
		[SerializeField]
        private Transform _target = null;            // The position that that camera will be following.

		[SerializeField]
		private Vector2 _offset = Vector2.zero;

		[SerializeField]
		private Vector3 _defaultPlayerPositionInScreen = new Vector3(0.5f,0.5f,0.0f);

		public void LateUpdate()
		{
			var mousePosition = Vector3.Min(Vector3.one, Camera.main.ScreenToViewportPoint(Input.mousePosition));
			var worldDistance = Camera.main.ViewportToWorldPoint(mousePosition) -  Camera.main.ViewportToWorldPoint(_defaultPlayerPositionInScreen);

			worldDistance.x *= (1.0f -_offset.x);
			worldDistance.y *= (1.0f -_offset.y);

			var cameraPosition = _target.position + worldDistance;
			cameraPosition.z = -99;

			transform.localPosition = cameraPosition;
		}
	}
}
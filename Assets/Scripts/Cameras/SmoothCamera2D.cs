using System;
using UnityEngine;
using System.Collections;

namespace Cameras {

	[RequireComponent(typeof(Camera))]
	public class SmoothCamera2D : MonoBehaviour {
		private const int Boundary = 50;
		public const int Speed = 50;

		private int _screenWidth;
		private int _screenHeight;
		
		public float dampTime = 0.15f;
		private Vector3 velocity = Vector3.zero;
		private Transform target;
		private Camera _camera;

		private void Start() {
			_camera = gameObject.GetComponent<Camera>();
			target = gameObject.GetComponent<Transform>();
			
			_screenWidth = Screen.width;
			_screenHeight = Screen.height;
			
		}

		// Update is called once per frame
		void Update() {
			HandleMouseMovement();
			if (target) {
				Vector3 point = _camera.WorldToViewportPoint(target.position);
				Vector3 delta = target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
				Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			}
		}

		private void HandleMouseMovement() {
			if (Input.mousePosition.x > _screenWidth - Boundary) {
				float translateX = Speed * Time.smoothDeltaTime;
				
				target.transform.Translate(translateX, 0, 0);
			}

			if (Input.mousePosition.x < 0 + Boundary) {
				float translateX = -Speed * Time.smoothDeltaTime;
				target.transform.Translate(translateX, 0, 0);
			}

			if (Input.mousePosition.y > _screenHeight - Boundary) {
				float translateY = Speed * Time.smoothDeltaTime;
				target.transform.Translate(0, translateY, 0);
			}

			if (Input.mousePosition.y < 0 + Boundary) {
				float translateY = -Speed * Time.smoothDeltaTime;
				target.transform.Translate(0, translateY, 0);
			}
		}
	}

}
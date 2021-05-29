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
		
		private Vector3 _target;
		private void Start() {
			_target = new Vector3();
			
			_screenWidth = Screen.width;
			_screenHeight = Screen.height;
			
		}

		// Update is called once per frame
		void Update() {
			HandleMouseMovement();
	
			Vector3 targetPos = new Vector3(_target.x, _target.y, transform.position.z);
			Vector3 velocity = (targetPos - transform.position);
			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);
			
		}

		private void HandleMouseMovement() {
			if (Input.mousePosition.x > _screenWidth - Boundary) {
				_target.x += Speed * Time.smoothDeltaTime;
			}
			else if (Input.mousePosition.x < 0 + Boundary) {
				_target.x -= Speed * Time.smoothDeltaTime;
			}

			if (Input.mousePosition.y > _screenHeight - Boundary) {
				_target.y += Speed * Time.smoothDeltaTime;
			}
			else if (Input.mousePosition.y < 0 + Boundary) {
				_target.y -= Speed * Time.smoothDeltaTime;
			}
		}
	}

}
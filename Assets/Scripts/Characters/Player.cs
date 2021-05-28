using System;
using UnityEngine;

namespace Characters {

	public class Player : Character {
		private void Update() {
			UpdateMovement();
		}

		private void UpdateMovement() {
			float horizontalInput = Input.GetAxis("Horizontal");
			float verticalInput = Input.GetAxis("Vertical");

			if (horizontalInput != 0.0f || verticalInput != 0.0f) {
				transform.Translate((Vector3.right * horizontalInput + Vector3.up * verticalInput)
				                    * (DefaultMoveSpeed * Time.deltaTime));
			}
		}
	}

}
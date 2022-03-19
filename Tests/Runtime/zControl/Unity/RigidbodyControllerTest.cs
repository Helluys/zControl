using System.Collections;

using NUnit.Framework;

using UnityEngine;
using UnityEngine.TestTools;

namespace zControl.Unity {
	public class RigidbodyControllerTest {
		private readonly int MAX_FRAMES = 100;

		private GameObject controlledInstance;
		private GameObject targetInstance;

		private UnityEngine.Rigidbody controllerRigidbody;

		[UnityTest]
		public IEnumerator PositionTest () {
			InstantiateTestSubjects(Vector3.right, Quaternion.identity);

			int frame;
			for (frame = 0; frame < MAX_FRAMES; frame++) {
				if (PositionError() < .1f) {
					UnityEngine.Debug.Log("Reached target position withing threshold after " + frame + " frames");
					break;
				}

				yield return null;
			}

			Assert.AreEqual(0f, PositionError(), .1f);
		}

		private float PositionError () {
			return (controlledInstance.transform.position - targetInstance.transform.position).magnitude;
		}

		private void InstantiateTestSubjects (Vector3 targetPosition, Quaternion targetAttitude) {
			controlledInstance = new GameObject();
			controllerRigidbody = controlledInstance.AddComponent<UnityEngine.Rigidbody>();
			controllerRigidbody.useGravity = false;
			LinearRigidbodyController controller = controlledInstance.AddComponent<LinearRigidbodyController>();
			controller.PositionGains = new Vector3(100, 10, 10);
			controller.AttitudeGains = controller.PositionGains / 10f;

			targetInstance = new GameObject();
			targetInstance.transform.SetPositionAndRotation(targetPosition, targetAttitude);
			TargetKinematicState target = targetInstance.AddComponent<TargetKinematicState>();
			
			controller.target = target;
		}
	}
}

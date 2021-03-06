using UnityEngine;

namespace Utils{
	public class VectorUtils {
		public VectorUtils () {
		}

		public static Vector3 Truncate(Vector3 vector, float max){
			if (vector.magnitude > max) {
				vector.Normalize();
				vector = vector * max;
			}

			return vector;
		}

		public static float AngleBetweenVectors(Vector3 vector, Vector3 other, float rotation = 0){
			//This angle is in a coordinate space of (-180,180] with 0 to the right and increasing to the counterclockwise
			float angle = Mathf.Atan2 (other.z - vector.z, other.x - vector.x);
			angle = angle * Mathf.Rad2Deg;
			
			//Convert to local coordinate system
			angle = 90 - angle;
			
			float localizedCurrentAngle = rotation;
			angle = angle - localizedCurrentAngle;
			angle = (angle+360) % 360;
			
			if (angle > 180) {
				angle = angle - 360;
			}
			
			return angle;
		}

		public static Vector3 FindClosestPointOnObject(Vector3 src, GameObject dest){
			Vector3 closest = dest.transform.position;
			RaycastHit hitInfo;
			if(dest.GetComponent<Renderer>() != null){
				if(Physics.Raycast (src, dest.GetComponent<Renderer>().bounds.center - src, out hitInfo)){
					closest = hitInfo.point;
				}
			}

			return closest;
		}
	}
}
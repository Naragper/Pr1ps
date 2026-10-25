using UnityEngine;
using static Utils;

public class DroneCamera : MonoBehaviour {

	[SerializeField] Transform drone;

    const float FOLLOW_DECAY  = 2.2f;
    const float CATCHUP_DECAY = 3f;
	const float MARGIN        = 0.00008f;

    Vector3 offset, lastPos, trailing;

    void Awake() {
        offset  = transform.position - drone.position;
		lastPos = drone.position;
    }

    void LateUpdate() {
		transform.position = ((lastPos - drone.position).sqrMagnitude < MARGIN)
			? ExpDecayV3(transform.position, drone.position + offset, CATCHUP_DECAY, Time.deltaTime)
			: ExpDecayV3(transform.position, drone.position + offset, FOLLOW_DECAY, Time.deltaTime);

		lastPos  = drone.position;
		trailing = transform.position - (drone.position + offset);
	}

	public void ForceMove(Vector3 newPosition) {
		transform.position = newPosition + offset + trailing;
        lastPos = newPosition;
	}
}

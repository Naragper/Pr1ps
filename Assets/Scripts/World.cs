using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class World : MonoBehaviour {

	[Header("In-Scene Components")]
	[SerializeField] Transform drone;
	[SerializeField] DroneCamera camera;

	const float TELEPORT_MARGIN = 0.001f;

	Vector3 worldMin, worldMax;
	Vector3 newPosition;

    void Awake() {
		MeshRenderer worldMesh = GetComponent<MeshRenderer>();
		worldMin = worldMesh.bounds.min;
		worldMax = worldMesh.bounds.max;
    }

    void FixedUpdate() {
        // Fake infinite world
		newPosition = drone.transform.position;

		if 		(drone.transform.position.x > worldMax.x) newPosition.x = worldMin.x + TELEPORT_MARGIN;
		else if (drone.transform.position.x < worldMin.x) newPosition.x = worldMax.x - TELEPORT_MARGIN;
		if 		(drone.transform.position.z > worldMax.z) newPosition.z = worldMin.z + TELEPORT_MARGIN;
		else if (drone.transform.position.z < worldMin.z) newPosition.z = worldMax.z - TELEPORT_MARGIN;

		if (newPosition != drone.transform.position) {
			drone.transform.position = newPosition;
			camera.ForceMove(newPosition);
    	}
	}
}

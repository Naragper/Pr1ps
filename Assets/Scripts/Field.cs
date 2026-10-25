using UnityEngine;

public class Field : MonoBehaviour {

	[Header("In-Scene Components")]
	[SerializeField] Transform plantParent;

	// TODO: Add array of GameObjects and a way to select a random plant from it
	[Header("Possible Plants")]
	[SerializeField] Plant plantPrefab;
	
	void Plant(Vector3 position) {
		Instantiate(plantPrefab, position, Quaternion.identity, plantParent);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Seed")) Plant(other.transform.position);
	}
}

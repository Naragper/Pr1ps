using UnityEngine;

public class Plant : MonoBehaviour {

	enum State {
		Init,
		Sprout,
		Mature,
		Wither,
		Explode,
	}

	[Header("Components")]
	[SerializeField] Animator animator;
	[SerializeField] MeshRenderer mesh;

	[Header("Prefab References")]
	[SerializeField] ParticleSystem wateredParticles;
	[SerializeField] ParticleSystem witheredParticles;
	[SerializeField] ParticleSystem collectedParticles;
	[SerializeField] ParticleSystem explodedParticles;
	
	[Header("Data")]
	public int value;

	const int WATER_NEEDED = 6;
	
	State currentState;
	int waterLevel;
	bool isWatered;

	public void OnSwitch(int stateId) { 
		currentState = (State)stateId;

		if (isWatered) UpdateAnimatorSpeed();
	}

	public void OnWater() {
		if (currentState == State.Init || currentState == State.Explode) return;
		
		if (!isWatered && waterLevel < WATER_NEEDED) waterLevel++;
		if (!isWatered && waterLevel >= WATER_NEEDED) {
			isWatered = true;
			wateredParticles.Play();
			UpdateAnimatorSpeed();
		}
	}

	public int OnCollect() {
		if (currentState == State.Mature) {
			Destroy(gameObject, 0.25f);
			return value;
		} else if (currentState == State.Wither) {
			Destroy(gameObject, 0.25f);
			return value / 2;
		} else return 0;
	}

	public void OnExplode() {
		explodedParticles.Play();
		mesh.enabled = false;

		Destroy(gameObject, 0.5f);
	}

	void UpdateAnimatorSpeed() {
		animator.speed = currentState switch {
        	State.Sprout => 2f,
        	State.Wither => 0.5f,
        	_ => 1f,
    	};
	}
}

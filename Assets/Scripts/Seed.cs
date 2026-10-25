using UnityEngine;

public class Seed : MonoBehaviour {

	[Header("Components")]
	[SerializeField] ParticleSystem particles;
	[SerializeField] AudioSource audio;
	[SerializeField] AudioClip impactClip;
	[SerializeField] AudioClip explodeClip;
	[SerializeField] Animator animator;

	[Header("Constants")]
	[SerializeField] float TIME_TO_DRY = 3f;

	float timeToDry;

	void Awake() {
		timeToDry = TIME_TO_DRY;
	}

	void Start() {
		audio.PlayOneShot(impactClip);
	}

	void FixedUpdate() {
		timeToDry -= Time.fixedDeltaTime;
		if (timeToDry <= 0f) Dry();
	}

	void Sow() {
		Destroy(gameObject);
	}

	void Dry() {
		// TODO: Implement both
		// audio.PlayOneShot(explodeClip);
		// animator.Play("Dry");
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Field")) Sow();
	}

}

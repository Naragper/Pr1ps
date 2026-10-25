using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Utils;

public class Drone : MonoBehaviour {

	[Header("In-Scene Components")]
	[SerializeField] Transform seedParent;

	[Header("Components")]
	[SerializeField] Transform body;
	[SerializeField] ParticleSystem waterParticles;
	[SerializeField] ParticleSystem collectParticles;
	[SerializeField] GameObject seedPrefab;
	[SerializeField] AudioSource wateringAudio;
	[SerializeField] AudioSource collectingAudio;

	[Header("Input Actions")]
	[SerializeField] InputActionReference moveRef;
	[SerializeField] InputActionReference dropRef;
	[SerializeField] InputActionReference waterRef;
	[SerializeField] InputActionReference collectRef;
	
	[Header("Constants")]
	[SerializeField] float MOVE_SPEED        = 5.5f;
	[SerializeField] float MOVE_DECAY        = 6f;
	[SerializeField] float STOP_DECAY        = 3.5f;
	[SerializeField] float STOP_EPSILON      = 0.01f;
	[SerializeField] float TILT_MAGNITUDE    = 5f;
	[SerializeField] float BOBBING_SPEED     = 1.5f;
	[SerializeField] float BOBBING_AMPLITUDE = 0.8f;
	[SerializeField] float DROP_IMPULSE      = 6f;
	[SerializeField] float DROP_DISTANCE     = 0.5f;
	[SerializeField] float DROP_COOLDOWN     = 0.333f;
	[SerializeField] float COLLECT_COOLDOWN  = 1.5f;
	[SerializeField] float WATER_COOLDOWN    = 0.25f;

	[HideInInspector] public Vector3 defaultPos;

	const float INTERACTION_RADIUS = 0.55f;
	Vector3     INTERACTION_POS    = new(0, -2.4f, 0);

	Vector2 moveValue, velocity, tilt;
	Vector3 bob;
	float bobTime; // Uncapped
	float dropCooldown, waterCooldown, collectCooldown;

	readonly Collider[] waterCollided   = new Collider[16];
	readonly Collider[] collectCollided = new Collider[16];

	void Start() => defaultPos = transform.position;

	void Update() {
		// Move
		moveValue = moveRef.action.ReadValue<Vector2>();

		velocity = (moveValue == Vector2.zero)
		    ? ExpDecayV2Eps(velocity, Vector2.zero, STOP_DECAY, Time.deltaTime, STOP_EPSILON)
		    : ExpDecayV2(velocity, moveValue * MOVE_SPEED, MOVE_DECAY, Time.deltaTime);

		transform.position += new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime;

		tilt = velocity * TILT_MAGNITUDE;
		body.rotation = Quaternion.Euler(new(tilt.y, 0, -tilt.x));

		bob = body.transform.localPosition;
		bob.y = MathF.Sin(bobTime += BOBBING_SPEED * Time.deltaTime) * BOBBING_AMPLITUDE; 
		body.transform.localPosition = bob;

		// Drop
		if (dropCooldown > 0f) dropCooldown -= Time.deltaTime;
		if (dropCooldown <= 0f && dropRef.action.IsPressed()) {
			dropCooldown = DROP_COOLDOWN;
			Drop();
		}

		// Water
		if (waterCooldown > 0f) waterCooldown -= Time.deltaTime;
		if (waterCooldown <= 0f && waterRef.action.IsPressed()) {
			waterCooldown = WATER_COOLDOWN;
			Water();
		}

		// Collect
		if (collectCooldown > 0f) collectCooldown -= Time.deltaTime;
		if (collectCooldown <= 0f && collectRef.action.IsPressed()) {
			collectCooldown = COLLECT_COOLDOWN;
			Collect();
		}
	}

	void Drop() {
		Instantiate(seedPrefab, body.position + (Vector3.down * DROP_DISTANCE), Quaternion.identity, seedParent)
		/* AND */.GetComponent<Rigidbody>().linearVelocity = Vector3.down * DROP_IMPULSE;
	}

	void Water() {
		waterParticles.Play();
		wateringAudio.Play();

		int count = Physics.OverlapSphereNonAlloc(transform.position + INTERACTION_POS, INTERACTION_RADIUS, waterCollided);

		for (int i = 0; i < count; i++) {
        	if (waterCollided[i].TryGetComponent<Plant>(out var plant)) {
				plant.OnWater();
			}
   		}
	}

	void Collect() {
		collectParticles.Play();
		collectingAudio.Play();
		
		int count = Physics.OverlapSphereNonAlloc(transform.position + INTERACTION_POS, INTERACTION_RADIUS, collectCollided);
		int score = 0;

		for (int i = 0; i < count; i++) {
        	if (collectCollided[i].TryGetComponent<Plant>(out var plant)) {
				score += plant.OnCollect();
			}
   		}

		if (score > 0) {
			GameManager.Instance.AddPoints(score);
			Debug.Log($"Harvest earned {score} points");
		}
	}
}

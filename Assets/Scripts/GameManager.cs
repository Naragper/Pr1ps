using TMPro;
using UnityEngine;
using static Utils;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

	[Header("In-Scene Components")]
	[SerializeField] Transform instantationsParent;
	[SerializeField] Drone drone;
	[SerializeField] DroneCamera droneCamera;
	[SerializeField] TextMeshProUGUI scoreMesh;
	[SerializeField] TextMeshProUGUI timerMesh;

	[Header("Constants")]
	[SerializeField] string GAME_TIMER; // String format: "m:s:ms" or "s:ms"

	const string BESTSCORE_KEY = "BestScore";

    public int Score     {get; private set;}
    public int BestScore {get; private set;}
	
	int timer = 10;  
	bool hasGameEnded;

	void Awake() => Instance = this;

	void Start() {
		Score = 0;
		BestScore = PlayerPrefs.GetInt(BESTSCORE_KEY, 0);
		timer = GetIntFromFormattedTime(GAME_TIMER);
		hasGameEnded = false;
	}

	void Update() {
		if (!hasGameEnded && timer > 0) {
			timer -= (int)(Time.deltaTime * 1000);
			timerMesh.SetText($"Time: {GetFormattedTimeFromInt(timer)}");
		}

		if (!hasGameEnded && timer <= 0) {
			hasGameEnded = true;
			SaveBestScore();
			Main.Instance.Switch(Main.State.End);
		}
	}

	void SaveBestScore() {
		if (Score > BestScore) {
			PlayerPrefs.SetInt(BESTSCORE_KEY, Score);
        	PlayerPrefs.Save();
		}
    }

    public void AddPoints(int points) { 
		Score += points;
		scoreMesh.SetText("Score: ", Score);
	}
}

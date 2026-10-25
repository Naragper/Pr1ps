using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour {
	
	[Header("Components")]
	[SerializeField] TextMeshProUGUI scoreMesh;
	[SerializeField] TextMeshProUGUI bestScoreMesh;

	void Start() {
		scoreMesh.SetText("Score: {0}", Main.Instance.Score);
		bestScoreMesh.SetText("Best score: {0}", Main.Instance.BestScore);
	}

	public void OnPlayPressed() {
		Main.Instance.Switch(Main.State.Game);
	}

	public void OnQuitPressed() {
		Application.Quit();
	}

}

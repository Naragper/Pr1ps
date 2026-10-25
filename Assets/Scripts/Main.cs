using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	public static Main Instance { get; private set; }

	public enum State {
		Menu,
		Game,
		End,
	}
	
	Scene currentScene;
	Coroutine loadingSceneCoroutine;

	public int Score     {get; private set;}
    public int BestScore {get; private set;}

	void Awake() {
		Instance = this;

		Application.targetFrameRate = 120;
	}
	void Start() => Switch(State.Menu);

	public void Switch(State nextState) {
		if (loadingSceneCoroutine != null) return;

		if (nextState == State.End) {
			Score     = GameManager.Instance.Score;
			BestScore = GameManager.Instance.BestScore;
		}

		string sceneName = nextState switch {
			State.Menu   => "MenuScene",	
			State.Game   => "GameScene",	
			State.End    => "EndScreenScene",
			_            => null
		};

		if (sceneName != null) loadingSceneCoroutine = StartCoroutine(SwitchRoutine(sceneName));
	}

	IEnumerator SwitchRoutine(string nextSceneName) {
		Scene previous = currentScene;

		AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
		
		while (!op.isDone) yield return null;

		currentScene = SceneManager.GetSceneByName(nextSceneName);

		if (previous.IsValid()) yield return SceneManager.UnloadSceneAsync(previous);

		loadingSceneCoroutine = null;

		SceneManager.SetActiveScene(currentScene);
	}
}

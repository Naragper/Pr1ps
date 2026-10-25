using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	[SerializeField] Button firstButton;

	void Start() {
		firstButton.Select();
	}
	
	public void OnPlayPressed() {
		Main.Instance.Switch(Main.State.Game);
	}

	public void OnQuitPressed() {
		Application.Quit();
	}
}

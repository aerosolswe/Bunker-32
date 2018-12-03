using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public Button newGameButton;
	public Button continueButton;

	public CanvasGroup canvasGroup;

	public void Start() {
		if(GameManager.instance.CurrentLevel == 0) {
			continueButton.interactable = false;
		} else {
			continueButton.interactable = true;
		}
	}

	public void ClickedContinue() {
		canvasGroup.interactable = false;
		AudioManager.instance.PlayClickSource();
		Loader.LoadLevel("Main");
	}

	public void ClickedNewGame() {
		canvasGroup.interactable = false;

		AudioManager.instance.PlayClickSource();
		GameManager.instance.CurrentLevel = 0;

		Loader.LoadLevel("Main");
	}
}

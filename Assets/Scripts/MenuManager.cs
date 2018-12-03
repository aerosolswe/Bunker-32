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
		}
	}

	public void ClickedContinue() {
		canvasGroup.interactable = false;

		Loader.LoadLevel("Main");
	}

	public void ClickedNewGame() {
		canvasGroup.interactable = false;

		GameManager.instance.CurrentLevel = 0;

		Loader.LoadLevel("Main");
	}
}

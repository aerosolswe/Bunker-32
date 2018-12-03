using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class EndScene : MonoBehaviour {

	public TextMeshProUGUI screenText;
	public TextMeshProUGUI inputText;

	public GameObject abortEnd;
	public GameObject firedEnd;

	public bool fired = false;
	public bool aborted = false;

	private string baseText;

	private float time = 60;
	private float percent = 0;
	private string passengerStatus = "OK";

	private bool ended = false;

	void Start() {
		baseText = screenText.text;
	}

	void Update() {
		if(ended) return;

		time -= Time.deltaTime;
		time = Mathf.Clamp(time, 0, 60);
		SetScreenText();

		if(time <= 0 && !aborted && !ended) {
			aborted = true;
			StartCoroutine(end(abortEnd));
		}

		if(fired || aborted && !ended) {
			percent += 0.5f * Time.deltaTime;
			percent = Mathf.Clamp(percent, 0, 1);
			float cpercent = percent * 100;
			inputText.text = "progress " + (int)cpercent;

			if(percent >= 1) {
				if(passengerStatus == "OK" && !aborted) {
					passengerStatus = "CRITICAL  CRITICAL  CRITICAL";
					StartCoroutine(end(firedEnd));
				} else {
					StartCoroutine(end(abortEnd));
				}
			}
			return;
		}
		
		foreach (char c in Input.inputString) {
			if (c == '\b') { // backspace/delete
                if (inputText.text.Length != 0) {
                    inputText.text = inputText.text.Substring(0, inputText.text.Length - 1);
                }
            } else if ((c == '\n') || (c == '\r'))  { // Enter
                ProcessCmd(inputText.text);
            }

			if(!IsAllLetters(c + "")) {
				return;
			} else {
                inputText.text += c;
            }
        }
	}

	IEnumerator end(GameObject endObject) {
		if(ended) yield break;

		ended = true;
		SetScreenText();

		if(fired) {
			yield return new WaitForSeconds(3f);
		} else {
			yield return new WaitForSeconds(0.1f);
		}

		endObject.SetActive(true);

		yield return new WaitForSeconds(1);

		bool clicked = false;
		while(!clicked) {
			if(Input.GetMouseButtonDown(0))
				clicked = true;
			
			yield return null;
		}
		AudioManager.instance.PlayClickSource();

		Loader.LoadLevel("Menu");
	}

	public void SetScreenText() {
		string bt = baseText;

		bt = bt.Replace("60", (int)time+"");
		bt = bt.Replace("OK", passengerStatus);

		screenText.text = bt;
	}

	public void ProcessCmd(string cmd) {
		string inCMD = cmd.ToLower();

		if(inCMD == "abort") {
			aborted = true;
		} else if(inCMD == "fire") {
			fired = true;
		}
	}

	public static bool IsAllLetters(string s) {
		foreach (char c in s) {
			if (!Char.IsLetter(c))
				return false;
		}
		return true;
	}

}

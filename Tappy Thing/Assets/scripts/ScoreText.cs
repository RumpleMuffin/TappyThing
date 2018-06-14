using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

	Text score;

	// Use this for initialization
	void Start () {
		score = GetComponent<Text>();
		score.text = "Score: " + GameMnager.Instance.Score;
	}
	
	
}

using System;
using TMPro;
using UnityEngine;

namespace UI
{
	public class UIScore : MonoBehaviour
	{
		private UnitySnakeGame _snakeGame;
		private TMP_Text _tmpText;
    
		private void OnEnable()
		{
			_tmpText = GetComponent<TMP_Text>();
			_snakeGame = FindObjectOfType<UnitySnakeGame>() ?? throw new NullReferenceException();
			_snakeGame.OnScoreChanged += UpdateScore;
		}
		private void OnDisable()
		{
			_snakeGame.OnScoreChanged -= UpdateScore;
		}

		private void UpdateScore(int score)
		{
			_tmpText.text = $"Score: {score}";
		}
	}
}

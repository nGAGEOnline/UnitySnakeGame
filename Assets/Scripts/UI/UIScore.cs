using System;
using TMPro;
using UnityEngine;

namespace UI
{
	public class UIScore : MonoBehaviour
	{
		[SerializeField] private UnitySnakeGame _snakeGame;
		[SerializeField] private TMP_Text _tmpText;
    
		private void Start()
		{
			if (_snakeGame != null)
				_snakeGame.OnScoreChanged += UpdateScore;
		}
		private void OnDisable()
		{
			if (_snakeGame != null)
				_snakeGame.OnScoreChanged -= UpdateScore;
		}

		private void UpdateScore(int score)
		{
			_tmpText.text = $"Score: {score}";
		}
	}
}

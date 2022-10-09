using System;
using SnakeLib.Interfaces.UI;
using SnakeLib.Structs;
using TMPro;
using UnityEngine;

namespace UI
{
	public class UIScore : MonoBehaviour, ITextField<Color>
	{
		public Coord Coord { get; }
		public string Text { get; }
		public string FullText { get; }
		public ITextStyle<Color> TextStyle { get; }

		[SerializeField] private string _prefix = "Score:";
		
		private UnitySnakeGame _snakeGame;
		private TMP_Text _tmpText;

		private void Start() 
			=> UpdateScore(0);

		private void OnEnable()
		{
			_tmpText = GetComponent<TMP_Text>();
			_snakeGame = FindObjectOfType<UnitySnakeGame>() ?? throw new NullReferenceException();
			if (_snakeGame != null)
				_snakeGame.OnScoreChanged += UpdateScore;
		}
		private void OnDisable()
		{
			if (_snakeGame != null)
				_snakeGame.OnScoreChanged -= UpdateScore;
		}

		public void UpdateText(string text) => _tmpText.text = $"{_prefix} {text}";
		private void UpdateScore(int score) => UpdateText($"{score}");
	}
}

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using SnakeLib;
using SnakeLib.Interfaces;
using UnityEngine;

public class UnitySnakeGame : MonoBehaviour, ISnakeGame
{
	public SnakeSettings Settings => _settings;
	
	public SnakeGame Game { get; private set; }
	public ISnakeGameInput Input { get; private set; }
	public ISnakeGameRenderer Renderer { get; private set; }
	
	public event Action<int> OnScoreChanged;

	[SerializeField] private SnakeSettings _settings;

	private Coroutine _playRoutine;
	private float _timeElapsed;

	private void Awake()
	{
		Input = GetComponent<ISnakeGameInput>();
		Renderer = GetComponent<ISnakeGameRenderer>();
		Game = new SnakeGame(_settings, Renderer, Input);
	}

	private void OnEnable() 
		=> Game.OnScoreChanged += ScoreChanged;
	private void OnDisable() 
		=> Game.OnScoreChanged -= ScoreChanged;

	private void ScoreChanged(int score) 
		=> OnScoreChanged?.Invoke(score);

	private void Start() 
		=> SetupGame();
	
	public void SetupGame()
	{
		Game.DrawBorder();
		Game.DrawGrid();
		Game.SpawnFruit();
		// Play(_settings.GetDelayByDifficulty());
	}

	// If we wanna use the Update-loop
	private void Update()
	{
		if (Game.GameOver || _playRoutine != null)
			return;
		
		_timeElapsed += Time.deltaTime;
		if (_timeElapsed < _settings.GetDelayByDifficulty() * 0.001f)
			return;
		
		Input.Listen();
		Game.Update();
		_timeElapsed = 0;
	}

	// If we wanna do it old-school (don't use in Unity)
	public void Play(int refreshDelayMS) 
		=> _playRoutine ??= StartCoroutine(PlayRoutine(_settings.GetDelayByDifficulty()));

	// If we wanna run Async
	public async Task PlayAsync(int refreshDelay)
	{
		while (!Game.GameOver)
		{
			await Task.Run(() => Input.Listen());
			await Task.Run(() => Game.Update());
			await Task.Delay(refreshDelay);
		}
	}

	// If we wanna use a Coroutine
	private IEnumerator PlayRoutine(int delay)
	{
		var wait = new WaitForSeconds(delay * 0.001f);
		while (!Game.GameOver)
		{
			Input.Listen();
			Game.Update();
			yield return wait;
		}
	}

	public void Reset() 
		=> Game = new SnakeGame(_settings, Renderer, Input);
}

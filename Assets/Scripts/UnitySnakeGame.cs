using System;
using System.Collections;
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

	private Coroutine GameLoopRoutine;
	private float _timeElapsed;

	private void Awake()
	{
		Input = GetComponent<ISnakeGameInput>();
		Renderer = GetComponent<ISnakeGameRenderer>();
		Game = new SnakeGame(_settings, Renderer, Input);
	}

	public void Start()
	{
		Game.DrawBorder();
		Game.DrawGrid();
		Game.SpawnFruit();
		_timeElapsed = _settings.GetDelayByDifficulty();
	}

	private void Update()
	{
		if (Game.GameOver)
			return;
		
		_timeElapsed -= Time.deltaTime * 1000f;
		if (_timeElapsed >= 0)
			return;
		
		Input.Listen();
		Game.Update();
		_timeElapsed += _settings.GetDelayByDifficulty();
	}

	public void Play(int refreshDelayMS)
	{
		GameLoopRoutine = StartCoroutine(StartGameLoopRoutine(refreshDelayMS));
	}

	private IEnumerator StartGameLoopRoutine(int delay)
	{
		var wait = new WaitForSeconds(delay * 0.001f);
		while (!Game.GameOver)
		{
			Input.Listen();
			Game.Update();
			yield return wait;
		}
	}

	public async Task PlayAsync(int refreshDelay)
	{
		while (!Game.GameOver)
		{
			await Task.Run(() => Input.Listen());
			await Task.Run(() => Game.Update());
			await Task.Delay(refreshDelay);
		}
	}

	public void Reset() 
		=> Game = new SnakeGame(_settings, Renderer, Input);
}

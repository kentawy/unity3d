using System;

using UnityEngine;

using UnityEngine.SceneManagement;

public sealed class GameSession : MonoBehaviour

{

[SerializeField] private GameRules rules;

public event Action Changed;

public int Collected { get; private set; }

public int Goal => rules == null ? 0 : rules.PickupsToWin;

public bool Won => rules != null && Collected >= Goal;

private bool restarting;

private void Awake()

{

Collected = 0;

if (rules == null)

Debug.LogError("Assign GameRules to GameSession.", this);

}

public bool Collect()

{

if (rules == null || Won || restarting) return false;

Collected++;

Changed?.Invoke();

return true;

}

public void Restart()

{

if (restarting) return;

int index = SceneManager.GetActiveScene().buildIndex;

if (index < 0)

{

Debug.LogError("Add this scene to Build Profiles Scene List.", this);

return;

}

restarting = true;

Time.timeScale = 1f;

SceneManager.LoadSceneAsync(index);

}

}


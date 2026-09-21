using TMPro;

using UnityEngine;

public sealed class SessionHud : MonoBehaviour

{

[SerializeField] private GameSession session;

[SerializeField] private TMP_Text label;

private void OnEnable()

{

if (session == null || label == null)

{

Debug.LogError("Assign Session and Label to SessionHud.", this);

return;

}

session.Changed += Refresh;

Refresh();

}

private void Start() => Refresh();

private void OnDisable()

{

if (session != null) session.Changed -= Refresh;

}

private void Refresh()

{

if (session == null || label == null) return;

label.text = session.Won ? "Victory! Press Restart."

: $"Collected: {session.Collected} / {session.Goal}";

}

}


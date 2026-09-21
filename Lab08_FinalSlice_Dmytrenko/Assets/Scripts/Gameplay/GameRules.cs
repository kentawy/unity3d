using UnityEngine;

[CreateAssetMenu(menuName = "Course/Game Rules")]

public sealed class GameRules : ScriptableObject

{

[SerializeField, Min(1)] private int pickupsToWin = 3;

public int PickupsToWin => Mathf.Max(1, pickupsToWin);

}



using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "Scriptable Objects/Difficulty")]
public class Difficulty : ScriptableObject
{
    public int DifficultySetting = 0;

    public void EasySelect()
    {
        DifficultySetting = 0;
    }

    public void NormalSelect()
    {
        DifficultySetting = 1;
    }

    public void HardSelect()
    {
        DifficultySetting = 2;
    }
}

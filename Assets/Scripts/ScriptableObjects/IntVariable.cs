using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "MyAssets/VariableObjects/IntVariable")]
public class IntVariable : ScriptableObject {
    public event Action<IntVariable> OnChange;

    [SerializeField] private int StoredValue = 0;

    public void ChangeValueBy(int Value) => SetNewValue(StoredValue + Value);
    public void ChangeValueTo(int Value) => SetNewValue(Value);
    public int GetValue() => StoredValue;

    private void SetNewValue(int NewValue) {
        if (StoredValue == NewValue) return;

        StoredValue = NewValue;
        OnChange?.Invoke(this);
    }


#if UNITY_EDITOR
    [SerializeField][HideInInspector] private int CashedValue;

    void OnEnable() {
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable() {
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state) {
        switch (state) {
            case UnityEditor.PlayModeStateChange.ExitingEditMode: CashedValue = StoredValue; break;
            case UnityEditor.PlayModeStateChange.EnteredEditMode: StoredValue = CashedValue; break;
        }
    }

    private void OnValidate() {
        if (Application.isPlaying) OnChange?.Invoke(this);
    }
#endif
}

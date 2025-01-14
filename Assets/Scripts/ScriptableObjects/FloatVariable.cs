using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "MyAssets/VariableObjects/FloatVariable")]
public class FloatVariable : ScriptableObject {
    public event Action<FloatVariable> OnChange;

    [SerializeField] private float StoredValue = 0;

    public void ChangeValueBy(float Value) => SetNewValue(StoredValue + Value);
    public void ChangeValueTo(float Value) => SetNewValue(Value);
    public float GetValue() => StoredValue;

    private void SetNewValue(float NewValue) {
        if (StoredValue == NewValue) return;

        StoredValue = NewValue;
        OnChange?.Invoke(this);
    }


#if UNITY_EDITOR
    [SerializeField][HideInInspector] private float CashedValue;

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

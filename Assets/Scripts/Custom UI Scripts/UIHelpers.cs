using UnityEngine.UIElements;

namespace Custom_UI_Scripts
{
    public static class UIHelpers
    {
        public static void DelayAddToClassList(VisualElement ui, string classToAdd = "Animation", int delay = 100)
        {
            ui.schedule.Execute(() => ui.AddToClassList(classToAdd)).StartingIn(delay);
        }
    }
}

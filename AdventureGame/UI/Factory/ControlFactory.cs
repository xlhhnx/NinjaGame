using System;
using NinjaGame.UI.Controls;

namespace NinjaGame.UI.Factory
{
    public static class ControlFactory
    {
        public static Button CreateButton(string id, Action action)
        {
            var button = MainGame.Instance.UIManager.GetButton(id);
            button.Action = action;

            return button;
        }
    }
}

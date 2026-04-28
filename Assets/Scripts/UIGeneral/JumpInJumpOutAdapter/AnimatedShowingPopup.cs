using TotalCreations.UI;
using UnityEngine;

namespace IdxZero.UIGeneral.JumpInJumpOutAdapter
{
    public class AnimatedShowingPopup : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private Canvas _screenCanvas;

        [SerializeField]
        private GameObject _contentPanel;

#pragma warning restore 0649

        protected Canvas ScreenCanvas => _screenCanvas;
        protected JumpInJumpOut JumpInJumpOut;
        private static JumpInJumpOutData _noPaidPanelJumpInJumpOutData;

        private void Awake()
        {
            CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
            JumpInJumpOut = _contentPanel.AddComponent<JumpInJumpOut>();
            if (_noPaidPanelJumpInJumpOutData == null)
                _noPaidPanelJumpInJumpOutData = JumpInJumpOutDataSetter.GetPanelJumpInJumpOutData();
            JumpInJumpOutDataSetter.SetJumpInJumpOutData(_noPaidPanelJumpInJumpOutData, JumpInJumpOut);
            JumpInJumpOut.InitWithCanvasGroup(canvasGroup);
        }
    }
}

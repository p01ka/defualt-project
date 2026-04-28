using System;
using IdxZero.Base.MVP;
using IdxZero.UIGeneral.JumpInJumpOutAdapter;
using TotalCreations.UI;
using UnityEngine;
using UnityEngine.UI;

namespace IdxZero.Base.MVP
{
    public abstract class AnimatedShowingBaseView : BaseView
    {
#pragma warning disable 0649

        [SerializeField] protected Canvas _screenCanvas;
        [SerializeField] protected GameObject _panel;

#pragma warning restore 0649

        private JumpInJumpOut _jumpInJumpOut;
        protected CanvasGroup _canvasGroup;
        protected GraphicRaycaster _graphicRaycaster;

        private bool _isInitialized;

        private static JumpInJumpOutData _panelJumpInJumpOutData;

        protected void Show(bool withAnimation = false,
                            Action callback = null)
        {
            _screenCanvas.enabled = true;
            if (_graphicRaycaster != null)
                _graphicRaycaster.enabled = true;
            if (!withAnimation)
            {
                if (_canvasGroup != null)
                {
                    _canvasGroup.alpha = 1;
                }
                _panel.transform.localScale = Vector3.one;
                if (_isInitialized) _jumpInJumpOut.SetStateImmediately(JumpInJumpOut.State.Visible);
                callback?.Invoke();
                return;
            }

            InitializeJumpInJumpOut();
            _jumpInJumpOut.JumpIn(() =>
                    { callback?.Invoke(); });
        }

        protected void Hide(bool withAnimation = false,
                            Action callback = null)
        {
            if (withAnimation)
            {
                InitializeJumpInJumpOut();
                _jumpInJumpOut.JumpOut(() =>
                {
                    _screenCanvas.enabled = false;
                    if (_graphicRaycaster != null)
                        _graphicRaycaster.enabled = false;
                    callback?.Invoke();
                });
            }
            else
            {
                if (_isInitialized) _jumpInJumpOut.SetStateImmediately(JumpInJumpOut.State.Invisible);
                _screenCanvas.enabled = false;
                if (_graphicRaycaster != null)
                    _graphicRaycaster.enabled = false;
                if (_canvasGroup != null) _canvasGroup.alpha = 0;
            }
        }

        private void InitializeJumpInJumpOut()
        {
            if (_isInitialized) return;

            // _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _canvasGroup = TryInitComponent<CanvasGroup>(gameObject);
            
            _jumpInJumpOut = _panel.AddComponent<JumpInJumpOut>();
            _panelJumpInJumpOutData ??= JumpInJumpOutDataSetter.GetPanelJumpInJumpOutData();
            JumpInJumpOutDataSetter.SetJumpInJumpOutData(_panelJumpInJumpOutData, _jumpInJumpOut);
            _jumpInJumpOut.InitWithCanvasGroup(_canvasGroup);

            _isInitialized = true;
        }

        private T TryInitComponent<T>(GameObject go) where T : Component
        {
            if (go.TryGetComponent(out T component))
                return component;
            
            return go.AddComponent<T>();
        }
        

        private void Awake()
        {
            _graphicRaycaster = _screenCanvas.gameObject.GetComponent<GraphicRaycaster>();
            InitializeJumpInJumpOut();
        }

        private void OnEnable()
        {
            SubscribeOnEvents();
            Presenter?.Initialize();
        }

        private void OnDisable()
        {
            Presenter?.Uninitialize();
            UnsubscribeOnEvents();
        }

        private void OnDestroy()
        {
            Presenter?.Dispose();
        }
    }
}
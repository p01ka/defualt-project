using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace IdxZero.UIGeneral.Switcher
{
    public class CustomToggle : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Transform _offKnobRoot;
        [SerializeField] private Transform _onKnobRoot;
        [SerializeField] private Transform _knob;
        [SerializeField] private VariableSpriteImage[] _variableColorImages;

        private const float SwitchDuration = .1f;

        private int _switchUITweenId;

        public bool IsOn { get; private set; }

        public event Action<bool> OnValueChanged;

        private void OnButtonClicked()
        {
            IsOn = !IsOn;
            SwitchUI(false);
            OnValueChanged?.Invoke(IsOn);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
            if (DOTween.IsTweening(_switchUITweenId)) DOTween.Kill(_switchUITweenId);
        }

        public void SetMode(bool isOn)
        {
            IsOn = isOn;
            SwitchUI(true);
        }

        private void SwitchUI(bool immediately)
        {
            void LerpParameters(float value)
            {
                _knob.localPosition = Vector3.Lerp(_offKnobRoot.localPosition, _onKnobRoot.localPosition, value);
            }

            if (immediately)
            {
                LerpParameters(IsOn ? 1 : 0);
                SetStatusSprites();
            }
            else
            {
                var startValue = 0;
                var endValue = 1;

                if (!IsOn)
                    (startValue, endValue) = (endValue, startValue);

                _switchUITweenId = DOVirtual
                    .Float(startValue, endValue, SwitchDuration, LerpParameters)
                    .OnComplete(SetStatusSprites)
                    .SetUpdate(true)
                    .intId;
            }
        }

        private void SetStatusSprites()
        {
            foreach (var elem in _variableColorImages)
            {
                elem.Image.sprite = IsOn ? elem.OnSprite : elem.OffSprite;
            }
        }
    }

    [Serializable]
    public class VariableSpriteImage
    {
        public Image Image;
        public Sprite OffSprite;
        public Sprite OnSprite;
    }
}
using DG.Tweening;
using TotalCreations.UI;
using UnityEngine;

namespace IdxZero.UIGeneral.JumpInJumpOutAdapter
{
    public class JumpInJumpOutData
    {
        public bool controlUIRaycast = false;
        public bool startWithoutAnimation = false;
        public bool hideOnStart = false;
        public bool activateOnEnabled = false;
        public bool hideBeforeShow = false;
        public float displayTime = 0f;
        public float showDuration = 0.5f;
        public float hideDuration = 0.5f;
        public bool blendActive = true;
        public Ease showBlendEase = Ease.InOutQuad;
        public Ease hideBlendEase = Ease.InOutQuad;
        public bool scaleActive = true;
        public Ease showScaleEase = Ease.InOutQuad;
        public Ease hideScaleEase = Ease.InOutQuad;
        public bool shakeActive = true;
        public float showShakeStrength = 0.1f;
        public int showShakeVibrato = 10;
        public float showShakeRandomness = 90;
        public Ease showShakeEase = Ease.InOutQuad;
        public Ease hideShakeEase = Ease.InOutQuad;
        public float hideShakeStrength = 0.1f;
        public int hideShakeVibrato = 10;
        public float hideShakeRandomness = 90;
        public bool rotationActive = true;
        public Vector3 rotationAxis = Vector3.forward;
        public Ease showRotationEase = Ease.InOutQuad;
        public Ease hideRotationEase = Ease.InOutQuad;

        public MinMaxHelper scaleMinMax;// = new MinMaxHelper(0f, 1f);
        public MinMaxHelper showStartMinMaxRotation;// = new MinMaxHelper(-15, 15);
        public MinMaxHelper showMinMaxRotation;// = new MinMaxHelper(-15, 15);
        public MinMaxHelper hideMinMaxRotation;// = new MinMaxHelper(-15, 15);
    }
}

using DG.Tweening;
using TotalCreations.UI;
using UnityEngine;

namespace IdxZero.UIGeneral.JumpInJumpOutAdapter
{
    public static class JumpInJumpOutDataSetter
    {
        public static void SetJumpInJumpOutData(JumpInJumpOutData data, JumpInJumpOut jumpInJumpOut)
        {
            jumpInJumpOut.controlUIRaycast = data.controlUIRaycast;
            jumpInJumpOut.startWithoutAnimation = data.startWithoutAnimation;
            jumpInJumpOut.hideOnStart = data.hideOnStart;
            jumpInJumpOut.activateOnEnabled = data.activateOnEnabled;
            jumpInJumpOut.hideBeforeShow = data.hideBeforeShow;
            jumpInJumpOut.displayTime = data.displayTime;
            jumpInJumpOut.showDuration = data.showDuration;
            jumpInJumpOut.hideDuration = data.hideDuration;
            jumpInJumpOut.blendActive = data.blendActive;
            jumpInJumpOut.showBlendEase = data.showBlendEase;
            jumpInJumpOut.hideBlendEase = data.hideBlendEase;
            jumpInJumpOut.scaleActive = data.scaleActive;
            jumpInJumpOut.showScaleEase = data.showScaleEase;
            jumpInJumpOut.hideScaleEase = data.hideScaleEase;
            jumpInJumpOut.shakeActive = data.shakeActive;
            jumpInJumpOut.showShakeStrength = data.showShakeStrength;
            jumpInJumpOut.showShakeVibrato = data.showShakeVibrato;
            jumpInJumpOut.showShakeRandomness = data.showShakeRandomness;
            jumpInJumpOut.showShakeEase = data.showShakeEase;
            jumpInJumpOut.hideShakeEase = data.hideShakeEase;
            jumpInJumpOut.hideShakeStrength = data.hideShakeStrength;
            jumpInJumpOut.hideShakeVibrato = data.hideShakeVibrato;
            jumpInJumpOut.hideShakeRandomness = data.hideShakeRandomness;
            jumpInJumpOut.rotationActive = data.rotationActive;
            jumpInJumpOut.rotationAxis = data.rotationAxis;
            jumpInJumpOut.showRotationEase = data.showRotationEase;
            jumpInJumpOut.hideRotationEase = data.hideRotationEase;

            jumpInJumpOut.scaleMinMax = data.scaleMinMax;
            jumpInJumpOut.showStartMinMaxRotation = data.showStartMinMaxRotation;
            jumpInJumpOut.showMinMaxRotation = data.showMinMaxRotation;
            jumpInJumpOut.hideMinMaxRotation = data.hideMinMaxRotation;
        }

        public static JumpInJumpOutData GetPanelJumpInJumpOutData()
        {
            JumpInJumpOutData jumpInJumpOutData = new JumpInJumpOutData();
            jumpInJumpOutData.controlUIRaycast = false;
            jumpInJumpOutData.startWithoutAnimation = true;
            jumpInJumpOutData.hideOnStart = true;
            jumpInJumpOutData.activateOnEnabled = false;
            jumpInJumpOutData.hideBeforeShow = false;
            jumpInJumpOutData.displayTime = 0;
            jumpInJumpOutData.showDuration = Utils.Consts.TRANSITION_TIME_CONTENT_SCREEN;
            jumpInJumpOutData.hideDuration = Utils.Consts.TRANSITION_TIME_CONTENT_SCREEN;
            jumpInJumpOutData.blendActive = true;
            jumpInJumpOutData.showBlendEase = Ease.OutQuart;
            jumpInJumpOutData.hideBlendEase = Ease.OutQuart;
            jumpInJumpOutData.scaleActive = true;
            jumpInJumpOutData.showScaleEase = Ease.InOutQuad;
            jumpInJumpOutData.hideScaleEase = Ease.InOutQuad;
            jumpInJumpOutData.shakeActive = false;
            jumpInJumpOutData.showShakeStrength = 0.1f;
            jumpInJumpOutData.showShakeVibrato = 10;
            jumpInJumpOutData.showShakeRandomness = 90;
            jumpInJumpOutData.showShakeEase = Ease.InOutQuad;
            jumpInJumpOutData.hideShakeEase = Ease.InOutQuad;
            jumpInJumpOutData.hideShakeStrength = 0.1f;
            jumpInJumpOutData.hideShakeVibrato = 10;
            jumpInJumpOutData.hideShakeRandomness = 90;
            jumpInJumpOutData.rotationActive = false;
            jumpInJumpOutData.rotationAxis = Vector3.forward;
            jumpInJumpOutData.showRotationEase = Ease.OutElastic;
            jumpInJumpOutData.hideRotationEase = Ease.OutQuad;

            jumpInJumpOutData.scaleMinMax = new MinMaxHelper(0.1f, 1f);
            jumpInJumpOutData.showStartMinMaxRotation = new MinMaxHelper(0, 0);
            jumpInJumpOutData.showMinMaxRotation = new MinMaxHelper(-25, 25);
            jumpInJumpOutData.hideMinMaxRotation = new MinMaxHelper(0, 0);

            return jumpInJumpOutData;
        }
    }
}


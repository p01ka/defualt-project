using UnityEngine;

namespace IdxZero.Editor
{

    [CreateAssetMenu(fileName = "DefaultRaycastTargetAndImageSettings", menuName = "Editor/DefaultRaycastTargetAndImageSettings")]
    public class DefaultRaycastTargetAndImageSettings : ScriptableObject
    {
        [SerializeField] private Material _defaultImageMaterial;
        [SerializeField] private Sprite _defaultImageSprite;

        public Material DefaultImageMaterial => _defaultImageMaterial;
        public Sprite DefaultImageSprite => _defaultImageSprite;
    }
}
using UnityEngine;

namespace Game.Scripts
{
    public class TouchscreenJoystick : MonoBehaviour
    {
        [field: SerializeField] 
        private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;

        [field: SerializeField]
        private RectTransform _circleRectTransform;
        public RectTransform CircleRectTransform => _circleRectTransform;
    }
}
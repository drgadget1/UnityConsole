using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Meta.Console.Scripts.GUI
{
    public class ConsoleResizer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 _minSize;
        private Vector2 _maxSize;
        
        private Canvas _canvas;

        private RectTransform _outlineRect;
        private GameObject _outlineGameObject;

        private RectTransform _baseRect;
        
        
        public void Init(ConsoleGUIBase instance, Vector2 min, Vector2 max)
        {
            this._minSize = min;
            this._maxSize = max;
            
            _canvas = instance.consoleCanvas;
            
            _outlineRect = instance.outlineRectTransform;
            _outlineGameObject = instance.outlineGameObject;
            
            _baseRect = instance.baseRectTransform;

        }
        
        
        
        private bool _resizeFromCenterModeActive;
        private float _speedMod;

        public void TurnOnResizeFromCenter(InputAction.CallbackContext c)
        { _resizeFromCenterModeActive = true;}

        public void TurnOffResizeFromCenter(InputAction.CallbackContext c)
        { _resizeFromCenterModeActive = false;}

        public void OnBeginDrag(PointerEventData eventData)
        {
            _speedMod = _canvas.scaleFactor; 
            _outlineGameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 currentSize = _outlineRect.rect.size;
            
            Vector2 newSize = currentSize + eventData.delta / _speedMod;
            
            newSize.x = Mathf.Clamp(newSize.x, _minSize.x, _maxSize.x);
            newSize.y = Mathf.Clamp(newSize.y, _minSize.y, _maxSize.y);
            
            if (newSize == _minSize || newSize == _maxSize) {return; }
            _outlineRect.offsetMax += newSize - currentSize;
            
            if (_resizeFromCenterModeActive)
            { _outlineRect.offsetMin -= newSize - currentSize; }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _outlineGameObject.SetActive(false);
            _baseRect.offsetMax = _outlineRect.offsetMax;
            _baseRect.offsetMin = _outlineRect.offsetMin;
        }
    }
}
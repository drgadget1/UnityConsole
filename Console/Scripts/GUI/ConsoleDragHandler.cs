using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Meta.Console.Scripts.GUI
{
    public class ConsoleDragHandler : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler
    {
        private Canvas _canvas;
        
        private RectTransform _outlineRect;
        private GameObject _outlineGameObject;

        private RectTransform _baseRect;

        private float _speedMod;

        public void Init(ConsoleGUIBase instance)
        {
            _canvas = instance.consoleCanvas;
            
            _outlineRect = instance.outlineRectTransform;
            _outlineGameObject = instance.outlineGameObject;
            
            _baseRect = instance.baseRectTransform;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            _outlineGameObject.SetActive(true);
            _speedMod = _canvas.scaleFactor; 
        }

        public void OnDrag(PointerEventData eventData)
        { _outlineRect.anchoredPosition += eventData.delta / _speedMod; }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            _outlineGameObject.SetActive(false);
            _baseRect.offsetMax = _outlineRect.offsetMax;
            _baseRect.offsetMin = _outlineRect.offsetMin;
        }
        
    }
}
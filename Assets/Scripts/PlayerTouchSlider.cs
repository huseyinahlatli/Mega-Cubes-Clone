using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerTouchSlider  : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityAction OnPointerDownEvent;
    public UnityAction<float> OnPointerDragEvent;
    public UnityAction OnPointerUpEvent;

    private Slider _uiSlideR;

    private void Awake()
    {
        _uiSlideR = GetComponent<Slider>();
        _uiSlideR.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        if (OnPointerDownEvent != null)
            OnPointerDownEvent.Invoke();

        if (OnPointerDragEvent != null)
            OnPointerDragEvent.Invoke(_uiSlideR.value);
    }

    private void OnSliderValueChanged(float value)
    {
        if (OnPointerDragEvent != null)
            OnPointerDragEvent.Invoke(value);
    }

    public void OnPointerUp (PointerEventData eventData)
    {
         if (OnPointerUpEvent != null)
            OnPointerUpEvent.Invoke();

         _uiSlideR.value = 0f;  // reset slider value
    }

    private void OnDestroy()
    {      
        //remove listeners: to avoid memory leaks
        _uiSlideR.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}

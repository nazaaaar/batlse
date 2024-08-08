using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

/// <summary>
/// A stick control displayed on screen and moved around by touch or other pointer
/// input. Floats to pointer down position.
/// </summary>
[AddComponentMenu("Input/Floating On-Screen Stick")]
public class FloatingOnScreenStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));
        EnableJoystick(true);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
        m_JoystickTransform.anchoredPosition = m_PointerDownPos;
        m_JoystickBGTransform.anchoredPosition = m_PointerDownPos;

    }

    private void EnableJoystick(bool val)
    {
        m_JoystickBGTransform.gameObject.SetActive(val);
        m_JoystickTransform.gameObject.SetActive(val);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, eventData.position, eventData.pressEventCamera, out m_DragPos);
        var delta = m_DragPos - m_PointerDownPos;

        delta = Vector2.ClampMagnitude(delta, movementRange);
        m_JoystickTransform.anchoredPosition = m_PointerDownPos + delta;

        var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);
        SendValueToControl(newPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SendValueToControl(Vector2.zero);
        EnableJoystick(false);
    }

    private void Start()
    {
       EnableJoystick(false);
    }

    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    [SerializeField]
    private float m_MovementRange = 50;

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    [SerializeField]
    private RectTransform m_JoystickTransform;
    
    [SerializeField]
    private RectTransform m_JoystickBGTransform;

    private Vector2 m_StartPos;
    private Vector2 m_PointerDownPos;
    private Vector2 m_DragPos;
    

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseSubPanel : UIBehaviour, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler 
{

    //[Serializable] public class BeginDragEvent : UnityEvent<BaseEventData> { }
    //[Serializable] public class EndDragEvent : UnityEvent<BaseEventData> { }
    //[Serializable] public class DragEvent : UnityEvent<BaseEventData> { }

    [SerializeField] private RectTransform m_Target;
    [SerializeField] private bool m_ConstrainWithinCanvas = false;
    [SerializeField] private bool m_ConstrainDrag = true;
    private bool m_Dragging = false;

    private Canvas m_Canvas;
    private RectTransform m_CanvasRectTransform;
    private Vector2 m_PointerStartPosition = Vector2.zero;
    private Vector2 m_TargetStartPosition = Vector2.zero;

    private Vector2 m_LastPosition = Vector2.zero;

    //public BeginDragEvent onBeginDrag = new BeginDragEvent();
    //public EndDragEvent onEndDrag = new EndDragEvent();
    //public DragEvent onDrag = new DragEvent();

    void Start()
    {
        this.m_Target = GetComponent<RectTransform>();
        this.m_Canvas = UIMain.Instance.canvas;
        this.m_CanvasRectTransform = UIMain.Instance.canvasRT;
    }

    public override bool IsActive()
    {
        return base.IsActive() && this.m_Target != null;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (!this.IsActive())
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_CanvasRectTransform, data.position, data.pressEventCamera, out this.m_PointerStartPosition);
        this.m_TargetStartPosition = this.m_Target.anchoredPosition;
    }

    public void OnEndDrag(PointerEventData data)
    {
        this.m_Dragging = false;

        if (!this.IsActive())
            return;
    }

    public void OnDrag(PointerEventData data)
    {
        if (!this.IsActive() || this.m_Canvas == null)
            return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_CanvasRectTransform, data.position, data.pressEventCamera, out mousePos);

        if (this.m_ConstrainWithinCanvas && this.m_ConstrainDrag)
        {
            mousePos = this.ClampToCanvas(mousePos);
        }

        Vector2 newPosition = this.m_TargetStartPosition + (mousePos - this.m_PointerStartPosition);

        // Apply the position change
        this.m_Target.anchoredPosition = newPosition;
    }

    protected virtual void LateUpdate()
    {
        if (!this.m_Target)
            return;

        this.m_LastPosition = this.m_Target.anchoredPosition;
    }

    protected Vector2 ClampToScreen(Vector2 position)
    {
        if (this.m_Canvas != null)
        {
            if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay || this.m_Canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                float clampedX = Mathf.Clamp(position.x, 0f, Screen.width);
                float clampedY = Mathf.Clamp(position.y, 0f, Screen.height);

                return new Vector2(clampedX, clampedY);
            }
        }

        // Default
        return position;
    }

    protected Vector2 ClampToCanvas(Vector2 position)
    {
        if (this.m_CanvasRectTransform != null)
        {
            Vector3[] corners = new Vector3[4];
            this.m_CanvasRectTransform.GetLocalCorners(corners);

            float clampedX = Mathf.Clamp(position.x, corners[0].x, corners[2].x);
            float clampedY = Mathf.Clamp(position.y, corners[3].y, corners[1].y);

            return new Vector2(clampedX, clampedY);
        }

        // Default
        return position;
    }

}

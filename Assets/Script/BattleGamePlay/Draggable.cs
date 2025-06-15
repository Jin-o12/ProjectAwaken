using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float snapThreshold = 1f;  // 스냅 허용 거리
    public List<Transform> snapPoints;  // Inspector에서 설정 가능

    private Vector3 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position) + offset;
        worldPos.z = 0f;
        transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float minDist = float.MaxValue;
        Transform closest = null;

        foreach (Transform point in snapPoints)
        {
            float dist = Vector3.Distance(transform.position, point.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = point;
            }
        }

        if (closest != null && minDist < snapThreshold)
            transform.position = closest.position;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEntity : MonoBehaviour {

    public Transform followedEntity;
    public Vector3 deltaPosition;
    protected RectTransform _canvas;

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    void LateUpdate () {
        Vector3 newTransform = Camera.main.WorldToViewportPoint(followedEntity.transform.TransformPoint(deltaPosition));
        newTransform -= 0.5f * Vector3.one;
        Rect rect = _canvas.rect;
        newTransform.x *= rect.width;
        newTransform.y *= rect.height;
        newTransform.z = 0;
        GetComponent<RectTransform>().localPosition = newTransform;
	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI check ke liye

public class TouchToHide : MonoBehaviour
{
    [Header("Objects to Hide")]
    public List<GameObject> objectsToHide = new List<GameObject>();
    public bool touched = true;

/*
    private void Start()
    {
        GameManager.Instance.StartGame();
    }*/
    void Update()
    {
        if (!touched) return;

        // Touch input (mobile / WebGL touch)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!IsTouchOverUI(Input.GetTouch(0).fingerId))
                HideAll();
        }
        // Mouse input (PC test)
        else if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
                HideAll();
        }
    }

    void HideAll()
    {
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        GameManager.Instance.StartGame();
        touched = false;
    }

    // Mobile ke liye fingerId se check
    bool IsTouchOverUI(int fingerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
    }

    // PC / Mouse ke liye
    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}

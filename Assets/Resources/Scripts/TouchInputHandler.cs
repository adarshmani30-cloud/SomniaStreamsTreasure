using UnityEngine;

public class TouchInputHandler : MonoBehaviour
{
    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Touch input for mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            HandleInput(Input.GetTouch(0).position);
        }
#else
        // Mouse input for Editor, WebGL, and Desktop
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
#endif
    }

    void HandleInput(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var collectible = hit.collider.GetComponent<Collectible>();
            if (collectible != null)
            {
                Debug.Log($"Tapped on: {collectible.rewardName} | Interactable: {collectible.GetComponent<Collider>().enabled}");
                RewardManager.Instance.PlayerSelects(collectible);
            }
        }
    }
}

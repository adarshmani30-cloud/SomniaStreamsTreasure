using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    [SerializeField]
    int WaitSec;

    public string sceneNme;
    //public GameObject titleImage;

    [SerializeField]
    float scaleDuration = 1.5f; // Kitne seconds me scale ho

    void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        // Animate scale
       // yield return StartCoroutine(AnimateTitleImageScale());

        // Wait remaining time (WaitSec - scaleDuration)
        float remainingTime = Mathf.Max(0, WaitSec - scaleDuration);
        yield return new WaitForSeconds(remainingTime);

        // Load next scene
        SceneManager.LoadScene(sceneNme);
    }

    /*IEnumerator AnimateTitleImageScale()
    {
        RectTransform rect = titleImage.GetComponent<RectTransform>();
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

        float elapsed = 0;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scaleDuration;
            rect.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        rect.localScale = targetScale;
    }*/
}

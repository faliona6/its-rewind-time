using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI cloneTimer;
    [SerializeField] TextMeshProUGUI clonesLeft;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] Canvas HUDCanvas;
    [SerializeField] Canvas levelEndCanvas;

    private void Start()
    {
        LevelManager.Instance.LevelEndEvent += endScreen;
    }

    private void endScreen()
    {
        levelEndCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        HUDCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        Cursor.visible = true;
    }

    void Update()
    {
        cloneTimer.SetText(LevelManager.Instance.getTimeRemaining().ToString());
        clonesLeft.SetText(LevelManager.Instance.getNumRemainingClones().ToString());
        health.SetText(LevelManager.Instance.getCurPlayer().GetComponentInChildren<PlayerHealth>().CurHealth.ToString());
    }
}

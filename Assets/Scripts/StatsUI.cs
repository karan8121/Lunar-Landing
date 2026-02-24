using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject SpeedUpArrowGameObject;
    [SerializeField] private GameObject SpeedDownArrowGameObject;
    [SerializeField] private GameObject SpeedLeftArrowGameObject;
    [SerializeField] private GameObject SpeedRightArrowGameObject;
    [SerializeField] private Image fuelImage;
    private void Update()
    {
        UpdateStatsTextMesh();
    }

    private void UpdateStatsTextMesh()
    {
        SpeedUpArrowGameObject.SetActive(Lander.Instance.GetSpeedY() >= 0);
        SpeedDownArrowGameObject.SetActive(Lander.Instance.GetSpeedY() < 0);
        SpeedRightArrowGameObject.SetActive(Lander.Instance.GetSpeedX() >= 0);
        SpeedLeftArrowGameObject.SetActive(Lander.Instance.GetSpeedX() < 0);

        fuelImage.fillAmount = Lander.Instance.GetFuelAmountNormalized();
        statsTextMesh.text =
        GameManager.Instance.GetScore() + "\n" +
        Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10f)) ;
    }

}

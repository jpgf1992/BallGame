using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Text cLevelText, nLevelText;
    private Image fill;

    private int level;
    private float startDistance, distance;

    private GameObject player, finish, handImg;
    private TextMesh levelNo;
    
    void Awake()
    {
        cLevelText = GameObject.Find("CurrentLevelText").GetComponent<Text>();
        nLevelText = GameObject.Find("NextLevelText").GetComponent<Text>();
        fill = GameObject.Find("Fill").GetComponent<Image>();

        player = GameObject.Find("Player");
        finish = GameObject.Find("Finish");
        handImg = GameObject.Find("Hand");

        levelNo = GameObject.Find("LevelNumber").GetComponent<TextMesh>();
    }

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level");
        levelNo.text = "LEVEL " + level;

        nLevelText.text = level + 1 + "";
        cLevelText.text = level.ToString();

        startDistance = Vector3.Distance(player.transform.position, finish.transform.position);
        // SceneManager.LoadScene("Level" + level);
    }
    
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, finish.transform.position);
        if (player.transform.position.z < finish.transform.position.z)
        {
            fill.fillAmount = 1 -(distance/startDistance);
        }
    }

    public void RemoveUI()
    {
        handImg.SetActive(false);
    }
}

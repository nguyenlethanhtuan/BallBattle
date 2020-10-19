using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    Text txtTime;
    Text txtNamePlayerA;
    Text txtNamePlayerB;
    public GameObject objRoundWin;
    Slider energyBarA;
    Slider energyBarB;
    Slider energyHighlightA;
    Slider energyHighlightB;

    void Awake()
    {
        txtTime = GameObject.Find("TextTimeShow").GetComponent<Text>();
        txtNamePlayerA = GameObject.Find("TextNameA").GetComponent<Text>();
        txtNamePlayerB = GameObject.Find("TextNameB").GetComponent<Text>();
        energyBarA = GameObject.Find("FillBarSliderA").GetComponent<Slider>();
        energyBarB = GameObject.Find("FillBarSliderB").GetComponent<Slider>();
        energyHighlightA = GameObject.Find("HighlightBarSliderA").GetComponent<Slider>();
        energyHighlightB = GameObject.Find("HighlightBarSliderB").GetComponent<Slider>();
        //objRoundWin = GameObject.Find("WinScreen");
    }

    // Update is called once per frame
    void Update()
    {
        drawTime();
        updateEnergyBar();
        //slowUpdate(0.1f);
        updateHighlightBar();
    }
    
    void drawTime(){
        txtTime.text = GameMaster.GM.timer.ToString();
    }
    
    void updateEnergyBar(){
        float energyA = GameMaster.GM.teamA.energy;
        float energyB = GameMaster.GM.teamB.energy;
        energyBarA.value = energyA / Team.MAX_ENERGY;
        energyBarB.value = energyB / Team.MAX_ENERGY;
    }
    IEnumerator slowUpdate(float delayTime){
        for(;;){
            updateHighlightBar();
            yield return new WaitForSeconds(delayTime);
        }
    }
    void updateHighlightBar(){
        float energyA = GameMaster.GM.teamA.energy;
        float energyB = GameMaster.GM.teamB.energy;
        
        if(energyA < 1)
            energyHighlightA.value = 0;
        else if(energyA < 2)
            energyHighlightA.value = 1.0f/Team.MAX_ENERGY;
        else if(energyA < 3)
            energyHighlightA.value = 2.0f/Team.MAX_ENERGY;
        else if(energyA < 4)
            energyHighlightA.value = 3.0f/Team.MAX_ENERGY;
        else if(energyA < 5)
            energyHighlightA.value = 4.0f/Team.MAX_ENERGY;
        else if(energyA < 6)
            energyHighlightA.value = 5.0f/Team.MAX_ENERGY;
        else
            energyHighlightA.value = 6.0f/Team.MAX_ENERGY;

        if(energyB < 1)
            energyHighlightB.value = 0;
        else if(energyB < 2)
            energyHighlightB.value = 1.0f/Team.MAX_ENERGY;
        else if(energyB < 3)
            energyHighlightB.value = 2.0f/Team.MAX_ENERGY;
        else if(energyB < 4)
            energyHighlightB.value = 3.0f/Team.MAX_ENERGY;
        else if(energyB < 5)
            energyHighlightB.value = 4.0f/Team.MAX_ENERGY;
        else if(energyB < 6)
            energyHighlightB.value = 5.0f/Team.MAX_ENERGY;
        else
            energyHighlightB.value = 6.0f/Team.MAX_ENERGY;
    }

    public void refreshPlayerInfo(){
        txtNamePlayerA.text = GameMaster.GM.teamA.getInfo();
        txtNamePlayerB.text = GameMaster.GM.teamB.getInfo();
        energyBarA.GetComponentInChildren<Image>().color = Utility.setAlpha(Utility.getColorCode(GameMaster.GM.teamA.color), 0.4f);
        energyBarB.GetComponentInChildren<Image>().color = Utility.setAlpha(Utility.getColorCode(GameMaster.GM.teamB.color), 0.4f);
        energyHighlightA.GetComponentInChildren<Image>().color = Utility.getColorCode(GameMaster.GM.teamA.color);
        energyHighlightB.GetComponentInChildren<Image>().color = Utility.getColorCode(GameMaster.GM.teamB.color);
    }

    public void onClickNextRoundButton(){
        Debug.Log("Tuan onClickNextRoundButton");
        GameMaster.GM.startNewRound();
        objRoundWin.SetActive(false);
    }

    public void showEndRoundWindow(Team winner){
        string winText;
        if(winner != null)
           winText = string.Format("Round {0}\n{1}\n{2}", GameMaster.GM.round, winner.getStringRole(), winner.playerName);
        else
            winText = string.Format("Round {0}\n\nDRAW", GameMaster.GM.round);
        objRoundWin.SetActive(true);
        objRoundWin.GetComponentInChildren<Text>().text = winText;
    }
}

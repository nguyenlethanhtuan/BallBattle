using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPlayerScript : ObjectScript
{
    public GameObject[] clothes;
    public float reactiveTime;
    public float spawnTime;
    public int color;
    public int team;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void setPlayerActive(bool isActive){
        this.isActive = isActive;
        setGreyScale(!isActive);
    }
    public virtual IEnumerator setActiveWithDelayTime(bool isActive, float delayTime){
        yield return new WaitForSeconds(delayTime);
        setPlayerActive(isActive);
    }
    public bool isPlayerActive(){
        return isActive;
    }

    public void applyColor(int color){
        //Material playerMaterial = GetComponent<MeshRenderer>().materials[0];
        for(int i=0; i<clothes.Length; i++){
            clothes[i].GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Utility.getColorCode(color));
        }
        //playerMaterial.SetColor("_Color", Utility.getColorCode(color));
    }

    public void initPlayer(Team team){
        this.team = team.side;
        color = team.color;
        applyColor(color);
    }

    public void setGreyScale(float rate){
        Material playerMaterial = GetComponent<MeshRenderer>().materials[0];
        
        //playerMaterial.SetFloat("_GrayscaleAmount", rate);
        //playerMaterial.SetColor("_Color", Color.grey);
    }

    public void setGreyScale(bool isGrayscale){
        //Material playerMaterial = GetComponent<MeshRenderer>().materials[0];
        if(isGrayscale)
            for(int i=0; i<clothes.Length; i++){
                clothes[i].GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.grey);
            }
            //playerMaterial.SetColor("_Color", Color.grey);
        else
            applyColor(color);
    }

    public void setArrowActive(bool isActive){
        transform.GetChild(1).gameObject.SetActive(isActive);
    }
}

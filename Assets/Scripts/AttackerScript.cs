using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerScript : SoccerPlayerScript
{
    const float NORMAL_SPEED = 1.5f;
    const float CARRYING_SPEED = 0.75f;
    const float REACTIVATE_TIME = 2.5f;
    const float SPAWN_TIME = 0.5f;
    public Transform goalTeamA;
    public Transform goalTeamB;
    public BallScript ball;
    public Transform soldierHolder;
    public bool isKeepingBall;
    FieldScript field;
    

    // Start is called before the first frame update
    void Start()
    {
        //find object
        goalTeamA = GameObject.Find("FarSoldierField").transform;
        goalTeamB = GameObject.Find("FarEnemyField").transform;
        ball = GameObject.Find("BallHolder").GetComponent<BallScript>();
        soldierHolder = GameObject.Find("SoldierHolder").transform;
        field = GameObject.Find("Field").GetComponent<FieldScript>();
        //////
        speed = NORMAL_SPEED;
        spawnTime = SPAWN_TIME;
        reactiveTime = REACTIVATE_TIME;
        isKeepingBall = false;
        //Using coroutine to make an delay action
        StartCoroutine(setActiveWithDelayTime(true, spawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(isPlayerActive()){
            if(ball.isActive)
                chasingBall();
            else if(isKeepingBall)
                moveToGoal();
            else
                moveForward();
        }
    }

    void OnTriggerEnter(Collider other){
        switch(other.tag){
            case "Ball": 
                isKeepingBall = true;
                speed = CARRYING_SPEED;
                ball.setBallActive(false);
                setHighLight(true);
                break;
                
            case "Defender":
                if(isKeepingBall){
                    if(other.GetComponent<DefenderScript>().isActive){
                        passBall();
                        speed = NORMAL_SPEED;
                        setPlayerActive(false);
                        StartCoroutine(setActiveWithDelayTime(true, reactiveTime));
                    }
                }
                break;

            case "Wall":
                die();
                break;

            case "Goal":
                if(isKeepingBall)
                    GameMaster.GM.endRound(GameMaster.GM.teamList[team]);
                break;
        }
    }
    void OnTriggerStay(Collider other){
        if(other.CompareTag("Defender")){
            if(isKeepingBall){
                if(other.GetComponent<DefenderScript>().isActive){
                    passBall();
                    speed = NORMAL_SPEED;
                    setPlayerActive(false);
                    StartCoroutine(setActiveWithDelayTime(true, reactiveTime));
                }
            }
        }
    }

    void init(){

    }

    void chasingBall(){
        moveToward(ball.gameObject);
    }

    void moveToGoal(){
        if(team == Team.TEAM_A){
            moveToward(goalTeamB);
        }
        else if(team == Team.TEAM_B)
            moveToward(goalTeamA);
    }

    GameObject findNearestAvailaleTeammate(){
        if(soldierHolder.childCount>1){
            int index=0;
            float minDistance = field.getLength()*2;

            for(int i=index;i<soldierHolder.childCount;i++){
                //check active
                if(soldierHolder.GetChild(i).GetComponent<AttackerScript>().isActive){
                    float distance = (transform.position - soldierHolder.GetChild(i).transform.position).magnitude;
                    if(distance<minDistance && distance!=0){
                        minDistance = distance;
                        index=i;
                    }
                }
            }
            return (minDistance<field.getLength()*2) ? soldierHolder.GetChild(index).gameObject : null;
        }
        else
            return null;
    }

    Vector3 getBallRespawnPosition(GameObject teammate){
            Vector3 directionToTeamate = teammate.transform.position - transform.position;
            float offsetPlayerToBall = GetComponent<CapsuleCollider>().bounds.size.x/2 + ball.getBallRadius();
            return transform.position + directionToTeamate.normalized*offsetPlayerToBall;
    }
    void passBall(){
        if(findNearestAvailaleTeammate() != null){
            ball.setBallActive(true);
            ball.transform.position = getBallRespawnPosition(findNearestAvailaleTeammate());
            ball.setTarget(findNearestAvailaleTeammate());
            isKeepingBall = false;
        }else{
            GameMaster.GM.endRound(GameMaster.GM.teamList[(team+1)%2]);
        }
    }

    void moveForward(){
        Vector3 movingDestination;
        Vector3 movingDirection;
        if(team == Team.TEAM_A){
            movingDirection = (goalTeamB.position - field.transform.position).normalized;        
        }
        else{
            movingDirection = (goalTeamA.position - field.transform.position).normalized;
        }
        movingDestination = transform.position + new Vector3(movingDirection.x, transform.position.y, movingDirection.z);
        moveToward(movingDestination);
    }

    void die(){
        isActive = false;
        GetComponentInChildren<Animator>().SetTrigger("Death");
    }

    void setHighLight(bool isActive){
        transform.GetChild(0).gameObject.SetActive(isActive);
    }

    /*public override IEnumerator setActiveWithDelayTime(bool isActive, float delayTime){
        yield return new WaitForSeconds(delayTime);
        setArrowActive(isActive);
    }*/

    public override void setPlayerActive(bool isActive){
        this.isActive = isActive;
        setGreyScale(!isActive);
        setArrowActive(isActive);
        GetComponentInChildren<Animator>().SetBool("isActive", isActive);
        if(isActive == false)
            setHighLight(false);
    }
}

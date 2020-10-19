using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster GM;

    /////Find Objects
    public Transform SoldierHolder;
    public Transform EnemyHolder;
    public GameObject prefabAttacker;
    public GameObject prefabDefender;
    public BallScript ball;
    public GameplayUIController gameplayUI;
    public FieldScript field;
    public Transform goalTeamA;
    public Transform goalTeamB;
    ///////
    public const int MAX_ROUND = 5;
    public const float TIME_LIMIT = 140;
    public Team teamA;
    public Team teamB;
    public Timer timer;
    public int round;
    bool isPause;
    public Team[] teamList;
    bool isAlreadyEndRound = false;
    void Awake(){
        if(GM != null)
            GameObject.Destroy(GM);
        else
            GM = this;

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        teamA = new Team(Team.ROLE_ATTACKER, Team.TEAM_A, Team.BLUE);
        teamB = new Team(Team.ROLE_DEFENDER, Team.TEAM_B, Team.RED);
        teamList = new Team[]{teamA, teamB};
        round = 1;
        isPause = false;
        timer = new Timer(TIME_LIMIT);
        timer.start();
        field.init();
        gameplayUI.refreshPlayerInfo();
     //   gameplayUI.onClickNextRoundButton();
        //startNewRound();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPause){
            gameUpdate();
            createPlayerOnClick();
        }
    }

    void createPlayerOnClick(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask teamALayerMask = 1<<LayerMask.NameToLayer("TeamA");
        LayerMask teamBLayerMask = 1<<LayerMask.NameToLayer("TeamB");
        if(Input.GetMouseButtonDown(0)){
            if(Physics.Raycast(ray, out hit, 1000, teamALayerMask))
                createPlayerBaseOnTeamRole(teamA,hit.point);
            if(Physics.Raycast(ray, out hit, 1000, teamBLayerMask))
                createPlayerBaseOnTeamRole(teamB,hit.point);
        }
    }

    AttackerScript createAttacker(Vector3 position){
        return Instantiate(prefabAttacker, position, Quaternion.identity, SoldierHolder).GetComponent<AttackerScript>();
    }
    DefenderScript createDefender(Vector3 position){
        return Instantiate(prefabDefender, position, Quaternion.identity, EnemyHolder).GetComponent<DefenderScript>();
    }
    void createPlayerBaseOnTeamRole(Team team, Vector3 position){
        if(team.energy >= team.energyCost){
            SoccerPlayerScript playerScript;
            if(team.role == Team.ROLE_ATTACKER)
                playerScript = createAttacker(position);
            else
                playerScript = createDefender(position);
            team.energy -= team.energyCost;
            playerScript.initPlayer(team);
            if(team == teamA)
                playerScript.transform.rotation = Quaternion.LookRotation(goalTeamB.position - playerScript.transform.position);
            else
                playerScript.transform.rotation = Quaternion.LookRotation(goalTeamA.position - playerScript.transform.position);
        }
    }


    void switchRole(){
        //Fixed bug switch role 2 times because delay deactive of defender
        int temp = teamA.role;
        teamA.role = teamB.role;
        teamB.role = temp;
    }

    void gameUpdate(){
        energyRegenOverTime();
        timer.timerUpdate();
    }

    void energyRegenOverTime(){
        if(teamA.energy < Team.MAX_ENERGY-teamA.energyRegen)
            teamA.energy += teamA.energyRegen*Time.deltaTime;
        else
            teamA.energy = Team.MAX_ENERGY;
        if(teamB.energy < Team.MAX_ENERGY-teamB.energyRegen)
            teamB.energy += teamB.energyRegen*Time.deltaTime;
        else
            teamB.energy = Team.MAX_ENERGY;
    }

    public void pauseGame(){
        Time.timeScale = 0;
        isPause = true;
    }
    public void resumeGame(){
        Time.timeScale = 1;
        isPause = false;
    }

    public void endRound(Team winner){
        if(!isAlreadyEndRound){
            gameplayUI.showEndRoundWindow(winner);
            if(round<MAX_ROUND || teamA.winTimes == teamB.winTimes){
                destroyAllSoccerPlayer();
                ball.setBallActive(false);
                switchRole();
                pauseGame();
                if(winner != null){
                    if(winner.side == teamA.side){
                        teamA.winTimes++;
                    }
                    else if(winner.side == teamB.side){
                        teamB.winTimes++;
                    }
                }
                round++;
            }else{
                endGame();
            }
            isAlreadyEndRound = true;
        }
    }
    void endGame(){
        if(teamA.winTimes>teamB.winTimes)
            Debug.Log("Soldier Win");
        else
            Debug.Log("Enemy Win");
    }

    void destroyAllSoccerPlayer(){
        for(int i=0;i<SoldierHolder.childCount;i++){
            Destroy(SoldierHolder.GetChild(i).gameObject);
        }
        for(int i=0;i<EnemyHolder.childCount;i++){
            Destroy(EnemyHolder.GetChild(i).gameObject);
        }
    }

    public int getAttackerSide(){
        if(teamA.role == Team.ROLE_ATTACKER)
            return teamA.side;
        else
            return teamB.side;
    }

    public void startNewRound(){
        isAlreadyEndRound = false;
        gameplayUI.refreshPlayerInfo();
        timer.restart();
        ball.initNewRound();
        resumeGame();
        teamA.initNewRound();
        teamB.initNewRound();
    }
}

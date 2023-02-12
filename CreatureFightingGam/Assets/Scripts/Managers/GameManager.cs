using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public WoogieTeamMember[] yourTeam = new WoogieTeamMember[4];
    private int yourSelectedMemeber;
    public WoogieTeamMember[] otherTeam = new WoogieTeamMember[4];
    private int otherSelectedMember;
    public SetMove[] setMoves = new SetMove[2];
    [Space]
    public WoogieStats[] woogieStats;
    public Transform[] woogieSpawns;
    [Space]
    public WoogieSave yourWoogie;
    public WoogieSave otherWoogie;

    private int currentTurn;
    private int currentOrder;
    private bool hpDepleating;
    private WoogieStats depleatingStats;
    private int startedHp;
    private CalculationUtils.DamageCalculaton damage;
    private float currentDamageDone;
    private bool spawning;
    private bool dead;

    IEnumerator roundRoutine;

    private void Awake()
    {
        Instance = this;

        //Your team
        for (int i = 0; i < 2; i++)
        {
            yourTeam[i].woogieSave = TeamManager.team[i];
            WoogieScrObj woogieScrObj = Resources.Load<WoogieScrObj>("Woogies/" + yourTeam[i].woogieSave.woogieScrObjName);
            yourTeam[i].woogieScrObj = woogieScrObj;
            yourTeam[i].healthLeft = CalculationUtils.HpCalculation(woogieScrObj.baseStats.hp, yourTeam[i].woogieSave.individualStats.hp, yourTeam[i].woogieSave.EffortStats.hp, yourTeam[i].woogieSave.currentLevel);
            yourTeam[i].woogieAttackUses = new(yourTeam[i].woogieSave);
        }
        //Other team
        for (int i = 0; i < 2; i++)
        {
            otherTeam[i].woogieSave = TeamManager.team[i+2];
            WoogieScrObj woogieScrObj = Resources.Load<WoogieScrObj>("Woogies/" + otherTeam[i].woogieSave.woogieScrObjName);
            otherTeam[i].woogieScrObj = woogieScrObj;
            otherTeam[i].healthLeft = CalculationUtils.HpCalculation(woogieScrObj.baseStats.hp, otherTeam[i].woogieSave.individualStats.hp, otherTeam[i].woogieSave.EffortStats.hp, otherTeam[i].woogieSave.currentLevel);
            otherTeam[i].woogieAttackUses = new(TeamManager.team[i+2]);
        }
    }
    private void Start()
    {
        StartCoroutine(SetUp());
    }
    private void Update()
    {
        if (hpDepleating)
        {
            if (currentDamageDone < damage.damage)
            {
                float dps = damage.damage * Time.deltaTime;

                float health = depleatingStats.health;
                health -= dps;
                depleatingStats.SetHealth(health);
                currentDamageDone += dps;

                if(health <= 0)
                {
                    hpDepleating = false;
                    dead = true;
                    StopCoroutine(roundRoutine);
                    depleatingStats.SetHealth(0);
                    Dead(currentOrder);
                }
            }
            else
            {
                int health = startedHp - damage.damage;
                depleatingStats.SetHealth(health);
                hpDepleating = false;

                if(currentOrder == 0)
                {
                    otherTeam[otherSelectedMember].healthLeft = health;
                }
                else
                {
                    yourTeam[yourSelectedMemeber].healthLeft = health;
                }
            }
        }
    }
    public void Dead(int order)
    {
        int deadOrder = 0;
        if (order == 0) deadOrder = 1;

        StartCoroutine(DeadWoogieRoutine("Dead", deadOrder));
    }
    /// <summary>
    /// Setting up the 2 facing woogies
    /// </summary>
    IEnumerator SetUp()
    {
        MenuManager.Instance.LockMenus();
        yield return new WaitForSeconds(2);
        //Your stats bar set up
        CameraManager.Instance.PerspectiveYou();
        StartCoroutine(SpawnWoogieRoutine(0 , 0));
        while (spawning)
            yield return null;
        //Other stats bar set up
        CameraManager.Instance.PerspectiveOther();
        StartCoroutine(SpawnWoogieRoutine(1, 0));
        while (spawning)
            yield return null;

        CameraManager.Instance.UnlockCam();
        currentTurn = 0;
        SetTurn();
        MenuManager.Instance.UnlockMenus();
    }
    public void SetWoogie(int order, int selected)
    {
        if(order == 0)
        {
            yourSelectedMemeber = selected;
            yourWoogie = yourTeam[selected].woogieSave;
            if (yourTeam[selected].woogieScrObj.prefab != null)
                Instantiate(yourTeam[selected].woogieScrObj.prefab, woogieSpawns[0]);
            if (!woogieStats[0].gameObject.activeSelf)
                woogieStats[0].gameObject.SetActive(true);
            woogieStats[0].SetUp(yourWoogie);
        }
        else if(order == 1)
        {
            otherSelectedMember = selected;
            otherWoogie = otherTeam[selected].woogieSave;
            if (otherTeam[selected].woogieScrObj.prefab != null)
                Instantiate(otherTeam[selected].woogieScrObj.prefab, woogieSpawns[1]);
            if (!woogieStats[1].gameObject.activeSelf)
                woogieStats[1].gameObject.SetActive(true);
            woogieStats[1].SetUp(otherWoogie);
        }
    }
    public void RemoveWoogie(int order)
    {
        if (woogieSpawns[order].childCount == 0)
            return;
        Destroy(woogieSpawns[order].GetChild(0).gameObject);

    }
    public void SetTurn()
    {
        if (currentTurn == 0)
        {
            MenuManager.Instance.SetAttackButtons(yourWoogie, yourTeam[yourSelectedMemeber].woogieAttackUses);
            MenuManager.Instance.SetTeamButtons(yourTeam);
        }
        else if (currentTurn == 1)
        {
            MenuManager.Instance.SetAttackButtons(otherWoogie, otherTeam[otherSelectedMember].woogieAttackUses);
            MenuManager.Instance.SetTeamButtons(otherTeam);
        }
    }
    public void Attack(AttackScrObj attack, int attackNumber)
    {
        MenuManager.Instance.CloseAllMenus();

        if (currentTurn == 0)
        {
            if (attack == null || yourTeam[yourSelectedMemeber].woogieAttackUses.attacksLeft[attackNumber] <= 0)
                return;
            setMoves[0].woogieAttack = attack;
            yourTeam[yourSelectedMemeber].woogieAttackUses.attacksLeft[attackNumber]--;
        }
        else if(currentTurn == 1)
        {
            if (attack == null || otherTeam[otherSelectedMember].woogieAttackUses.attacksLeft[attackNumber] <= 0)
                return;
            setMoves[1].woogieAttack = attack;
            otherTeam[otherSelectedMember].woogieAttackUses.attacksLeft[attackNumber]--;

            roundRoutine = Round();
            StartCoroutine(roundRoutine);
            return;
        }

        currentTurn++;
        SetTurn();
    }
    public void Switch(int slot)
    {
        MenuManager.Instance.CloseAllMenus();

        if (!dead)
        {
            if (currentTurn == 0)
            {
                if (slot == yourSelectedMemeber)
                    return;
                setMoves[0].woogieSwitch = slot;
            }
            else
            {
                if (slot == otherSelectedMember)
                    return;
                setMoves[1].woogieSwitch = slot;

                roundRoutine = Round();
                StartCoroutine(roundRoutine);
                return;
            }

            currentTurn++;
            SetTurn();
        }
        else
        {
            StartCoroutine(SwitchNewRoutine(slot));
        }
    }
    IEnumerator SwitchNewRoutine(int slot)
    {
        int deadOrder = 0;
        if (currentOrder == 0) deadOrder = 1;

        StartCoroutine(SpawnWoogieRoutine(deadOrder, slot));

        while (spawning)
            yield return null;

        yield return new WaitForSeconds(1);

        CameraManager.Instance.UnlockCam();
        currentTurn = 0;
        SetTurn();
        MenuManager.Instance.UnlockMenus();
    }

    IEnumerator Round()
    {
        Debug.Log("Play");
        MenuManager.Instance.LockMenus();

        WoogieScrObj woogieScrObj = yourTeam[yourSelectedMemeber].woogieScrObj;
        NatureScrObj natureScrObj = Resources.Load<NatureScrObj>("Natures/" + yourWoogie.natureScrObjName);
        WoogieScrObj otherWoogieScrObj = otherTeam[otherSelectedMember].woogieScrObj;
        NatureScrObj otherNatureScrObj = Resources.Load<NatureScrObj>("Natures/" + otherWoogie.natureScrObjName);

        int[] order = new int[2];
        bool orderHasBeenSet = false;
        //Check who goes first by switching order
        if (setMoves[0].woogieAttack == null)
        {
            order[0] = 0;
            order[1] = 1;
            orderHasBeenSet = true;
        }
        else if(setMoves[1].woogieAttack == null)
        {
            order[0] = 1;
            order[1] = 0;
            orderHasBeenSet = true;
        }
        //Check who goes first by attack priority
        else if (setMoves[0].woogieAttack.priority)
        {
            if (!setMoves[0].woogieAttack.priority)
            {
                order[0] = 0;
                order[1] = 1;
                orderHasBeenSet = true;
            }
        }
        else if (setMoves[1].woogieAttack.priority)
        {
            order[0] = 1;
            order[1] = 0;
            orderHasBeenSet = true;
        }
        //Check who goes first by speed
        if (!orderHasBeenSet)
        {
            int speedYourWoogie = CalculationUtils.StatCaclulation(woogieScrObj.baseStats.spd, yourWoogie.individualStats.spd, yourWoogie.EffortStats.spd, yourWoogie.currentLevel, natureScrObj.speed);
            int speedOtherWoogie = CalculationUtils.StatCaclulation(otherWoogieScrObj.baseStats.spd, otherWoogie.individualStats.spd, otherWoogie.EffortStats.spd, otherWoogie.currentLevel, otherNatureScrObj.speed);
            if (speedYourWoogie >= speedOtherWoogie)
            {
                order[0] = 0;
                order[1] = 1;
            }
            else
            {
                order[0] = 1;
                order[1] = 0;
            }
        }

        //Attack loop
        for (int i = 0; i < order.Length; i++)
        {
            currentOrder = order[i];
            GameObject currentVFX = null;
            int lastWoogieOut = 0;

            if (order[i] == 0)
            {
                //When set move is attack
                if (setMoves[0].woogieAttack != null)
                {
                    //playVfx
                    currentVFX = Instantiate(setMoves[0].woogieAttack.vfx, woogieSpawns[0]);
                    damage = CalcDamage(setMoves[0].woogieAttack, woogieScrObj, yourWoogie, natureScrObj, otherWoogieScrObj, otherWoogie, otherNatureScrObj);

                    Debug.Log("Crit: " + damage.crit);
                    Debug.Log("Effectiveness: " + damage.effectiveness);
                    Debug.Log("Roll: " + damage.roll);
                    Debug.Log("Damage: " + damage.damage);

                    depleatingStats = woogieStats[1];
                    startedHp = (int)woogieStats[1].health;
                }
                //When set move is switch
                else
                {
                    Debug.Log($"Switch: {yourTeam[yourSelectedMemeber].woogieScrObj.woogieName} with {yourTeam[setMoves[0].woogieSwitch].woogieScrObj.woogieName}");

                    lastWoogieOut = yourSelectedMemeber;
                }
            }
            else if (order[i] == 1)
            {
                //When set move is attack
                if (setMoves[1].woogieAttack != null)
                {
                    //playVfx
                    currentVFX = Instantiate(setMoves[1].woogieAttack.vfx, woogieSpawns[1]);
                    float playTime = 1;
                    if (currentVFX.GetComponent<ParticleSystem>())
                        playTime = currentVFX.GetComponent<ParticleSystem>().main.duration;
                    else playTime = currentVFX.GetComponentInChildren<VisualEffect>().GetFloat("MaxTime");
                    Destroy(currentVFX, playTime + 5);

                    damage = CalcDamage(setMoves[1].woogieAttack, otherWoogieScrObj, otherWoogie, otherNatureScrObj, woogieScrObj, yourWoogie, natureScrObj);
                    depleatingStats = woogieStats[0];
                    startedHp = (int)woogieStats[0].health;
                }
                //When set move is switch
                else
                {
                    //playVfx
                    Debug.Log($"Switch: {otherTeam[otherSelectedMember].woogieScrObj.woogieName} with {otherTeam[setMoves[1].woogieSwitch].woogieScrObj.woogieName}");

                    lastWoogieOut = otherSelectedMember;
                }
            }

            //Round when setmove is attack
            if (setMoves[order[i]].woogieAttack != null)
            {
                if (order[i] == 0)
                    CameraManager.Instance.PerspectiveYou();
                else if (order[i] == 1)
                    CameraManager.Instance.PerspectiveOther();

                //Wait for attack vfx time
                float playTime = 1;
                if (currentVFX.GetComponent<ParticleSystem>())
                    playTime = currentVFX.GetComponent<ParticleSystem>().main.duration;
                else playTime = currentVFX.GetComponentInChildren<VisualEffect>().GetFloat("MaxTime");
                Debug.Log(playTime);
                yield return new WaitForSeconds(playTime * .75f);
                if (order[i] == 0)
                    CameraManager.Instance.PerspectiveOther();
                else if (order[i] == 1)
                    CameraManager.Instance.PerspectiveYou();
                yield return new WaitForSeconds(playTime * .25f);

                currentDamageDone = 0;
                hpDepleating = true;

                //Wait for hp bar decreasing
                while (hpDepleating)
                    yield return null;

                //Popup message
                switch (damage.effectiveness)
                {
                    case CalculationUtils.DamageCalculaton.Effectiveness.defaultEffective:
                        MenuManager.Instance.MessagePopUp("Basic attack!");
                        break;
                    case CalculationUtils.DamageCalculaton.Effectiveness.notVeryEffective:
                        MenuManager.Instance.MessagePopUp("It's not very effective....");
                        break;
                    case CalculationUtils.DamageCalculaton.Effectiveness.notEffective:
                        MenuManager.Instance.MessagePopUp("It had no effect....");
                        break;
                    case CalculationUtils.DamageCalculaton.Effectiveness.superEffective:
                        MenuManager.Instance.MessagePopUp("It's super effective!");
                        break;
                }

                //Wait for going to next attack
                while (MenuManager.Instance.messageDisplaying)
                    yield return null;
                CameraManager.Instance.UnlockCam();
                yield return new WaitForSeconds(1);
            }
            else //Round when setmove is switch
            {
                if (order[i] == 0)
                {
                    CameraManager.Instance.PerspectiveYou();
                    MenuManager.Instance.MessagePopUp("Come back " + yourTeam[lastWoogieOut].woogieScrObj.woogieName);
                }
                else if (order[i] == 1)
                {
                    CameraManager.Instance.PerspectiveOther();
                    MenuManager.Instance.MessagePopUp("Come back " + otherTeam[lastWoogieOut].woogieScrObj.woogieName);
                }

                //Despawn playVfx at right spawn point
                //Wait for despawn vfx
                yield return new WaitForSeconds(.5f);
                RemoveWoogie(order[i]);
                while (MenuManager.Instance.messageDisplaying)
                    yield return null;

                yield return new WaitForSeconds(1);

                //Spawn playVfx at right spawn point
                //Wait for spawn vfx
                yield return new WaitForSeconds(.5f);
                StartCoroutine(SpawnWoogieRoutine(order[i], setMoves[order[i]].woogieSwitch));
                while (MenuManager.Instance.messageDisplaying)
                    yield return null;

                yield return new WaitForSeconds(1);
                CameraManager.Instance.UnlockCam();
                yield return new WaitForSeconds(1);
            }
        }

        //Clear set moves
        setMoves[0].SetAttackNull();
        setMoves[1].SetAttackNull();

        Debug.Log("Finished attacking");
        MenuManager.Instance.UnlockMenus();
        currentTurn = 0;
        SetTurn();
    }
    IEnumerator SpawnWoogieRoutine(int order, int selected)
    {
        spawning = true;
        //Spawn vfx
        if(order == 0)
            MenuManager.Instance.MessagePopUp("Go " + yourTeam[selected].woogieScrObj.woogieName);
        else
            MenuManager.Instance.MessagePopUp("Go " + otherTeam[selected].woogieScrObj.woogieName);
        //Wait for spawn vfx
        yield return new WaitForSeconds(1);
        SetWoogie(order, selected);
        yield return new WaitForSeconds(1);
        spawning = false;
    }
    IEnumerator DeadWoogieRoutine(string anim, int deadOrder)
    {
        int selected = 0;
        if (deadOrder == 0)
        {
            CameraManager.Instance.PerspectiveYou();
            MenuManager.Instance.MessagePopUp(yourTeam[yourSelectedMemeber].woogieScrObj.woogieName + " died");
            selected = yourSelectedMemeber;
        }
        else if (deadOrder == 1)
        {
            CameraManager.Instance.PerspectiveOther();
            MenuManager.Instance.MessagePopUp(otherTeam[otherSelectedMember].woogieScrObj.woogieName + " died");
            selected = otherSelectedMember;
        }

        //Spawn vfx
        //Wait for vfx
        yield return new WaitForSeconds(1);
        RemoveWoogie(deadOrder);
        yield return new WaitForSeconds(1);
        MenuManager.Instance.teamButtonsMenu.SetActive(true);
    }
    public CalculationUtils.DamageCalculaton CalcDamage(AttackScrObj attackingAttack, WoogieScrObj attackingWoogieScrObj, WoogieSave attackingWoogieSave, NatureScrObj attackingNature, WoogieScrObj defendingWoogieScrObj, WoogieSave defendingWoogieSave, NatureScrObj defendingNature)
    {
        int attack;
        if (attackingAttack.category == AttackCategory.physical)
            attack = CalculationUtils.StatCaclulation(attackingWoogieScrObj.baseStats.att, attackingWoogieSave.individualStats.att, attackingWoogieSave.EffortStats.att, attackingWoogieSave.currentLevel, attackingNature.attack);
        else
            attack = CalculationUtils.StatCaclulation(attackingWoogieScrObj.baseStats.s_att, attackingWoogieSave.individualStats.s_att, attackingWoogieSave.EffortStats.s_att, attackingWoogieSave.currentLevel, attackingNature.sp_att);

        int defence;
        if (attackingAttack.category == AttackCategory.physical)
            defence = CalculationUtils.StatCaclulation(defendingWoogieScrObj.baseStats.def, defendingWoogieSave.individualStats.def, defendingWoogieSave.EffortStats.def, defendingWoogieSave.currentLevel, defendingNature.defence);
        else
            defence = CalculationUtils.StatCaclulation(defendingWoogieScrObj.baseStats.s_def, defendingWoogieSave.individualStats.s_def, defendingWoogieSave.EffortStats.s_def, defendingWoogieSave.currentLevel, defendingNature.sp_def);

         return CalculationUtils.DamageCalcualtion(attackingWoogieSave.currentLevel, attackingAttack.damage, attack, defence, CalculationUtils.Stab(attackingWoogieScrObj, attackingAttack), CalculationUtils.Type(attackingAttack.type, defendingWoogieScrObj.typing), 1, 1);
    }
}
[System.Serializable]
public class WoogieAttackUses
{
    public WoogieAttackUses(WoogieSave woogieSave)
    {
        woogieSecretId = woogieSave.secretId;
        for (int i = 0; i < woogieSave.selectedAttacksScrObjNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(woogieSave.selectedAttacksScrObjNames[i]))
            {
                AttackScrObj attackScrObj = Resources.Load<AttackScrObj>("Attacks/" + woogieSave.selectedAttacksScrObjNames[i]);
                attacksLeft[i] = attackScrObj.uses;
            }
        }
    }

    public string woogieSecretId;
    public int[] attacksLeft = new int[4];
}
[System.Serializable]
public class WoogieTeamMember
{
    public WoogieSave woogieSave;
    public WoogieScrObj woogieScrObj;
    public int healthLeft;
    public WoogieAttackUses woogieAttackUses;
}
[System.Serializable]
public class SetMove
{
    public void SetAttackNull()
    {
        woogieAttack = null;
    }
    public AttackScrObj woogieAttack;
    public int woogieSwitch;
}

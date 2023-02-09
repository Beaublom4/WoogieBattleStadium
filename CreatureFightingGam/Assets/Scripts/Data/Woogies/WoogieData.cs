[System.Serializable]
public struct WoogieData
{
    //Overall Info
    public string secretId;
    public WoogieSave woogie;
    //Personal Info
    public AttackScrObj[] woogieAttacks;
    public int xp;
    public NatureScrObj nature;
    //Stats
    public int health;
    public int attack;
    public int defence;
    public int sp_attack;
    public int sp_defence;
    public int speed;
}

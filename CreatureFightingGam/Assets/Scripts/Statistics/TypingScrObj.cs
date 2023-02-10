using UnityEngine;

[CreateAssetMenu(menuName = "Woogies/Typing")]
public class TypingScrObj : ScriptableObject
{
    public string typeName;
    public Color typeColor;

    public TypingScrObj[] superEffective;
    public TypingScrObj[] notVeryEffective;
    public TypingScrObj[] notEffective;
}

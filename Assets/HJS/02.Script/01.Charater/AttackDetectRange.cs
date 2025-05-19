using System.ComponentModel; 
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// ĳ���͵��� ���� ���� ����
/// </summary>
public class AttackDetectRange : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    public Character myCharacter;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Object") && myCharacter is Monster)
        {
            Monster monster = myCharacter as Monster;
            monster.attackAction(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
    }
}

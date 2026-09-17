using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public enum ZoneType { Detection, Attack }
    public ZoneType zoneType;
    public MonsterAI monsterAI;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (zoneType == ZoneType.Detection)
            monsterAI.OnPlayerEnterDetection(other);
        else
            monsterAI.OnPlayerEnterAttackRange(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (zoneType == ZoneType.Detection)
            monsterAI.OnPlayerExitDetection(other);
        else
            monsterAI.OnPlayerExitAttackRange(other);
    }
}
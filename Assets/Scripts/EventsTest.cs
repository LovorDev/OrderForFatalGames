using BuffsScript;
using UnityEngine;

public class EventsTest : MonoBehaviour
{
    public void OnCharacterIdle()
    {
        print("Character idle");
    }

    public void OnCharacterRun()
    {
        print("Character run");
    }

    public void OnCharacterGetDamage()
    {
        print("Character get damage");
    }

    public void OnCharacterPickSupplies(float value)
    {
        print("Character pick supplies: " + value);
    }

    public void OnCharacterPlaceSupplies()
    {
        print("Character place supplies");
    }

    public void OnCharacterPickBuff(Buff buff)
    {
        print("Character pick buff: "+buff.BuffType.ToString());
    }

    public void OnHomeGetSupplies()
    {
        print("Home get supplies");
    }

    public void OnHomeGetDamage()
    {
        print("Home get damage");
    }
}
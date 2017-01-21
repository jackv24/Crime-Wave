using UnityEngine;
using System.Collections;

public class HideRegion : MonoBehaviour
{
    public CharacterStats stats;

    void OnTriggerEnter2D(Collider2D col)
    {
        stats = col.GetComponent<CharacterStats>();

        if(stats)
        {
            stats.inHideRegion = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (stats)
        {
            stats.inHideRegion = false;


            stats = null;
        }
    }
}

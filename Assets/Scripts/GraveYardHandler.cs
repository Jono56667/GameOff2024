using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveYardHandler : MonoBehaviour
{
    public List<GameObject> GraveStonesSpawns;
    public List<GameObject> GraveStones;
    // Start is called before the first frame update
    void Start()
    {
            for (int i = 0; i < GraveStones.Count; i++)
            {
            int SpawnedStoneInt = Random.Range(0, GraveStones.Count);
                GameObject SpawnedStone = GraveStones[SpawnedStoneInt];
                
                // Instantiate a gravestone at the spawn point's position and rotation
                Instantiate(SpawnedStone, GraveStonesSpawns[i].transform.position, GraveStonesSpawns[i].transform.rotation);

                // Remove the spawn point from the list after using it
                GraveStonesSpawns.RemoveAt(i);

                // If you remove an item from the list, it shifts the indices, so decrement i to compensate
                i--;
            }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheResetter : MonoBehaviour
{
    List<GhostEnemy> ghosts = new List<GhostEnemy>();
    List<LockedDoor> doors = new List<LockedDoor>();
    List<BreakableWall> walls = new List<BreakableWall>();
    List<BreakablePlatform> platforms = new List<BreakablePlatform>();
    List<DungeonPlayerController> dpcs = new List<DungeonPlayerController>();

    private void Start()
    {
        ghosts.AddRange(FindObjectsOfType<GhostEnemy>(true));
        doors.AddRange(FindObjectsOfType<LockedDoor>(true));
        walls.AddRange(FindObjectsOfType<BreakableWall>(true));
        dpcs.AddRange(FindObjectsOfType<DungeonPlayerController>(true));
        platforms.AddRange(FindObjectsOfType<BreakablePlatform>(true));
        Reset();
    }

    public void Reset()
    {
        foreach (GhostEnemy ghost in ghosts)
        {
            ghost.Reset();
        }

        foreach(LockedDoor door in doors)
        {
            door.Reset();
        }

        foreach(BreakableWall wall in walls)
        {
            wall.Reset();
        }

        foreach(BreakablePlatform platform in platforms)
        {
            platform.Reset();
        }

        foreach(DungeonPlayerController dpc in dpcs)
        {
            dpc.Reset();
        }

        foreach(KeyPickup key in FindObjectsOfType<KeyPickup>())
        {
            Destroy(key.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{   void Start()
    {
        EntityDataLoader.LoadEntityStatus();
        CardDataLoader.LoadCards();
    }
}

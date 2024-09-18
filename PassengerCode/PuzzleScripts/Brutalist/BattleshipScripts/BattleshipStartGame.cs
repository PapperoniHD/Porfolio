using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipStartGame : MonoBehaviour, IDialogueEndEvent
{
    private BattleshipsGame battleShipsStart;

    public void Event()
    {
        if (this.enabled)
        {
            battleShipsStart.StartBattleship();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        battleShipsStart = FindObjectOfType<BattleshipsGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

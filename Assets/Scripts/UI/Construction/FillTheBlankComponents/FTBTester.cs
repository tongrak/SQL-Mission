using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI.Construction
{
    public class FTBTester : GameplayController
    {
        [SerializeField] private GameObject _FTBGO;
        public void runFTBTest()
        {
            IConstructionConsole currCon = mustGetComponent<IConstructionConsole>(_FTBGO);
            Debug.Log("Current input: " +  currCon.queryString);
        }
    }
}



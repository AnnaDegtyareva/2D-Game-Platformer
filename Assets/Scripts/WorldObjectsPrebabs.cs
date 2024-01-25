using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "WorldObjectsPrebabs", menuName = "Game/New WorldObjectsPrebabs")]
public class WorldObjectsPrebabs : ScriptableObject
{
    [System.Serializable]
    public class platforms
    {
        [System.Serializable]
        public class type
        {
            [System.Serializable]
            public class item
            {
                public GameObject Prefab;
                public int weight = 1;
                public string FileWay;
            }
            public item[] size3x2;
            public item[] size5x2;
            public item[] size5x5;
            public item[] size7x2;
            public item[] size7x5;
            public item[] size9x3;
        }
        public type[] Types;
    }
    [System.Serializable]
    public class props
    {
        [System.Serializable]
        public class type
        {

        }
        public type[] Types;
    }

    public platforms Platforms;
    public props Props;
}
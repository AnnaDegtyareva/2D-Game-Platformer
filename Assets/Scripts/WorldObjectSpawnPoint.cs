using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WorldObjectSpawnPoint : MonoBehaviour
{
    [System.Serializable]
    public enum size {size3x2, size5x2, size5x5, size7x2, size7x5, size9x3 }
    public size[] Size;
    void Start()
    {
        List<WorldObjectsPrebabs.platforms.type.item> Platforms = new();
        for (int i = 0; i < Size.Length; i++)
        {
            switch (Size[i])
            {
                case size.size3x2:
                    Platforms.AddRange(GameManager.instance.WorldPrefabs.Platforms.Types[0].size3x2);
                    break;
                case size.size5x2:
                    Platforms.AddRange(GameManager.instance.WorldPrefabs.Platforms.Types[0].size5x2);
                    break;
                case size.size5x5:
                    Platforms.AddRange(GameManager.instance.WorldPrefabs.Platforms.Types[0].size5x5);
                    break;
                case size.size7x2:
                    Platforms.AddRange(GameManager.instance.WorldPrefabs.Platforms.Types[0].size7x2);
                    break;
                case size.size7x5:
                    Platforms.AddRange(GameManager.instance.WorldPrefabs.Platforms.Types[0].size7x5);
                    break;
                case size.size9x3:
                    Platforms.AddRange(GameManager.instance.WorldPrefabs.Platforms.Types[0].size9x3);
                    break;
            }
        }
        int[] AllWeight = new int[Platforms.Count];
        AllWeight[0] = Platforms[0].weight;
        for (int i = 1; i < Platforms.Count; i++)
        {
            AllWeight[i] = AllWeight[i-1] + Platforms[i].weight;
        }
        int Weight = Random.Range(0, AllWeight[AllWeight.Length - 1]);
        GameObject Platform = Platforms[Platforms.Count - 1].Prefab;
        string FileWay = Platforms[Platforms.Count - 1].FileWay;
        for (int i = 0; i < AllWeight.Length - 1; i++)
        {
            if (Weight < AllWeight[i])
            {
                Platform = Platforms[i].Prefab;
                FileWay = Platforms[i].FileWay;
                break;
            }
        }
        PhotonNetwork.Instantiate(FileWay + Platform.name, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
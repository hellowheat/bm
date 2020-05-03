using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMap : MonoBehaviour
{
    public Vector2Int[] MapTrack;
    Vector3[] allPositions;
    List<List<int>> map;
    void Awake()
    {
        allPositions = new Vector3[transform.childCount];
        map = new List<List<int>>();
        for(int i = 0; i < transform.childCount; i++)
        {
            map.Add(new List<int>());
            allPositions[i] = transform.GetChild(i).position;
        }
        foreach(Vector2Int track in MapTrack)
        {
            map[track.x].Add(track.y);
            map[track.y].Add(track.x);
        }
        

    }

    public int getFequence(Vector3 position)
    {
        for (int i = 0; i < allPositions.Length; i++) 
        {
            if (allPositions[i]== position) return i;
        }
        return -1;
    }

    public Vector3 getPosition(int index)
    {
        return allPositions[index];
    }

    public List<int> getNear(int index)
    {
        return map[index];
    }

    void Update()
    {
        
    }
}

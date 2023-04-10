using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    private Cell[,] grid;
    public int nCell;
    public float cellSize;
    private float[,] noiseMap;
    public GameObject nebula;
    public GameObject environment;
    public int nebulaDensity;
    public float maxNebulaScale;
    public Color[] nebulaColors;

    void Start() {
        noiseMap = new float[nCell, nCell];
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int i = 0; i < nCell; i++) {
            for (int j = 0; j < nCell; j++) {
                float noisevalue = Mathf.PerlinNoise(j * .1f + xOffset, i * .1f + yOffset);
                noiseMap[j, i] = noisevalue;
            }
        }

        grid = new Cell[nCell, nCell];
        for(int i = 0; i < nCell; i++) {
            for(int j = 0; j < nCell; j++) {
                Cell cell = new Cell();
                grid[j, i] = cell;
            }
        }

        for (int i = 0; i < nCell; i++) {
            for (int j = 0; j < nCell; j++) {
                if (noiseMap[j, i] > .8f && !grid[j, i].isNebula) {
                    ParticleSystem.MainModule ps = nebula.GetComponent<ParticleSystem>().main;
                    ps.startColor = nebulaColors[Random.Range(0, nebulaColors.Length)];

                    GameObject newNebula = Instantiate(nebula, new Vector3(cellSize * i, cellSize * j, 0f), Quaternion.identity) as GameObject;
                    newNebula.transform.localScale = new Vector3(Random.Range(10f / nebulaDensity, maxNebulaScale), Random.Range(10f / nebulaDensity, maxNebulaScale), 0f);
                    newNebula.transform.parent = environment.transform;

                    setNeighborIsNebula(j, i);
                }
            }
        }
    }

    private void setNeighborIsNebula(int x, int y) {
        int d = 100 / nebulaDensity;
        for(int i = y - d; i < y + d; i++) {
            for (int j = x - d; j < x + d; j++) {
                if(i >= 0 && j >= 0 && i < nCell && j < nCell) {
                    grid[j, i].isNebula = true;
                }
            }
        }
    }

    //private void OnDrawGizmos() {
    //    if (!Application.isPlaying) {
    //        return;
    //    }

    //    for (int i = 0; i < size; i++) {
    //        for (int j = 0; j < size; j++) {
    //            //Debug.Log(noiseMap[j, i]);
    //            if (noiseMap[j, i] > .75f) {
    //                Gizmos.color = Color.black;
    //            } else if (noiseMap[j, i] < .25f) {
    //                Gizmos.color = Color.white;
    //            } else {
    //                Gizmos.color = Color.grey;
    //            }

    //            Vector3 pos = new Vector3(scale * i, scale * j, 0);
    //            Vector3 scaleVector = new Vector3(scale, scale, 0);
    //            Gizmos.DrawCube(pos, scaleVector);
    //        }
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataWorldSpawnPoints : MonoBehaviour
{
    public float radius = 1, displayRadius = 1, NumberOfTreesSpawnOnStart = 30;
    public Vector2 regionSize = Vector2.one;
    public int rejectSamples = 30;
    public int Chunks;
    List<Vector2> points;
    public GameObject DataMonSpawnPointPrefab;
    //List<GameObject> Trees = new List<GameObject>();
    //List<GameObject> TreesActive = new List<GameObject>();
    //public static GenerateTrees Tree_Generator;
    //public GameObject[] GeneratedChunks;
    //GameObject TreesParent;
    // Start is called before the first frame update
    public static float SpawnPointsRadius;
    void Awake()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectSamples);
        SpawnPointsRadius = radius;
        RoamingSpawner.MonsInChunk.Clear();
        GameObject temp;
        for (int i = 0; i < points.Count; i++)
        {
            temp = Instantiate(DataMonSpawnPointPrefab, points[i], Quaternion.identity);
            temp.transform.SetParent(transform);
            temp.transform.localScale = transform.InverseTransformVector(Vector2.one * SpawnPointsRadius);
            RoamingSpawner.MonsInChunk.Add(points[i], new DataMonsInChunk(0));

        }
        //GeneratedChunks = new GameObject[(int)Mathf.Pow(Chunks, 2)];
        //int index = 0;
        //for (int y = 315; y < 560; y += 70)
        //{
        //    for (int x = 315; x < 560; x += 70)
        //    {

        //GameObject newObject = new GameObject();

        //GameObject parentChunk = new GameObject();
        //parentChunk.transform.position = regionSize / 2;
        //newObject.transform.position = regionSize / 2;
        //parentChunk.transform.SetParent(transform);
        //newObject.transform.SetParent(parentChunk.transform);
        //BoxCollider2D bc2D = parentChunk.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        //bc2D.size = regionSize;
        //bc2D.isTrigger = true;
        //GeneratedChunks[index] = parentChunk;
        //GeneratedChunks[index].tag = "ChunkTag";
        //GeneratedChunks[index].name = "Chunk_" + index;
        //GameObject treesInChunks = new GameObject();
        //treesInChunks.name = "Trees In Chunks";
        //treesInChunks.transform.SetParent(GeneratedChunks[index].transform);
        //treesInChunks.transform.SetSiblingIndex(1);
        //points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectSamples);
        //foreach (Vector2 point in points)
        //{
        //    Instantiate(TreePointPrefab, point, Quaternion.identity, newObject.transform);
        //}
        //GeneratedChunks[index].transform.position = new Vector2(x, y);
        //        //index++;
        //    }
        //}

        //GameObject tempTreesParent = new GameObject();
        //TreesParent = tempTreesParent;
        //TreesParent.name = "Trees";
        //TreesParent.transform.SetParent(transform);
        //for (int i = 0; i <= NumberOfTreesSpawnOnStart; i++)
        //{
        //    GameObject TreeInstance = Instantiate(TreePrefab, transform.position, Quaternion.identity, TreesParent.transform);
        //    Trees.Add(TreeInstance);
        //    Trees[i].SetActive(false);
        //}
        ////Tree_Generator = this;
        //foreach (GameObject child in GeneratedChunks)
        //{
        //    child.transform.GetChild(0).gameObject.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
    //public void SpawnTree(Vector3 position)
    //{
    //    GameObject tree = Instantiate(TreePrefab, position, Quaternion.identity);
    //    Transform Leafbig = tree.transform.Find("Leafbig");
    //    Leafbig.Rotate(new Vector3(0, 0, Random.Range(0,361)));
    //    Transform LeafSmall = tree.transform.Find("LeafSmall");
    //    LeafSmall.Rotate(new Vector3(0, 0, Random.Range(0, 361)));
    //    float randomScale = Random.Range(0.4f, 1);
    //    tree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    //}
    private void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectSamples);
    }
    private void OnDrawGizmos()
    {
        if (points != null)
        {
            Gizmos.DrawWireCube(regionSize / 2, regionSize);
            foreach (Vector2 point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
    //public void PlaceTreeOnPoint(Transform Point)
    //{
    //    if (Trees.Count > 0)
    //    {
    //        Trees[0].transform.position = Point.position;
    //        Trees[0].SetActive(true);
    //        Trees[0].transform.SetParent(Point.parent.parent.GetChild(1));
    //        TreesActive.Add(Trees[0]);
    //        Trees.RemoveAt(0);
    //    }



    //}
    //public void RemoveAtPoint(GameObject treeInstance)
    //{
    //    int index = TreesActive.IndexOf(treeInstance);
    //    TreesActive[index].SetActive(false);
    //    Trees.Add(TreesActive[index]);
    //    TreesActive.RemoveAt(index);
    //    treeInstance.transform.SetParent(TreesParent.transform);
    //}
}

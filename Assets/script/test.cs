using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int beginX = -3;
    public int beginY = 5;

    public float offsetX;
    public float offsetY;

    public int mapWidth = 5;
    public int mapHeight = 5;

    private Vector2 endPos;

    private AstarNode[,] nodes;
    public List<AstarNode> openlist;
    private List<AstarNode> closelist = new List<AstarNode>();
    private Vector2 beginPos = Vector2.right * -1;

    private Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();

    public Material red;
    public Material yellow;
    public Material green;
    public Material purper;
    public Material normal;
    public Material gress;
    public Material sand;

    private List<AstarNode> list;
    private bool findOk = false;
    public float flashTime;
    public float reFreshTime;
    public float reFreshTimer;
    private float timer;
    public int counter = 1;

    [System.Obsolete]
    private void deleteCube()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                DestroyObject(cubes[i + "_" + j].gameObject);
            }
        }
        cubes.Clear();
    }


    [System.Obsolete]
    private void reFresh(int w, int h)
    {
        deleteCube();
        
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                nodes[i, j].type = NodeType.undefined;
            }
        }


        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (nodes[i, j].type == NodeType.undefined)
                {
                    nodes[i, j].type = Random.Range(1, 100) > 20 ? NodeType.normal : NodeType.barrier;
                    if (nodes[i, j].type == NodeType.normal && Random.Range(1, 100) < 20)
                    {
                        if (Random.Range(1, 100) > 30)
                        {
                            int wid = i + Random.Range(0, 3);
                            if (wid >= mapWidth) wid = mapWidth - 1;
                            int height = j + Random.Range(0, 3);
                            if (height >= mapHeight) height = mapHeight - 1;
                            for (int x = i; x <= wid; x++)
                            {
                                for (int y = j; y <= height; y++)
                                {
                                    nodes[x, y].type = NodeType.gress;
                                }
                            }
                        }
                        else
                        {
                            int wid = i + Random.Range(0, 5);
                            if (wid >= mapWidth) wid = mapWidth - 1;
                            int height = j + Random.Range(0, 5);
                            if (height >= mapHeight) height = mapHeight - 1;
                            for (int x = i; x <= wid; x++)
                            {
                                for (int y = j; y <= height; y++)
                                {
                                    nodes[x, y].type = NodeType.sand;
                                }
                            }
                        }
                    }
                }
            }
        }


        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 0);
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);
                obj.GetComponent<MeshRenderer>().material = normal;
                if (nodes[i, j].type == NodeType.barrier)
                    obj.GetComponent<MeshRenderer>().material = red;
                else if (nodes[i, j].type == NodeType.normal)
                    obj.GetComponent<MeshRenderer>().material = normal;
                else if (nodes[i, j].type == NodeType.sand)
                    obj.GetComponent<MeshRenderer>().material = sand;
                else if (nodes[i, j].type == NodeType.gress)
                    obj.GetComponent<MeshRenderer>().material = gress;
            }
        }


        if (nodes[(int)beginPos.x, (int)beginPos.y].type != NodeType.barrier && nodes[(int)endPos.x, (int)endPos.y].type != NodeType.barrier)
        {

                counter = 1;
                list = FindPath(beginPos, endPos);
                if (list != null)
                {
                    for (int i = 1; i < list.Count - 1; i++)
                    {
                        cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = purper;
                    }
                    findOk = true;
                }
            

            cubes[endPos.x + "_" + endPos.y].GetComponent<MeshRenderer>().material = green;
            cubes[beginPos.x + "_" + beginPos.y].GetComponent<MeshRenderer>().material = yellow;
        }
        else if(nodes[(int)beginPos.x, (int)beginPos.y].type == NodeType.barrier)
        {
            findOk = false;
            beginPos = Vector2.right * -1;
        }
        else
        {
            findOk = false;
            cubes[beginPos.x + "_" + beginPos.y].GetComponent<MeshRenderer>().material = yellow;
        }
    }
    



    public void MapInfo(int w, int h)
    {

        nodes = new AstarNode[w, h];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                AstarNode node = new AstarNode(i, j, NodeType.undefined);
                nodes[i, j] = node;
            }
        }

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (nodes[i, j].type == NodeType.undefined)
                {
                    nodes[i, j].type = Random.Range(1, 100) > 20 ? NodeType.normal : NodeType.barrier;
                    if (nodes[i, j].type == NodeType.normal && Random.Range(1, 100) < 20)
                    {
                        if (Random.Range(1, 100) > 50)
                        {
                            int wid = i + Random.Range(0, 3);
                            if (wid >= mapWidth) wid = mapWidth - 1;
                            int height = j + Random.Range(0, 3);
                            if (height >= mapHeight) height = mapHeight - 1;
                            for (int x = i; x <= wid; x++)
                            {
                                for (int y = j; y <= height; y++)
                                {
                                    nodes[x, y].type = NodeType.gress;
                                }
                            }
                        }
                        else
                        {
                            int wid = i + Random.Range(0, 5);
                            if (wid >= mapWidth) wid = mapWidth - 1;
                            int height = j + Random.Range(0, 5);
                            if (height >= mapHeight) height = mapHeight - 1;
                            for (int x = i; x <= wid; x++)
                            {
                                for (int y = j; y <= height; y++)
                                {
                                    nodes[x, y].type = NodeType.sand;
                                }
                            }
                        }
                    }
                }
            }

        }

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 0);
                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);
                obj.GetComponent<MeshRenderer>().material = normal;
                if (nodes[i, j].type == NodeType.barrier)
                    obj.GetComponent<MeshRenderer>().material = red;
                else if (nodes[i, j].type == NodeType.normal)
                    obj.GetComponent<MeshRenderer>().material = normal;
                else if (nodes[i, j].type == NodeType.sand)
                    obj.GetComponent<MeshRenderer>().material = sand;
                else if (nodes[i, j].type == NodeType.gress)
                    obj.GetComponent<MeshRenderer>().material = gress;
            }
        }
    }

    public List<AstarNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        if (startPos.x >= mapWidth || startPos.x < 0 || endPos.x >= mapWidth || endPos.x < 0 || endPos.y < 0 || endPos.y >= mapHeight || startPos.y < 0 || startPos.y >= mapHeight)
            return null;

        AstarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AstarNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == NodeType.barrier || end.type == NodeType.barrier)
        {
            return null;
        }

        closelist.Clear();
        openlist.Clear();

        start.father = null;
        start.f = start.g = start.h = 0;

        closelist.Add(start);
        while (true)
        {
            NearNode(start.x - 1, start.y - 1, 1.4f, start, end);
            NearNode(start.x, start.y - 1, 1, start, end);
            NearNode(start.x + 1, start.y - 1, 1.4f, start, end);
            NearNode(start.x - 1, start.y, 1, start, end);
            NearNode(start.x - 1, start.y + 1, 1.4f, start, end);
            NearNode(start.x, start.y + 1, 1, start, end);
            NearNode(start.x + 1, start.y, 1, start, end);
            NearNode(start.x + 1, start.y + 1, 1.4f, start, end);


            if (openlist.Count == 0)
            {
                return null;
            }
            if(openlist.Count>1)
            openlist.Sort(SortOpenList);

            closelist.Add(openlist[0]);
            start = openlist[0];
            openlist.RemoveAt(0);
            if (start == end)
            {
                List<AstarNode> path = new List<AstarNode>();
                path.Add(end);
                while (end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                path.Reverse();
                return path;
            }
        }
    }

    private int SortOpenList(AstarNode a, AstarNode b)
    {
        if (a.f >= b.f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    private void NearNode(int x, int y, float g, AstarNode father, AstarNode end)
    {
        if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight || nodes[x, y].type == NodeType.barrier || openlist.Contains(nodes[x, y]) || closelist.Contains(nodes[x, y]) || nodes[x, y] == null)
      return;

        nodes[x, y].g = father.g + g;
        nodes[x, y].h = Mathf.Abs(end.x - nodes[x, y].x) + Mathf.Abs(end.y - nodes[x, y].y);
        nodes[x, y].f = nodes[x, y].h + nodes[x, y].g;
        nodes[x, y].father = father;

        if (nodes[x, y].type == NodeType.gress)
            nodes[x, y].f += 1;
        else if (nodes[x, y].type == NodeType.sand)
            nodes[x, y].f += 2;

        openlist.Add(nodes[x, y]);
    }

    private void clear()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                cubes[i+ "_" + j].GetComponent<MeshRenderer>().material =normal;
                if (nodes[i, j].type == NodeType.barrier)
                    cubes[i+ "_" +j].GetComponent<MeshRenderer>().material = red;
                else if (nodes[i, j].type == NodeType.normal)
                    cubes[i + "_" + j].GetComponent<MeshRenderer>().material = normal;
                else if (nodes[i, j].type == NodeType.sand)
                    cubes[i + "_" + j].GetComponent<MeshRenderer>().material = sand;
                else if (nodes[i, j].type == NodeType.gress)
                    cubes[i + "_" + j].GetComponent<MeshRenderer>().material = gress;
            }
        }
        cubes[beginPos.x + "_" + beginPos.y].GetComponent<MeshRenderer>().material = yellow;
    }
    // Start is called before the first frame update
    void Start()
    {
        reFreshTimer = reFreshTime;
        openlist = new List<AstarNode>();
        MapInfo(mapWidth,mapHeight);
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (beginPos != Vector2.right * -1) {
            if (reFreshTimer <= 0)
            {
                reFresh(mapWidth, mapHeight);
                reFreshTimer = reFreshTime;
            }
            else
            {
                reFreshTimer -= Time.deltaTime;
            } 
        }

        if (findOk)
        {
            if (timer <= 0)
                {
                cubes[list[counter].x + "_" + list[counter].y].GetComponent<MeshRenderer>().material = yellow;
                if (nodes[(int)list[counter-1].x,(int)list[counter-1].y].type==NodeType.normal)
                {
                    cubes[list[counter - 1].x + "_" + list[counter - 1].y].GetComponent<MeshRenderer>().material = normal;
                }else if (nodes[(int)list[counter - 1].x, (int)list[counter - 1].y].type == NodeType.gress)
                {
                    cubes[list[counter - 1].x + "_" + list[counter - 1].y].GetComponent<MeshRenderer>().material = gress;
                }
                else if (nodes[(int)list[counter - 1].x, (int)list[counter - 1].y].type == NodeType.sand)
                {
                    cubes[list[counter - 1].x + "_" + list[counter - 1].y].GetComponent<MeshRenderer>().material = sand;
                }
                beginPos = new Vector2(list[counter].x, list[counter].y);
                    timer = flashTime;
                counter++;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
                if (counter == list.Count)
                {
                counter = 1;
                findOk = false;
            }
            
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out info,1000))
            {
                if (beginPos== Vector2.right*-1)
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    if (nodes[int.Parse(strs[0]), int.Parse(strs[1])].type != NodeType.barrier)
                    {
                        findOk = false;
                        beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                        info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                    }
                }
                else
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    if (nodes[int.Parse(strs[0]), int.Parse(strs[1])].type!= NodeType.barrier)
                    {
                        cubes[endPos.x + "_" + endPos.y].GetComponent<MeshRenderer>().material = normal;
                        endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                        findOk = false;
                        clear();
                        counter = 1;
                        info.collider.gameObject.GetComponent<MeshRenderer>().material = green;            
                        list=  FindPath(beginPos,endPos);
                        if (list != null)
                        {
                            for (int i=1;i<list.Count-1;i++)
                            {
                                cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = purper;
                            }
                            findOk = true;
                        }
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarManager
{
    private int mapWidth;
    private int mapHeight;
    private AstarNode[,] nodes;
    private List<AstarNode> openlist;
    private List<AstarNode> closelist;

    public void MapInfo(int w,int h)
    {
        this.mapWidth = w;
        this.mapHeight = h;

        nodes = new AstarNode[w,h];
        for(int i = 0; i < w;i++)
        {
            for(int j = 0; i < h; j++)
            {
                AstarNode node = new AstarNode(i,j,NodeType.normal);
                nodes[i, j] = node;
            }
        }
    }

    public List<AstarNode> FindPath(Vector2 startPos,Vector3 endPos)
    {
        if(startPos.x>=mapWidth||startPos.x<0||endPos.x>=mapWidth||endPos.x<0||endPos.y<0||endPos.y>=mapHeight||startPos.y<0||startPos.y>=mapHeight)
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
            NearNode(start.x-1,start.y-1,1.4f,start,end);
            NearNode(start.x , start.y - 1, 1, start, end);
            NearNode(start.x + 1, start.y - 1, 1.4f, start, end);
            NearNode(start.x - 1, start.y , 1, start, end);
            NearNode(start.x - 1, start.y + 1, 1.4f, start, end);
            NearNode(start.x , start.y + 1, 1, start, end);
            NearNode(start.x + 1, start.y , 1, start, end);
            NearNode(start.x + 1, start.y + 1, 1.4f, start, end);

            if (openlist.Count == 0)
            {
                return null;
            }

                openlist.Sort(SortOpenList);
            closelist.Add(openlist[0]);
            start = openlist[0];
            openlist.RemoveAt(0);
            if (start == end)
            {
                List<AstarNode> path = new List<AstarNode>();
                path.Add(end);
                while (end.father!=null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                path.Reverse();
                return path;
            }   
            
        }
    }

    private int SortOpenList(AstarNode a,AstarNode b)
    {
        if (a.f > b.f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    private void NearNode(int x,int y,float g,AstarNode father,AstarNode end)
    {
                if (x < 0 || x >= mapWidth || y < 0||y>=mapHeight||nodes[x,y].type==NodeType.barrier||openlist.Contains(nodes[x,y])||closelist.Contains(nodes[x,y])||nodes[x,y]==null)
                
            return;

        nodes[x, y].g = father.g + g;
        nodes[x, y].h = Mathf.Abs(end.x - nodes[x, y].x)+Mathf.Abs(end.y-nodes[x,y].y);
        nodes[x, y].f = nodes[x, y].f + nodes[x, y].g;
                
            openlist.Add(nodes[x, y]);
    }
}

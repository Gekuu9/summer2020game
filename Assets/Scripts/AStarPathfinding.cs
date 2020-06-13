using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Packages.Rider.Editor;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class AStarPathfinding {
    private PathfindingGraph.PathfindingNode start;
    private PathfindingGraph.PathfindingNode end;

    private PathfindingGraph graph;

    public AStarPathfinding(PathfindingGraph graph) {
        this.graph = graph;
    }

    public APath FindPath(int startIndex, int endIndex) {
        start = graph.nodes[startIndex];
        end = graph.nodes[endIndex];

        List<APath> openList = new List<APath>();
        List<PathfindingGraph.PathfindingNode> closedList = new List<PathfindingGraph.PathfindingNode>();

        APath current = new APath();
        current.node = start;
        openList.Add(current);

        while (openList.Count > 0) {
            bool hasValue = false;
            foreach (APath n in openList) {
                if (hasValue) {
                    if (n.node.f < current.node.f) {
                        current = n;
                    }
                } else {
                    current = n;
                    hasValue = true;
                }
            }

            if (current.node == end) {
                APath prev = current.parent;
                int index = 0;
                while (prev.node != null && prev.node != start) {
                    prev.next = current;
                    current = prev;
                    prev = current.parent;
                    index++;
                    if (index > 1000) {
                        int k = 7;
                    }
                }

                prev.next = current;

                return prev;
            }

            openList.Remove(current);
            closedList.Add(current.node);

            foreach (PathfindingGraph.PathfindingNode child in current.node.outgoing) {
                float tentativeGScore = current.node.g + Vector3.Distance(child.location, current.node.location);

                if (closedList.Contains(child) && tentativeGScore >= child.g) {
                    // LevelRenderer.instance.blockObjects[(int)child.location.x + ((int)child.location.y - 1) * LevelRenderer.instance.blockGrid.xSize + (int)child.location.z * LevelRenderer.instance.blockGrid.xSize * LevelRenderer.instance.blockGrid.ySize].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    continue;
                }

                bool cont = false;
                foreach (APath path in openList) {
                    if (child == path.node && tentativeGScore >= child.g) {
                        cont = true;
                        // LevelRenderer.instance.blockObjects[(int)child.location.x + ((int)child.location.y - 1) * LevelRenderer.instance.blockGrid.xSize + (int)child.location.z * LevelRenderer.instance.blockGrid.xSize * LevelRenderer.instance.blockGrid.ySize].transform.Translate(0f, 0f, 0.1f);
                    }
                }
                if (cont) continue;

                APath childPath = new APath();
                childPath.parent = current;
                childPath.node = child;

                childPath.node.g = tentativeGScore;

                childPath.node.h =  Vector3.Distance(childPath.node.location, end.location); // Mathf.Pow(childPath.node.location.x - end.location.x, 2) + Mathf.Pow(childPath.node.location.y - end.location.y, 2) + Mathf.Pow(childPath.node.location.z - end.location.z, 2);
                childPath.node.f = childPath.node.g + childPath.node.h;

                // child.obj.GetComponent<BlockInfo>().fgh = new Vector3(child.f, child.g, child.h);
                // LevelRenderer.instance.blockObjects[(int)child.location.x + ((int)child.location.y - 1) * LevelRenderer.instance.blockGrid.xSize + (int)child.location.z * LevelRenderer.instance.blockGrid.xSize * LevelRenderer.instance.blockGrid.ySize].GetComponent<BlockInfo>().fgh = new Vector3(child.f, child.g, child.h);

                APath temp = new APath();
                foreach (APath path in openList) {
                    if (child == path.node) {
                        cont = true;
                        temp = path;
                    }
                }
                if (cont) openList.Remove(temp);

                openList.Add(childPath);
            }
        }

        return null;
    }

    public class APath {
        public PathfindingGraph.PathfindingNode node;
        public APath next;
        public APath parent;
    }
}

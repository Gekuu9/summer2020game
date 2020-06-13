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

            openList.Remove(current);
            closedList.Add(current.node);

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

            foreach (PathfindingGraph.PathfindingNode child in current.node.outgoing) {
                APath childPath = new APath();
                childPath.node = child;
                childPath.parent = current;

                if (closedList.Contains(child)) continue;

                child.g = current.node.g + Vector3.Distance(child.location, current.node.location);
                child.h = Mathf.Pow(child.location.x - end.location.x, 2) + Mathf.Pow(child.location.y - end.location.y, 2) + Mathf.Pow(child.location.z - end.location.z, 2);
                child.f = child.g + child.h;

                foreach (APath path in openList) {
                    if (child == path.node && child.g > path.node.g) continue;
                }

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

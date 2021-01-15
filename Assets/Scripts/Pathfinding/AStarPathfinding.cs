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

    public PathfindingGraph graph;

    public AStarPathfinding(PathfindingGraph graph) {
        this.graph = graph;
    }

    public APath FindPath(int startIndex, int endIndex, bool findClosest = false) {
        start = graph.nodes[startIndex];
        end = graph.nodes[endIndex];

        if (start == null || end == null) return null;

        if (start == end) return new APath();
        if (findClosest && end.adjacent.Contains(start)) return new APath();

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
                APath prev;
                while (current.parent.node != null && current.parent.node != start) {
                    prev = current.parent;
                    prev.next = current;
                    current = prev;
                }

                prev = current.parent;
                prev.next = current;

                return prev;
            }

            if (findClosest && end.adjacent.Contains(current.node)) {
                APath prev;
                while (current.parent.node != null && current.parent.node != start) {
                    prev = current.parent;
                    prev.next = current;
                    current = prev;
                }

                prev = current.parent;
                prev.next = current;

                return prev;
            }

            openList.Remove(current);
            closedList.Add(current.node);

            if (current == null || current.node == null || current.node.outgoing == null) {
                Debug.LogError("Pathfinding node found null");
            }

            foreach (PathfindingGraph.PathfindingNode child in current.node.outgoing) {
                float tentativeGScore = current.node.g + Vector3.Distance(child.location, current.node.location);

                if (closedList.Contains(child) && tentativeGScore >= child.g) {
                    continue;
                }

                bool cont = false;
                foreach (APath path in openList) {
                    if (child == path.node && tentativeGScore >= child.g) {
                        cont = true;
                    }
                }
                if (cont) continue;

                APath childPath = new APath();
                childPath.parent = current;
                childPath.node = child;

                childPath.node.g = tentativeGScore;

                childPath.node.h =  Vector3.Distance(childPath.node.location, end.location);
                childPath.node.f = childPath.node.g + childPath.node.h;

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

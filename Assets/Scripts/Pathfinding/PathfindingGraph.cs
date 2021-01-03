using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PathfindingGraph {

    public PathfindingNode[] nodes;

    private int xSize;
    private int ySize;
    private int zSize;

    public void InitializeGraph(LevelObject levelObject) {
        nodes = new PathfindingNode[levelObject.blockTypes.Length];

        xSize = levelObject.xSize;
        ySize = levelObject.ySize;
        zSize = levelObject.zSize;

        for (int i = 0; i < nodes.Length; i++) {
            PathfindingNode node = new PathfindingNode();

            node.location = new Vector3Int(i % xSize, i / xSize % ySize, i / xSize / ySize);
            if (levelObject.blocks[i] == null) continue;
            node.block = levelObject.blocks[i];
            node.incoming = new List<PathfindingNode>();
            node.outgoing = new List<PathfindingNode>();
            node.adjacent = new List<PathfindingNode>();

            nodes[i] = node;
        }

        for (int i = 0; i < nodes.Length; i++) {
            if (nodes[i] == null) continue;
            PathfindingNode node = nodes[i];
            InitializeNode(node, i);
        }
    }

    public void InitializeNode(PathfindingNode node, int i) {
        if (node == null || node.block == null) return;

        if (node.block.pathfindingRules == null && node.block.defaultPathRules != null) {
            node.block.pathfindingRules = node.block.defaultPathRules;
        }

        PathRules nodeRules = node.block.pathfindingRules;

        for (int d = 0; d <= 26; d++) {
            Block.Direction d1 = (Block.Direction)d;
            Block.Direction d2 = Block.Opposite(d1);
            if (d1 == Block.Direction.None || d2 == Block.Direction.None) continue;

            int dx = d % 3 - 1;
            int dy = d % 9 / 3 - 1;
            int dz = d / 9 - 1;

            Vector3Int otherLoc = node.location;
            otherLoc.x += dx;
            otherLoc.y += dy;
            otherLoc.z += dz;

            if (otherLoc.x < 0 || otherLoc.y < 0 || otherLoc.z < 0 || otherLoc.x >= xSize || otherLoc.y >= ySize || otherLoc.z >= zSize) continue;
            PathfindingNode other = nodes[otherLoc.x + otherLoc.y * xSize + otherLoc.z * xSize * ySize];

            if (other == null || other.block == null) continue;

            if (other.block.pathfindingRules == null && other.block.defaultPathRules != null) {
                other.block.pathfindingRules = other.block.defaultPathRules;
            }
            PathRules otherRules = other.block.pathfindingRules;

            if (otherRules == null) Debug.LogError("Object " + other.block.name + " has no pathfinding rules");

            node.adjacent.Add(other);

            if (nodeRules.inRules.Contains(d1) && otherRules.outRules.Contains(d2)) {
                if (d1 == Block.Direction.NorthEast) {
                    if (CheckPath(node, Block.Direction.North, true) && CheckPath(node, Block.Direction.East, true) && 
                        CheckPath(other, Block.Direction.South, false) && CheckPath(other, Block.Direction.West, false)) {
                        node.incoming.Add(other);
                    }
                }
                else if (d1 == Block.Direction.SouthEast) {
                    if (CheckPath(node, Block.Direction.South, true) && CheckPath(node, Block.Direction.East, true) &&
                        CheckPath(other, Block.Direction.North, false) && CheckPath(other, Block.Direction.West, false)) {
                        node.incoming.Add(other);
                    }
                }
                else if (d1 == Block.Direction.SouthWest) {
                    if (CheckPath(node, Block.Direction.South, true) && CheckPath(node, Block.Direction.West, true) &&
                        CheckPath(other, Block.Direction.North, false) && CheckPath(other, Block.Direction.East, false)) {
                        node.incoming.Add(other);
                    }
                }
                else if (d1 == Block.Direction.NorthWest) {
                    if (CheckPath(node, Block.Direction.North, true) && CheckPath(node, Block.Direction.West, true) &&
                        CheckPath(other, Block.Direction.South, false) && CheckPath(other, Block.Direction.East, false)) {
                        node.incoming.Add(other);
                    }
                }
                else {
                    node.incoming.Add(other);
                }
            }

            if (nodeRules.outRules.Contains(d1) && otherRules.inRules.Contains(d2)) {
                if (d1 == Block.Direction.NorthEast) {
                    if (CheckPath(node, Block.Direction.North, false) && CheckPath(node, Block.Direction.East, false) &&
                        CheckPath(other, Block.Direction.South, true) && CheckPath(other, Block.Direction.West, true)) {
                        node.outgoing.Add(other);
                    }
                }
                else if (d1 == Block.Direction.SouthEast) {
                    if (CheckPath(node, Block.Direction.South, false) && CheckPath(node, Block.Direction.East, false) &&
                        CheckPath(other, Block.Direction.North, true) && CheckPath(other, Block.Direction.West, true)) {
                        node.outgoing.Add(other);
                    }
                }
                else if (d1 == Block.Direction.SouthWest) {
                    if (CheckPath(node, Block.Direction.South, false) && CheckPath(node, Block.Direction.West, false) &&
                        CheckPath(other, Block.Direction.North, true) && CheckPath(other, Block.Direction.East, true)) {
                        node.outgoing.Add(other);
                    }
                }
                else if (d1 == Block.Direction.NorthWest) {
                    if (CheckPath(node, Block.Direction.North, false) && CheckPath(node, Block.Direction.West, false) &&
                        CheckPath(other, Block.Direction.South, true) && CheckPath(other, Block.Direction.East, true)) {
                        node.outgoing.Add(other);
                    }
                }
                else {
                    node.outgoing.Add(other);
                }
            }
        }
    }

    public bool CheckPath(PathfindingNode node, Block.Direction d, bool inDir) {
        int dx = (int)d % 3 - 1;
        int dy = (int)d % 9 / 3 - 1;
        int dz = (int)d / 9 - 1;

        Vector3Int otherLoc = node.location;
        otherLoc.x += dx;
        otherLoc.y += dy;
        otherLoc.z += dz;

        if (otherLoc.x < 0 || otherLoc.y < 0 || otherLoc.z < 0 || otherLoc.x >= xSize || otherLoc.y >= ySize || otherLoc.z >= zSize) return false;
        PathfindingNode other = nodes[otherLoc.x + otherLoc.y * xSize + otherLoc.z * xSize * ySize];
        if (other == null || other.block == null) return false;

        if (inDir) {
            if (node.block.pathfindingRules.inRules.Contains(d) && other.block.pathfindingRules.outRules.Contains(Block.Opposite(d))) {
                return true;
            }
        } else {
            if (node.block.pathfindingRules.outRules.Contains(d) && other.block.pathfindingRules.inRules.Contains(Block.Opposite(d))) {
                return true;
            }
        }

        return false;
    }

    public void UpdateNode(Vector3Int position) {
        int index = position.x + position.y * xSize + position.z * xSize * ySize;
        PathfindingNode node = nodes[index];
        if (node == null) {
            node = new PathfindingNode();
            node.location = position;
            node.incoming = new List<PathfindingNode>();
            node.outgoing = new List<PathfindingNode>();
            node.adjacent = new List<PathfindingNode>();
        }

        node.incoming.Clear();
        node.outgoing.Clear();
        node.adjacent.Clear();

        InitializeNode(node, index);
        CheckSurroundingNodes(node, position);
    }

    public void UpdateNode(Vector3Int position, Block newBlock) {
        int index = position.x + position.y * xSize + position.z * xSize * ySize;
        PathfindingNode node = nodes[index];

        if (newBlock == null) {
            nodes[index] = null;
        }

        if (node == null) {
            node = new PathfindingNode();
            node.location = position;
            node.incoming = new List<PathfindingNode>();
            node.outgoing = new List<PathfindingNode>();
            node.adjacent = new List<PathfindingNode>();
        }
        node.block = newBlock;

        node.incoming.Clear();
        node.outgoing.Clear();
        node.adjacent.Clear();

        InitializeNode(node, index);
        CheckSurroundingNodes(node, position);

        nodes[index] = node;
    }

    public void CheckSurroundingNodes(PathfindingNode node, Vector3Int position) {

        for (int d = 0; d < 26; d++) {
            Block.Direction d1 = (Block.Direction)d;
            Block.Direction d2 = Block.Opposite(d1);

            int dx = d % 3 - 1;
            int dy = d % 9 / 3 - 1;
            int dz = d / 9 - 1;

            Vector3Int otherLoc = position;
            otherLoc.x += dx;
            otherLoc.y += dy;
            otherLoc.z += dz;

            PathfindingNode other = nodes[otherLoc.x + otherLoc.y * xSize + otherLoc.z * xSize * ySize];
            if (other == null || other.block == null) continue;
            PathRules otherRules = other.block.pathfindingRules;

            other.incoming.Remove(node);
            other.outgoing.Remove(node);

            if (node == null || node.block == null) continue;
            PathRules nodeRules = node.block.pathfindingRules;

            if (nodeRules.inRules.Contains(d1) && otherRules.outRules.Contains(d2)) {
                if (d1 == Block.Direction.NorthEast) {
                    if (nodeRules.inRules.Contains(Block.Direction.North) && nodeRules.inRules.Contains(Block.Direction.East) &&
                        otherRules.outRules.Contains(Block.Direction.South) && otherRules.outRules.Contains(Block.Direction.West)) {
                        other.outgoing.Add(node);
                    }
                }
                else if (d1 == Block.Direction.SouthEast) {
                    if (nodeRules.inRules.Contains(Block.Direction.South) && nodeRules.inRules.Contains(Block.Direction.East) &&
                        otherRules.outRules.Contains(Block.Direction.North) && otherRules.outRules.Contains(Block.Direction.West)) {
                        other.outgoing.Add(node);
                    }
                }
                else if (d1 == Block.Direction.SouthWest) {
                    if (nodeRules.inRules.Contains(Block.Direction.South) && nodeRules.inRules.Contains(Block.Direction.West) &&
                        otherRules.outRules.Contains(Block.Direction.North) && otherRules.outRules.Contains(Block.Direction.East)) {
                        other.outgoing.Add(node);
                    }
                }
                else if (d1 == Block.Direction.NorthWest) {
                    if (nodeRules.inRules.Contains(Block.Direction.North) && nodeRules.inRules.Contains(Block.Direction.West) &&
                        otherRules.outRules.Contains(Block.Direction.South) && otherRules.outRules.Contains(Block.Direction.East)) {
                        other.outgoing.Add(node);
                    }
                }
                else {
                    other.outgoing.Add(node);
                }
            }

            if (nodeRules.outRules.Contains(d1) && otherRules.inRules.Contains(d2)) {
                if (d1 == Block.Direction.NorthEast) {
                    if (nodeRules.outRules.Contains(Block.Direction.North) && nodeRules.outRules.Contains(Block.Direction.East) &&
                        otherRules.inRules.Contains(Block.Direction.South) && otherRules.inRules.Contains(Block.Direction.West)) {
                        other.incoming.Add(node);
                    }
                }
                else if (d1 == Block.Direction.SouthEast) {
                    if (nodeRules.outRules.Contains(Block.Direction.South) && nodeRules.outRules.Contains(Block.Direction.East) &&
                        otherRules.inRules.Contains(Block.Direction.North) && otherRules.inRules.Contains(Block.Direction.West)) {
                        other.incoming.Add(node);
                    }
                }
                else if (d1 == Block.Direction.SouthWest) {
                    if (nodeRules.outRules.Contains(Block.Direction.South) && nodeRules.outRules.Contains(Block.Direction.West) &&
                        otherRules.inRules.Contains(Block.Direction.North) && otherRules.inRules.Contains(Block.Direction.East)) {
                        other.incoming.Add(node);
                    }
                }
                else if (d1 == Block.Direction.NorthWest) {
                    if (nodeRules.outRules.Contains(Block.Direction.North) && nodeRules.outRules.Contains(Block.Direction.West) &&
                        otherRules.inRules.Contains(Block.Direction.South) && otherRules.inRules.Contains(Block.Direction.East)) {
                        other.incoming.Add(node);
                    }
                }
                else {
                    other.incoming.Add(node);
                }
            }
        }
    }

    public void SwapNodes(Vector3Int location1, Vector3Int location2) {
        PathfindingNode temp1 = nodes[location1.x + location1.y * xSize + location1.z * xSize * ySize];
        PathfindingNode temp2 = nodes[location2.x + location2.y * xSize + location2.z * xSize * ySize];
        Vector3Int tempLoc = temp1.location;
        temp1.location = temp2.location;
        temp2.location = tempLoc;
        nodes[location1.x + location1.y * xSize + location1.z * xSize * ySize] = temp2;
        nodes[location2.x + location2.y * xSize + location2.z * xSize * ySize] = temp1;

        UpdateNode(location1);
        UpdateNode(location2);
    }

    public void MoveNode(Vector3Int location1, Vector3Int location2, Block block1, Block block2) {
        nodes[location1.x + location1.y * xSize + location1.z * xSize * ySize].location = nodes[location2.x + location2.y * xSize + location2.z * xSize * ySize].location;
        nodes[location2.x + location2.y * xSize + location2.z * xSize * ySize] = nodes[location1.x + location1.y * xSize + location1.z * xSize * ySize];
        UpdateNode(location1, block1);
        UpdateNode(location2, block2);
    }

    public void CreateNode(Vector3Int location) {
        PathfindingNode node = new PathfindingNode();

        node.location = location;
        if (LevelRenderer.instance.GetBlock(location) == null) return;
        node.block = LevelRenderer.instance.GetBlock(location);
        node.incoming = new List<PathfindingNode>();
        node.outgoing = new List<PathfindingNode>();
        node.adjacent = new List<PathfindingNode>();

        nodes[location.x + location.y * xSize + location.z * xSize * ySize] = node;

        UpdateNode(location);
    }

    public bool CheckMove(Vector3Int location1, Vector3Int location2) {
        return nodes[location1.x + location1.y * xSize + location1.z * xSize * ySize].outgoing.Contains(nodes[location2.x + location2.y * xSize + location2.z * xSize * ySize]);
    }

    public class PathfindingNode {
        public Vector3Int location;
        public List<PathfindingNode> incoming;
        public List<PathfindingNode> outgoing;
        public List<PathfindingNode> adjacent;
        public Block block;

        public float f = 0f;
        public float g = 0f;
        public float h = 0f;
    }
}

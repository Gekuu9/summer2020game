using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathfindingGraph {

    public PathfindingNode[] nodes;

    public void InitializeGraph() {
        nodes = new PathfindingNode[LevelRenderer.instance.blocks.Length];

        int xSize = LevelRenderer.instance.blockGrid.xSize;
        int ySize = LevelRenderer.instance.blockGrid.ySize;
        int zSize = LevelRenderer.instance.blockGrid.zSize;

        Dictionary<BlockGrid.BlockType, PathfindingRules.BlockRules> rulesDict = new Dictionary<BlockGrid.BlockType, PathfindingRules.BlockRules>();
        foreach (PathfindingRules.BlockRules rule in LevelRenderer.instance.pathfindingRules.rules) {
            rulesDict.Add(rule.blockType, rule);
        }

        for (int i = 0; i < nodes.Length; i++) {
            PathfindingNode node = new PathfindingNode();

            node.location = new Vector3(i % xSize, i / xSize % ySize, i / xSize / ySize);
            node.type = LevelRenderer.instance.blocks[i];
            node.obj = LevelRenderer.instance.blockObjects[i];
            node.incoming = new List<PathfindingNode>();
            node.outgoing = new List<PathfindingNode>();

            nodes[i] = node;
        }

        for (int i = 0; i < nodes.Length; i++) {
            PathfindingNode node = nodes[i];
            PathfindingNode other;
            PathfindingRules.BlockRules nodeRules = rulesDict[node.type];
            PathfindingRules.BlockRules otherRules;

            // East
            if ((i % xSize) + 1 < xSize) {
                other = nodes[i + 1];
                otherRules = rulesDict[other.type];
                if (nodeRules.eastIn && otherRules.westOut) node.incoming.Add(other);
                if (nodeRules.eastOut && otherRules.westIn) node.outgoing.Add(other);
            }
            // West
            if ((i % xSize) - 1 >= 0) {
                other = nodes[i - 1];
                otherRules = rulesDict[other.type];
                if (nodeRules.westIn && otherRules.eastOut) node.incoming.Add(other);
                if (nodeRules.westOut && otherRules.eastIn) node.outgoing.Add(other);
            }
            // Up
            if ((i / xSize % ySize) + 1 < ySize) {
                other = nodes[i + xSize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upIn && otherRules.downOut) node.incoming.Add(other);
                if (nodeRules.upOut && otherRules.downIn) node.outgoing.Add(other);
            }
            // Down
            if ((i / xSize % ySize) - 1 >= 0) {
                other = nodes[i - xSize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downIn && otherRules.upOut) node.incoming.Add(other);
                if (nodeRules.downOut && otherRules.upIn) node.outgoing.Add(other);
            }
            // North
            if ((i / xSize / ySize) + 1 < zSize) {
                other = nodes[i + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.northIn && otherRules.southOut) node.incoming.Add(other);
                if (nodeRules.northOut && otherRules.southIn) node.outgoing.Add(other);
            }
            // South
            if ((i / xSize / ySize) - 1 > 0) {
                other = nodes[i - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.southIn && otherRules.northOut) node.incoming.Add(other);
                if (nodeRules.southOut && otherRules.northIn) node.outgoing.Add(other);
            }
            // Up East
            if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize) {
                other = nodes[i + 1 + xSize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upEastIn && otherRules.downWestOut) node.incoming.Add(other);
                if (nodeRules.upEastOut && otherRules.downWestIn) node.outgoing.Add(other);
            }
            // Up West
            if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize) {
                other = nodes[i - 1 + xSize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upWestIn && otherRules.downEastOut) node.incoming.Add(other);
                if (nodeRules.upWestOut && otherRules.downEastIn) node.outgoing.Add(other);
            }
            // Down East
            if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0) {
                other = nodes[i + 1 - xSize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downEastIn && otherRules.upWestOut) node.incoming.Add(other);
                if (nodeRules.downEastOut && otherRules.upWestIn) node.outgoing.Add(other);
            }
            // Down West
            if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0) {
                other = nodes[i - 1 - xSize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downWestIn && otherRules.upEastOut) node.incoming.Add(other);
                if (nodeRules.downWestOut && otherRules.upEastIn) node.outgoing.Add(other);
            }
            // North East
            if ((i % xSize) + 1 < xSize && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i + 1 + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.northEastIn && otherRules.southWestOut) node.incoming.Add(other);
                if (nodeRules.northEastOut && otherRules.southWestIn) node.outgoing.Add(other);
            }
            // North West
            if ((i % xSize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i - 1 + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.northWestIn && otherRules.southEastOut) node.incoming.Add(other);
                if (nodeRules.northWestOut && otherRules.southEastIn) node.outgoing.Add(other);
            }
            // South East
            if ((i % xSize) + 1 < xSize && (i / xSize / ySize) - 1 >= 0) {
                other = nodes[i + 1 - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.southEastIn && otherRules.northWestOut) node.incoming.Add(other);
                if (nodeRules.southEastOut && otherRules.northWestIn) node.outgoing.Add(other);
            }
            // South West
            if ((i % xSize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0) {
                other = nodes[i - 1 - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.southWestIn && otherRules.northEastOut) node.incoming.Add(other);
                if (nodeRules.southWestOut && otherRules.northEastIn) node.outgoing.Add(other);
            }
            // Up North
            if ((i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i + xSize + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upNorthIn && otherRules.downSouthOut) node.incoming.Add(other);
                if (nodeRules.upNorthOut && otherRules.downSouthIn) node.outgoing.Add(other);
            }
            // Down North
            if ((i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i - xSize + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downNorthIn && otherRules.upSouthOut) node.incoming.Add(other);
                if (nodeRules.downNorthOut && otherRules.upSouthIn) node.outgoing.Add(other);
            }
            // Down South
            if ((i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0) {
                other = nodes[i - xSize - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downSouthIn && otherRules.upNorthOut) node.incoming.Add(other);
                if (nodeRules.downSouthOut && otherRules.upNorthIn) node.outgoing.Add(other);
            }
            // Up North East
            if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i + 1 + xSize + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upNorthEastIn && otherRules.downSouthWestOut) node.incoming.Add(other);
                if (nodeRules.upNorthEastOut && otherRules.downSouthWestIn) node.outgoing.Add(other);
            }
            // Up North West
            if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i - 1 + xSize + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upNorthWestIn && otherRules.downSouthEastOut) node.incoming.Add(other);
                if (nodeRules.upNorthWestOut && otherRules.downSouthEastIn) node.outgoing.Add(other);
            }
            // Down North East
            if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i + 1 - xSize + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downNorthEastIn && otherRules.upSouthWestOut) node.incoming.Add(other);
                if (nodeRules.downNorthEastOut && otherRules.upSouthWestIn) node.outgoing.Add(other);
            }
            // Down North West
            if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize) {
                other = nodes[i - 1 - xSize + xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downNorthWestIn && otherRules.upSouthEastOut) node.incoming.Add(other);
                if (nodeRules.downNorthWestOut && otherRules.upSouthEastIn) node.outgoing.Add(other);
            }
            // Up South East
            if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0) {
                other = nodes[i + 1 + xSize - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upSouthEastIn && otherRules.downNorthWestOut) node.incoming.Add(other);
                if (nodeRules.upSouthEastOut && otherRules.downNorthWestIn) node.outgoing.Add(other);
            }
            // Up South West
            if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0) {
                other = nodes[i - 1 + xSize - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.upSouthWestIn && otherRules.downNorthEastOut) node.incoming.Add(other);
                if (nodeRules.upSouthWestOut && otherRules.downNorthEastIn) node.outgoing.Add(other);
            }
            // Down South East
            if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0) {
                other = nodes[i + 1 - xSize - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downSouthEastIn && otherRules.upNorthWestOut) node.incoming.Add(other);
                if (nodeRules.downSouthEastOut && otherRules.upNorthWestIn) node.outgoing.Add(other);
            }
            // Down South West
            if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0) {
                other = nodes[i - 1 - xSize - xSize * ySize];
                otherRules = rulesDict[other.type];
                if (nodeRules.downSouthWestIn && otherRules.upNorthEastOut) node.incoming.Add(other);
                if (nodeRules.downSouthWestOut && otherRules.upNorthEastIn) node.outgoing.Add(other);
            }
        }
    }

    public class PathfindingNode {
        public Vector3 location;
        public BlockGrid.BlockType type;
        public List<PathfindingNode> incoming;
        public List<PathfindingNode> outgoing;
        public GameObject obj;

        public float f = 0f;
        public float g = 0f;
        public float h = 0f;
    }
}

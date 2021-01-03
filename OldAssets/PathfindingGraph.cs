using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        PathfindingNode other;
        PathfindingRules nodeRules = node.block.pathfindingRules;
        PathfindingRules otherRules;

        for (int d = 0; d < 26; d++) {
            Block.Direction d1 = (Block.Direction)d;
            Block.Direction d2 = Block.Opposite(d1);

            if (nodeRules.c)
        }

        // East
        if ((i % xSize) + 1 < xSize && nodes[i + 1] != null) {
            other = nodes[i + 1];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.eastIn && otherRules.westOut) node.incoming.Add(other);
            if (nodeRules.eastOut && otherRules.westIn) node.outgoing.Add(other);
        }
        // West
        if ((i % xSize) - 1 >= 0 && nodes[i - 1] != null) {
            other = nodes[i - 1];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.westIn && otherRules.eastOut) node.incoming.Add(other);
            if (nodeRules.westOut && otherRules.eastIn) node.outgoing.Add(other);
        }
        // Up
        if ((i / xSize % ySize) + 1 < ySize && nodes[i + xSize] != null) {
            other = nodes[i + xSize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upIn && otherRules.downOut) node.incoming.Add(other);
            if (nodeRules.upOut && otherRules.downIn) node.outgoing.Add(other);
        }
        // Down
        if ((i / xSize % ySize) - 1 >= 0 && nodes[i - xSize] != null) {
            other = nodes[i - xSize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downIn && otherRules.upOut) node.incoming.Add(other);
            if (nodeRules.downOut && otherRules.upIn) node.outgoing.Add(other);
        }
        // North
        if ((i / xSize / ySize) + 1 < zSize && nodes[i + xSize * ySize] != null) {
            other = nodes[i + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.northIn && otherRules.southOut) node.incoming.Add(other);
            if (nodeRules.northOut && otherRules.southIn) node.outgoing.Add(other);
        }
        // South
        if ((i / xSize / ySize) - 1 >= 0 && nodes[i - xSize * ySize] != null) {
            other = nodes[i - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.southIn && otherRules.northOut) node.incoming.Add(other);
            if (nodeRules.southOut && otherRules.northIn) node.outgoing.Add(other);
        }
        // Up East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && nodes[i + 1 + xSize] != null) {
            other = nodes[i + 1 + xSize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upEastIn && otherRules.downWestOut) node.incoming.Add(other);
            if (nodeRules.upEastOut && otherRules.downWestIn) node.outgoing.Add(other);
        }
        // Up West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && nodes[i - 1 + xSize] != null) {
            other = nodes[i - 1 + xSize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upWestIn && otherRules.downEastOut) node.incoming.Add(other);
            if (nodeRules.upWestOut && otherRules.downEastIn) node.outgoing.Add(other);
        }
        // Down East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && nodes[i + 1 - xSize] != null) {
            other = nodes[i + 1 - xSize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downEastIn && otherRules.upWestOut) node.incoming.Add(other);
            if (nodeRules.downEastOut && otherRules.upWestIn) node.outgoing.Add(other);
        }
        // Down West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && nodes[i - 1 - xSize] != null) {
            other = nodes[i - 1 - xSize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downWestIn && otherRules.upEastOut) node.incoming.Add(other);
            if (nodeRules.downWestOut && otherRules.upEastIn) node.outgoing.Add(other);
        }
        // North East
        if ((i % xSize) + 1 < xSize && (i / xSize / ySize) + 1 < zSize && nodes[i + 1 + xSize * ySize] != null) {
            other = nodes[i + 1 + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i + 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!nodeRules.eastOut || !otherRules2.westIn) outgoing = false;
                if (!otherRules.southIn || !otherRules2.northOut) incoming = false;
                if (!otherRules.southOut || !otherRules2.northIn) outgoing = false;
            }

            other2 = nodes[i + xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.northIn || !otherRules2.southOut) incoming = false;
                if (!nodeRules.northOut || !otherRules2.southIn) outgoing = false;
                if (!otherRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!otherRules.westOut || !otherRules2.eastIn) outgoing = false;
            }

            if (nodeRules.northEastIn && otherRules.southWestOut && incoming) node.incoming.Add(other);
            if (nodeRules.northEastOut && otherRules.southWestIn && outgoing) node.outgoing.Add(other);
        }
        // North West
        if ((i % xSize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i - 1 + xSize * ySize] != null) {
            other = nodes[i - 1 + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i - 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!nodeRules.westOut || !otherRules2.eastIn) outgoing = false;
                if (!otherRules.southIn || !otherRules2.northOut) incoming = false;
                if (!otherRules.southOut || !otherRules2.northIn) outgoing = false;
            }

            other2 = nodes[i + xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.northIn || !otherRules2.southOut) incoming = false;
                if (!nodeRules.northOut || !otherRules2.southIn) outgoing = false;
                if (!otherRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!otherRules.eastOut || !otherRules2.westIn) outgoing = false;
            }

            if (nodeRules.northWestIn && otherRules.southEastOut && incoming) node.incoming.Add(other);
            if (nodeRules.northWestOut && otherRules.southEastIn && outgoing) node.outgoing.Add(other);
        }
        // South East
        if ((i % xSize) + 1 < xSize && (i / xSize / ySize) - 1 >= 0 && nodes[i + 1 - xSize * ySize] != null) {
            other = nodes[i + 1 - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i + 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!nodeRules.eastOut || !otherRules2.westIn) outgoing = false;
                if (!otherRules.northIn || !otherRules2.southOut) incoming = false;
                if (!otherRules.northOut || !otherRules2.southIn) outgoing = false;
            }

            other2 = nodes[i - xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.southIn || !otherRules2.northOut) incoming = false;
                if (!nodeRules.southOut || !otherRules2.northIn) outgoing = false;
                if (!otherRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!otherRules.westOut || !otherRules2.eastIn) outgoing = false;
            }

            if (nodeRules.southEastIn && otherRules.northWestOut && incoming) node.incoming.Add(other);
            if (nodeRules.southEastOut && otherRules.northWestIn && outgoing) node.outgoing.Add(other);
        }
        // South West
        if ((i % xSize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i - 1 - xSize * ySize] != null) {
            other = nodes[i - 1 - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i - 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!nodeRules.westOut || !otherRules2.eastIn) outgoing = false;
                if (!otherRules.northIn || !otherRules2.southOut) incoming = false;
                if (!otherRules.northOut || !otherRules2.southIn) outgoing = false;
            }

            other2 = nodes[i - xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.southIn || !otherRules2.northOut) incoming = false;
                if (!nodeRules.southOut || !otherRules2.northIn) outgoing = false;
                if (!otherRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!otherRules.eastOut || !otherRules2.westIn) outgoing = false;
            }

            if (nodeRules.southWestIn && otherRules.northEastOut && incoming) node.incoming.Add(other);
            if (nodeRules.southWestOut && otherRules.northEastIn && outgoing) node.outgoing.Add(other);
        }
        // Up North
        if ((i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize && nodes[i + xSize + xSize * ySize] != null) {
            other = nodes[i + xSize + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upNorthIn && otherRules.downSouthOut) node.incoming.Add(other);
            if (nodeRules.upNorthOut && otherRules.downSouthIn) node.outgoing.Add(other);
        }
        // Up South
        if ((i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0 && nodes[i + xSize - xSize * ySize] != null) {
            other = nodes[i + xSize - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upSouthIn && otherRules.downNorthOut) node.incoming.Add(other);
            if (nodeRules.upSouthOut && otherRules.downNorthIn) node.outgoing.Add(other);
        }
        // Down North
        if ((i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i - xSize + xSize * ySize] != null) {
            other = nodes[i - xSize + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downNorthIn && otherRules.upSouthOut) node.incoming.Add(other);
            if (nodeRules.downNorthOut && otherRules.upSouthIn) node.outgoing.Add(other);
        }
        // Down South
        if ((i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i - xSize - xSize * ySize] != null) {
            other = nodes[i - xSize - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downSouthIn && otherRules.upNorthOut) node.incoming.Add(other);
            if (nodeRules.downSouthOut && otherRules.upNorthIn) node.outgoing.Add(other);
        }
        // Up North East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize && nodes[i + 1 + xSize + xSize * ySize] != null) {
            other = nodes[i + 1 + xSize + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upNorthEastIn && otherRules.downSouthWestOut) node.incoming.Add(other);
            if (nodeRules.upNorthEastOut && otherRules.downSouthWestIn) node.outgoing.Add(other);
        }
        // Up North West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize && nodes[i - 1 + xSize + xSize * ySize] != null) {
            other = nodes[i - 1 + xSize + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upNorthWestIn && otherRules.downSouthEastOut) node.incoming.Add(other);
            if (nodeRules.upNorthWestOut && otherRules.downSouthEastIn) node.outgoing.Add(other);
        }
        // Down North East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i + 1 - xSize + xSize * ySize] != null) {
            other = nodes[i + 1 - xSize + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downNorthEastIn && otherRules.upSouthWestOut) node.incoming.Add(other);
            if (nodeRules.downNorthEastOut && otherRules.upSouthWestIn) node.outgoing.Add(other);
        }
        // Down North West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i - 1 - xSize + xSize * ySize] != null) {
            other = nodes[i - 1 - xSize + xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downNorthWestIn && otherRules.upSouthEastOut) node.incoming.Add(other);
            if (nodeRules.downNorthWestOut && otherRules.upSouthEastIn) node.outgoing.Add(other);
        }
        // Up South East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0 && nodes[i + 1 + xSize - xSize * ySize] != null) {
            other = nodes[i + 1 + xSize - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upSouthEastIn && otherRules.downNorthWestOut) node.incoming.Add(other);
            if (nodeRules.upSouthEastOut && otherRules.downNorthWestIn) node.outgoing.Add(other);
        }
        // Up South West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0 && nodes[i - 1 + xSize - xSize * ySize] != null) {
            other = nodes[i - 1 + xSize - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.upSouthWestIn && otherRules.downNorthEastOut) node.incoming.Add(other);
            if (nodeRules.upSouthWestOut && otherRules.downNorthEastIn) node.outgoing.Add(other);
        }
        // Down South East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i + 1 - xSize - xSize * ySize] != null) {
            other = nodes[i + 1 - xSize - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downSouthEastIn && otherRules.upNorthWestOut) node.incoming.Add(other);
            if (nodeRules.downSouthEastOut && otherRules.upNorthWestIn) node.outgoing.Add(other);
        }
        // Down South West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i - 1 - xSize - xSize * ySize] != null) {
            other = nodes[i - 1 - xSize - xSize * ySize];
            node.adjacent.Add(other);
            otherRules = other.block.GetComponent<PathfindingRules>();
            if (nodeRules.downSouthWestIn && otherRules.upNorthEastOut) node.incoming.Add(other);
            if (nodeRules.downSouthWestOut && otherRules.upNorthEastIn) node.outgoing.Add(other);
        }
    }

    public void UpdateNode(Vector3Int position) {
        int index = position.x + position.y * xSize + position.z * xSize * ySize;
        PathfindingNode node = nodes[index];

        node.incoming.Clear();
        node.outgoing.Clear();
        node.adjacent.Clear();

        InitializeNode(node, index);
        CheckSurroundingNodes(node, index);
    }

    public void UpdateNode(Vector3Int position, Block newBlock) {
        int index = position.x + position.y * xSize + position.z * xSize * ySize;
        PathfindingNode node = nodes[index];
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
        CheckSurroundingNodes(node, index);

        nodes[index] = node;
    }

    public void CheckSurroundingNodes(PathfindingNode node, int i) {
        PathfindingNode other;
        PathfindingRules nodeRules = node.block.GetComponent<PathfindingRules>();
        PathfindingRules otherRules;

        // East
        if ((i % xSize) + 1 < xSize && nodes[i + 1] != null) {
            other = nodes[i + 1];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.eastIn && otherRules.westOut) other.outgoing.Add(node);
            if (nodeRules.eastOut && otherRules.westIn) other.incoming.Add(node);
        }
        // West
        if ((i % xSize) - 1 >= 0 && nodes[i - 1] != null) {
            other = nodes[i - 1];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.westIn && otherRules.eastOut) other.outgoing.Add(node);
            if (nodeRules.westOut && otherRules.eastIn) other.incoming.Add(node);
        }
        // Up
        if ((i / xSize % ySize) + 1 < ySize && nodes[i + xSize] != null) {
            other = nodes[i + xSize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upIn && otherRules.downOut) other.outgoing.Add(node);
            if (nodeRules.upOut && otherRules.downIn) other.incoming.Add(node);
        }
        // Down
        if ((i / xSize % ySize) - 1 >= 0 && nodes[i - xSize] != null) {
            other = nodes[i - xSize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downIn && otherRules.upOut) other.outgoing.Add(node);
            if (nodeRules.downOut && otherRules.upIn) other.incoming.Add(node);
        }
        // North
        if ((i / xSize / ySize) + 1 < zSize && nodes[i + xSize * ySize] != null) {
            other = nodes[i + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.northIn && otherRules.southOut) other.outgoing.Add(node);
            if (nodeRules.northOut && otherRules.southIn) other.incoming.Add(node);
        }
        // South
        if ((i / xSize / ySize) - 1 >= 0 && nodes[i - xSize * ySize] != null) {
            other = nodes[i - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.southIn && otherRules.northOut) other.outgoing.Add(node);
            if (nodeRules.southOut && otherRules.northIn) other.incoming.Add(node);
        }
        // Up East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && nodes[i + 1 + xSize] != null) {
            other = nodes[i + 1 + xSize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upEastIn && otherRules.downWestOut) other.outgoing.Add(node);
            if (nodeRules.upEastOut && otherRules.downWestIn) other.incoming.Add(node);
        }
        // Up West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && nodes[i - 1 + xSize] != null) {
            other = nodes[i - 1 + xSize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upWestIn && otherRules.downEastOut) other.outgoing.Add(node);
            if (nodeRules.upWestOut && otherRules.downEastIn) other.incoming.Add(node);
        }
        // Down East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && nodes[i + 1 - xSize] != null) {
            other = nodes[i + 1 - xSize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downEastIn && otherRules.upWestOut) other.outgoing.Add(node);
            if (nodeRules.downEastOut && otherRules.upWestIn) other.incoming.Add(node);
        }
        // Down West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && nodes[i - 1 - xSize] != null) {
            other = nodes[i - 1 - xSize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downWestIn && otherRules.upEastOut) other.outgoing.Add(node);
            if (nodeRules.downWestOut && otherRules.upEastIn) other.incoming.Add(node);
        }
        // North East
        if ((i % xSize) + 1 < xSize && (i / xSize / ySize) + 1 < zSize && nodes[i + 1 + xSize * ySize] != null) {
            other = nodes[i + 1 + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i + 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!nodeRules.eastOut || !otherRules2.westIn) outgoing = false;
                if (!otherRules.southIn || !otherRules2.northOut) incoming = false;
                if (!otherRules.southOut || !otherRules2.northIn) outgoing = false;
            }

            other2 = nodes[i + xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.northIn || !otherRules2.southOut) incoming = false;
                if (!nodeRules.northOut || !otherRules2.southIn) outgoing = false;
                if (!otherRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!otherRules.westOut || !otherRules2.eastIn) outgoing = false;
            }

            if (nodeRules.northEastIn && otherRules.southWestOut && incoming) other.outgoing.Add(node);
            if (nodeRules.northEastOut && otherRules.southWestIn && outgoing) other.incoming.Add(node);
        }
        // North West
        if ((i % xSize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i - 1 + xSize * ySize] != null) {
            other = nodes[i - 1 + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i - 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!nodeRules.westOut || !otherRules2.eastIn) outgoing = false;
                if (!otherRules.southIn || !otherRules2.northOut) incoming = false;
                if (!otherRules.southOut || !otherRules2.northIn) outgoing = false;
            }

            other2 = nodes[i + xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.northIn || !otherRules2.southOut) incoming = false;
                if (!nodeRules.northOut || !otherRules2.southIn) outgoing = false;
                if (!otherRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!otherRules.eastOut || !otherRules2.westIn) outgoing = false;
            }

            if (nodeRules.northWestIn && otherRules.southEastOut && incoming) other.outgoing.Add(node);
            if (nodeRules.northWestOut && otherRules.southEastIn && outgoing) other.incoming.Add(node);
        }
        // South East
        if ((i % xSize) + 1 < xSize && (i / xSize / ySize) - 1 >= 0 && nodes[i + 1 - xSize * ySize] != null) {
            other = nodes[i + 1 - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i + 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!nodeRules.eastOut || !otherRules2.westIn) outgoing = false;
                if (!otherRules.northIn || !otherRules2.southOut) incoming = false;
                if (!otherRules.northOut || !otherRules2.southIn) outgoing = false;
            }

            other2 = nodes[i - xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.southIn || !otherRules2.northOut) incoming = false;
                if (!nodeRules.southOut || !otherRules2.northIn) outgoing = false;
                if (!otherRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!otherRules.westOut || !otherRules2.eastIn) outgoing = false;
            }

            if (nodeRules.southEastIn && otherRules.northWestOut && incoming) other.outgoing.Add(node);
            if (nodeRules.southEastOut && otherRules.northWestIn && outgoing) other.incoming.Add(node);
        }
        // South West
        if ((i % xSize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i - 1 - xSize * ySize] != null) {
            other = nodes[i - 1 - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);

            bool incoming = true;
            bool outgoing = true;

            PathfindingNode other2 = nodes[i - 1];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.westIn || !otherRules2.eastOut) incoming = false;
                if (!nodeRules.westOut || !otherRules2.eastIn) outgoing = false;
                if (!otherRules.northIn || !otherRules2.southOut) incoming = false;
                if (!otherRules.northOut || !otherRules2.southIn) outgoing = false;
            }

            other2 = nodes[i - xSize * ySize];
            if (other2 == null) {
                incoming = false;
                outgoing = false;
            }
            else {
                PathfindingRules otherRules2 = other2.block.GetComponent<PathfindingRules>();

                if (!nodeRules.southIn || !otherRules2.northOut) incoming = false;
                if (!nodeRules.southOut || !otherRules2.northIn) outgoing = false;
                if (!otherRules.eastIn || !otherRules2.westOut) incoming = false;
                if (!otherRules.eastOut || !otherRules2.westIn) outgoing = false;
            }

            if (nodeRules.southWestIn && otherRules.northEastOut && incoming) other.outgoing.Add(node);
            if (nodeRules.southWestOut && otherRules.northEastIn && outgoing) other.incoming.Add(node);
        }
        // Up North
        if ((i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize && nodes[i + xSize + xSize * ySize] != null) {
            other = nodes[i + xSize + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upNorthIn && otherRules.downSouthOut) other.outgoing.Add(node);
            if (nodeRules.upNorthOut && otherRules.downSouthIn) other.incoming.Add(node);
        }
        // Up South
        if ((i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0 && nodes[i + xSize - xSize * ySize] != null) {
            other = nodes[i + xSize - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upSouthIn && otherRules.downNorthOut) other.outgoing.Add(other);
            if (nodeRules.upSouthOut && otherRules.downNorthIn) other.incoming.Add(other);
        }
        // Down North
        if ((i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i - xSize + xSize * ySize] != null) {
            other = nodes[i - xSize + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downNorthIn && otherRules.upSouthOut) other.outgoing.Add(node);
            if (nodeRules.downNorthOut && otherRules.upSouthIn) other.incoming.Add(node);
        }
        // Down South
        if ((i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i - xSize - xSize * ySize] != null) {
            other = nodes[i - xSize - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downSouthIn && otherRules.upNorthOut) other.outgoing.Add(node);
            if (nodeRules.downSouthOut && otherRules.upNorthIn) other.incoming.Add(node);
        }
        // Up North East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize && nodes[i + 1 + xSize + xSize * ySize] != null) {
            other = nodes[i + 1 + xSize + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upNorthEastIn && otherRules.downSouthWestOut) other.outgoing.Add(node);
            if (nodeRules.upNorthEastOut && otherRules.downSouthWestIn) other.incoming.Add(node);
        }
        // Up North West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) + 1 < zSize && nodes[i - 1 + xSize + xSize * ySize] != null) {
            other = nodes[i - 1 + xSize + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upNorthWestIn && otherRules.downSouthEastOut) other.outgoing.Add(node);
            if (nodeRules.upNorthWestOut && otherRules.downSouthEastIn) other.incoming.Add(node);
        }
        // Down North East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i + 1 - xSize + xSize * ySize] != null) {
            other = nodes[i + 1 - xSize + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downNorthEastIn && otherRules.upSouthWestOut) other.outgoing.Add(node);
            if (nodeRules.downNorthEastOut && otherRules.upSouthWestIn) other.incoming.Add(node);
        }
        // Down North West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) + 1 < zSize && nodes[i - 1 - xSize + xSize * ySize] != null) {
            other = nodes[i - 1 - xSize + xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downNorthWestIn && otherRules.upSouthEastOut) other.outgoing.Add(node);
            if (nodeRules.downNorthWestOut && otherRules.upSouthEastIn) other.incoming.Add(node);
        }
        // Up South East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0 && nodes[i + 1 + xSize - xSize * ySize] != null) {
            other = nodes[i + 1 + xSize - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upSouthEastIn && otherRules.downNorthWestOut) other.outgoing.Add(node);
            if (nodeRules.upSouthEastOut && otherRules.downNorthWestIn) other.incoming.Add(node);
        }
        // Up South West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) + 1 < ySize && (i / xSize / ySize) - 1 >= 0 && nodes[i - 1 + xSize - xSize * ySize] != null) {
            other = nodes[i - 1 + xSize - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.upSouthWestIn && otherRules.downNorthEastOut) other.outgoing.Add(node);
            if (nodeRules.upSouthWestOut && otherRules.downNorthEastIn) other.incoming.Add(node);
        }
        // Down South East
        if ((i % xSize) + 1 < xSize && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i + 1 - xSize - xSize * ySize] != null) {
            other = nodes[i + 1 - xSize - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downSouthEastIn && otherRules.upNorthWestOut) other.outgoing.Add(node);
            if (nodeRules.downSouthEastOut && otherRules.upNorthWestIn) other.incoming.Add(node);
        }
        // Down South West
        if ((i % xSize) - 1 >= 0 && (i / xSize % ySize) - 1 >= 0 && (i / xSize / ySize) - 1 >= 0 && nodes[i - 1 - xSize - xSize * ySize] != null) {
            other = nodes[i - 1 - xSize - xSize * ySize];
            otherRules = other.block.GetComponent<PathfindingRules>();
            other.incoming.Remove(node);
            other.outgoing.Remove(node);
            if (nodeRules.downSouthWestIn && otherRules.upNorthEastOut) other.outgoing.Add(node);
            if (nodeRules.downSouthWestOut && otherRules.upNorthEastIn) other.incoming.Add(node);
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
        if (LevelRenderer.instance.GetObject(location) == null) return;
        node.block = LevelRenderer.instance.GetObject(location);
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

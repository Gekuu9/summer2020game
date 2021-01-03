using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovingOrb : MonoBehaviour {

    public OrbState state;
    public float orbSpeed;

    public enum OrbState {
        Static,
        MovingNorth,
        MovingEast,
        MovingSouth,
        MovingWest,
        MovingUp,
        MovingDown
    }

    public void DoMove() {
        Vector3 position = transform.position;

        Block block = LevelRenderer.instance.GetBlock(Vector3Int.FloorToInt(position));
        if (block != null) {
            if (block is OrbMirror) {
                OrbMirror mirror = (OrbMirror)block;
                switch (mirror.currentDirection) {
                    case OrbMirror.FaceDirection.NorthEast:
                        if (state == OrbState.MovingSouth) state = OrbState.MovingEast;
                        else if (state == OrbState.MovingWest) state = OrbState.MovingNorth;
                        else Destroy(gameObject);
                        break;
                    case OrbMirror.FaceDirection.SouthEast:
                        if (state == OrbState.MovingNorth) state = OrbState.MovingEast;
                        else if (state == OrbState.MovingWest) state = OrbState.MovingSouth;
                        else Destroy(gameObject);
                        break;
                    case OrbMirror.FaceDirection.SouthWest:
                        if (state == OrbState.MovingNorth) state = OrbState.MovingWest;
                        else if (state == OrbState.MovingEast) state = OrbState.MovingSouth;
                        else Destroy(gameObject);
                        break;
                    case OrbMirror.FaceDirection.NorthWest:
                        if (state == OrbState.MovingSouth) state = OrbState.MovingWest;
                        else if (state == OrbState.MovingEast) state = OrbState.MovingNorth;
                        else Destroy(gameObject);
                        break;
                }
            }
        }

        Vector3 targetPosition = position;
        switch (state) {
            case OrbState.Static:
                return;
            case OrbState.MovingNorth:
                targetPosition.z += 1;
                break;
            case OrbState.MovingEast:
                targetPosition.x += 1;
                break;
            case OrbState.MovingSouth:
                targetPosition.z -= 1;
                break;
            case OrbState.MovingWest:
                targetPosition.x -= 1;
                break;
            case OrbState.MovingUp:
                targetPosition.y += 1;
                break;
            case OrbState.MovingDown:
                targetPosition.y -= 1;
                break;
        }
        Hashtable args = new Hashtable();
        args.Add("position", targetPosition);
        args.Add("speed", orbSpeed);
        args.Add("easetype", iTween.EaseType.linear);
        args.Add("oncomplete", "DoMove");
        iTween.MoveTo(gameObject, args);
    }

    [CustomEditor(typeof(MovingOrb))]
    public class OrbEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            MovingOrb orb = (MovingOrb)target;

            if (GUILayout.Button("Do Move")) {
                orb.DoMove();
            }
        }
    }
}

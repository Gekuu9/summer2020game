using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovingOrb : MonoBehaviour {

    public OrbState state;
    public float orbSpeed;
    public float fadeoutTime;

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
                MirrorCollide(((OrbMirror)block).currentDirection);
            } else if (block is OrbTrigger) {
                OrbTrigger orbTrigger = (OrbTrigger)block;
                orbTrigger.Trigger();
                DoDestroy();
            } else if (block is OrbRingTrigger) {
                OrbRingTrigger orbRingTrigger = (OrbRingTrigger)block;
                orbRingTrigger.Trigger();
            } else if (!(block is Empty) && !(block is OrbCreator)) {
                DoDestroy();
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

    public void DoDestroy() {
        GetComponents<AudioSource>()[1].Play();
        IEnumerator coroutine = FadeOut();
        StartCoroutine(coroutine);
    }

    public void MirrorCollide(OrbMirror.FaceDirection currentDirection) {
        switch (currentDirection) {
            case OrbMirror.FaceDirection.NorthEast:
                if (state == OrbState.MovingSouth) state = OrbState.MovingEast;
                else if (state == OrbState.MovingWest) state = OrbState.MovingNorth;
                else DoDestroy();
                break;
            case OrbMirror.FaceDirection.SouthEast:
                if (state == OrbState.MovingNorth) state = OrbState.MovingEast;
                else if (state == OrbState.MovingWest) state = OrbState.MovingSouth;
                else DoDestroy();
                break;
            case OrbMirror.FaceDirection.SouthWest:
                if (state == OrbState.MovingNorth) state = OrbState.MovingWest;
                else if (state == OrbState.MovingEast) state = OrbState.MovingSouth;
                else DoDestroy();
                break;
            case OrbMirror.FaceDirection.NorthWest:
                if (state == OrbState.MovingSouth) state = OrbState.MovingWest;
                else if (state == OrbState.MovingEast) state = OrbState.MovingNorth;
                else DoDestroy();
                break;
        }
        GetComponents<AudioSource>()[1].Play();
    }

    public IEnumerator FadeOut() {
        state = OrbState.Static;
        Vector3 scale = Vector3.one;
        while (scale.magnitude > 0.01) {
            Vector3 step = Vector3.one * Time.deltaTime / fadeoutTime;
            foreach (ParticleSystem item in GetComponentsInChildren<ParticleSystem>()) {
                item.Stop();
                item.transform.localScale -= step;
            }
            scale -= step;
            yield return null;
        }
        Destroy(gameObject);
    }
}

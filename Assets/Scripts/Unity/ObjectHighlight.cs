﻿using R1Engine;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHighlight : MonoBehaviour {
    public Unity_ObjBehaviour highlightedObject = null;
    public Unity_Tile[] highlightedCollision = null;
    public Unity_IsometricCollisionTile highlightedCollision3D = null;
    public Unity_Tile[] highlightedTile = null;
    public Unity_CollisionLine highlightedCollisionLine = null;

    private void HandleCollision() {
        var ec = Controller.obj?.levelEventController?.editor?.cam;
        bool mouselook = ec != null && (ec.MouseLookEnabled || ec.MouseLookRMBEnabled);
        if(mouselook) return;

        int layerMask = 0;
        Camera cam = Camera.main;
        //layerMask |= 1 << LayerMask.NameToLayer("Object");
        if (LevelEditorData.Level?.IsometricData != null) {
            cam = ec?.camera3D ?? cam;


            // Isometric: Raycast objects
            if (Settings.ShowObjects) {
                layerMask = 1 << LayerMask.NameToLayer("3D Object");
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore);
                if (hits != null && hits.Length > 0) {
                    System.Array.Sort(hits, (x, y) => (x.distance.CompareTo(y.distance)));
                    for (int i = 0; i < hits.Length; i++) {
                        // the object identified by hit.transform was clicked
                        Unity_ObjBehaviour ob = hits[i].transform.GetComponentInParent<Unity_ObjBehaviour>();
                        if (ob != null) {
                            highlightedObject = ob;
                            break;
                        }
                    }
                }
            }

            // Isometric: Raycast collision
            if (LevelEditorData.Level?.IsometricData?.Collision != null) {
                layerMask = 1 << LayerMask.NameToLayer("3D Collision");
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore);
                if (hits != null && hits.Length > 0) {
                    System.Array.Sort(hits, (x, y) => (x.distance.CompareTo(y.distance)));
                    for (int i = 0; i < hits.Length; i++) {
                        // the object identified by hit.transform was clicked
                        // Hack, for now use the gameobject name
                        string[] name = hits[i].transform.gameObject.name.Split(',');
                        if (name.Length == 2 && int.TryParse(name[0], out int x) && int.TryParse(name[1], out int y)) {
                            var c3dt = LevelEditorData.Level.IsometricData.GetCollisionTile(x, y);
                            if (c3dt != null) {
                                highlightedCollision3D = c3dt;
                                break;
                            }
                        }
                    }
                }
            }
        } else {
            if (Settings.ShowObjects) {
                layerMask = 1 << LayerMask.NameToLayer("Object");
                RaycastHit2D[] hits = Physics2D.RaycastAll(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
                if (hits != null && hits.Length > 0) {
                    System.Array.Sort(hits, (x, y) => (x.distance.CompareTo(y.distance)));
                    for (int i = 0; i < hits.Length; i++) {
                        // the object identified by hit.transform was clicked
                        Unity_ObjBehaviour ob = hits[i].transform.GetComponentInParent<Unity_ObjBehaviour>();
                        if (ob != null) {
                            highlightedObject = ob;
                            break;
                        }
                    }
                }
            }
            if (Controller.obj?.levelController?.controllerTilemap?.CollisionLines != null
                && LevelEditorData.Level?.CollisionLines != null) {
                layerMask = 1 << LayerMask.NameToLayer("Collision Lines");
                RaycastHit2D[] hits = Physics2D.RaycastAll(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
                if (hits != null && hits.Length > 0) {
                    System.Array.Sort(hits, (x, y) => (x.distance.CompareTo(y.distance)));
                    for (int i = 0; i < hits.Length; i++) {
                        // Hack: identify line through name
                        if (int.TryParse(hits[i].transform.gameObject.name, out int x)
                            && x < LevelEditorData.Level.CollisionLines.Length) {
                            highlightedCollisionLine = LevelEditorData.Level.CollisionLines[x];
                            break;
                        }
                    }
                }
            }
        }
        Vector2Int mouseTileCollision = Controller.obj.levelController.controllerTilemap.MouseToTileInt(Input.mousePosition, collision: true);
        highlightedCollision = LevelEditorData.Level?.Maps?.Where(x => x.Type.HasFlag(Unity_Map.MapType.Collision)).Select(x => x.GetMapTile(mouseTileCollision.x, mouseTileCollision.y)).ToArray();
        Vector2Int mouseTile = Controller.obj.levelController.controllerTilemap.MouseToTileInt(Input.mousePosition);
        highlightedTile = LevelEditorData.Level?.Maps?.Where(x => x.Type.HasFlag(Unity_Map.MapType.Graphics)).Select(x => x.GetMapTile(mouseTile.x, mouseTile.y)).ToArray();
    }

    void Update() {
        highlightedObject = null;
        highlightedCollision3D = null;
        highlightedCollision = null;
        highlightedCollisionLine = null;
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (Controller.LoadState == Controller.State.Finished
            && !EventSystem.current.IsPointerOverGameObject()
            && screenRect.Contains(Input.mousePosition)
            ) HandleCollision();
    }
}

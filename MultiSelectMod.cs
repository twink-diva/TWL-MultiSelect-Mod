using System;
using System.Collections;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(TWLMod.MultiSelectMod), "TWL MultiSelect", "1.0.0", "Salem")]
[assembly: MelonGame("Toca Boca", "Toca Life World")]

namespace TWLMod
{
    public class MultiSelectMod : MelonMod
    {
        private List<GameObject> activeGroup = new List<GameObject>();
        private float moveSpeed = 4.0f;

        public override void OnUpdate()
        {
            HandleSelectionInput();
            HandleGroupMovement();
        }

        private void HandleSelectionInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                if (Camera.main != null)
                {
                    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

                    if (hit.collider != null)
                    {
                        GameObject target = hit.collider.transform.root.gameObject;

                        if (activeGroup.Contains(target))
                        {
                            activeGroup.Remove(target);
                            MelonLogger.Msg($"Removed {target.name} from group.");
                        }
                        else
                        {
                            activeGroup.Add(target);
                            MelonLogger.Msg($"Added {target.name} to group.");
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                activeGroup.Clear();
                MelonLogger.Msg("Cleared active group.");
            }
        }

        private void HandleGroupMovement()
        {
            float horizontal = 0f;
            float vertical = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) vertical += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) vertical -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) horizontal += 1f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) horizontal -= 1f;

            Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized;

            if (direction.magnitude > 0 && activeGroup.Count > 0)
            {
                Vector3 frameMovement = direction * moveSpeed * Time.deltaTime;

                for (int i = activeGroup.Count - 1; i >= 0; i--)
                {
                    if (activeGroup[i] != null)
                    {
                        activeGroup[i].transform.position += frameMovement;
                    }
                    else
                    {
                        activeGroup.RemoveAt(i);
                    }
                }
            }
        }
    }
}

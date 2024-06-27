using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

[Serializable]
public class Cutscene
{
    public List<Cut> cuts;
    
    private bool playerFinishedTask = true;
    private string choosenPath;
    private List<List<Cut>> organizedCuts;

    private bool isOrganized;

    private int groupCutsRunning = 0;

    public IEnumerator PlayCutscene()
    {

        if (!isOrganized)
        {
            organizedCuts = cuts.GroupBy(cut => cut.order).OrderBy(group => group.Key).Select(group => group.ToList()).ToList();
        }
        
        foreach (var group in organizedCuts)
        {

            foreach (var cut in group)
            {
                
                if (cut.type.Equals(CutType.Movement))
                {

                    if (cut.movement.isItCamera)
                    {
                        foreach (var cinemachineCamera in GameObject.FindObjectsByType<CinemachineCamera>(
                                     FindObjectsSortMode.None))
                        {
                            cinemachineCamera.gameObject.SetActive(false);
                        }
                    }
                    
                    cut.movement.gameObject.SetActive(true);

                    groupCutsRunning++;
                    
                    Tween.Position(cut.movement.gameObject.transform, cut.movement.movementSettings)
                        .OnComplete(() => groupCutsRunning--);
                }

                if (cut.type.Equals(CutType.Dialogue))
                {
                    groupCutsRunning++;
                    
                    yield return new WaitForSeconds(cut.dialogue.delay);
                    
                    UIController.ShowDialogue(cut.dialogue.dialogue);
                    playerFinishedTask = false;
                    GameEvents.OnPlayerKeyPressAfterDialogue.AddListener(SetDialogueFinished);
                    
                    yield return new WaitUntil(() => playerFinishedTask);
                    GameEvents.OnPlayerKeyPressAfterDialogue.RemoveListener(SetDialogueFinished);

                    groupCutsRunning--;
                }

                if (cut.type.Equals(CutType.Question))
                {
                    groupCutsRunning++;
                    UIController.ShowChoices(cut.choice.choiceRight, cut.choice.choiceLeft);
                    playerFinishedTask = false;
                    GameEvents.OnPlayerChooses.AddListener(ChoseOption);
                    yield return new WaitUntil(() => playerFinishedTask);
                    
                    UIController.HideChoices();
                    
                    if (choosenPath == "left")
                    {
                        if (cut.choice.cutsceneLeft != null)
                        {
                            cut.choice.cutsceneLeft.ContinueCutscene();
                            groupCutsRunning--;
                            yield break;
                        }
                    }
                    else
                    {
                        if (cut.choice.cutsceneRight != null)
                        {
                            cut.choice.cutsceneRight.ContinueCutscene();
                            groupCutsRunning--;
                            yield break;
                        }
                    }
                    
                }
            }

            yield return new WaitUntil(() => groupCutsRunning == 0);
        }
        
        GameEvents.OnCutsceneFinished.Invoke();

        groupCutsRunning = 0;

    }

    private void SetDialogueFinished()
    {
        playerFinishedTask = true;
    }
    
    private void ChoseOption(string option)
    {
        playerFinishedTask = true;
        choosenPath = option;
    }
}
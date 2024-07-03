using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelenitasDev.SoundsGood;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

[Serializable]
public class Cutscene
{
    public List<Cut> cuts;

    private bool playerPressedAnyButton;
    private bool playerChoosed;
    private string choosenPath;
    private List<List<Cut>> organizedCuts;

    private bool isOrganized;

    private int groupCutsRunning = 0;

    private Dictionary<int, Tween> _tweens = new Dictionary<int, Tween>();

    private CinemachineCamera lastCameraSelected;

    public IEnumerator PlayCutscene()
    {

        if (!isOrganized)
        {
            organizedCuts = cuts.GroupBy(cut => cut.order).OrderBy(group => group.Key).Select(group => group.ToList()).ToList();
        }

        groupCutsRunning = 0;
        
        foreach (var group in organizedCuts)
        {

            foreach (var cut in group)
            {

                //==================== SELECT CAMERA
                if (cut.type.Equals(CutType.SelectCamera))
                {
                    
                   foreach (var cinemachineCamera in GameObject.FindObjectsByType<CinemachineCamera>(
                                FindObjectsSortMode.None))
                   {
                       cinemachineCamera.gameObject.SetActive(false);
                   }

                   lastCameraSelected = cut.selectCamera.camera.GetComponent<CinemachineCamera>();
                    
                   cut.selectCamera.camera.SetActive(true);
                }
                
                //==================== MOVEMENT
                if (cut.type.Equals(CutType.Movement))
                {
                    groupCutsRunning++;
                    
                    Tween.LocalPosition(cut.movement.gameObject.transform, cut.movement.movementSettings)
                        .OnComplete(() => groupCutsRunning--);
                }
                
                //==================== ANIMATION
                if (cut.type.Equals(CutType.Animation))
                {
                    cut.animation.animator.CrossFade(cut.animation.animationName, 0);
                }

                //==================== DIALOGUE
                if (cut.type.Equals(CutType.Dialogue))
                {
                    groupCutsRunning++;

                    Tween.Delay(cut.dialogue.delay + 0.01f, () =>
                    {
                        UIController.ShowDialogue(cut.dialogue.dialogue);

                        GameEvents.OnUIDialogueFinishWriting.AddListener(WaitForUIDialogue);
                        
                        void WaitForUIDialogue()
                        {
                            GameEvents.OnUIDialogueFinishWriting.RemoveListener(WaitForUIDialogue);
                            groupCutsRunning--;
                        }
                    });
                    
                }
                
                //==================== MISTAKE TITLE
                if (cut.type.Equals(CutType.MistakeTitle))
                {
                    groupCutsRunning++;

                    Tween.Delay(cut.mistakeTitle.delay + 0.01f, () =>
                    {
                        UIController.ShowMistakeTitle(cut.mistakeTitle.title);
                        Tween.Delay(0.5f, () => groupCutsRunning--);
                    });

                }
                
                //==================== DELAY
                if (cut.type.Equals(CutType.Delay))
                {
                    yield return new WaitForSeconds(cut.delay.time);
                }
                
                //==================== PLAY CUTSCENE
                if (cut.type.Equals(CutType.Cutscene))
                {
                    CutsceneManager.PlayCutscene(cut.cutscene.cutsceneName);
                    UIController.HideDialogue();
                    UIController.HideChoices();
                    UIController.HideMistakeTitle();
                    yield break;
                }
                
                //==================== WAIT FOR INPUT
                if (cut.type.Equals(CutType.WaitForInput))
                {
                    groupCutsRunning++;
                    GameEvents.OnAnyKeyPress.AddListener(OnAnyKeyPress);
                    
                    void OnAnyKeyPress()
                    {
                        GameEvents.OnAnyKeyPress.RemoveListener(OnAnyKeyPress);
                        groupCutsRunning--;
                    }
                }
                
                //==================== PLAY MISTAKE
                if (cut.type.Equals(CutType.PlayMistake))
                {
                    Tween.Delay(1f, () =>
                    {
                        CutsceneManager.PlayMistake(cut.mistake.mistake, lastCameraSelected);
                        UIController.HideDialogue();
                        UIController.HideChoices();
                        UIController.HideMistakeTitle();
                    });
                    
                    yield break;
                }

                //==================== QUESTION
                if (cut.type.Equals(CutType.Question))
                {
                    UIController.ShowChoices(cut.choice.choiceRight, cut.choice.choiceLeft);
                    
                    if (GameManager.IsMistakeDone(cut.choiceId))
                    {
                        UIController.HideLeftButton();
                    }
                    GameEvents.OnPlayerChooses.AddListener(ChoseOption);

                    yield return new WaitUntil(() => playerChoosed);
                    
                    playerChoosed = false;
                    
                    if (choosenPath == "left")
                    {
                        if (cut.choice.cutsceneLeft != null)
                        {
                            cut.choice.cutsceneLeft.ContinueCutscene();
                            UIController.HideChoices();
                            UIController.ShowLeftButton();
                            
                            yield break;
                        }
                    }
                    else
                    {
                        if (cut.choice.cutsceneRight != null)
                        {
                            cut.choice.cutsceneRight.ContinueCutscene();
                            UIController.HideChoices();
                            UIController.ShowLeftButton();
                            
                            yield break;
                        }
                    }

                    groupCutsRunning = 0;
                }
            }

            yield return new WaitUntil(() =>
            {
                return groupCutsRunning == 0;
            });
        }
        
        GameEvents.OnCutsceneFinished.Invoke();

    }
    
    private void ChoseOption(string option)
    {
        playerChoosed = true;
        choosenPath = option;
    }
}
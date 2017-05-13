﻿using UnityEngine;
using System.Collections;

namespace Quest
{

    public class NPC_Trigger : MonoBehaviour
    {
        
        private NPCSystem.NPC _npc;
        

        void Start()
        {
            
            _npc = this.GetComponentInChildren<NPCSystem.NPC>();
            
        }

        void OnTriggerEnter(Collider coll)
        {


            // IF THE PLAYER HAS SELECTED THE NPC
            if (_npc.ReturnIsSelected())
            {
                if (coll.tag == "Player")
                {

                    CombatSystem.PlayerMovement.StopMoving();

                    _npc.PlayerInteraction(coll.gameObject, false);

                    if (!_npc.ReturnMetBefore())
                    {
                        // If the NPC is not a quest giver
                        if (!_npc.ReturnQuestGiver())
                        {
                            Debug.Log("test");
                            Dialogue.DialogueManager.SetDialogue("", _npc.ReturnDialogue1(), false, -1, -1);
                        }

                        if (_npc.ReturnQuestGiver() && !Quest.QuestDatabase.GetActiveFromNPC(_npc.ReturnNpcID()))
                        {
                            // IF THE NPC HAS A QUEST
                            Quest.QuestDatabase.GetQuestFromNpc(_npc.ReturnNpcID());
                            Dialogue.DialogueManager.SetDialogue(Quest.QuestDatabase.ReturnQuestTitle(), Quest.QuestDatabase.ReturnQuestText(), true, _npc.ReturnNpcID(), Quest.QuestDatabase.ReturnQuestID());
                        }
                        else
                        {
                            Debug.Log("QUEST IS ACTIVE");
                        }
                        
                        PlayerPrefs.SetString("MetNPC_" + _npc.ReturnNpcName(), "True");
                    }
                    if (_npc.ReturnMetBefore())
                    {

                        if (!_npc.ReturnQuestGiver())
                        {

                            Dialogue.DialogueManager.SetDialogue("", _npc.ReturnDialogue2(), false, -1, -1);
                            
                        }

                        if (_npc.ReturnQuestGiver() && Quest.QuestDatabase.GetActiveFromNPC(_npc.ReturnNpcID()))
                        {
                            Debug.Log("WE HAVE QUEST");
                            if (!Quest.QuestDatabase.CheckQuestCompleteNpc(_npc.ReturnNpcID()))
                            {
                                Quest.QuestDatabase.GetQuestFromNpc(_npc.ReturnNpcID());
                                Dialogue.DialogueManager.SetDialogue(Quest.QuestDatabase.ReturnQuestTitle(), Quest.QuestDatabase.ReturnQuestText(), false, _npc.ReturnNpcID(), Quest.QuestDatabase.ReturnQuestID());
                            }
                            if (Quest.QuestDatabase.CheckQuestCompleteNpc(_npc.ReturnNpcID()))
                            {

                                Quest.QuestDatabase.GetQuestFromNpc(_npc.ReturnNpcID());
                                Dialogue.DialogueManager.SetDialogue(Quest.QuestDatabase.ReturnQuestTitle(), Quest.QuestDatabase.ReturnQuestCompleteText(), true, _npc.ReturnNpcID(), Quest.QuestDatabase.ReturnQuestID());
                            }
                        }
                        if(_npc.ReturnQuestGiver() && !Quest.QuestDatabase.GetActiveFromNPC(_npc.ReturnNpcID()))
                        {
                            // IF THE NPC HAS A QUEST
                            Quest.QuestDatabase.GetQuestFromNpc(_npc.ReturnNpcID());
                            Dialogue.DialogueManager.SetDialogue(Quest.QuestDatabase.ReturnQuestTitle(), Quest.QuestDatabase.ReturnQuestText(), true, _npc.ReturnNpcID(), Quest.QuestDatabase.ReturnQuestID());
                        }
                    }
                }

           }
           
        }

        void OnTriggerExit(Collider coll)
        {
            if (_npc.ReturnPatrol())
            {
                _npc.PlayerInteraction(coll.gameObject, true);
            }
            if(!_npc.ReturnPatrol())
            {
                _npc.PlayerInteraction(coll.gameObject, false);
            }

            if (!_npc.ReturnMetBefore())
            {
                _npc.HasMetPlayer(true);
            }

                _npc.IsSelected(false);
            Dialogue.DialogueManager.ExitDialogue(false);
        }


    }
}
import { createReducer, on } from "@ngrx/store";
import { CurrentChatInfoModel } from "src/app/models/chat/CurrentChatInfoModel";
import { ChatActions } from "./chat.actions";

export interface IChatInfoInitialState {
    currentChatInfo: CurrentChatInfoModel;
  }
  export const ChatInfoInitialState1: IChatInfoInitialState = {
    currentChatInfo: new CurrentChatInfoModel()
  };
  export const ChatInfoReducer = createReducer(
    ChatInfoInitialState1,
    on(ChatActions.updateMessages, (_state, payload) => {
      return {
        ..._state,
        currentChatInfo: {
            ..._state.currentChatInfo,
            Messages: payload.newMessages
        }
      };
    }),
    on(ChatActions.updateUsers, (_state, payload) => {
      return {
        ..._state,
        currentChatInfo: {
            ..._state.currentChatInfo,
            Users: payload.newUsers
        }
      };
    }),
    on(ChatActions.addNewMessage, (_state, payload)=>{
        return{
            ..._state,
            currentChatInfo:{
                ..._state.currentChatInfo,
                Messages: [..._state.currentChatInfo.Messages, payload.newMessage]
            }
        }
    })
  );
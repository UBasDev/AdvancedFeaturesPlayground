import { createActionGroup, props } from "@ngrx/store"
import { IChatMessage, IUserInformation } from "src/app/models/chat/CurrentChatInfoModel";

export const ChatActions = createActionGroup({
    source: "CurrentChatInfo",
    events: {
      "Update Users": props<{ newUsers: Array<IUserInformation> }>(),
      "Update Messages": props<{ newMessages: Array<IChatMessage> }>(),
      "Add New Message": props<{ newMessage: IChatMessage }>()
    },
  });
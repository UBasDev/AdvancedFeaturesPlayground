export interface IChatReceivedRequestModel{
    senderUsername: string;
    senderConnectionId: string;
    sendDate: Date;
    content: string;
}

export class ChatReceivedRequestModel implements IChatReceivedRequestModel{
    senderUsername: string = "";
    senderConnectionId: string = "";
    sendDate: Date = new Date();
    content: string = "";
    
}
export interface ICurrentChatInfoModel{
    Users: Array<IUserInformation>
    Messages?: Array<IChatMessage>;
}
export interface IUserInformation{
    ConnectionId: string;
    Username: string;
    Age: number;
    City: string;
    Gender: string;
}
export class UserInformation implements IUserInformation{
    ConnectionId: string = "";
    Username: string = "";
    Age: number = 0;
    City: string = "";
    Gender: string = "";
}
export interface IChatMessage{
    SenderUsername: string;
    SenderConnectionId: string;
    SendDate: Date;
    Content: string;
}
export class CurrentChatInfoModel implements ICurrentChatInfoModel{
    Users: IUserInformation[] = [];
    Messages: IChatMessage[] = [];
}
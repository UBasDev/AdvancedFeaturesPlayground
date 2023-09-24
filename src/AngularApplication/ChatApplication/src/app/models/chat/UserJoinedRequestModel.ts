export interface IUserJoinedRequestModel{
    users: Array<IUserJoinedInformation>
    messages?: Array<IChatJoinedMessage>;
}
export interface IUserJoinedInformation{
    connectionId: string;
    username: string;
    age: number;
    city: string;
    gender: string;
}
export interface IChatJoinedMessage{
    senderConnectionId: string;
    sendDate: Date;
    content: string;
}
export class UserJoinerRequestModel implements IUserJoinedRequestModel{
    users: IUserJoinedInformation[] = [];
    messages?: IChatJoinedMessage[] = [];
}
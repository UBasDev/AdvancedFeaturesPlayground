export interface ISendMessageToStrangerModel{
    MessageContent: string;
    Username: string;
}
export class SendMessageToStrangerModel implements ISendMessageToStrangerModel{
    MessageContent: string = "";
    Username: string = "";
}
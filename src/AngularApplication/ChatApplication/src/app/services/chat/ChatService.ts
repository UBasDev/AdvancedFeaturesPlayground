import { Injectable } from "@angular/core";
import * as SignalR from "@microsoft/signalr"
import { IChatLoginRequestModel } from "../../models/chat/ChatLoginRequestModel";
import { SpinnerService } from "../spinner/SpinnerService";
import { CurrentChatInfoModel, IChatMessage, ICurrentChatInfoModel, IUserInformation } from "src/app/models/chat/CurrentChatInfoModel";
import {  IUserJoinedRequestModel } from "src/app/models/chat/UserJoinedRequestModel";
import { ChatReceivedRequestModel, IChatReceivedRequestModel } from "src/app/models/chat/ChatReceivedRequestModel";
import { ISendMessageToStrangerModel } from "src/app/models/chat/SendMessageToStrangerModel";

@Injectable({
    providedIn: 'root'
})
export class ChatService{
    constructor(private readonly spinnerService:SpinnerService){}
    private globalSocketConnection!: SignalR.HubConnection;
    private currentChatInfo: CurrentChatInfoModel = new CurrentChatInfoModel();
    setSocketConnection(data1: SignalR.HubConnection){
        this.globalSocketConnection = data1;
    }
    async startSocketConnection(){
        this.globalSocketConnection = new SignalR.HubConnectionBuilder().withUrl("https://localhost:7115/ServerChatHub").build();
        
        await this.globalSocketConnection.start().then(()=>{
            console.log("Socket connection has been started with 'https://localhost:7115/ServerChatHub' url")
        }).catch((error)=>{
            console.error("Error occured while connecting to socket server with 'https://localhost:7115/ServerChatHub' url")
        })
    }
    async addClientSideSocketListeners(){
        this.globalSocketConnection.on("ClientListener1", (data)=>{
            console.log("Data received from server: ", data)
        })
        this.globalSocketConnection.on("UserJoinedClientListener", (requestBody: IUserJoinedRequestModel)=>{
            const newUsers: Array<IUserInformation> = []
            requestBody.users.forEach((currentUser)=>{
                const newUser: IUserInformation = {
                    ConnectionId: currentUser.connectionId,
                    Age: currentUser.age,
                    City: currentUser.city,
                    Gender: currentUser.gender,
                    Username: currentUser.username
                }
                newUsers.push(newUser)
            })
            this.currentChatInfo.Users = newUsers
            if(requestBody?.messages && requestBody?.messages?.length>1) {
                const newMessages: Array<IChatMessage> = []
                requestBody.messages.forEach((currentMessage)=>{
                    const newMessage: IChatMessage = {
                        SenderConnectionId: currentMessage.senderConnectionId,
                        Content: currentMessage.content,
                        SendDate: currentMessage.sendDate,
                        SenderUsername: currentMessage.senderUsername
                    }
                    newMessages.push(newMessage)
                })
                this.currentChatInfo.Messages = newMessages
            }
            this.spinnerService.closeSpinner();
        })
        this.globalSocketConnection.on("ChatReceivedClientListener", (requestBody: IChatReceivedRequestModel)=>{
            const newMessage: IChatMessage = {
                Content: requestBody.content,
                SendDate: requestBody.sendDate,
                SenderConnectionId: requestBody.senderConnectionId,
                SenderUsername: requestBody.senderUsername
            }
            this.currentChatInfo.Messages.push(newMessage)
        })
    }
    sendUserDataAfterLoginForQueueAndMatch(requestBody: IChatLoginRequestModel){
        this.globalSocketConnection.invoke("UserConnectedServerListener", requestBody)
    }
    sendMessageToStrangerUser(message: string){
        const requestToSend: ISendMessageToStrangerModel = {
            MessageContent: message,
            Username: this.currentChatInfo.Users.find(x => x.ConnectionId == this.globalSocketConnection.connectionId)?.Username ?? ""
        }
        this.globalSocketConnection.invoke("SendMessageToStrangerUserServerListener", requestToSend)
    }
    get getCurrentChatInfo(): ICurrentChatInfoModel{
        return this.currentChatInfo
    }
}
import { Injectable } from "@angular/core";
import * as SignalR from "@microsoft/signalr"
import { BehaviorSubject, Subject, of } from "rxjs";
import { ChatLoginRequestModel, IChatLoginRequestModel } from "../../models/chat/ChatLoginRequestModel";
import { SpinnerService } from "../spinner/SpinnerService";
import { CurrentChatInfoModel, IChatMessage, ICurrentChatInfoModel, IUserInformation } from "src/app/models/chat/CurrentChatInfoModel";
import { IChatJoinedMessage, IUserJoinedRequestModel } from "src/app/models/chat/UserJoinedRequestModel";

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
            console.log('USERJOINED', requestBody)
            requestBody.users.forEach((currentUser)=>{
                const newUser: IUserInformation = {
                    ConnectionId: currentUser.connectionId,
                    Age: currentUser.age,
                    City: currentUser.city,
                    Gender: currentUser.gender,
                    Username: currentUser.username
                }
                this.currentChatInfo.Users.push(newUser)
            })
            if(requestBody?.messages && requestBody?.messages?.length>1) {
                requestBody.messages.forEach((currentMessage)=>{
                    const newMessage: IChatMessage = {
                        SenderConnectionId: currentMessage.senderConnectionId,
                        Content: currentMessage.content,
                        SendDate: currentMessage.sendDate
                    }
                    this.currentChatInfo.Messages.push(newMessage)
                })
            }
            
            
            this.spinnerService.closeSpinner();
        })
    }
    sendDataAfterLogin(requestBody: ChatLoginRequestModel){
        this.globalSocketConnection.invoke("UserConnectedServerListener", requestBody)
    }
    get getCurrentChatInfo(): ICurrentChatInfoModel{
        return this.currentChatInfo
    }
}
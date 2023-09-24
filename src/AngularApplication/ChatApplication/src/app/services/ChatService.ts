import { Injectable } from "@angular/core";
import * as SignalR from "@microsoft/signalr"
import { BehaviorSubject, Subject, of } from "rxjs";
import { ChatLoginRequestModel } from "../models/chat/ChatLoginRequestModel";

@Injectable({
    providedIn: 'root'
})
export class ChatService{
    private globalSocketConnection!: SignalR.HubConnection;
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
    }
    sendDataAfterLogin(requestBody: ChatLoginRequestModel){
        this.globalSocketConnection.invoke("UserConnectedServerListener", requestBody)
    }
}
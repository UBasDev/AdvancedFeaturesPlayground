import { Component } from '@angular/core';
import { ChatLoginRequestModel } from 'src/app/models/chat/ChatLoginRequestModel';
import { ChatService } from 'src/app/services/ChatService';

@Component({
  selector: 'app-chat-homepage',
  templateUrl: './chat-homepage.component.html',
  styleUrls: ['./chat-homepage.component.css']
})
export class ChatHomepageComponent {
  constructor(private readonly chatService: ChatService){}
  test1(){
    const requestBodyToSend: ChatLoginRequestModel = {
      Age: 33,
      City: "Selasd",
      Gender: "male",
      Username: "useamsd"
    }
    this.chatService.sendDataAfterLogin(requestBodyToSend);
  }
}

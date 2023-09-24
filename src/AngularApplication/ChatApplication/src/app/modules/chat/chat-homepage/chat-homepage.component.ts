import { Component } from '@angular/core';
import { IChatLoginRequestModel } from 'src/app/models/chat/ChatLoginRequestModel';
import { ChatService } from 'src/app/services/chat/ChatService';

@Component({
  selector: 'app-chat-homepage',
  templateUrl: './chat-homepage.component.html',
  styleUrls: ['./chat-homepage.component.css']
})
export class ChatHomepageComponent {
  constructor(private readonly chatService: ChatService){}
  test1(){
    console.log('CHAT INFO',this.chatService.getCurrentChatInfo)
  }
}

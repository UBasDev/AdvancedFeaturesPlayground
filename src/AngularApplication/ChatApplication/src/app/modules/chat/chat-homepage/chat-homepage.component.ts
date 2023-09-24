import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { IChatLoginRequestModel } from 'src/app/models/chat/ChatLoginRequestModel';
import { ChatService } from 'src/app/services/chat/ChatService';

@Component({
  selector: 'app-chat-homepage',
  templateUrl: './chat-homepage.component.html',
  styleUrls: ['./chat-homepage.component.css']
})
export class ChatHomepageComponent {
  public componentFormInputKeys = {
    message: 'message',
  };

  constructor(private readonly chatService: ChatService, private readonly formBuilder:FormBuilder){}
  test1(){
    console.log('CHAT INFO',this.chatService.getCurrentChatInfo)
  }
  public sendMessageForm = this.formBuilder.group({
    [this.componentFormInputKeys.message]: ['', Validators.required]
  })
  public sendMessage(){
    this.chatService.sendMessageToStrangerUser(this.sendMessageForm.controls[this.componentFormInputKeys.message]?.value ?? "")
  }
}

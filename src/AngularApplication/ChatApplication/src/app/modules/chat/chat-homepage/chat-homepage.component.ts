import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import {
  IUserInformation,
  UserInformation,
} from 'src/app/models/chat/CurrentChatInfoModel';
import { ChatService } from 'src/app/services/chat/ChatService';
import { IChatInfoInitialState } from 'src/app/store/chat/chat_reducer';

@Component({
  selector: 'app-chat-homepage',
  templateUrl: './chat-homepage.component.html',
  styleUrls: ['./chat-homepage.component.css'],
})
export class ChatHomepageComponent implements OnInit {
  public currentUserInfo: UserInformation = new UserInformation();
  public componentFormInputKeys = {
    message: 'message',
  };

  constructor(
    private readonly chatService: ChatService,
    private readonly formBuilder: FormBuilder,
    private stateStore: Store<{ globalChatInfo: IChatInfoInitialState }>
  ) {}
  ngOnInit(): void {
    this.stateStore
      .select('globalChatInfo')
      .subscribe((data: IChatInfoInitialState) => {
        const currentUser: IUserInformation | undefined =
          data.currentChatInfo.Users.find(
            (x) =>
              x.ConnectionId ==
              this.chatService.globalSocketConnection.connectionId
          );
        if (!currentUser)
          console.error(
            `There is no user with ${this.chatService.globalSocketConnection.connectionId} id`
          );
        this.currentUserInfo = {
          Username: currentUser?.Username ?? '',
          Age: currentUser?.Age ?? 0,
          City: currentUser?.City ?? '',
          ConnectionId: currentUser?.ConnectionId ?? '',
          Gender: currentUser?.Gender ?? '',
        };
      });
  }
  test1() {
    this.stateStore
      .select('globalChatInfo')
      .subscribe((data: IChatInfoInitialState) => {
        console.log('CHAT INFO', data.currentChatInfo);
      });
  }
  public sendMessageForm = this.formBuilder.group({
    [this.componentFormInputKeys.message]: ['', Validators.required],
  });
  public async sendMessage() {
    await this.chatService.sendMessageToStrangerUser(
      this.sendMessageForm.controls[this.componentFormInputKeys.message]
        ?.value ?? ''
    );
  }
}

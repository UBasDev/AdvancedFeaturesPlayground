import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import {
  IChatMessage,
  IUserInformation,
  UserInformation,
} from 'src/app/models/chat/CurrentChatInfoModel';
import { ChatService } from 'src/app/services/chat/ChatService';
import { IChatInfoInitialState } from 'src/app/store/chat/chat_reducer';
import { ISpinnerStateInitialState } from 'src/app/store/spinner/spinner.reducer';

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
  public isSpinnerActive: boolean = false;
  public chatMessages: Array<IChatMessage> = []

  constructor(
    private readonly chatService: ChatService,
    private readonly formBuilder: FormBuilder,
    private chatInfoStore: Store<{ globalChatInfo: IChatInfoInitialState }>,
    private spinnerStore: Store<{ globalSpinnerInfo: ISpinnerStateInitialState }>
  ) {}
  ngOnInit(): void {
    this.chatInfoStore
      .select('globalChatInfo')
      .subscribe((chatData: IChatInfoInitialState) => {
        const currentUser: IUserInformation | undefined =
        chatData.currentChatInfo.Users.find(
            (currentUser) =>
            currentUser.ConnectionId ==
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
        this.chatMessages = chatData.currentChatInfo.Messages
      });
    this.spinnerStore.select('globalSpinnerInfo').subscribe((spinnerData: ISpinnerStateInitialState)=>{
      this.isSpinnerActive = spinnerData.isOpen
    })
  }
  test1() {
    this.chatInfoStore
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

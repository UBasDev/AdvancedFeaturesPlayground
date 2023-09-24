import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ChatLoginValidators } from './chat-login-validators/chat-login-validators';
import * as SignalR from '@microsoft/signalr';
import { ChatService } from 'src/app/services/ChatService';
import { Observable, of } from 'rxjs';
import { ChatLoginRequestModel } from 'src/app/models/chat/ChatLoginRequestModel';

interface IGenderOptions {
  id: number;
  key: string;
  value: string;
}

@Component({
  selector: 'app-chat-login',
  templateUrl: './chat-login.component.html',
  styleUrls: ['./chat-login.component.css'],
})
export class ChatLoginComponent {
  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly chatLoginValidators: ChatLoginValidators,
    private readonly chatService: ChatService
  ) {}

  public genderOptions: ReadonlyArray<IGenderOptions> = [
    {
      id: 1,
      key: 'Male',
      value: 'male',
    },
    {
      id: 2,
      key: 'Female',
      value: 'female',
    },
  ];
  public componentFormInputKeys = {
    usernameInputKey: 'username',
    ageInputKey: 'age',
    cityInputKey: 'city',
    genderInputKey: 'gender',
  };

  public chatLoginForm = this.formBuilder.group({
    [this.componentFormInputKeys.usernameInputKey]: [
      '',
      this.chatLoginValidators.usernameInputValidator(),
    ],
    [this.componentFormInputKeys.ageInputKey]: [
      1,
      this.chatLoginValidators.ageInputValidator(),
    ],
    [this.componentFormInputKeys.cityInputKey]: [
      '',
      this.chatLoginValidators.cityInputValidator(),
    ],
    [this.componentFormInputKeys.genderInputKey]: [
      this.genderOptions[0].value,
      this.chatLoginValidators.genderInputValidator(),
    ],
  });
  get isAgeInputValid(): boolean {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.ageInputKey]
        .errors?.['isValid'] ?? true
    );
  }
  get ageInputErrorMessage(): string {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.ageInputKey]
        .errors?.['errorMessage'] ?? ''
    );
  }
  get isUsernameInputValid(): boolean {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.usernameInputKey]
        .errors?.['isValid'] ?? true
    );
  }
  get usernameInputErrorMessage(): string {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.usernameInputKey]
        .errors?.['errorMessage'] ?? ''
    );
  }
  get isCityInputValid(): boolean {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.cityInputKey]
        .errors?.['isValid'] ?? true
    );
  }
  get cityInputErrorMessage(): string {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.cityInputKey]
        .errors?.['errorMessage'] ?? ''
    );
  }
  get isGenderInputValid(): boolean {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.genderInputKey]
        .errors?.['isValid'] ?? true
    );
  }
  get genderInputErrorMessage(): string {
    return (
      this.chatLoginForm.controls[this.componentFormInputKeys.genderInputKey]
        .errors?.['errorMessage'] ?? ''
    );
  }
  get isFormValid(): boolean {
    return (
      this.isAgeInputValid &&
      this.isCityInputValid &&
      this.isGenderInputValid &&
      this.isUsernameInputValid
    );
  }

  async onChatLoginFormSubmit() {
    await this.chatService.startSocketConnection();
    await this.chatService.addClientSideSocketListeners();
  }
  async test1(){
    const formValues = this.chatLoginForm.value;
    const requestBodyToSend: ChatLoginRequestModel = {
      Age:
        (formValues?.[this.componentFormInputKeys.ageInputKey] as number) ??
        0,
      City:
        (formValues?.[
          this.componentFormInputKeys.cityInputKey
        ] as string) ?? '',
      Gender:
        (formValues?.[
          this.componentFormInputKeys.genderInputKey
        ] as string) ?? '',
      Username:
        (formValues?.[
          this.componentFormInputKeys.usernameInputKey
        ] as string) ?? '',
    };
    this.chatService.sendDataAfterLogin(requestBodyToSend);
  }
}

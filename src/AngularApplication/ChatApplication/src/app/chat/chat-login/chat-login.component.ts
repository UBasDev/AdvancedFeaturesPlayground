import { Component } from '@angular/core';
import {
  
  FormBuilder,
  
} from '@angular/forms';
import { ChatLoginValidators } from './chat-login-validators/chat-login-validators';

interface IGenderOptions{
  id:number;
  key:string;
  value:string;
}

@Component({
  selector: 'app-chat-login',
  templateUrl: './chat-login.component.html',
  styleUrls: ['./chat-login.component.css'],
})
export class ChatLoginComponent {
  public genderOptions: ReadonlyArray<IGenderOptions> = [
    {
      id: 1,
      key: "Male",
      value: "male"
    },
    {
      id: 2,
      key: "Female",
      value: "female"
    }
  ]
  public componentFormInputKeys = {
    usernameInputKey: 'username',
    ageInputKey: 'age',
    cityInputKey: 'city',
    genderInputKey: 'gender',
  };
  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly chatLoginValidators: ChatLoginValidators
  ) {}
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
  get isAgeInputValid():boolean {
    return this.chatLoginForm.controls[this.componentFormInputKeys.ageInputKey]
      .errors?.['isValid'] ?? true;
  }
  get ageInputErrorMessage():string {
    return this.chatLoginForm.controls[this.componentFormInputKeys.ageInputKey]
      .errors?.['errorMessage'] ?? "";
  }
  get isUsernameInputValid():boolean {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.usernameInputKey
    ].errors?.['isValid'] ?? true;
  }
  get usernameInputErrorMessage():string {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.usernameInputKey
    ].errors?.['errorMessage'] ?? "";
  }
  get isCityInputValid():boolean {
    return this.chatLoginForm.controls[this.componentFormInputKeys.cityInputKey]
      .errors?.['isValid'] ?? true;
  }
  get cityInputErrorMessage():string {
    return this.chatLoginForm.controls[this.componentFormInputKeys.cityInputKey]
      .errors?.['errorMessage'] ?? "";
  }
  get isGenderInputValid():boolean {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.genderInputKey
    ].errors?.['isValid'] ?? true;
  }
  get genderInputErrorMessage():string {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.genderInputKey
    ].errors?.['errorMessage'] ?? "";
  }
  get isFormValid():boolean{
    return this.isAgeInputValid && this.isCityInputValid && this.isGenderInputValid && this.isUsernameInputValid
  }
  onChatLoginFormSubmit(){
    console.log(this.chatLoginForm.value)
  }
}

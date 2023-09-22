import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ChatLoginValidators } from './chat-login-validators/chat-login-validators';

@Component({
  selector: 'app-chat-login',
  templateUrl: './chat-login.component.html',
  styleUrls: ['./chat-login.component.css'],
})
export class ChatLoginComponent {
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
      '',
      this.chatLoginValidators.genderInputValidator(),
    ],
  });
  get isAgeInputValid() {
    return this.chatLoginForm.controls[this.componentFormInputKeys.ageInputKey]
      .errors?.['isValid'];
  }
  get ageInputErrorMessage() {
    return this.chatLoginForm.controls[this.componentFormInputKeys.ageInputKey]
      .errors?.['errorMessage'];
  }
  get isUsernameInputValid() {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.usernameInputKey
    ].errors?.['isValid'];
  }
  get usernameInputErrorMessage() {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.usernameInputKey
    ].errors?.['errorMessage'];
  }
  get isCityInputValid() {
    return this.chatLoginForm.controls[this.componentFormInputKeys.cityInputKey]
      .errors?.['isValid'];
  }
  get cityInputErrorMessage() {
    return this.chatLoginForm.controls[this.componentFormInputKeys.cityInputKey]
      .errors?.['errorMessage'];
  }
  get isGenderInputValid() {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.genderInputKey
    ].errors?.['isValid'];
  }
  get genderInputErrorMessage() {
    return this.chatLoginForm.controls[
      this.componentFormInputKeys.genderInputKey
    ].errors?.['errorMessage'];
  }
  public test1() {
    console.log(this.chatLoginForm);
  }
}

import { Injectable } from '@angular/core';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class ChatLoginValidators {
  public usernameInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.dirty && control.touched) {
        const value: string = control.value;
        if (!value)
          return {
            isValid: false,
            errorMessage: 'Username field is required',
          };
        else if (typeof value != 'string')
          return {
            isValid: false,
            errorMessage: 'Please check your username',
          };
        else if (value.length > 30)
          return {
            isValid: false,
            errorMessage: 'Your username must be at max 30 characters',
          };
      }
      return { isValid: true };
    };
  }
  public ageInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.dirty || control.touched) {
        const value: number = control.value;
        if (!value)
          return {
            isValid: false,
            errorMessage: 'Age field is required',
          };
        else if (typeof value != 'number')
          return {
            isValid: false,
            errorMessage: 'Please check your age',
          };
        else if (value < 1)
          return {
            isValid: false,
            errorMessage: 'Your age cannot be smaller than 1',
          };
        else if (value > 99)
          return {
            isValid: false,
            errorMessage: 'Your age cannot be greater than 99',
          };
      }
      return { isValid: true };
    };
  }
  public cityInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.dirty && control.touched) {
        const value: string = control.value;
        if (!value)
          return {
            isValid: false,
            errorMessage: 'City field is required',
          };
        else if (typeof value != 'string')
          return {
            isValid: false,
            errorMessage: 'Please check your city',
          };
        else if (value.length > 30)
          return {
            isValid: false,
            errorMessage: 'Your city must be at max 30 characters',
          };
      }
      return { isValid: true };
    };
  }
  public genderInputValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (control.dirty && control.touched) {
        const value: string = control.value;
        if (!value)
          return {
            isValid: false,
            errorMessage: 'Gender field is required',
          };
        else if (typeof value != 'string')
          return {
            isValid: false,
            errorMessage: 'Please check your gender',
          };
      }
      return { isValid: true };
    };
  }
}

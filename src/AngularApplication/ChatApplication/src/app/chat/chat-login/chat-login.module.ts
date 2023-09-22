import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatLoginComponent } from './chat-login.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [ChatLoginComponent],
  imports: [CommonModule, ReactiveFormsModule],
})
export class ChatLoginModule {}

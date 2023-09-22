import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatComponent } from './chat.component';
import { ChatLoginComponent } from './chat-login/chat-login.component';
import { AppRoutingModule } from '../app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ChatLoginModule } from './chat-login/chat-login.module';

@NgModule({
  declarations: [ChatComponent],
  imports: [CommonModule, AppRoutingModule, ChatLoginModule],
})
export class ChatModule {}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatHomepageComponent } from './chat-homepage.component';
import { AppRoutingModule } from 'src/app/app-routing.module';

@NgModule({
  declarations: [
    ChatHomepageComponent
  ],
  imports: [
    CommonModule, AppRoutingModule
  ]
})
export class ChatHomepageModule { }

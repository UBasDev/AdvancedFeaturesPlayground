import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatHomepageComponent } from './chat-homepage.component';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ChatHomepageComponent
  ],
  imports: [
    CommonModule, AppRoutingModule, ReactiveFormsModule
  ]
})
export class ChatHomepageModule { }

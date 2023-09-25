import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { ChatModule } from './modules/chat/chat.module';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GlobalSpinnerModule } from './single-modules/global-spinner/global-spinner.module';
import { StoreModule } from '@ngrx/store';
import { ChatInfoReducer } from './store/chat/chat_reducer';
import { SpinnerStateReducer } from './store/spinner/spinner.reducer';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    ChatModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    GlobalSpinnerModule,
    StoreModule.forRoot({
      globalChatInfo: ChatInfoReducer,
      globalSpinnerInfo: SpinnerStateReducer
    }, {})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

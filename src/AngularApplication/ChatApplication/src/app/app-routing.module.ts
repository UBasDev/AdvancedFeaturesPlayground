import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ChatComponent } from "./chat/chat.component";
import { ChatLoginComponent } from "./chat/chat-login/chat-login.component";

const allRoutes: Routes = [
  {
    path: '',
    component: ChatComponent,
    children: [
      {
        path: '',
        component: ChatLoginComponent
      }
    ]
  }
]
@NgModule({
  imports: [RouterModule.forRoot(allRoutes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }

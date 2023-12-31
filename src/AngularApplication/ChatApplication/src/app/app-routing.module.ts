import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ChatComponent } from "./modules/chat/chat.component";
import { ChatLoginComponent } from "./modules/chat/chat-login/chat-login.component";
import { ChatHomepageComponent } from "./modules/chat/chat-homepage/chat-homepage.component";
import { chatHomepageGuard } from "./guards/chat/chat-homepage.guard";

const allRoutes: Routes = [
  {
    path: '',
    component: ChatComponent,
    children: [
      {
        path: '',
        component: ChatLoginComponent
      },
      {
        path:'homepage',
        component: ChatHomepageComponent,
        canActivate: [chatHomepageGuard]
      }
    ]
  }
]
@NgModule({
  imports: [RouterModule.forRoot(allRoutes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }

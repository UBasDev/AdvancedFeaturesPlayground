import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { IChatInfoInitialState } from 'src/app/store/chat/chat_reducer';

export const chatHomepageGuard: CanActivateFn = (route, state) => {
  var guardResult = true
  const chatStore = inject(Store<{ globalChatInfo: IChatInfoInitialState }>)
  const router: Router = inject(Router);
  chatStore.select('globalChatInfo').subscribe((chatData: IChatInfoInitialState)=>{
    if(chatData.currentChatInfo.Users == null || chatData.currentChatInfo.Users.length<1){
      guardResult = false
    }
  })
  return guardResult || router.createUrlTree([''])
};

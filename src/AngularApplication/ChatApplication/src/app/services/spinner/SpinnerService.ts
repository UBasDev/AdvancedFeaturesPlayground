import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { BehaviorSubject, Subject } from "rxjs";
import { SpinnerActions } from "src/app/store/spinner/spinner.actionts";
import { ISpinnerStateInitialState } from "src/app/store/spinner/spinner.reducer";

@Injectable({
    providedIn: 'root'
})
export class SpinnerService{
    constructor(
        private stateStore: Store<{ globalSpinnerInfo: ISpinnerStateInitialState }>
    ){}
    public openSpinner(){
        this.stateStore.dispatch(
            SpinnerActions.toggleSpinner({
                isOpen: true
            })
        )
    }
    public closeSpinner(){
        this.stateStore.dispatch(
            SpinnerActions.toggleSpinner({
                isOpen: false
            })
        )
    }
}
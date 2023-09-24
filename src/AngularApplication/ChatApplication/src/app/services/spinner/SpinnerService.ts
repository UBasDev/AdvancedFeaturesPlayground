import { Injectable } from "@angular/core";
import { BehaviorSubject, Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class SpinnerService{
    public isSpinnerActive: Subject<boolean> = new BehaviorSubject<boolean>(false);
    public openSpinner(){
        this.isSpinnerActive.next(true)
    }
    public closeSpinner(){
        this.isSpinnerActive.next(false)
    }
}
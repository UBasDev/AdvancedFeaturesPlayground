import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { ISpinnerStateInitialState } from './store/spinner/spinner.reducer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(
    private stateStore: Store<{ globalSpinnerInfo: ISpinnerStateInitialState }>
    ){}
  ngOnInit(): void {
    this.stateStore.select('globalSpinnerInfo').subscribe((spinnerValue: ISpinnerStateInitialState)=>{
      this.isSpinnerActive = spinnerValue.isOpen
    })
  }
  
  public isSpinnerActive: boolean = false;
  title = 'ChatApplication';
}

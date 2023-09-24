import { Component, OnInit } from '@angular/core';
import { SpinnerService } from './services/spinner/SpinnerService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private readonly spinnerService:SpinnerService){}
  ngOnInit(): void {
    this.spinnerService.isSpinnerActive.subscribe((spinnerValue: boolean)=>{
      this.isSpinnerActive = spinnerValue;
    })
  }
  
  public isSpinnerActive: boolean = false;
  title = 'ChatApplication';
}

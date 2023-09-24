import { Component } from '@angular/core';

@Component({
  selector: 'app-global-spinner',
  templateUrl: './global-spinner.component.html',
  styleUrls: ['./global-spinner.component.css']
})
export class GlobalSpinnerComponent {
  public spinnerStyles={
    position:'fixed',
    bottom: 0,
    borderRadius: '50px',
    transform: 'scaleY(2)',
    zIndex: 999
  }
  public spinnerWrapperStyles={
    position:'fixed',
    width:'100%',
    height:'100%',
    backgroundColor:'rgba(0,0,0,0.5)',
    zIndex: 998
  }
  get getSpinnerStyles(){
    return this.spinnerStyles
  }
  get getSpinnerWrapperStyles(){
    return this.spinnerWrapperStyles
  }
}

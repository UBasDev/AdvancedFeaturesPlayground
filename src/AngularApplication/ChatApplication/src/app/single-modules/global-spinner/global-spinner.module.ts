import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GlobalSpinnerComponent } from './global-spinner.component';
import {MatProgressBarModule} from '@angular/material/progress-bar';



@NgModule({
  declarations: [
    GlobalSpinnerComponent
  ],
  imports: [
    CommonModule,
    MatProgressBarModule
  ],
  exports: [
    GlobalSpinnerComponent
  ]
})
export class GlobalSpinnerModule { }

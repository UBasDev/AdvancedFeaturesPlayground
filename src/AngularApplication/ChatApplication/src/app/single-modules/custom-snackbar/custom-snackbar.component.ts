import { Component, Inject, InjectionToken, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_SNACK_BAR_DATA, MatSnackBar } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-custom-snackbar',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  template: `
    <div>
      <div class="flex items-center justify-between">
        <p>Hello {{currentUsername}}!</p>
        <mat-icon class="cursor-pointer" (click)="closeSnackbar()">close</mat-icon>
      </div>
      <p>You are in queue now, please wait...</p>
    </div>
  `,
  styles:[
    `
    `
  ]
})
export class CustomSnackbarComponent implements OnInit {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public snackbarData:{username:string}, private readonly snackBarService: MatSnackBar){}
  public currentUsername:string = ""
  ngOnInit(): void {
    this.currentUsername = this.snackbarData.username
  }
  closeSnackbar():void{
    this.snackBarService.ngOnDestroy()
  }
}

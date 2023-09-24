import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatLoginComponent } from './chat-login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDividerModule } from '@angular/material/divider';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatButtonModule} from '@angular/material/button';
@NgModule({
  declarations: [ChatLoginComponent],
  imports: [CommonModule, ReactiveFormsModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatInputModule, MatSelectModule, MatButtonModule],
})
export class ChatLoginModule {}

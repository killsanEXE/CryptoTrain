import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from '../account/login/login.component';
import { RegisterComponent } from '../account/register/register.component';
import { FormsSharedModule } from './formsShared.module';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
  ],
  imports: [
    CommonModule,
    FormsSharedModule
  ],
  exports: [
    LoginComponent,
    RegisterComponent,
  ]
})
export class SharedModule { }

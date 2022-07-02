import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AccountComponent } from './account/account.component';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from './_modules/shared.module';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { FormsSharedModule } from './_modules/formsShared.module';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    AccountComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    FormsSharedModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }

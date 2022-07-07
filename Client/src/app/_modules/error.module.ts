import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotFoundComponent } from '../errors/not-found/not-found.component';
import { ServerErrorComponent } from '../errors/server-error/server-error.component';



@NgModule({
  declarations: [
    NotFoundComponent,
    ServerErrorComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    NotFoundComponent,
    ServerErrorComponent
  ]
})
export class ErrorModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CryptoComponent } from '../crypto/crypto/crypto.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    CryptoComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    CryptoComponent
  ]
})
export class CryptoModule { }

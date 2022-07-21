import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CryptoComponent } from '../crypto/crypto.component';

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

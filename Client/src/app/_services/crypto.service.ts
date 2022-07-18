import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CryptoModel } from '../_models/cryptoModel';

@Injectable({
  providedIn: 'root'
})
export class CryptoService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getBTCHistory(){
    return this.http.get<CryptoModel[]>(this.baseUrl + "crypto/btc");
  }
}

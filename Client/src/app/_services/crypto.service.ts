import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CryptoModel } from '../_models/cryptoModel';
import { PaginatedResult } from '../_models/pagination';
import { getPaginatedResult, getPaginationHeaders } from '../_models/paginationHelper';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class CryptoService {

  baseUrl = environment.apiUrl;
  cryptoCache = new Map();
  userParams: UserParams;

  private cryptoThreadSource = new BehaviorSubject<CryptoModel[]>([]);
  crypto: CryptoModel[] = [];
  crypto$ = this.cryptoThreadSource.asObservable();

  constructor(private http: HttpClient) { this.userParams = new UserParams(); }

  getUserParams(){ return this.userParams; }
  setUserParams(params: UserParams){ this.userParams = params; }

  getBTCHistory(userParams: UserParams){
    var response = this.cryptoCache.get(Object.values(userParams).join("-"));
    if(!response){
      let params = getPaginationHeaders(userParams.pageNumber, userParams.PageSize);
      return getPaginatedResult<CryptoModel[]>(this.baseUrl + "crypto/btc", params, 
        this.http).pipe(map(response => {
          this.cryptoCache.set(Object.values(userParams).join("-"), response);
          response.result.forEach(f => this.crypto.push(f));
          // this.cryptoThreadSource.subscribe(f => this.cryptoThreadSource.next([...f, { id: 2, date: new Date(), price: 2}]));
          // this.cryptoThreadSource.subscribe(f => this.cryptoThreadSource.next([...f, ...response.result]))
          this.cryptoThreadSource.next(this.crypto);
          return response;
      }));
    }
    return of(response);
  }
}

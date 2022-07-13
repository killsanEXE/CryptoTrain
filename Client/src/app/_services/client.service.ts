import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, ReplaySubject, take, scan, BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from '../_models/paginationHelper';
import { Transaction } from '../_models/transaction';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  baseUrl = environment.apiUrl;
  transactionCache = new Map();
  userParams: UserParams;

  private transactionsThreadSource = new BehaviorSubject<Transaction[]>([]);
  transactions: Transaction[] = [];
  transactions$ = this.transactionsThreadSource.asObservable();

  constructor(private http: HttpClient) {
    this.userParams = new UserParams();
  }

  getUserParams(){ return this.userParams; }
  setUserParams(params: UserParams){ this.userParams = params; }

  getTransactions(userParams: UserParams){
    var response = this.transactionCache.get(Object.values(userParams).join("-"));
    if(response){
      return of(response);
    }else{
      let params = getPaginationHeaders(userParams.pageNumber, userParams.PageSize);

      return getPaginatedResult<Transaction[]>(this.baseUrl + "client/transactions", params, 
        this.http).pipe(map(response => {
        this.transactionCache.set(Object.values(userParams).join("-"), response);
        response.result.forEach(f => this.transactions.push(f));
        this.transactionsThreadSource.next(this.transactions);
        return response;
      }));
    }
  }

  replenishUsd(){
    return this.http.put<User>(this.baseUrl + "client/replenish-usd", {});
  }

}
import { AfterViewChecked, Component, ElementRef, Input, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { pipe, take } from 'rxjs';
import { Transaction } from 'src/app/_models/transaction';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { ClientService } from 'src/app/_services/client.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements AfterViewChecked{

  @Input() accountService: AccountService;
  transactions: Transaction[] = [];
  @ViewChild("scrollMe") scrollMe: ElementRef;
  @ViewChild("scrollMeMobile") scrollMeMobile: ElementRef;
  user: User;

  constructor(public clientService: ClientService, private snackBar: MatSnackBar) { }

  ngAfterViewChecked(): void {
    this.loadTransactions(this.clientService.getUserParams());
  }

  loadTransactions(userParams: UserParams){
    this.clientService.getTransactions(userParams).subscribe((f) => { 
      f.result.forEach(f => this.transactions.push(f));
    });
  }

  logout(){
    this.accountService.logout();
  }

  loadMore(){
    let userParams = this.clientService.getUserParams();
    ++userParams.pageNumber;
    this.clientService.setUserParams(userParams);
    this.loadTransactions(userParams);
  }

  scrollToBottom(lastcall: boolean){
    if(lastcall){
      setTimeout(() => {
        this.scrollMe.nativeElement.scrollTop = this.scrollMe.nativeElement.scrollHeight;
      })
    }
  }

  scrollToBottomMobile(lastcallMobile: boolean){
    if(lastcallMobile){
      setTimeout(() => {
        this.scrollMeMobile.nativeElement.scrollTop = this.scrollMeMobile.nativeElement.scrollHeight;
      })
    }
  }

  replenishUsd(){
    this.accountService.currentUser$.pipe(take(1)).subscribe(f => this.user = f);
    this.clientService.replenishUsd().subscribe(user => {
      this.user.usdAmount = user.usdAmount;
      this.accountService.setCurrentUser(this.user);
      this.snackBar.open("+5000$", "Ok", { duration: 3000 })
    })
  }
}

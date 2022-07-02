import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AccountComponent implements OnInit {

  loginTabActive = true;

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  openRegister = () => this.loginTabActive = false;
  openLogin = () => this.loginTabActive = true;
}

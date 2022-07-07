import { ChangeDetectionStrategy, Component, Input, OnInit, Self } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  @Input() accountService: AccountService;
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router) {
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.loginForm = this.fb.group({
      username: ["", [Validators.required, this.checkForSpaces()]],
      password: ["", [Validators.required]]
    })
  }

  checkForSpaces(): ValidatorFn{
    return (control: AbstractControl) => {
      if(String(control.value).indexOf(" ") >= 0){
        return { withSpaces: true }
      }
      return null;
    }
  }

  login(){
    this.accountService.login(this.loginForm.value).subscribe(() => {
      this.router.navigateByUrl("/");
    });
  }
}
